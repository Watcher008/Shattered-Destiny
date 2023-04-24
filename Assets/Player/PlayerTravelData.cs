using UnityEngine;
using SD.Pathfinding;
using SD.EventSystem;

[CreateAssetMenu(fileName = "Player Travel Data", menuName = "Player/Player Travel Data")]
public class PlayerTravelData : ScriptableObject
{
    [SerializeField] private GameEvent playerStartTravelEvent;
    [SerializeField] private GameEvent playerEnterNodeEvent;

    [Space]

    [Tooltip("The real-life time it takes the player to move one tile.")]
    [SerializeField] private float playerTimeToMove = 0.5f;

    [Tooltip("The in-game time it takes for the player to move one tile. Measured in hours/tile.")]
    [SerializeField] private int playerTravelSpeed = 1;
    public bool PlayerIsMoving { get; private set; }

    public PathNode CurrentPlayerNode { get; private set; }

    public float PlayerTimeToMove => playerTimeToMove;
    public int PlayerTravelSpeed => playerTravelSpeed;

    public void Init()
    {
        PlayerIsMoving = false;
    }

    public void OnPlayerStartMovement()
    {
        PlayerIsMoving = true;
        playerStartTravelEvent?.Invoke();
    }

    public void OnPlayerEndMovement()
    {
        PlayerIsMoving = false;
    }

    public void OnPlayerEnterNode(PathNode newNode)
    {
        CurrentPlayerNode = newNode;
        playerEnterNodeEvent?.Invoke();
    }

    private void OnValidate()
    {
        if (playerTravelSpeed < 1) playerTravelSpeed = 1;
    }
}