using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    private playerScript player; //for initial testing only
    
    private Grid grid;
    private Camera cam;

    [SerializeField] private InputActionProperty mousePosition;
    [SerializeField] private InputActionProperty leftMouseButton;
    private Vector3Int previousMousePos;
    private Vector2 mousePos;
    
    [Space]

    [SerializeField] private Tilemap terrainMap;
    [SerializeField] private Tilemap interactionMap;
    [SerializeField] private RuleTile hoverTile;

    private void Start()
    {
        player = FindObjectOfType<playerScript>();

        grid = GetComponent<Grid>();
        cam = Camera.main;

        leftMouseButton.action.performed += i => OnLeftClick();
        mousePosition.action.performed += i => mousePos = i.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        leftMouseButton.action.performed -= i => OnLeftClick();
        mousePosition.action.performed -= i => mousePos = i.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        HighlightTile();
    }

    private void HighlightTile()
    {
        var currentMousePos = grid.WorldToCell(cam.ScreenToWorldPoint(mousePos));
        if (currentMousePos != previousMousePos)
        {
            interactionMap.SetTile(previousMousePos, null);
            interactionMap.SetTile(currentMousePos, hoverTile);
        }
        previousMousePos = currentMousePos;
    }

    private void OnLeftClick()
    {
        var tilePosition = terrainMap.WorldToCell(cam.ScreenToWorldPoint(mousePos));
        tilePosition.z = 0;

        var tile = terrainMap.GetInstantiatedObject(tilePosition);
        if (tile != null && tile.TryGetComponent(out TerrainNode node))
        {
            Debug.Log("Moving to " + node.terrainType + " node in " + node.territory);
            player.SetPlayerDestination(grid.CellToWorld(tilePosition));
        }
    }
}