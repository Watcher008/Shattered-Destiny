using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.EventSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float turnDelay = 0.1f;
    [SerializeField] private bool isPlayerTurn = true;

    public bool IsPlayerTurn => isPlayerTurn;

    [SerializeField] private int entityNum = 0;
    [SerializeField] private List<Entity> entities = new List<Entity>();

    [SerializeField] private GameEvent playerTurnStart;
    [SerializeField] private GameEvent playerTurnEnd;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void StartTurn()
    {
        if (entities[entityNum].GetComponent<PlayerLocomotion>()) isPlayerTurn = true;
        else if (entities[entityNum].IsSentient) Action.SkipAction(entities[entityNum]); //this will change once AI logic is in place
    }

    //currently not a huge fan of calling getcomponent on every pass
    //I can just have a bool on each entity to toggle that
    //then for the player specifically, set up an event
    public void EndTurn()
    {
        if (entities[entityNum].GetComponent<PlayerLocomotion>())
        {
            isPlayerTurn = false;
            playerTurnEnd?.Invoke();
        }

        if (entityNum == entities.Count - 1)
        {
            //Start of new round
            entityNum = 0;
        }
        else
        {
            //start next turn
            entityNum++;
        }

        StartCoroutine(WaitForTurn());
    }

    private IEnumerator WaitForTurn()
    {
        yield return new WaitForSeconds(turnDelay);
        StartTurn();
    }

    public void InsertEntity(Entity entity, int index)
    {
        entities.Insert(index, entity);
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }
}
