using System.Collections.Generic;
using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Location Database")]
    public class LocationDatabase : ScriptableObject
    {
        [SerializeField] private LocationXMLReader presetLocations;

        private Dictionary<string, LocationData> locationsByName;

        public void Init() => LoadPresetLocations();

        private void LoadPresetLocations()
        {
            locationsByName = new Dictionary<string, LocationData>();

            //later on also get all new locations from saved data
        }

        public LocationData GetLocation(string name)
        {
            if (locationsByName.ContainsKey(name))
            {
                return locationsByName[name];
            }
            return null;
        }

        public List<LocationData> GetAllLocations()
        {
            var locations = new List<LocationData>();

            foreach (var location in locationsByName)
            {
                locations.Add(location.Value);
            }
            return locations;

        }
    }
}