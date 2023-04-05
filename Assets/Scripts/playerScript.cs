using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerScript : MonoBehaviour
{
    public WorldManager worldManager;

    [SerializeField] private float movementSpeed = 0.5f;
    public bool playerIsMoving { get; private set; } = false;
    private Coroutine movementCoroutine;

    /*
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //WHERE IS PLAYER
        switch (other.gameObject.tag)
        {
            case "Land1Borders":
                Debug.Log("Player is in land 1");
                break;
            case "Land2Borders":
                Debug.Log("Player is in land 2");
                break;
            case "Land3Borders":
                Debug.Log("Player is in land 3");
                break;
            case "Land4Borders":
                Debug.Log("Player is in land 4");
                break;
            default:
                break;
        }

        //ROUGH TERRAINS
        if (other.gameObject.CompareTag("Mountain"))
        {
            movementModifier = 0.3f;
            Debug.Log("Player movement speed: " + movementSpeed);
            movementModifierChanged = true;
        }
        if (other.gameObject.CompareTag("Forest"))
        {
            movementModifier = 0.5f;
            Debug.Log("Player movement speed: " + movementSpeed);
            movementModifierChanged = true;
        }


        //NOT WALKABLE TERRAIN
        if (other.gameObject.CompareTag("NotWalkable"))
        {
            playerIsMoving = false;
            playerRb.velocity = Vector2.zero;
            Debug.Log("Player has stopped moving");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //PLAYER EXITING ROUGH TERRAIN
        if (other.gameObject.CompareTag("Mountain") || other.gameObject.CompareTag("Forest"))
        {
            movementModifier = 1.0f;
            movementSpeed = playerDefaultMovementSpeed;
            movementModifierChanged = false;
            playerSpeedChanged = false;
        }
    }
    */

    public void SetPlayerDestination(Vector3 destination)
    {
        if (playerIsMoving) return;

        if (movementCoroutine != null) StopCoroutine(movementCoroutine);
        movementCoroutine = StartCoroutine(MovePlayerCoroutine(destination));
    }

    private IEnumerator MovePlayerCoroutine(Vector3 destination)
    {
        playerIsMoving = true;
        destination.z = transform.position.z;

        while(Vector2.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destination;
        playerIsMoving = false;
    }
}
