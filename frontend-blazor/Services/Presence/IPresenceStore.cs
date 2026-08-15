namespace frontend_blazor.Services.Presence;

/// <summary>
/// Three states, not two (پڕۆمپت ٩).
///
/// An open circuit does NOT mean someone is working. A teacher leaves the tab open and walks away
/// to teach a class; treating that as «چالاک» tells a colleague to expect an answer that is not
/// coming. Presence is derived from LastActivityAt, never from the connection alone.
/// </summary>
public enum PresenceStatus
{
    /// <summary>چالاک — input within the last two minutes.</summary>
    Active = 0,

    /// <summary>بێ‌چالاکی — circuit open, nothing typed or moved for 2+ minutes.</summary>
    Idle = 1,

    /// <summary>دەرچوو — circuit closed. Shows «دوایین جار X پێش ئێستا».</summary>
    Offline = 2,
}

public record PresenceSnapshot(
    Guid UserId,
    string UserName,
    string? AvatarUrl,
    PresenceStatus Status,
    DateTime LastActivityAt,
    DateTime? LastSeenAt,
    string? CurrentPage,
    int? CurrentSenseId);

/// <summary>
/// Live presence for everyone currently connected.
///
/// Behind an interface from day one because the in-memory implementation is wrong the moment this
/// app runs on more than one instance: each would know only about its own circuits, and two
/// teachers on different instances would each see the other as offline — including for the claim
/// lock, which would then let both of them edit the same sense. Swapping in a Redis-backed store
/// is a registration change, not a rewrite.
/// </summary>
public interface IPresenceStore
{
    /// <summary>A circuit opened or reconnected.</summary>
    void MarkOnline(Guid userId, string userName, string? avatarUrl = null);

    /// <summary>
    /// Raised whenever anyone's presence changes — a circuit opening or closing, or activity
    /// crossing the idle boundary. The top bar subscribes so the avatar stack is genuinely live
    /// rather than polled: presence is already an in-memory fact this process holds, and asking
    /// it on a timer would be building a slower version of something free.
    ///
    /// Fires on the caller's thread. Handlers must not block.
    /// </summary>
    event Action? Changed;

    /// <summary>
    /// The socket dropped but the circuit is retained. NOT the same as leaving: the tab may be
    /// coming back in a few seconds. It does mean we have stopped receiving input, so the person
    /// cannot honestly be reported as چالاک until they return.
    /// </summary>
    void MarkDisconnected(Guid userId);

    /// <summary>Signed out. Ends presence NOW, whatever the circuit is still doing.</summary>
    void SignOut(Guid userId);

    /// <summary>A circuit closed. Records LastSeenAt; the row stays so «دوایین جار» can be shown.</summary>
    void MarkOffline(Guid userId);

    /// <summary>Bumps LastActivityAt. Called from the throttled client heartbeat, at most every 30s.</summary>
    void Touch(Guid userId, string? currentPage = null);

    /// <summary>
    /// Which sense this person currently has open. This is the SAME FACT as the claim lock — see
    /// <c>PresenceService</c>, which sets both in one call so they cannot disagree.
    /// </summary>
    void SetCurrentSense(Guid userId, int? senseId);

    PresenceSnapshot? Get(Guid userId);
    IReadOnlyCollection<PresenceSnapshot> All();

    /// <summary>How many people are «چالاک» right now — the header count.</summary>
    int ActiveCount();

    /// <summary>Rows whose LastActivityAt has moved since the last flush, for the 60s DB write.</summary>
    IReadOnlyCollection<PresenceSnapshot> DrainDirty();
}
