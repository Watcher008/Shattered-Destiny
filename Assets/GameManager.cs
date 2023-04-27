using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.EventSystem;
using SD.ECS;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float turnDelay = 0.1f;

    [SerializeField] private List<Entity> entities = new List<Entity>();
    [SerializeField] private Entity entityToAct;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #region - Turn Cycle -
    private void TurnTransition()
    {
        FindLowest();
        ReplenishEnergy();
        StartTurn();
    }

    private void FindLowest()
    {
        int value = int.MaxValue;
        Entity nextEntity = null;

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i].ActionPoints < value)
            {
                value = entities[i].ActionPoints;
                nextEntity = entities[i];
            }
        }

        entityToAct = nextEntity;
    }

    private void ReplenishEnergy()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].RegainEnergy(entityToAct.ActionPoints);
        }
    }

    private void StartTurn()
    {
        entityToAct.IsTurn = true;

        if (entityToAct.CompareTag("Player"))
        {
            //Do nothing, player doesn't get special treatment
        }
        else if (entityToAct.IsSentient)
        {
            //this will change once AI logic is in place
            Debug.Log("Passing turn.");
            Action.SkipAction(entityToAct); 
        }
    }

    public void EndTurn()
    {
        entityToAct.IsTurn = false;
        StartCoroutine(WaitForTurn());
    }

    private IEnumerator WaitForTurn()
    {
        yield return new WaitForSeconds(turnDelay);
        TurnTransition();
    }
    #endregion

    public void InsertEntity(Entity entity, int index)
    {
        entities.Insert(index, entity);

        if (entityToAct == null)
        {
            entityToAct = entity;
            StartTurn();
        }
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity(Entity entity)
    {
        entities.Remove(entity);
    }
}
