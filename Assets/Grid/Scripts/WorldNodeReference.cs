using UnityEngine;
using SD.PathingSystem;

[CreateAssetMenu(fileName = "Node Reference", menuName = "Scriptable Objects/World Node Reference")]
public class WorldNodeReference : ScriptableObject
{
    public PathNode NodeReference { get; set; }
}