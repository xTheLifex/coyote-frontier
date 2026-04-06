namespace Content.Shared._NF.Shipyard.Prototypes;

public sealed partial class VesselPrototype
{
    /// <summary>
    ///     Extra price added per tile of the vessel when appraising/selling. Default is 1 spesos per tile. This should be changed to the LL0 price once all ships are within standards.
    [DataField("pricePerTile")]
    public int PricePerTile = 1;

    /// <summary>
    ///     Increase applied to the total appraisal/sale value (before price per tile).
    ///     For example, a markup of 1.25 means +25% on the appraisal's value.
    /// </summary>
    [DataField("markup")]
    public float Markup = 1f;

    /// <summary>
    ///     Is the vessel exped capable?
    /// </summary>
    [DataField("expedCapable")]
    public bool ExpedCapable = false;

    /// <summary>
    ///     Is the vessel donk capable?
    /// </summary>
    [DataField("donkCapable")]
    public bool DonkCapable = false;
}
