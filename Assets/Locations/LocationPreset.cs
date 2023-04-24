using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(fileName = "New Location", menuName = "Locations/Preset Location")]
    public class LocationPreset : ScriptableObject
    {
        [field: SerializeField] public string LocationName { get; private set; }
    }
}