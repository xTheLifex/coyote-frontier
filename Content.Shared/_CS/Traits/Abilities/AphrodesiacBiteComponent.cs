using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._CS.Traits.Abilities;

[RegisterComponent, NetworkedComponent]
public sealed partial class AphrodesiacBiteComponent : Component
{
    [DataField]
    public EntProtoId Action = "ActionAphrodesiacBite";

    [DataField]
    public EntityUid? ActionEntity;
}

public sealed partial class AphrodesiacBiteEvent : EntityTargetActionEvent
{

}
