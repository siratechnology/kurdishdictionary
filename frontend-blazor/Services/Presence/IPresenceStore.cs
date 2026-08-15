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

    /// <summary>
    /// The socket came back. Undoes <see cref="MarkDisconnected"/>.
    ///
    /// This has to be its own call. Touch was doing the job and could not: it bumps the activity
    /// clock but never cleared the disconnected flag, which made that flag a ONE-WAY LATCH. One
    /// dropped socket — a phone sleeping, wifi blinking, an idle timeout at the proxy — and the
    /// person was pinned to بێ‌چالاکی for the rest of the session. They still saw everyone else,
    /// because their own strip reads the store; nobody who signed in afterwards could see THEM,
    /// because their entry never came back to چالاک. Only a full reload cleared it, by opening a
    /// new circuit and going through MarkOnline.
    /// </summary>
    void MarkReconnected(Guid userId);

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

    /// <summary>
    /// Puts rows back in the dirty set after a failed flush.
    ///
    /// DrainDirty clears the flag as it reads, so a flush that then failed dropped those rows on
    /// the floor while the log claimed they would be retried on the next tick. They were not:
    /// nothing marks them dirty again until the person moves. This makes the promise true.
    /// </summary>
    void MarkDirty(IEnumerable<Guid> userIds);

    /// <summary>
    /// Re-checks everyone's status and raises <see cref="Changed"/> if any of them moved.
    ///
    /// Status is DERIVED from a timestamp, so going بێ‌چالاکی is the one transition that happens
    /// because time passed rather than because somebody did something — and an event-driven store
    /// has nothing to announce it with. Without this the face strip kept showing people who had
    /// gone quiet until an unrelated write happened to refresh it, so two colleagues looking at the
    /// same moment saw different lists.
    /// </summary>
    void SweepStatuses();
}
