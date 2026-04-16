using Content.Server.Actions;
using Content.Server.Popups;
using Content.Shared._PS.Clothing;
using Content.Shared.Inventory.Events;
using Robust.Shared.Timing;

namespace Content.Server._PS.Clothing;

public sealed class ServerConcealableClothingSystem : SharedConcealableClothingSystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ConcealableClothingComponent, GotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<ConcealableClothingComponent, GotEquippedEvent>(OnGotEquipped);
    }

    private void OnGotEquipped(EntityUid uid, ConcealableClothingComponent component, GotEquippedEvent args)
    {
        component.User = args.Equipee;
    }

    private void OnGotUnequipped(EntityUid uid, ConcealableClothingComponent component, GotUnequippedEvent args)
    {
        component.User = null;
    }
}
