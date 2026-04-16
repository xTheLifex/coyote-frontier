using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._PS.Clothing;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ConcealableClothingComponent : Component
{
    /// <summary>
    /// Action for toggling visibility of this piece of clothing.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId ToggleAction = "ActionToggleConcealment";

    /// <summary>
    /// Action for toggling visibility of this piece of clothing.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? ToggleActionEntity = new();

    /// <summary>
    /// When <see langword="true"/>, it will require an implant to be used.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool RequireImplant = false;

    /// <summary>
    /// Current user that wears concealable clothing.
    /// </summary>
    [ViewVariables]
    public EntityUid? User;

    /// <summary>
    /// Whether the clothing is to be displayed or not.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public bool IsConcealed;
}
