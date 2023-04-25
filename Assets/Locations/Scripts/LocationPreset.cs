using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(fileName = "New Location", menuName = "Locations/Preset Location")]
    public class LocationPreset : ScriptableObject
    {
        public enum LocationType { City, Fort, Village, Tower }
        [field: SerializeField] public string LocationName { get; private set; }
        [field: SerializeField] public LocationType type { get; private set; }
    }
}