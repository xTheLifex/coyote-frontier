using Content.Shared.DoAfter; // Frontier: Upstream, #30704 - MIT
using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Audio;

namespace Content.Shared.Chemistry.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HyposprayComponent : Component
{
    [DataField]
    public float MaxPressure = float.MaxValue;

    [DataField]
    public float InjectTime = 0f;

    [DataField]
    public string SolutionName = "hypospray";

    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 TransferAmount = FixedPoint2.New(5);

    [DataField]
    public SoundSpecifier InjectSound = new SoundPathSpecifier("/Audio/Items/hypospray.ogg");

    /// <summary>
    /// Decides whether you can inject everything or just mobs.
    /// </summary>
    [AutoNetworkedField]
    [DataField(required: true)]
    public bool OnlyAffectsMobs = false;

    /// <summary>
    /// If this can draw from containers in mob-only mode.
    /// </summary>
    [AutoNetworkedField]
    [DataField]
    public bool CanContainerDraw = true;

    /// <summary>
    /// Whether or not the hypospray is able to draw from containers or if it's a single use
    /// device that can only inject.
    /// </summary>
    [DataField]
    public bool InjectOnly = false;

    /// <summary>
    /// Frontier: if true, object will not inject when attacking.
    /// </summary>
    [DataField]
    public bool PreventCombatInjection;

    [ViewVariables(VVAccess.ReadWrite)]
    public bool NeedHands = true;

    /// <summary>
    /// Whether or not the hypospray injects it's entire capacity on use.
    /// Used by Jet Injectors.
    /// </summary>
    [DataField]
    public bool InjectMaxCapacity = false;

    /// <summary>
    /// Whether or not the hypospray can inject chitinids.
    /// Used by Jet Injectors.
    /// </summary>
    [DataField]
    public bool BypassBlockInjection = true;

    /// <summary>
    /// Whether or not this hypospray self injects instantly.
    /// </summary>
    [DataField]
    public bool InstantSelfInject = false;
}

// Frontier: Upstream, #30704 - MIT
[Serializable, NetSerializable]
public sealed partial class HyposprayDoAfterEvent : SimpleDoAfterEvent
{
}
// End Frontier
