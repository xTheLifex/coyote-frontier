namespace Content.Shared._NF.Shipyard.Prototypes;

public sealed partial class VesselPrototype
{
    /// <summary>
    ///     Extra price added per tile of the vessel when selling. Default is 0 spesos per tile.
    /// </summary>
    [DataField("pricePerTile")]
    public int PricePerTile = 0;

    /// <summary>
    ///     Increase applied to the total sale value (before price per tile).
    ///     For example, a markup of 1.25 means +25% value.
    /// </summary>
    [DataField("markup")]
    public float Markup = 0f;

    /// <summary>
    ///     Is the vessel exped capable? Adds half grid value or 50k to cost.
    /// </summary>
    [DataField("expedCapable")]
    public bool ExpedCapable = false;

    /// <summary>
    ///     Is the vessel donk capable? Adds 0.3*grid value or 30k to cost.
    /// </summary>
    [DataField("donkCapable")]
    public bool DonkCapable = false;
}
