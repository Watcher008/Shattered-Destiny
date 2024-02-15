using SD.ECS;
using SD.PathingSystem;
using UnityEngine;

public class PlayerExhaustion : MonoBehaviour
{
    private const int maxExhaustion = 100;
    [SerializeField] private int currentExhaustion;
    private MapCharacter _player;

    private void Start()
    {
        _player = GetComponent<MapCharacter>();
        _player.onPositionChange += OnPositionChanged;

        currentExhaustion = maxExhaustion;
    }

    private void OnPositionChanged()
    {
        if (_player.Node != null && _player.Node.Terrain != null)
        {
            currentExhaustion -= _player.Node.Terrain.ExhaustionCost;
        }
        else
        {
            Debug.Log("Node or terrain is null");
        }

        //subtract appropriate number from exhaustion
        //road/plains - 0
        //forest - 10
        //mountain - 20
        //water - 50
    }
}
