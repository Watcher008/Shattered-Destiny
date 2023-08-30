using UnityEngine;
using SD.EventSystem;
using SD.Primitives;

namespace SD.CommandSystem
{
    [CreateAssetMenu(menuName = "Command System/Toggle Inventory Command")]
    public class UITogglePlayerMenuCommand : CommandBase
    {
        [SerializeField] private IntReference _playerMenuStatusReference;

        [Space]

        [SerializeField] private GameEvent openPlayerMenuEvent;
        [SerializeField] private GameEvent closePlayerMenuEvent;

        protected override bool ExecuteCommand()
        {
            OnToggleCharacter();
            return true;
        }

        public void OnToggleCharacter()
        {
            if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Closed)
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Character;
                openPlayerMenuEvent.Invoke();
            }
            else if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Character)
            {
                CloseMenu();
            }
            else
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Character;
            }
        }

        public void OnToggleInventory()
        {
            if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Closed)
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Inventory;
                openPlayerMenuEvent.Invoke();
            }
            else if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Inventory)
            {
                CloseMenu();
            }
            else
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Inventory;
            }
        }

        public void OnToggleCompendium()
        {
            if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Closed)
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Compendium;
                openPlayerMenuEvent.Invoke();
            }
            else if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Compendium)
            {
                CloseMenu();
            }
            else
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Compendium;
            }
        }

        public void OnToggleJournal()
        {
            if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Closed)
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Journal;
                openPlayerMenuEvent.Invoke();
            }
            else if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Journal)
            {
                CloseMenu();
            }
            else
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Journal;
            }
        }

        public void OnToggleSkills()
        {
            if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Closed)
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Skills;
                openPlayerMenuEvent.Invoke();
            }
            else if (_playerMenuStatusReference.Value == (int)PlayerMenuStatus.Skills)
            {
                CloseMenu();
            }
            else
            {
                _playerMenuStatusReference.Value = (int)PlayerMenuStatus.Skills;
            }
        }

        private void CloseMenu()
        {
            closePlayerMenuEvent.Invoke();
        }
    }
}

public enum PlayerMenuStatus
{
    Closed,
    Character,
    Inventory,
    Compendium,
    Journal,
    Skills,
}