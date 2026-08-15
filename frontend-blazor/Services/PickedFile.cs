using Telerik.Blazor.Components;

namespace frontend_blazor.Services;

/// <summary>
/// Gets the bytes of a file the user picked with <c>TelerikFileSelect</c>.
///
/// The stream on <see cref="FileSelectFileInfo"/> is not a file on disk — it is the browser
/// feeding the file down the SignalR circuit, chunk by chunk, and it is alive only for as long as
/// the OnSelect handler is running. Two things follow, and both were upload failures before this
/// existed:
///
///   • It must be drained INSIDE the handler. Passing it onwards to something that reads it later
///     — an HttpClient posting to the API, say — leaves the read racing the handler's own return.
///   • It must be drained on its own, not while something else consumes it at the same time.
///     Piping browser → circuit → API in one pass makes the API's read depend on how fast a phone
///     on mobile data can push, and a stall there arrives at the API as a body that stopped early.
///
/// So: read it here, all of it, into memory, and let the caller deal in a byte[]. A profile
/// picture is capped at 5MB, which is a sane thing to hold for the second it takes to forward it.
/// </summary>
public static class PickedFile
{
    public static async Task<byte[]> ReadAsync(
        FileSelectFileInfo file, long maxBytes, CancellationToken ct = default)
    {
        using var buffer = new MemoryStream();

        // The stream is the browser's and is disposed with it — owning it here means the circuit
        // is not left holding a half-read one if the copy throws.
        await using (var source = file.Stream)
            await source.CopyToAsync(buffer, ct);

        // Belt and braces against a size that only became knowable once the bytes were counted.
        // FileSelect reports Size from the browser, and the browser is not a witness we trust.
        if (buffer.Length > maxBytes)
            throw new InvalidOperationException("وێنەکە زۆر گەورەیە.");

        if (buffer.Length == 0)
            throw new InvalidOperationException("فایلەکە بەتاڵە.");

        return buffer.ToArray();
    }
}
