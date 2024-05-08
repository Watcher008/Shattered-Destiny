using UnityEngine;

public class LocationTile : RuleTile
{
    [SerializeField] private LocationType _type;
    public LocationType Type => _type;
}

public enum LocationType
{
    Town,
    Cave,
    Camp
}
