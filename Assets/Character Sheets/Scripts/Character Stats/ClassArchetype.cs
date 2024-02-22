using UnityEngine;

namespace SD.CharacterSystem
{
    [CreateAssetMenu(menuName = "Characters/Stats/Class Archetype")]
    public class ClassArchetype : ScriptableObject
    {
        [field: SerializeField] public string ClassName { get; private set; }
        [field: SerializeField] public AttributeReference[] ClassAttributes { get; private set; }
    }
}