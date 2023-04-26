using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SD.PathingSystem;
using SD.EventSystem;

public class PlayerLocomotion : MonoBehaviour
{
    private Pathfinding pathfinding;
    private Camera cam;
    private Vector2 mousePos;

    [SerializeField] private PlayerTravelData playerTravelData;
    [SerializeField] private GameEvent playerActionTakenEvent;
    [Space]

    //This is kind of ugly...
    [SerializeField] private InputActionProperty mousePosition;
    [SerializeField] private InputActionProperty leftMouseButton;
    [Space]
    [SerializeField] private InputActionProperty moveSouthWest;
    [SerializeField] private InputActionProperty moveSouth;
    [SerializeField] private InputActionProperty moveSouthEast;
    [SerializeField] private InputActionProperty moveWest;
    [SerializeField] private InputActionProperty moveEast;
    [SerializeField] private InputActionProperty wait;
    [SerializeField] private InputActionProperty moveNorthWest;
    [SerializeField] private InputActionProperty moveNorth;
    [SerializeField] private InputActionProperty moveNorthEast;

    public bool playerCanMove { get; private set; } = true;
    private Coroutine movementCoroutine;

    private void Start()
    {
        cam = Camera.main;
        pathfinding = Pathfinding.instance;
        playerTravelData.Init();
        SubscribeToInput();

        OccupyStartingNode();
        GetComponent<Entity>().onTurnStart += OnPlayerTurnStart;
        GetComponent<Entity>().onTurnEnd += OnPlayerTurnEnd;
    }

    private void OnDestroy()
    {
        UnsubscribeFromInput();
        GetComponent<Entity>().onTurnStart -= OnPlayerTurnStart;
        GetComponent<Entity>().onTurnEnd -= OnPlayerTurnEnd;
    }

    private void SubscribeToInput()
    {
        leftMouseButton.action.performed += i => OnLeftClick();
        mousePosition.action.performed += i => mousePos = i.ReadValue<Vector2>();

        moveSouthWest.action.performed += i => Move(1);
        moveSouth.action.performed += i => Move(2);
        moveSouthEast.action.performed += i => Move(3);

        moveWest.action.performed += i => Move(4);
        moveEast.action.performed += i => Move(6);

        moveNorthWest.action.performed += i => Move(7);
        moveNorth.action.performed += i => Move(8);
        moveNorthEast.action.performed += i => Move(9);

    }

    private void UnsubscribeFromInput()
    {
        leftMouseButton.action.performed -= i => OnLeftClick();
        mousePosition.action.performed -= i => mousePos = i.ReadValue<Vector2>();

        moveSouthWest.action.performed -= i => Move(1);
        moveSouth.action.performed -= i => Move(2);
        moveSouthEast.action.performed -= i => Move(3);

        moveWest.action.performed -= i => Move(4);
        moveEast.action.performed -= i => Move(6);

        moveNorthWest.action.performed -= i => Move(7);
        moveNorth.action.performed -= i => Move(8);
        moveNorthEast.action.performed -= i => Move(9);
    }

    private void OccupyStartingNode()
    {
        var currentNode = pathfinding.GetNode(transform.position);
        transform.position = currentNode.globalPosition;
        playerTravelData.OnPlayerEnterNode(currentNode);
    }

    public void OnPlayerTurnStart()
    {
        playerCanMove = true;
    }

    public void OnPlayerTurnEnd()
    {
        playerCanMove = false;
    }

    private void Move(int direction)
    {
        if (playerTravelData.PlayerIsMoving || !playerCanMove) return;

        int x = playerTravelData.CurrentPlayerNode.x;
        int y = playerTravelData.CurrentPlayerNode.y;
        switch (direction)
        {
            case 1:
                x--;
                y--;
                break;
            case 2:
                y--;
                break;
            case 3:
                x++;
                y--;
                break;
            case 4:
                x--;
                break;
            case 6:
                x++;
                break;
            case 7:
                x--;
                y++;
                break;
            case 8:
                y++;
                break;
            case 9:
                x++;
                y++;
                break;
        }

        var newNode = pathfinding.GetNode(x, y);
        if (newNode != null)
        {
            playerTravelData.OnPlayerStartMovement();
            transform.position = newNode.globalPosition;
            playerTravelData.OnPlayerEnterNode(pathfinding.GetNode(transform.position));
            playerTravelData.OnPlayerEndMovement();
        }

        playerActionTakenEvent?.Invoke();
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
