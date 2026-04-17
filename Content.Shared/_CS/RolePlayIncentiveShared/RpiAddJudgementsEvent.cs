using Content.Shared._CS.RolePlayIncentiveShared;
using Robust.Shared.Prototypes;

namespace Content.Server._CS;

public sealed class RpiAddJudgementsEvent(
    EntityUid targetEntity,
    List<ProtoId<RpiChatJudgementModifierPrototype>> stuffToAdd)
    : EntityEventArgs
{
    public EntityUid TargetEntity = targetEntity;
    public List<ProtoId<RpiChatJudgementModifierPrototype>> StuffToAdd = stuffToAdd;
}
