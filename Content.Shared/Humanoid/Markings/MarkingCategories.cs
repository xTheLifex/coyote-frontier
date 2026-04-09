using Robust.Shared.Serialization;

namespace Content.Shared.Humanoid.Markings
{
    [Serializable, NetSerializable]
    public enum MarkingCategories : byte
    {
        Special,
        Hair,
        FacialHair,
        Head,
        HeadTop,
        HeadSide,
        Snout,
        Chest,
        NeckFluff,
        UndergarmentTop,
        UndergarmentBottom,
        Genital,
        Arms,
        Legs,
        Tail,
        TailExtras, // Starlight
        Overlay,
        BaseChest,
        BaseHead,
        BaseLArm,
        BaseLFoot,
        BaseLHand,
        BaseLLeg,
        BaseRArm,
        BaseRFoot,
        BaseRHand,
        BaseRLeg,
        BaseArms,
        BaseLegs,
    }

    public static class MarkingCategoriesConversion
    {
        /// <summary>
        /// Easy cheat cheet for converting BaseLayers to Bodyparts to hide!
        /// Basically if they have a base marking on a bodypart, we need to hide the base layer of that bodypart!
        /// However, we only have four categories: Arms, legs, chest, head.
        /// Chest and head are easy, arms and legs need to rout to left or right, and armleg or handfoot respectively.
        /// </summary>
        public static bool Category2Layer(
            MarkingCategories category,
            HumanoidVisualLayers hvLayers,
            out HumanoidVisualLayers baseLayerToHide
            )
        {
            baseLayerToHide = HumanoidVisualLayers.Disregard;
            switch (category)
            {
                case MarkingCategories.BaseChest:
                    baseLayerToHide = HumanoidVisualLayers.Chest;
                    return true;
                case MarkingCategories.BaseHead:
                    baseLayerToHide = HumanoidVisualLayers.Head;
                    return true;
                // idk
                case MarkingCategories.BaseArms
                    or MarkingCategories.BaseLegs
                    when hvLayers
                        is HumanoidVisualLayers.LArm
                        or HumanoidVisualLayers.LHand
                        or HumanoidVisualLayers.RArm
                        or HumanoidVisualLayers.RHand
                        or HumanoidVisualLayers.LLeg
                        or HumanoidVisualLayers.LLegBehind
                        or HumanoidVisualLayers.LFoot
                        or HumanoidVisualLayers.LFootBehind
                        or HumanoidVisualLayers.RLeg
                        or HumanoidVisualLayers.RLegBehind
                        or HumanoidVisualLayers.RFoot
                        or HumanoidVisualLayers.RFootBehind:
                    baseLayerToHide = hvLayers;
                    return true;
                default:
                    return false;
            }
        }

        public static MarkingCategories FromHumanoidVisualLayers(HumanoidVisualLayers layer)
        {
            return layer switch
            {
                HumanoidVisualLayers.Special             => MarkingCategories.Special,
                HumanoidVisualLayers.Hair                => MarkingCategories.Hair,
                HumanoidVisualLayers.FacialHair          => MarkingCategories.FacialHair,
                HumanoidVisualLayers.Head                => MarkingCategories.Head,
                HumanoidVisualLayers.HeadTop             => MarkingCategories.HeadTop,
                HumanoidVisualLayers.HeadSide            => MarkingCategories.HeadSide,
                HumanoidVisualLayers.Snout               => MarkingCategories.Snout,
                HumanoidVisualLayers.Chest               => MarkingCategories.Chest,
                HumanoidVisualLayers.NeckFluff           => MarkingCategories.NeckFluff, // TheDen - Ovinia, for fluff on necks
                HumanoidVisualLayers.UndergarmentTop     => MarkingCategories.UndergarmentTop,
                HumanoidVisualLayers.UndergarmentBottom  => MarkingCategories.UndergarmentBottom,
                HumanoidVisualLayers.Genital             => MarkingCategories.Genital,
                HumanoidVisualLayers.RArm                => MarkingCategories.Arms,
                HumanoidVisualLayers.LArm                => MarkingCategories.Arms,
                HumanoidVisualLayers.RHand               => MarkingCategories.Arms,
                HumanoidVisualLayers.LHand               => MarkingCategories.Arms,
                HumanoidVisualLayers.LLeg                => MarkingCategories.Legs,
                HumanoidVisualLayers.LLegBehind      => MarkingCategories.Legs,
                HumanoidVisualLayers.RLeg                => MarkingCategories.Legs,
                HumanoidVisualLayers.RLegBehind      => MarkingCategories.Legs,
                HumanoidVisualLayers.LFoot               => MarkingCategories.Legs,
                HumanoidVisualLayers.LFootBehind     => MarkingCategories.Legs,
                HumanoidVisualLayers.RFoot               => MarkingCategories.Legs,
                HumanoidVisualLayers.RFootBehind     => MarkingCategories.Legs,
                HumanoidVisualLayers.TailExtras => MarkingCategories.TailExtras, // Starlight
                HumanoidVisualLayers.Tail                => MarkingCategories.Tail,
                HumanoidVisualLayers.RArmExtension       => MarkingCategories.Arms, // Frontier: species-specific layer
                HumanoidVisualLayers.LArmExtension       => MarkingCategories.Arms, // Frontier: species-specific layer
                _ => MarkingCategories.Overlay
            };
        }
    }
}
