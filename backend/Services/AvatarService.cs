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
        await upload.CopyToAsync(buffer, ct);
        buffer.Position = 0;

        using var original = SKBitmap.Decode(buffer);
        if (original is null)
            throw new InvalidOperationException("ئەمە وێنەیەکی دروست نییە.");

        using var square = CropSquare(original);
        using var image = SKImage.FromBitmap(square);
        using var encoded = image.Encode(SKEncodedImageFormat.Jpeg, JpegQuality)
            ?? throw new InvalidOperationException("وێنەکە نەتوانرا پرۆسێس بکرێت.");

        var name = $"{Guid.NewGuid():N}.jpg";
        var path = Path.Combine(_folder, name);

        await using (var file = File.Create(path))
            encoded.SaveTo(file);

        return name;
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
