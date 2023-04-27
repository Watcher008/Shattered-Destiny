using UnityEngine;
using UnityEngine.InputSystem;
using SD.ECS;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private Entity playerEntity;
    private Locomotion locomotion;
    private AutoPathing autoPathing;

    private void OnEnable()
    {
        CheckForComponents();
        SubscribeToInput();
    }

    private void OnDisable()
    {
        UnsubscribeFromInput();
    }

    private void CheckForComponents()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();

        if (playerEntity == null) playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        if (locomotion == null) locomotion = playerEntity.GetComponent<Locomotion>();
        if (autoPathing == null) autoPathing = playerEntity.GetComponent<AutoPathing>();
    }

    private void SubscribeToInput()
    {
        playerInput.actions["SouthWest"].performed += i => MovePlayer(-Vector2Int.one);
        playerInput.actions["South"].performed += i => MovePlayer(Vector2Int.down);
        playerInput.actions["SouthEast"].performed += i => MovePlayer(Vector2Int.down + Vector2Int.right);

        playerInput.actions["West"].performed += i => MovePlayer(Vector2Int.left);
        playerInput.actions["East"].performed += i => MovePlayer(Vector2Int.right);

        playerInput.actions["NorthWest"].performed += i => MovePlayer(Vector2Int.up + Vector2Int.left);
        playerInput.actions["North"].performed += i => MovePlayer(Vector2Int.up);
        playerInput.actions["NorthEast"].performed += i => MovePlayer(Vector2Int.one);

        playerInput.actions["Wait"].performed += i => Action.SkipAction(playerEntity);

        playerInput.actions["LMB"].performed += i => AutoPathPlayer();
        playerInput.actions["RMB"].performed += i => CancelAutoPath();
    }

    private void UnsubscribeFromInput()
    {
        playerInput.actions["SouthWest"].performed -= i => MovePlayer(-Vector2Int.one);
        playerInput.actions["South"].performed -= i => MovePlayer(Vector2Int.down);
        playerInput.actions["SouthEast"].performed -= i => MovePlayer(Vector2Int.down + Vector2Int.right);

        playerInput.actions["West"].performed -= i => MovePlayer(Vector2Int.left);
        playerInput.actions["East"].performed -= i => MovePlayer(Vector2Int.right);

        playerInput.actions["NorthWest"].performed -= i => MovePlayer(Vector2Int.up + Vector2Int.left);
        playerInput.actions["North"].performed -= i => MovePlayer(Vector2Int.up);
        playerInput.actions["NorthEast"].performed -= i => MovePlayer(Vector2Int.one);

        playerInput.actions["Wait"].performed -= i => Action.SkipAction(playerEntity);

        playerInput.actions["LMB"].performed -= i => AutoPathPlayer();
        playerInput.actions["RMB"].performed -= i => CancelAutoPath();
    }

    private void MovePlayer(Vector2Int direction)
    {
        if (playerEntity.IsTurn) Action.MovementAction(locomotion, direction);
    }

    private void AutoPathPlayer()
    {
        if (!playerEntity.IsTurn) return;

        var pos = Camera.main.ScreenToWorldPoint(playerInput.actions["Mouse Position"].ReadValue<Vector2>());
        var node = SD.PathingSystem.Pathfinding.instance.GetNode(pos);
        if (node == null) return;

        autoPathing.SetAutoPathTarget(node.x, node.y);
    }

    private void CancelAutoPath()
    {
        autoPathing.CancelAutoPathing();
    }
}