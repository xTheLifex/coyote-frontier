using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Shared._CS.Helpers;

// Base helper class for all related aphrodisiac checking.
// Virtual cause we need a server-side version of it, and might need a client-side one in the future.
[Virtual]
public class SharedAphrodisiacChecker
{
    public readonly string HideTag = "HideInAphroVis";

    private readonly string _aphrodisiacGroup = "Aphrodisiac";
    private readonly string _aphrodisiacDrinkGroup = "AphrodisiacDrink";

    public SharedAphrodisiacChecker() { }
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

    public bool IsReagentAphrodisiac(IPrototypeManager prototypeManager, string reagent) => IsReagentAphrodisiac(prototypeManager, new ReagentId(reagent, null));

    public bool IsReagentAphrodisiac(IPrototypeManager prototypeManager, ReagentId reagent)
    {
        var prototype = prototypeManager.Index<ReagentPrototype>(reagent.Prototype);
        return prototype.Group == _aphrodisiacGroup || prototype.Group == _aphrodisiacDrinkGroup;
    }
}
