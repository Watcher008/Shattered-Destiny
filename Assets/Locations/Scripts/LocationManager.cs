using UnityEngine;
using SD.ECS;

namespace SD.LocationSystem
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private LocationDatabase database;

        [Space]

        [SerializeField] private LocationEncounter locationPrefab;

        private void Awake() => database.Init();
        private void Start() => PlaceLocations();

        private void PlaceLocations()
        {
            foreach(var location in database.GetAllLocations())
            {
                var newLocation = Instantiate(locationPrefab);
                newLocation.SetLocation(location);
                newLocation.transform.SetParent(transform);
            }
            //will also need to include IsDiscovered values for all presets

            //work this in later
        }
    }
}