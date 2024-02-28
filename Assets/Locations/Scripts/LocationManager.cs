using UnityEngine;
using System.Collections.Generic;
using SD.PathingSystem;

namespace SD.LocationSystem
{
    public class LocationManager : MonoBehaviour
    {
        private static LocationManager instance;

        private HashSet<MapLocation> _locations;

        [SerializeField] private LocationDatabase database;

        //[SerializeField] private LocationEncounter locationPrefab;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            database.Init();
            _locations = new HashSet<MapLocation>();
        }

        private void Start() => PlaceLocations();

        private void PlaceLocations()
        {
          
            /*
            foreach(var location in database.GetAllLocations())
            {
                var newLocation = Instantiate(locationPrefab);
                newLocation.SetLocation(location);
                newLocation.transform.SetParent(transform);
            }*/
            //will also need to include IsDiscovered values for all presets

            //work this in later
        }

        public static void Register(MapLocation location)
        {
            if (instance._locations.Contains(location)) return;
            instance._locations.Add(location);
        }

        public static bool TryGetLocation(PathNode node, out MapLocation location)
        {
            location = null;
            if (node == null) return false;
            foreach(var loc in instance._locations)
            {
                if (loc.Node == node)
                {
                    location = loc;
                    return true;
                }
            }
            return false;
        }
    }
}