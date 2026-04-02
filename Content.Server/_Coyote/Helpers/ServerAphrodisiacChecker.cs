
using Content.Server.Botany;
using Content.Shared._Coyote.Helpers;
using Robust.Shared.Prototypes;

namespace Content.Server._Coyote.Helpers;

// We use this helper server-side for helping with specific server side issues (e.g.checking reagents in seeds, which is server-side only.)
// Sealed cause we don't need more inheritance atop of this.
public sealed class ServerAphrodisiacChecker : SharedAphrodisiacChecker
{
    public ServerAphrodisiacChecker() : base() { }

    public bool IsSeedLaced(IPrototypeManager prototypeManager, SeedData seed)
    {
        foreach ((var reagent, _) in seed.Chemicals)
        {
            if (IsReagentAphrodisiac(prototypeManager, reagent))
                return true;
        }

        return false;
    }
}
