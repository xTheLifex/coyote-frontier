using Robust.Shared.GameStates;

namespace Content.Shared._PS.Clothing;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedConcealableClothingSystem))]
public sealed partial class ConcealableClothingImplantComponent : Component;
