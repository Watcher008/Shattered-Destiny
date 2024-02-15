using UnityEngine;
using SD.EventSystem;
using SD.LocationSystem;

namespace SD.ECS
{
    public class LocationEncounter : MonoBehaviour
    {
        private LocationData location;

        [SerializeField] private PlayerLocationReference playerLocation;
        [SerializeField] private GameEvent playerLocationEncounterEvent;

        public void SetLocation(LocationData location)
        {
            this.location = location;
            GetComponent<SpriteRenderer>().sprite = location.type.sprite;
            GetComponent<GridPosition>().SetPosition(location.x, location.y);
        }

        private void SetLocationValues()
        {
            if (location == null) Debug.Log("Location null");
            if (location.type == null) Debug.Log("type null");
            GetComponent<SpriteRenderer>().sprite = location.type.sprite;
            GetComponent<GridPosition>().SetPosition(location.x, location.y);
        }

        public void OnPlayerVisitLocation()
        {
            playerLocation.playerLocation = location;
            playerLocationEncounterEvent?.Invoke();
        }
    }
}