using Content.Shared.Contraband;

namespace Content.Shared._CS;

/// <summary>
/// This handles...
/// </summary>
public sealed class UnContrabandSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<UnContrabandComponent, ComponentInit>(OnInit);
    }

    private void OnInit(EntityUid uid, UnContrabandComponent component, ref ComponentInit args)
    {
        RemComp<ContrabandComponent>(uid);
    }
}
