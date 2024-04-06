using System.Collections.Generic;
using UnityEngine;
using SD.Combat.WeaponArts;

namespace SD.Characters
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Weapon Data", fileName = "Player Weapon Data")]
    public class PlayerWeaponData : ScriptableObject
    {
        private readonly int[] _slotsPerTier = { 2, 4, 6, 9};
        private int[] _weaponTiers;

         private WeaponTypes _rightHand = WeaponTypes.Sword;
        private WeaponTypes _leftHand = WeaponTypes.Shield;

        public WeaponTypes RightHand => _rightHand;
        public WeaponTypes LeftHand => _leftHand;

        private Dictionary<WeaponTypes, List<WeaponArt>> _knownWeaponArts;

        private WeaponArt[] _rightHandWeaponArts;
        private WeaponArt[] _leftHandWeaponArts;

        public void Init()
        {
            // first will go unused... oh well
            _weaponTiers = new int[System.Enum.GetNames(typeof(WeaponTypes)).Length];

            _knownWeaponArts = new Dictionary<WeaponTypes, List<WeaponArt>>();

            // Skip first because it's None
            for (int i = 1; i < System.Enum.GetNames(typeof(WeaponTypes)).Length; i++)
            {
                _knownWeaponArts.Add((WeaponTypes)i, new List<WeaponArt>());
            }
        }

        public void SetWeapon(WeaponTypes weapon, Hand hand)
        {
            if (weapon == WeaponTypes.None) return;

            // Ignore repeat values
            if (hand == Hand.Right && _rightHand == weapon) return;
            else if (hand == Hand.Left && _leftHand == weapon) return;

            // Set weapon
            if (hand == Hand.Right) _rightHand = weapon;
            else _leftHand = weapon;

            // If two handed, empty other hand
            if (IsTwoHanded(weapon))
            {
                if (hand == Hand.Right) _leftHand = WeaponTypes.None;
                else _rightHand = WeaponTypes.None;
            }
            // Handle equipping a one-handed when already equipped a two handed
            else if (hand == Hand.Right && (IsTwoHanded(_leftHand) || _leftHand == WeaponTypes.None))
            {
                if (weapon == WeaponTypes.Sword) _leftHand = WeaponTypes.Shield;
                else _leftHand = WeaponTypes.Sword;
            }
            else if (hand == Hand.Left && (IsTwoHanded(_rightHand) || _rightHand == WeaponTypes.None))
            {
                if (weapon == WeaponTypes.Sword) _rightHand = WeaponTypes.Shield;
                else _rightHand = WeaponTypes.Sword;
            }

            // Handle wielding same weapon twice, default to sword or shield
            if (_rightHand == _leftHand)
            {
                if (hand == Hand.Right)
                {
                    if (weapon == WeaponTypes.Sword) _leftHand = WeaponTypes.Shield;
                    else _leftHand = WeaponTypes.Sword;
                }
                else
                {
                    if (weapon == WeaponTypes.Sword) _rightHand = WeaponTypes.Shield;
                    else _rightHand = WeaponTypes.Sword;
                }
            }
            
            UpdateWeaponArts();
        }

        private bool IsTwoHanded(WeaponTypes weapon)
        {
            if (weapon == WeaponTypes.Warhammer) return true;
            if (weapon == WeaponTypes.Staff) return true;
            if (weapon == WeaponTypes.Bow) return true;
            return false;
        }

        private void UpdateWeaponArts()
        {
            _rightHandWeaponArts = null;
            _leftHandWeaponArts = null;

            if (_rightHand != WeaponTypes.None)
            {
                _rightHandWeaponArts = new WeaponArt[GetWeaponArtSlotCount(_rightHand)];

                // Fill in with first from list
            }
            if (_leftHand != WeaponTypes.None)
            {
                _leftHandWeaponArts = new WeaponArt[GetWeaponArtSlotCount(_leftHand)];

                // Fill in with first from list
            }

            // Note that I could (should) also have a stored list of the previously used weapon arts for this weapon
        }

        private int GetWeaponArtSlotCount(WeaponTypes weapon)
        {
            // I could one-line this but it's kinda ugly and harms readability
            int weaponTier = _weaponTiers[(int)weapon];
            return _slotsPerTier[weaponTier];
        }
    }
}