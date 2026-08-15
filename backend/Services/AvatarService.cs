using SkiaSharp;

namespace backend.Services;

/// <summary>
/// Stores profile pictures on disk.
///
/// Accepting a file from a browser and writing it to a folder that a web server also serves is
/// one of the oldest ways to hand out remote code execution, so this deliberately does not trust
/// anything the client says:
///
///   • The NAME is thrown away. Every file is saved as a fresh Guid plus a fixed extension, so a
///     name like "../../../etc/cron.d/x" or "shell.aspx" cannot reach the file system at all.
///   • The CONTENT-TYPE is thrown away. A header is a claim, not evidence.
///   • The BYTES decide. The stream is decoded as an image; anything that fails to decode is
///     rejected, which rules out a PHP script wearing a .png extension.
///   • The output is RE-ENCODED, never copied through. Re-encoding drops EXIF (including the GPS
///     tags phones attach), and it means the bytes finally written were produced by our encoder
///     rather than supplied by the caller — so a payload smuggled in a comment block does not
///     survive the round trip.
///
/// Images are also squared and capped at 512px. A profile picture is displayed at 40px; storing
/// the 4MB original to render it at 40 is bandwidth nobody asked for.
/// </summary>
public class AvatarService
{
    /// <summary>Anything larger is refused before a byte is read into memory.</summary>
    public const long MaxBytes = 5 * 1024 * 1024;

    /// <summary>Subfolder of <c>Uploads:Root</c>. Siblings will be "words", "audio", and so on.</summary>
    private const string Avatars = "avatars";

    /// <summary>Long edge of the stored square, in pixels.</summary>
    private const int Size = 512;

    private const int JpegQuality = 85;

    private readonly string _folder;
    private readonly ILogger<AvatarService> _log;

    public AvatarService(IConfiguration config, ILogger<AvatarService> log)
    {
        _log = log;

        // Always OUTSIDE the project tree, and always configured.
        //
        // Uploads inside the deployment directory are lost on every redeploy, land in `git status`
        // waiting to be committed, and put user-supplied files in the same tree as the application
        // binaries. On the server this is the host's own upload folder (/root/uploads/jinzar);
        // in the container it is the mount of that folder.
        //
        // Uploads:Root is the root for EVERY kind of upload, and each kind takes a subfolder of
        // it. Adding word images or audio later is then a folder name, not a new deployment
        // setting, a new mount and a new thing to forget on the next server.
        var root = config["Uploads:Root"];

        // Kept as an escape hatch for putting avatars somewhere else entirely; unset normally.
        var explicitPath = config["Uploads:AvatarPath"];

        _folder = !string.IsNullOrWhiteSpace(explicitPath) ? Expand(explicitPath)
                : !string.IsNullOrWhiteSpace(root)         ? Path.Combine(Expand(root), Avatars)
                : Path.Combine(Path.GetTempPath(), "jinzar-uploads", Avatars);

        Directory.CreateDirectory(_folder);
        _log.LogInformation("Avatar uploads: {Folder}", _folder);
    }

    public string Folder => _folder;

    /// <summary>
    /// Expands both shell conventions, so one setting reads the same on either OS:
    /// <c>%LOCALAPPDATA%</c> on Windows and <c>$HOME</c> / <c>${HOME}</c> on Linux.
    /// .NET's configuration binder does neither on its own.
    /// </summary>
    private static string Expand(string path)
    {
        path = Environment.ExpandEnvironmentVariables(path);

        path = System.Text.RegularExpressions.Regex.Replace(
            path,
            @"\$\{?(\w+)\}?",
            m => Environment.GetEnvironmentVariable(m.Groups[1].Value) ?? m.Value);

        return Path.GetFullPath(path);
    }

    /// <summary>Absolute path of a stored avatar, or null if the name is missing or suspicious.</summary>
    public string? PathFor(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName)) return null;

        // Defence in depth. These names are ours and are always plain Guids, but a row edited by
        // hand — or restored from an older schema — must not be able to read outside the folder.
        if (fileName.Contains("..") || fileName.Contains('/') || fileName.Contains('\\'))
        {
            _log.LogWarning("Rejected avatar name with path characters: {Name}", fileName);
            return null;
        }

        var full = Path.GetFullPath(Path.Combine(_folder, fileName));

        // Even after the checks above, confirm the resolved path really is inside the folder.
        return full.StartsWith(Path.GetFullPath(_folder), StringComparison.OrdinalIgnoreCase)
            ? full
            : null;
    }

    /// <summary>
    /// Validates, squares, re-encodes and stores. Returns the new file name, or throws
    /// <see cref="InvalidOperationException"/> with a message meant for the user.
    /// </summary>
    public async Task<string> SaveAsync(Stream upload, long length, CancellationToken ct = default)
    {
        if (length <= 0)
            throw new InvalidOperationException("فایلەکە بەتاڵە.");

        if (length > MaxBytes)
            throw new InvalidOperationException("وێنەکە زۆر گەورەیە — زۆرترین قەبارە ٥ مێگابایتە.");

        // Buffered because SkiaSharp needs to seek, and the request stream cannot.
        using var buffer = new MemoryStream();

        try
        {
            await upload.CopyToAsync(buffer, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // A body that stops arriving half way — phone losing signal mid-upload — surfaces
            // here, and it is the caller's problem to retry, not a fault in this server.
            _log.LogWarning(ex, "Avatar upload body could not be read to the end");
            throw new InvalidOperationException("ناردنی وێنەکە تەواو نەبوو. دووبارە هەوڵبدەوە.");
        }

        buffer.Position = 0;

        if (buffer.Length == 0)
            throw new InvalidOperationException("فایلەکە بەتاڵە.");

        var jpeg = Encode(buffer);

        var name = $"{Guid.NewGuid():N}.jpg";
        var path = Path.Combine(_folder, name);

        try
        {
            await File.WriteAllBytesAsync(path, jpeg, ct);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // A missing mount or a read-only one. The user cannot fix it, so say so plainly
            // rather than letting it read as "your picture is bad".
            _log.LogError(ex, "Could not write an avatar into {Folder}", _folder);
            throw new InvalidOperationException("وێنەکە نەتوانرا پاشەکەوت بکرێت. پەیوەندی بە بەڕێوەبەرەوە بکە.");
        }

        return name;
    }

    /// <summary>
    /// Decode → square → re-encode, with every failure turned into a sentence the user can act on.
    ///
    /// Everything in here is native SkiaSharp, and native code fails in ways that are not
    /// <see cref="InvalidOperationException"/>: a HEIC renamed to .jpg decodes to null, a truncated
    /// file throws inside the codec, and a container without libfontconfig1 cannot load
    /// libSkiaSharp at all and throws DllNotFoundException on the very first call. Uncaught, all
    /// three leave the controller as a bare 500 — which tells the person holding the phone nothing
    /// and tells us nothing either, because a 500 carries no message.
    ///
    /// So the real exception goes to the log, and the caller gets something readable.
    /// </summary>
    private byte[] Encode(Stream image)
    {
        try
        {
            using var original = SKBitmap.Decode(image)
                ?? throw new InvalidOperationException("ئەمە وێنەیەکی دروست نییە.");

            using var square = CropSquare(original);
            using var bitmap = SKImage.FromBitmap(square);
            using var encoded = bitmap.Encode(SKEncodedImageFormat.Jpeg, JpegQuality)
                ?? throw new InvalidOperationException("وێنەکە نەتوانرا پرۆسێس بکرێت.");

            return encoded.ToArray();
        }
        catch (InvalidOperationException)
        {
            // Already carries a message written for the user — do not bury it.
            throw;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Avatar image processing failed");
            throw new InvalidOperationException(
                "وێنەکە نەتوانرا پرۆسێس بکرێت. وێنەیەکی JPG یان PNG تاقی بکەرەوە.");
        }
    }

    /// <summary>Best-effort cleanup of the file a user just replaced. Never throws.</summary>
    public void TryDelete(string? fileName)
    {
        var path = PathFor(fileName);
        if (path is null) return;

        try
        {
            if (File.Exists(path)) File.Delete(path);
        }
        catch (Exception ex)
        {
            // A leftover file is litter; a failed profile save because of one is a bug.
            _log.LogWarning(ex, "Could not delete replaced avatar {Name}", fileName);
        }
    }

    /// <summary>
    /// Centre-crops to a square, then scales to <see cref="Size"/>. Cropping rather than
    /// squashing: a stretched face is worse than a cropped one, and every surface that shows
    /// these renders them in a circle anyway.
    /// </summary>
    private static SKBitmap CropSquare(SKBitmap source)
    {
        var edge = Math.Min(source.Width, source.Height);
        var left = (source.Width - edge) / 2;
        var top = (source.Height - edge) / 2;

        using var cropped = new SKBitmap(edge, edge);
        using (var canvas = new SKCanvas(cropped))
        {
            canvas.DrawBitmap(source, new SKRect(left, top, left + edge, top + edge),
                                      new SKRect(0, 0, edge, edge));
        }

        // Never upscale: a 64px avatar blown up to 512 is a blurrier file that is also bigger.
        var target = Math.Min(edge, Size);

        return cropped.Resize(new SKImageInfo(target, target), SKFilterQuality.High)
               ?? throw new InvalidOperationException("وێنەکە نەتوانرا بچووک بکرێتەوە.");
    }
}
