using UnityEngine;
using UnityEngine.InputSystem;
using SD.ECS;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private Entity playerEntity;
    private Actor playerActor;
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

    private void OnDestroy()
    {
        //Debug.LogWarning("Input Destroyed");
    }

    private void CheckForComponents()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();

        if (playerEntity == null) playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        playerEntity.HasBehavior = true;

        if (playerActor == null) playerActor = playerEntity.GetComponent<Actor>();
        if (locomotion == null) locomotion = playerEntity.GetComponent<Locomotion>();
        if (autoPathing == null) autoPathing = playerEntity.GetComponent<AutoPathing>();
    }

    private void SubscribeToInput()
    {
        playerInput.actions["SouthWest"].performed += _ => MovePlayer(-Vector2Int.one);
        playerInput.actions["South"].performed += _ => MovePlayer(Vector2Int.down);
        playerInput.actions["SouthEast"].performed += _ => MovePlayer(Vector2Int.down + Vector2Int.right);

        playerInput.actions["West"].performed += _ => MovePlayer(Vector2Int.left);
        playerInput.actions["East"].performed += _ => MovePlayer(Vector2Int.right);

        playerInput.actions["NorthWest"].performed += _ => MovePlayer(Vector2Int.up + Vector2Int.left);
        playerInput.actions["North"].performed += _ => MovePlayer(Vector2Int.up);
        playerInput.actions["NorthEast"].performed += _ => MovePlayer(Vector2Int.one);

        playerInput.actions["Wait"].performed += _ => Action.SkipAction(playerActor);

        playerInput.actions["LMB"].performed += _ => AutoPathPlayer();
        playerInput.actions["RMB"].performed += _ => CancelAutoPath();
    }

    private void UnsubscribeFromInput()
    {
        playerInput.actions["SouthWest"].performed -= _ => MovePlayer(-Vector2Int.one);
        playerInput.actions["South"].performed -= _ => MovePlayer(Vector2Int.down);
        playerInput.actions["SouthEast"].performed -= _ => MovePlayer(Vector2Int.down + Vector2Int.right);

        playerInput.actions["West"].performed -= _ => MovePlayer(Vector2Int.left);
        playerInput.actions["East"].performed -= _ => MovePlayer(Vector2Int.right);

        playerInput.actions["NorthWest"].performed -= _ => MovePlayer(Vector2Int.up + Vector2Int.left);
        playerInput.actions["North"].performed -= _ => MovePlayer(Vector2Int.up);
        playerInput.actions["NorthEast"].performed -= _ => MovePlayer(Vector2Int.one);

        playerInput.actions["Wait"].performed -= _ => Action.SkipAction(playerActor);

        playerInput.actions["LMB"].performed -= _ => AutoPathPlayer();
        playerInput.actions["RMB"].performed -= _ => CancelAutoPath();
    }

    private void MovePlayer(Vector2Int direction)
    {
        if (playerActor.IsTurn) Action.MovementAction(locomotion, direction);
    }

    private void AutoPathPlayer()
    {
        //I *think* this should stop the player from clicking through UI objects
        //If that's not the case, the next best option may be to also constantly check
        //What the cursor is hovering over (or if it's over a UI element, and handle that accordingly
        if (EventSystem.current.currentInputModule != null) return;
        Debug.Log("No UI here.");
        if (!playerActor.IsTurn) return;

        var pos = Camera.main.ScreenToWorldPoint(playerInput.actions["Mouse Position"].ReadValue<Vector2>());
        var node = SD.PathingSystem.Pathfinding.instance.GetNode(pos);
        if (node == null) return;

        autoPathing.SetAutoPathTarget(node.X, node.Y);
    }

    private void CancelAutoPath()
    {
        autoPathing.CancelAutoPathing();
    }
}