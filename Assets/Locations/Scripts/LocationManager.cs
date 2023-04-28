using UnityEngine;
using SD.PathingSystem;
using SD.ECS;

namespace SD.LocationSystem
{
    public class LocationManager : MonoBehaviour
    {
        [SerializeField] private LocationDatabase database;

        [Space]

        [SerializeField] private Entity[] presetEntities;
        [SerializeField] private LocationPreset[] presetLocations;

        private void Awake() => database.Init();

        private void Start()
        {
            LoadPresets();
            LoadSavedValues();
        }

        private void LoadPresets()
        {
            foreach(var preset in presetLocations)
            {
                var entity = FindEntity(preset.name);
                entity.GetComponentBase<Position>().SetPosition(preset.X, preset.Y);
                entity.GetComponentBase<EntityRenderer>().SetSprite(preset.Type.sprite);
            }
        }

        private void LoadSavedValues()
        {
            //will also need to include IsDiscovered values for all presets

            //work this in later
        }

        private Entity FindEntity(string name)
        {
            for (int i = 0; i < presetEntities.Length; i++)
            {
                if (presetEntities[i].name == name) return presetEntities[i];
            }
            return null;
        }

        private void CreateNewLocation(string name, LocationType type, int x, int y)
        {
            var location = Instantiate(Resources.Load<GameObject>("Entities/location"));
            location.name = name;

            var entity = location.GetComponent<Entity>();
            entity.GetComponentBase<EntityRenderer>().SetSprite(type.sprite);
            entity.GetComponentBase<Position>().SetPosition(x, y);

        }
        private void foo()
        {
            var locations = database.GetAllLocations();

            for (int i = 0; i < locations.Count; i++)
            {
                var obj = new GameObject();
                obj.name = locations[i].Name;

                var renderer = obj.AddComponent<SpriteRenderer>();
                renderer.sortingOrder = -5;
                renderer.sprite = locations[i].type.sprite;

                var node = Pathfinding.instance.GetNode(locations[i].x, locations[i].y);

                node.SetLocation(locations[i]);
                obj.transform.position = node.worldPosition;
                obj.transform.SetParent(transform);
            }
        }
    }
}