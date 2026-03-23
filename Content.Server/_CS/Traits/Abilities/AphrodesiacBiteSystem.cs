using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared._CS.Traits.Abilities;
using Content.Shared.Actions;
using Content.Shared.Chemistry.Components;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;

namespace Content.Server._CS.Traits.Abilities;

public sealed class AphrodesiacBiteSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly BloodstreamSystem _bloodstream = default!;
    private readonly SoundSpecifier _bite = new SoundPathSpecifier("/Audio/Effects/bite.ogg");

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AphrodesiacBiteEvent>(OnBite);
        SubscribeLocalEvent<AphrodesiacBiteComponent, ComponentInit>(OnInit);
    }

    public void OnInit(EntityUid uid, AphrodesiacBiteComponent component, ComponentInit args)
    {
        _actions.AddAction(uid, ref component.ActionEntity, component.Action, uid);
    }

    public void OnBite(AphrodesiacBiteEvent ev)
    {
        if (ev.Handled)
            return;

        TryInject(ev.Target, ev.Performer);
    }

    public void TryInject(EntityUid target, EntityUid user)
    {
        if (!TryComp<BloodstreamComponent>(target, out var bloodstream))
            return;

        var solution = new Solution("Libidozenithizine", 5);
        if (_bloodstream.TryAddToChemicals(target, solution, bloodstream))
            _audio.PlayPvs(_bite, user);
    }
}
