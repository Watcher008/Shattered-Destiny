using UnityEngine;
using SD.PathingSystem;

namespace SD.LocationSystem
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private LocationDatabase database;

        private void Awake() => database.Init();

        private void Start()
        {
            var locations = database.GetAllLocations();

            for (int i = 0; i < locations.Count; i++)
            {
                var obj = new GameObject();
                obj.name = locations[i].name;

                var renderer = obj.AddComponent<SpriteRenderer>();
                renderer.sortingOrder = -5;
                renderer.sprite = locations[i].types.sprite;

                var node = Pathfinding.instance.GetNode(locations[i].x, locations[i].y);

                node.SetLocation(locations[i]);
                obj.transform.position = node.globalPosition;
                obj.transform.SetParent(transform);
            }
        }
    }
}