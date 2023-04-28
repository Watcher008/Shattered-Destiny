using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Preset")]
    public class LocationPreset : ScriptableObject
    {
        [SerializeField] private int x, y;
        [SerializeField] private string Name;
        [SerializeField] private LocationType type;

        public int X => x;
        public int Y => y;
        public LocationType Type => type;

        public void SetValues(int x, int y, string name, LocationType type)
        {
            this.x = x;
            this.y = y;
            Name = name;
            this.type = type;
        }
    }
}