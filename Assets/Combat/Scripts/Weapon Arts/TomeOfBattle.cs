using UnityEngine;

namespace SD.Combat.WeaponArts
{
    /// <summary>
    /// A class to hold reference to all weapon arts.
    /// </summary>
    [CreateAssetMenu(menuName = "Combat/Weapon Arts/Tome Of Battle")]
    public class TomeOfBattle : ScriptableObject
    {
        [SerializeField] private WeaponArt[] _swordArts;
        [SerializeField] private WeaponArt[] _shieldArts;
        [SerializeField] private WeaponArt[] _warhammerArts;
        [SerializeField] private WeaponArt[] _bowArts;
        [SerializeField] private WeaponArt[] _staffArts;
        [SerializeField] private WeaponArt[] _bookArts;


        public WeaponArt GetRandom()
        {
            int school = Random.Range(1, System.Enum.GetNames(typeof(WeaponTypes)).Length);
            switch (school)
            {
                case (int)WeaponTypes.Sword:
                    return _swordArts[Random.Range(0, _swordArts.Length)];
                case (int)WeaponTypes.Shield:
                    return _swordArts[Random.Range(0, _shieldArts.Length)];
                case (int)WeaponTypes.Warhammer:
                    return _swordArts[Random.Range(0, _warhammerArts.Length)];
                case (int)WeaponTypes.Bow:
                    return _swordArts[Random.Range(0, _bowArts.Length)];
                case (int)WeaponTypes.Staff:
                    return _swordArts[Random.Range(0, _staffArts.Length)];
                case (int)WeaponTypes.Book:
                    return _swordArts[Random.Range(0, _bookArts.Length)];
                default: return null;
            }
        }
    }
}