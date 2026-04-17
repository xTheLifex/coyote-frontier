using Content.Shared._CS.RolePlayIncentiveShared;
using Robust.Shared.Prototypes;

namespace Content.Server._CS;

public sealed class RpiAddJobModifierEvent(
    EntityUid targetEntity,
    List<ProtoId<RpiJobModifierPrototype>> stuffToAdd)
    : EntityEventArgs
{
    public EntityUid TargetEntity = targetEntity;
    public List<ProtoId<RpiJobModifierPrototype>> StuffToAdd = stuffToAdd;
}
