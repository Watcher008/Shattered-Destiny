using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.Pathfinding;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    private Pathfinding pathfinding;
    private Camera cam;
    private Vector2 mousePos;

    [SerializeField] private InputActionProperty mousePosition;
    [SerializeField] private InputActionProperty leftMouseButton;

    [SerializeField] private float movementSpeed = 0.5f;
    public bool playerIsMoving { get; private set; } = false;
    public bool playerCanMove { get; private set; } = true;
    private Coroutine movementCoroutine;

    private void Start()
    {
        cam = Camera.main;
        pathfinding = Pathfinding.instance;
        leftMouseButton.action.performed += i => OnLeftClick();
        mousePosition.action.performed += i => mousePos = i.ReadValue<Vector2>();

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
    }

    private void OnLeftClick()
    {
        if (playerIsMoving || !playerCanMove) return;

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
        playerIsMoving = true;

        while (nodes.Count > 0)
        {
            float elapsedTime = 0;
            var endPos = nodes[0];
            var origin = transform.position;

            while (elapsedTime < movementSpeed)
            {
                transform.position = Vector3.Lerp(origin, endPos, elapsedTime / movementSpeed);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.position = endPos;
            nodes.RemoveAt(0);

            //Probably invoke some sort of global event when entering the new node
            //This would be to check if there is anything there to interact with
            yield return null;
        }

        playerIsMoving = false;
    }
}
