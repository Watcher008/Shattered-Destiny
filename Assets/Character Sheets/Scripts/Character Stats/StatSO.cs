using UnityEngine;

namespace SD.CharacterSystem
{
    public class StatSO : ScriptableObject
    {
        [Header("Base Stat Properties")]
        [SerializeField] private int _id;
        [SerializeField] private string _fullName;

        [Space]

        [SerializeField] private string _shortName;
        [SerializeField] private string _description;

        public int ID => _id;
        public string FullName => _fullName;
        public string ShortName => _shortName;
        public string Description => _description;
    }
}