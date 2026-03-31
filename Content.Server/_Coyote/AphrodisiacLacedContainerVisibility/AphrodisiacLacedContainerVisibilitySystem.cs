using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;
using Content.Server._Coyote.Helpers;
using Content.Shared.SSDIndicator;
using Content.Shared.StatusIcon.Components;
using Content.Shared._Coyote.AphrodisiacLacedContainerVisibility;

namespace Content.Server._Coyote.AphrodisiacLacedContainerVisibility;

/// <summary>
/// System that shows visual feedback to any container that is injected with a aphrodisiac.
/// </summary>
public sealed class AphrodisiacLacedContainerVisibilitySystem : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    private AphrodisiacChecker _helper = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AphrodisiacLacedContainerVisibilityComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<AphrodisiacLacedContainerVisibilityComponent, SolutionContainerChangedEvent>(OnSolutionChange);
    }

    public void OnMapInit(Entity<AphrodisiacLacedContainerVisibilityComponent> entity, ref MapInitEvent args)
    {
        CheckForAphrodisiacs(entity);
    }

    public void OnSolutionChange(Entity<AphrodisiacLacedContainerVisibilityComponent> entity, ref SolutionContainerChangedEvent args)
    {
        CheckForAphrodisiacs(entity);
    }

    public void CheckForAphrodisiacs(Entity<AphrodisiacLacedContainerVisibilityComponent> entity)
    {
        if (!EntityManager.HasComponent<SolutionContainerManagerComponent>(entity))
            return;

        if (_solutionContainerSystem.TryGetSolution(entity.Owner, entity.Comp.Solution, out _, out var solution))
        {
            var laced = _helper.CheckForAphrodisiacs(_prototypeManager, solution);
            entity.Comp.Laced = laced;

            if (laced)
            {
                EnsureComp<StatusIconComponent>(entity);
            }
            else if (!laced && HasComp<StatusIconComponent>(entity))
            {
                RemComp<StatusIconComponent>(entity);
            }
        }
    }
}
