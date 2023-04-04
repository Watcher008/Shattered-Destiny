using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerScript : MonoBehaviour
{

    public WorldManager worldManager;

    private float playerDefaultMovementSpeed = 5;
    public float playerMovementSpeed;
    public float movementModifier;



    public bool playerIsMoving = false;
    public bool movementModifierChanged = false;
    public bool playerSpeedChanged = false;
    
    
    
    private Vector2 targetPositionOnClick;
    private Rigidbody2D playerRb;






    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerMovementSpeed = playerDefaultMovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ModifyPlayerMovementSpeed();
        
    }

    private void ModifyPlayerMovementSpeed()
    {
        if (movementModifierChanged == true && playerSpeedChanged == false)
        {
            playerMovementSpeed = playerMovementSpeed * movementModifier;
            playerSpeedChanged = true;
        }
    }

    public void MovePlayerFixed()
    {
        if (playerIsMoving)
        {
            //calculate distance between player current location and target location
            float distance = Vector2.Distance(playerRb.position, targetPositionOnClick);
            
            if (distance <= 0.1f) //stop player when it reaches destination
            {
                playerRb.velocity = Vector2.zero;
                playerIsMoving = false;
                Debug.Log("Player has stopped moving");
            }
            else
            {
                //find out which direction the player needs to move towards
                Vector2 direction = (targetPositionOnClick - playerRb.position).normalized;

                 //move player to destination with playerMovementSpeed             
                playerRb.MovePosition(playerRb.position + direction * playerMovementSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void FixedUpdate()
    {

        MovePlayerFixed();
       
    }

    public void MovePlayer()
    {
        //player clicked and where did they click
        if (!playerIsMoving && Input.GetMouseButtonDown(0))
        {
            targetPositionOnClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //check if there is a collider at the clicked position
            Collider2D[] colliders = Physics2D.OverlapPointAll(targetPositionOnClick);
            foreach (Collider2D collider in colliders)
            {
                //checks if the collider is player's collider, if it is not player's collider or NotWalkable tagged collider player moves
                if (collider.gameObject != gameObject && !collider.CompareTag("NotWalkable"))
                {
                    playerIsMoving = true;
                    Debug.Log("Player is now moving");
                }
                else
                {
                    Debug.Log("No collider at clicked position, cannot move!");
                }

                
            }

        }
            //stop player when they press space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerIsMoving = false;
            playerRb.velocity = Vector2.zero;
            Debug.Log("Player has stopped moving");
        }
    }



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
            Debug.Log("Player movement speed: " + playerMovementSpeed);
            movementModifierChanged = true;
        }
        if (other.gameObject.CompareTag("Forest"))
        {
            movementModifier = 0.5f;
            Debug.Log("Player movement speed: " + playerMovementSpeed);
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
            playerMovementSpeed = playerDefaultMovementSpeed;
            movementModifierChanged = false;
            playerSpeedChanged = false;
        }




    }


}
