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
        public int[] WeaponTiers => _weaponTiers;

        #region - Weapons -
        private WeaponTypes _rightHand;
        private WeaponTypes _leftHand;
        public WeaponTypes RightHand => _rightHand;
        public WeaponTypes LeftHand => _leftHand;
        #endregion

        private Dictionary<WeaponTypes, List<WeaponArt>> _knownWeaponArts;
        public Dictionary<WeaponTypes, List<WeaponArt>> KnownWeaponArts => _knownWeaponArts;

        private WeaponArt[] _rightHandWeaponArts;
        private WeaponArt[] _leftHandWeaponArts;

        public WeaponArt[] RightHandArts => _rightHandWeaponArts;
        public WeaponArt[] LeftHandArts => _leftHandWeaponArts;

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

            _rightHandWeaponArts = null;
            _leftHandWeaponArts = null;
            SetWeapon(WeaponTypes.Sword, Hand.Right);
            UpdateWeaponArts();
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
                for (int i = 0; i < _rightHandWeaponArts.Length; i++)
                {
                    if (i >= _knownWeaponArts[_rightHand].Count) break;

                    _rightHandWeaponArts[i] = _knownWeaponArts[_rightHand][i];
                }
            }
            if (_leftHand != WeaponTypes.None)
            {
                _leftHandWeaponArts = new WeaponArt[GetWeaponArtSlotCount(_leftHand)];

                // Fill in with first from list
                for (int i = 0; i < _leftHandWeaponArts.Length; i++)
                {
                    if (i >= _knownWeaponArts[_leftHand].Count) break;

                    _leftHandWeaponArts[i] = _knownWeaponArts[_leftHand][i];
                }
            }

            // Note that I could (should) also have a stored list of the previously used weapon arts for this weapon
        }

        private int GetWeaponArtSlotCount(WeaponTypes weapon)
        {
            // I could one-line this but it's kinda ugly and harms readability
            int weaponTier = _weaponTiers[(int)weapon];
            return _slotsPerTier[weaponTier];
        }

        public bool LearnArt(WeaponArt art)
        {
            if (art == null) return false;

            if (_knownWeaponArts[art.Type].Contains(art))
            {
                Debug.Log("This art is already known");
                return false;
            }

            _knownWeaponArts[art.Type].Add(art);

            // Auto-equip in any empty slots
            if (art.Type == _rightHand)
            {
                for (int i = 0; i < _rightHandWeaponArts.Length; i++)
                {
                    if (_rightHandWeaponArts[i] == null)
                    {
                        _rightHandWeaponArts[i] = art;
                        break;
                    }
                }
            }
            else if (art.Type == _leftHand)
            {
                for (int i = 0; i < _leftHandWeaponArts.Length; i++)
                {
                    if (_leftHandWeaponArts[i] == null)
                    {
                        _leftHandWeaponArts[i] = art;
                        break;
                    }
                }
            }

            return true;
        }

        public void SetArt(WeaponArt art, Hand hand, int index)
        {
            if (art == null) return;
            if (hand == Hand.Right)
            {
                if (_rightHand != art.Type) return;
                if (index >= _rightHandWeaponArts.Length) return;
                _rightHandWeaponArts[index] = art;
            }
            else
            {
                if (_leftHand != art.Type) return;
                if (index >= _leftHandWeaponArts.Length) return;
                _leftHandWeaponArts[index] = art;
            }
        }
    }
}

public enum Hand { Right, Left };

public enum WeaponTypes
{
    None,
    Sword,
    Shield,
    Warhammer,
    Bow,
    Staff,
    Book
}