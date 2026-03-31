using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;

namespace Content.Server.Coyote.AphrodisiacLacedContainerVisibility;

/// <summary>
/// Component that keeps track of solution containers to see if they have been injected by horny juice.
/// </summary>
[RegisterComponent]
public sealed partial class AphrodisiacLacedContainerVisibilityComponent : Component
{
    /// <summary>
    /// The name of the solution of which to check for HORNY
    /// </summary>
    [DataField("solution")]
    public string Solution { get; set; } = string.Empty;

    // REVIEW: Maybe instead of the laced bool, I just remove the component using its own system when the aphro runs out.
    // <summary>
    // If this entity has been laced.
    // </summary>
    [DataField("laced")]
    public bool Laced { get; set; } = false;

    // <summary>
    // The icon to be used for the status effect.
    // </summary>
    [DataField("icon")]
    public ProtoId<StatusIconPrototype> Icon = "AphrodisiacLacing";
}
