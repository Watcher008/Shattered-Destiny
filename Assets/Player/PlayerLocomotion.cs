using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SD.PathingSystem;

public class PlayerLocomotion : MonoBehaviour
{
    private Pathfinding pathfinding;
    private Camera cam;
    private Vector2 mousePos;

    [SerializeField] private PlayerTravelData playerTravelData;

    [Space]

    [SerializeField] private InputActionProperty mousePosition;
    [SerializeField] private InputActionProperty leftMouseButton;

    public bool playerCanMove { get; private set; } = true;
    private Coroutine movementCoroutine;

    private void Start()
    {
        cam = Camera.main;
        pathfinding = Pathfinding.instance;
        leftMouseButton.action.performed += i => OnLeftClick();
        mousePosition.action.performed += i => mousePos = i.ReadValue<Vector2>();
        playerTravelData.Init();

        OccupyStartingNode();
    }

    private void OnDestroy()
    {
        leftMouseButton.action.performed -= i => OnLeftClick();
        mousePosition.action.performed -= i => mousePos = i.ReadValue<Vector2>();
    }

    private void OccupyStartingNode()
    {
        var currentNode = pathfinding.GetNode(transform.position);
        transform.position = pathfinding.GetNodePosition(currentNode.x, currentNode.y);
        playerTravelData.OnPlayerEnterNode(currentNode);
    }

    private void OnLeftClick()
    {
        if (playerTravelData.PlayerIsMoving || !playerCanMove) return;

        var clickPosition = cam.ScreenToWorldPoint(mousePos);
        var endNode = pathfinding.GetNode(clickPosition);
        if (endNode == null) return;

        if (endNode.terrain != null && !endNode.terrain.CanTravelOnFoot)
        {
            Debug.Log("Cannot move to this node.");
            return;
        }

        var path = pathfinding.FindVectorPath(transform.position, clickPosition);
        SetPlayerPath(path);
    }

    public void SetPlayerPath(List<Vector3> pathNodes)
    {
        if (pathNodes[0] == transform.position) pathNodes.RemoveAt(0);

        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        movementCoroutine = StartCoroutine(FollowNodePath(pathNodes));
    }

    private IEnumerator FollowNodePath(List<Vector3> nodes)
    {
        playerTravelData.OnPlayerStartMovement();

        while (nodes.Count > 0)
        {
            float elapsedTime = 0;
            float timeToMove = playerTravelData.PlayerTimeToMove;
            var endPos = nodes[0];
            var origin = transform.position;

            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(origin, endPos, elapsedTime / timeToMove);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.position = endPos;

            playerTravelData.OnPlayerEnterNode(pathfinding.GetNode(transform.position));

            nodes.RemoveAt(0);
            yield return null;
        }

        playerTravelData.OnPlayerEndMovement();
    }
}
