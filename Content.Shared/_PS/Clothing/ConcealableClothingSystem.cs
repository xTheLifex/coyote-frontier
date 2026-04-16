using Content.Shared.Actions;
using Content.Shared.Clothing;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Inventory.Events;
using Content.Shared.Popups;
using Robust.Shared.Timing;

namespace Content.Shared._PS.Clothing;

public sealed class ConcealableClothingSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ConcealableClothingComponent, GotUnequippedEvent>(OnGotUnequipped);
        SubscribeLocalEvent<ConcealableClothingComponent, GotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<ConcealableClothingComponent, GetItemActionsEvent>(GetActions);
        SubscribeLocalEvent<ConcealableClothingComponent, ToggleClothingConcealmentEvent>(OnToggle);
        SubscribeLocalEvent<ConcealableClothingComponent, GetEquipmentVisualsEvent>(OnGetVisuals);
    }

    private void OnGetVisuals(EntityUid uid, ConcealableClothingComponent component, GetEquipmentVisualsEvent args)
    {
        if (!component.IsConcealed)
            return;

        args.Layers.Clear();
    }

    private void OnToggle(EntityUid uid, ConcealableClothingComponent component, ToggleClothingConcealmentEvent args)
    {
        // We need a user defined to be able to toggle this.
        if (component.User == null)
            return;

        args.Handled = true;
        var user = (EntityUid) component.User;
        component.IsConcealed = !component.IsConcealed;
        _actions.SetToggled(component.ToggleActionEntity, component.IsConcealed);
        Dirty(uid, component);

        // Popup
        var state = component.IsConcealed ? "hidden" : "shown";
        var itemName = Identity.Entity(uid, EntityManager);
        var userName = Identity.Entity(user, EntityManager);
        var selfText = Loc.GetString($"actions-concealment-{state}-self", ("item", itemName));
        var othersText = Loc.GetString($"actions-concealment-{state}-others", ("user", userName), ("item", userName));
        _popup.PopupPredicted(selfText, othersText, user, user);

        // Spawn Sparks
        if (!_timing.IsFirstTimePredicted)
            return;

        var coord = Transform(uid).Coordinates;
        PredictedSpawnAtPosition("BlueFlashEffect", coord);
    }

    private void GetActions(EntityUid uid, ConcealableClothingComponent component, GetItemActionsEvent args)
    {
        args.AddAction(ref component.ToggleActionEntity, component.ToggleAction);
    }

    private void OnGotUnequipped(EntityUid uid, ConcealableClothingComponent component, GotUnequippedEvent args)
    {
        component.User = null;
    }

    private void OnGotEquipped(EntityUid uid, ConcealableClothingComponent component, GotEquippedEvent args)
    {
        component.User = args.Equipee;
    }

}

public sealed partial class ToggleClothingConcealmentEvent : InstantActionEvent;
