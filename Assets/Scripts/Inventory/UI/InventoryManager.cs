using SD.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace SD.Inventories
{
    public class InventoryManager : MonoBehaviour
    {
        public const int CELL_SIZE = 100;

        public static InventoryManager Instance;

        private List<InventoryItem> spawnedItems = new();

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private ItemCodex _itemCodex;
        [SerializeField] private ItemGrid _playerGrid;

        [SerializeField] private ItemGrid _gridPrefab;
        [SerializeField] private InventoryElement _elementPrefab;

        private void ForTestingOnly()
        {
            Debug.LogWarning("Adding Test Items.");

            var boots = _itemCodex.GetItem("Boots");
            var armor = _itemCodex.GetItem("Armor");
            var armor2 = _itemCodex.GetItem("Steel Armor");

            _playerData.Inventory.TryFitItem(new InventoryItem(boots, Vector2Int.zero));
            _playerData.Inventory.TryFitItem(new InventoryItem(armor, Vector2Int.zero));
            _playerData.Inventory.TryFitItem(new InventoryItem(armor2, Vector2Int.zero));

            for (int i = 0; i < 13; i++)
            {
                var ration = _itemCodex.GetItem("Ration");
                _playerData.Inventory.TryFitItem(new InventoryItem(ration, Vector2Int.zero));
            }
        }

        private void Awake()
        {
            Instance = this;

            //ForTestingOnly();
        }

        private void Start()
        {
            WorldMapManager.Instance.onPauseInput?.Invoke();

            _playerGrid.SetValues(_playerData.Inventory);
        }

        private void OnDestroy()
        {
            WorldMapManager.Instance.onResumeInput?.Invoke();
        }

        /*private void ClearAllDisplays()
        {
            for (int i = _gridParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_gridParent.GetChild(i).gameObject);
            }
        }

        private void RefreshAllDisplays()
        {
            ClearAllDisplays();

            foreach (var inventory in _inventoryGroup.Inventories)
            {
                var itemGrid = Instantiate(_gridPrefab, _gridParent);
                itemGrid.SetValues(inventory);
                RefreshDisplay(itemGrid);
            }
        }*/

        public void RefreshDisplay(ItemGrid grid)
        {
            // Remove all existing UI_InventoryElements
            for (int i = grid.ElementParent.childCount - 1; i >= 0; i--)
            {
                Destroy(grid.ElementParent.GetChild(i).gameObject);
            }

            spawnedItems.Clear();

            for (int x = 0; x < grid.Inventory.Grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.Inventory.Grid.GetHeight(); y++)
                {
                    if (grid.Inventory.Grid.GetGridObject(x, y).HasItem())
                    {
                        spawnedItems.Remove(grid.Inventory.Grid.GetGridObject(x, y).GetItem());
                        spawnedItems.Add(grid.Inventory.Grid.GetGridObject(x, y).GetItem());
                    }
                }
            }

            for (int i = 0; i < spawnedItems.Count; i++)
            {
                var element = DragManager.Instance.GetDragElement(spawnedItems[i], grid.ElementParent);
                PlaceElement(element);
            }
        }


        private void PlaceElement(InventoryElement element)
        {
            var item = element.Item;

            element.Rect.anchoredPosition = (Vector2)item.Origin * CELL_SIZE;

            if (item.IsRotated)
            {
                var pivot = element.Rect.position + (Vector3.one * CELL_SIZE) / 2;
                element.Rect.RotateAround(pivot, Vector3.forward, -90f);
            }
        }

        public void DropItem(Inventory inventory, InventoryItem item)
        {
            // Gamepad hold button West to drop
            // Keyboard Ctrl + Click or Right Click to open context menu and drop
            inventory.RemoveItemAt(item.Origin, item.IsRotated);
        }
    }
}