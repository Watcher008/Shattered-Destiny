using System.Collections.Generic;
using UnityEngine;

namespace SD.LocationSystem
{
    [CreateAssetMenu(menuName = "Locations/Location Database")]
    public class LocationDatabase : ScriptableObject
    {
        [SerializeField] private LocationXMLReader locationsXML;

        //Locations which can be visited by traversing the worldMap
        private Dictionary<string, LocationData> primaryLocations;

        //Locations which can be visited by travelling to their parent location
        private Dictionary<string, LocationData> secondaryLocations;

        public void Init() => LoadPresetLocations();

        private void LoadPresetLocations()
        {
            primaryLocations = new Dictionary<string, LocationData>();

            var presets = locationsXML.LoadXMLFile();
            foreach ( var preset in presets )
            {
                primaryLocations.Add(preset.name, preset);
            }

            //later on also get all new locations from saved data
        }

        public LocationData GetLocation(string name)
        {
            if (primaryLocations.ContainsKey(name))
            {
                return primaryLocations[name];
            }
            return null;
        }

        public List<LocationData> GetAllLocations()
        {
            var locations = new List<LocationData>();

            foreach (var location in primaryLocations)
            {
                locations.Add(location.Value);
            }
            return locations;

        }
    }
}