using System.Collections.Generic;
using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Location Database")]
    public class LocationDatabase : ScriptableObject
    {
        [SerializeField] private LocationXMLReader presetLocations;

        private Dictionary<string, Location> locationsByName;

        public void Init() => LoadPresetLocations();

        private void LoadPresetLocations()
        {
            locationsByName = new Dictionary<string, Location>();

            var presets = presetLocations.LoadXMLFile();

            for (int i = 0; i < presets.Length; i++)
            {
                locationsByName.Add(presets[i].name, presets[i]);
            }

            //later on also get all new locations from saved data
        }

        public Location GetLocation(string name)
        {
            if (locationsByName.ContainsKey(name))
            {
                return locationsByName[name];
            }
            return null;
        }

        public List<Location> GetAllLocations()
        {
            var locations = new List<Location>();

            foreach (var location in locationsByName)
            {
                locations.Add(location.Value);
            }
            return locations;

        }
    }
}