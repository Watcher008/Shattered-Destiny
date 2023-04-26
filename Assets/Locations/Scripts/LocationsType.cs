using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Location Type")]
    public class LocationsType : ScriptableObject
    {
        [field: SerializeField] public Sprite sprite { get; private set; }
    }
}