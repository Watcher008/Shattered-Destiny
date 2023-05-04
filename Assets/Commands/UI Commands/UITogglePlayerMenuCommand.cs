using UnityEngine;
using SD.EventSystem;

namespace SD.CommandSystem
{
    [CreateAssetMenu(menuName = "Command System/Toggle Inventory Command")]
    public class UITogglePlayerMenuCommand : CommandBase
    {
        private bool inventoryIsOpen = false;

        [Space]

        [SerializeField] private GameEvent openInventoryEvent;
        [SerializeField] private GameEvent closeInventoryEvent;

        protected override bool ExecuteCommand()
        {
            return ToggleInventory();
        }

        /// <summary>
        /// Toggle Inventory Menu.
        /// </summary>
        /// <returns>Command success result.</returns>
        private bool ToggleInventory()
        {
            Debug.Log("Pickup here.");

            //So I think that clicking any of the toolbar buttons should just open a single menu
            //that can be tabbed between all of the available options,
            //rather than having separate scenes and menus for all of them

            if (inventoryIsOpen)
            {
                closeInventoryEvent?.Invoke();
                inventoryIsOpen = false;
            }
            else
            {
                openInventoryEvent?.Invoke();
                inventoryIsOpen= true;
            }
            return true;
        }
    }
}