
using Content.Server.Botany;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Server._Coyote.Helpers;

public struct AphrodisiacChecker
{
    private readonly string _aphrodisiacGroup = "Aphrodisiac";
    private readonly string _aphrodisiacDrinkGroup = "AphrodisiacDrink";

    public AphrodisiacChecker() { }
    public bool CheckForAphrodisiacs(IPrototypeManager prototypeManager, Solution solution)
    {
        foreach (var (reagent, _) in solution.Contents)
        {
            if (IsReagentAphrodisiac(prototypeManager, reagent))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsSeedLaced(IPrototypeManager prototypeManager, SeedData seed)
    {
        foreach ((var reagent, _) in seed.Chemicals)
        {
            if (IsReagentAphrodisiac(prototypeManager, reagent))
                return true;
        }

        return false;
    }

    public bool IsReagentAphrodisiac(IPrototypeManager prototypeManager, string reagent) => IsReagentAphrodisiac(prototypeManager, new ReagentId(reagent, null));

    public bool IsReagentAphrodisiac(IPrototypeManager prototypeManager, ReagentId reagent)
    {
        var prototype = prototypeManager.Index<ReagentPrototype>(reagent.Prototype);
        return prototype.Group == _aphrodisiacGroup || prototype.Group == _aphrodisiacDrinkGroup;
    }
}
