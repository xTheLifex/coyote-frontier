using Content.Shared._CS.RolePlayIncentiveShared;
using Robust.Shared.Prototypes;

namespace Content.Server._CS;

public sealed class RpiAddAurasEvent : EntityEventArgs
{
    public EntityUid TargetEntity;
    public List<ProtoId<RpiAuraPrototype>> AurasToAdd;

    public RpiAddAurasEvent(EntityUid targetEntity, List<ProtoId<RpiAuraPrototype>> aurasToAdd)
    {
        TargetEntity = targetEntity;
        AurasToAdd = aurasToAdd;
    }
}
