using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Robust.Shared.Prototypes;
using Content.Shared.StatusIcon.Components;
using Content.Shared._Coyote.AphrodisiacLacedContainerVisibility;

namespace Content.Client._Coyote.AphrodisiacLacedContainerVisibility;

/// <summary>
/// System that shows visual feedback to any container that is injected with a aphrodisiac.
/// </summary>
public sealed class AphrodisiacLacedContainerVisibilitySystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AphrodisiacLacedContainerVisibilityComponent, GetStatusIconsEvent>(OnGetStatusIcon);
    }

    private void OnGetStatusIcon(EntityUid uid, AphrodisiacLacedContainerVisibilityComponent component, ref GetStatusIconsEvent args)
    {
        // TODO: Add check for preference here
        if (!component.Laced)
            return;

        args.StatusIcons.Add(_prototypeManager.Index(component.Icon));
    }
}
