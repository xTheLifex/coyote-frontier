using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Map.Components;
using System.Linq;
using Content.Shared._NF.Shipyard.Components;
using Content.Shared._NF.Shipyard.Prototypes;

namespace Content.Server.Cargo.Systems;

public sealed partial class PricingSystem
{
    [Dependency] private readonly SharedMapSystem _map = default!;

    /// <summary>
    /// Minimum price added to a ship for being exped capable.
    /// </summary>
    private int _expedCapableMinPrice = 50000;

    /// <summary>
    /// Minimum price added to a ship for using donk appliances.
    /// </summary>
    private int _donkCapableMinPrice = 30000;

    /// <summary>
    /// How much should we return of the price per tile on resale? 0.5 means 50%.
    /// </summary>
    private float _tileCostPercentReturn = 0.5f;

    private void CSInitialize()
    {
        _consoleHost.RegisterCommand("coyoteappraisegrid",
            "Calculates the total value of a grid including tile count, markup, price per tile, and expedition/donk bonuses.",
            "coyoteappraisegrid <gridId> [markup=value] [pricePerTile=value] [expedCapable=bool] [donkCapable=bool]",
            CoyoteAppraiseGridCommand);
    }
    public double CoyoteAppraiseGrid(EntityUid shuttleUid, Func<EntityUid, bool>? lacksPreserveOnSaleComp = null, ShuttleDeedComponent? deed = null, bool isSale = true)
    {
        // 1. Raw appraisal (excluding items that shouldn't be sold)
        var appraisal = AppraiseGrid(shuttleUid, lacksPreserveOnSaleComp);

        // 2. If no vessel prototype is stored, return raw value
        if (string.IsNullOrEmpty(deed?.VesselID))
            return appraisal;

        // 3. Found an id, try the prototype manager index. If that doesn't work, return raw appraisal.
        if (!_prototypeManager.TryIndex<VesselPrototype>(deed.VesselID, out var vessel))
            return appraisal;

        // 4. Count tiles on the grid
        if (!TryComp<MapGridComponent>(shuttleUid, out var gridComp))
            return appraisal;

        var tileCount = _map.GetAllTiles(shuttleUid, gridComp).Count();

        // 5. Apply modifiers
        var modified = appraisal * vessel.Markup;

        if (!isSale) // Costs that are not taken into account when reselling the vessel. isSale will always be true unless you're implementing a dynamic ship purchase system.
        {
            if (vessel.PricePerTile > 0)
                modified += tileCount * vessel.PricePerTile;

            if (vessel.DonkCapable)
            {
                var expedBonus = modified * 0.5;
                modified += expedBonus <= _expedCapableMinPrice ? _expedCapableMinPrice : expedBonus;
            }
            if (vessel.ExpedCapable)
            {
                var donkBonus = modified * 0.3;
                modified += donkBonus <= _donkCapableMinPrice ? _donkCapableMinPrice : donkBonus;
            }
        }
        else // Tile price can actually get fairly expensive. Let's return at least some of it.
        {
            if (vessel.PricePerTile > 0)
                modified += (tileCount * vessel.PricePerTile) * _tileCostPercentReturn;
        }
        return modified;
    }

    [AdminCommand(AdminFlags.Debug)]
    private void CoyoteAppraiseGridCommand(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0)
        {
            shell.WriteError("Not enough arguments. Usage: coyoteappraisegrid <gridId> [markup=value] [pricePerTile=value] [expedCapable=bool] [donkCapable=bool]");
            return;
        }

        // Parse overrides from arguments (format: key=value)
        float? markupOverride = null;
        int? pricePerTileOverride = null;
        bool? expedCapableOverride = null;
        bool? donkCapableOverride = null;

        List<string> gridArgs = new();
        foreach (var arg in args)
        {
            var parts = arg.Split('=', 2);
            if (parts.Length == 2)
            {
                var key = parts[0].ToLowerInvariant();
                var val = parts[1];
                switch (key)
                {
                    case "markup":
                        if (float.TryParse(val, out var m))
                            markupOverride = m;
                        else
                            shell.WriteError($"Invalid markup value: {val}");
                        break;
                    case "pricepertile":
                        if (int.TryParse(val, out var p))
                            pricePerTileOverride = p;
                        else
                            shell.WriteError($"Invalid pricePerTile value: {val}");
                        break;
                    case "expedcapable":
                        if (bool.TryParse(val, out var e))
                            expedCapableOverride = e;
                        else
                            shell.WriteError($"Invalid expedCapable value: {val}");
                        break;
                    case "donkcapable":
                        if (bool.TryParse(val, out var d))
                            donkCapableOverride = d;
                        else
                            shell.WriteError($"Invalid donkCapable value: {val}");
                        break;
                    default:
                        shell.WriteError($"Unknown parameter: {key}");
                        break;
                }
            }
            else
            {
                gridArgs.Add(arg);
            }
        }

        // Process each grid ID
        foreach (var gid in gridArgs)
        {
            if (!EntityManager.TryParseNetEntity(gid, out var gridId) || !gridId.Value.IsValid())
            {
                shell.WriteError($"Invalid grid ID \"{gid}\".");
                continue;
            }

            if (!TryComp(gridId, out MapGridComponent? mapGrid))
            {
                shell.WriteError($"Grid \"{gridId}\" doesn't exist.");
                continue;
            }

            // 1. Raw appraisal
            var rawAppraisal = AppraiseGrid(gridId.Value, null);

            // 2. Get tile count
            var tileCount = 0;
            if (TryComp(gridId, out MapGridComponent? gridComp))
            {
                tileCount = _map.GetAllTiles(gridId.Value, gridComp).Count();
            }

            // 3. Determine modifiers (deed or overrides)
            float markup = 1f;
            int pricePerTile = 100;
            bool expedCapable = false;
            bool donkCapable = false;

            // Try to get deed
            if (TryComp<ShuttleDeedComponent>(gridId, out var deed) && !string.IsNullOrEmpty(deed.VesselID))
            {
                if (_prototypeManager.TryIndex<VesselPrototype>(deed.VesselID, out var vessel))
                {
                    markup = vessel.Markup;
                    pricePerTile = vessel.PricePerTile;
                    expedCapable = vessel.ExpedCapable;
                    donkCapable = vessel.DonkCapable;
                }
                else
                {
                    shell.WriteError($"Vessel prototype {deed.VesselID} not found for grid {gid}. Using defaults.");
                }
            }

            // Apply overrides
            if (markupOverride.HasValue) markup = markupOverride.Value;
            if (pricePerTileOverride.HasValue) pricePerTile = pricePerTileOverride.Value;
            if (expedCapableOverride.HasValue) expedCapable = expedCapableOverride.Value;
            if (donkCapableOverride.HasValue) donkCapable = donkCapableOverride.Value;

            // 4. Compute modified value
            var modified = rawAppraisal * markup;

            if (pricePerTile > 0)
                modified += tileCount * pricePerTile;

            if (expedCapable)
            {
                var expedBonus = modified * 0.5;
                modified += expedBonus <= _expedCapableMinPrice ? _expedCapableMinPrice : expedBonus;
            }
            if (donkCapable)
            {
                var donkBonus = modified * 0.3;
                modified += donkBonus <= _donkCapableMinPrice ? _donkCapableMinPrice : donkBonus;
            }

            int modifiedInt = (int)Math.Round(modified);

            // 5. Output
            shell.WriteLine($"Grid {gid}:");
            shell.WriteLine($"  Raw appraisal: {rawAppraisal:F2} spesos");
            shell.WriteLine($"  Tile count: {tileCount}");
            shell.WriteLine($"  Markup: {markup:F2}x");
            shell.WriteLine($"  Price per tile: {pricePerTile} spesos/tile");
            shell.WriteLine($"  Exped capable: {expedCapable}");
            shell.WriteLine($"  Donk capable: {donkCapable}");
            shell.WriteLine($"  Modified appraisal: {modifiedInt:F2} spesos");
            shell.WriteLine("");
        }
    }
}
