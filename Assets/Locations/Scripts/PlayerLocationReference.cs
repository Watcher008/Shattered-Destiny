using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Player Location Reference")]
    public class PlayerLocationReference : ScriptableObject
    {
        public LocationData playerLocation { get; set; }
    }
}