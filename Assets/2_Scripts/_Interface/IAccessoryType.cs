using UnityEngine;

public interface IAccessoryType
{
    AccessoryType AccessoryType { get; set; }
}

public enum AccessoryType
{
    Bang,
    Bracer,
    Times,
    EmptyTome,
    Thunder,
    TheFlags,
    MagicHood,
    FloppyDisk,
    Attractorb,
    RedAja
}