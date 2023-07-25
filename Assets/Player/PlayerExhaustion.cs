using SD.ECS;
using SD.PathingSystem;
using UnityEngine;

public class PlayerExhaustion : MonoBehaviour
{
    private const int maxExhaustion = 100;
    [SerializeField] private int currentExhaustion;
    private GridPosition _gridPos;

    private void Start()
    {
        _gridPos = GetComponent<GridPosition>();
        _gridPos.onPositionChange += OnPositionChanged;

        currentExhaustion = maxExhaustion;
    }

    private void OnPositionChanged()
    {
        var newNode = Pathfinding.instance.GetNode(_gridPos.x, _gridPos.y);

        if (newNode != null && newNode.terrain != null)
        {
            currentExhaustion -= newNode.terrain.ExhaustionCost;
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
