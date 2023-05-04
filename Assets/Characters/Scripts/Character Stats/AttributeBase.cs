using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Stats/Attribute")]
    public class AttributeBase : ScriptableObject
    {
        [field: SerializeField] public string FullName { get; private set; }
        [field: SerializeField] public string ShortName { get; private set; }

        [field: SerializeField] public bool PrimaryAttribute { get; private set; }

        [field: SerializeField] public string Description { get; private set; }
    }
}