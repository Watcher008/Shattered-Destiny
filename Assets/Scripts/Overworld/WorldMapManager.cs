using UnityEngine;
using SD.PathingSystem;

public class WorldMapManager : MonoBehaviour
{
    public static WorldMapManager Instance;

    public delegate void ToggleInputEvent();
    public ToggleInputEvent onPauseInput;
    public ToggleInputEvent onResumeInput;

    private RandomEncounterManager _randomEncounters;
    private LocationManager _locationManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _randomEncounters = GetComponent<RandomEncounterManager>();
        _locationManager = GetComponent<LocationManager>();
    }

    // Called each time the player moves/acts
    // Should not be called if the player landed on a location
    public void OnPlayerActed(PathNode currentNode)
    {
        DateTime.IncrementHour();

        // Check to see if current node has a location
        if (_locationManager.IsLocation(currentNode)) return;

        // Then roll for chance based on 'safety' of node terrain type
        // will also determine what types of enemies are encountered
        // or just what type of encounter occurs
        
        // Roll for random encounter
        _randomEncounters.RollForEvent();
    }
}
