using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.ECS;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private float turnDelay = 0.1f;

    [SerializeField] private List<Actor> actors = new List<Actor>();
    [SerializeField] private Actor currentActor;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #region - Actor Registration -
    public static void AddActor(Actor actor)
    {
        instance.AddNewActor(actor);
    }

    private void AddNewActor(Actor actor)
    {
        actors.Add(actor);
        if (currentActor == null)
        {
            currentActor = actor;
            StartTurn();
        }
    }

    public static void InsertActor(Actor actor, int index)
    {
        instance.InsertNewActor(actor, index);
    }

    private void InsertNewActor(Actor actor, int index)
    {
        actors.Insert(index, actor);
        if (currentActor == null)
        {
            currentActor = actor;
            StartTurn();
        }
    }

    public static void RemoveActor(Actor actor)
    {
        instance.RemoveOldActor(actor);
    }

    private void RemoveOldActor(Actor actor)
    {
        actors.Remove(actor);
        if (currentActor == actor) GetNextActor();
    }
    #endregion

    #region - Turn Cycle -
    private void TurnTransition()
    {
        GetNextActor();
        ReplenishEnergy();
        StartTurn();
    }

    private void GetNextActor()
    {
        int value = int.MaxValue;
        Actor nextActor = null;

        for (int i = 0; i < actors.Count; i++)
        {
            if (actors[i].ActionPoints < value)
            {
                value = actors[i].ActionPoints;
                nextActor = actors[i];
            }
        }

        currentActor = nextActor;
    }

    private void ReplenishEnergy()
    {
        int points = currentActor.ActionPoints;
        for (int i = 0; i < actors.Count; i++)
        {
            actors[i].RegainEnergy(points);
        }
    }

    private void StartTurn()
    {
        currentActor.IsTurn = true;

        if (!currentActor.entity.IsSentient) Action.SkipAction(currentActor);
        else if (!currentActor.entity.HasBehavior) Action.SkipAction(currentActor);
    }

    public static void EndTurn()
    {
        instance.EndActorTurn();
    }

    private void EndActorTurn()
    {
        currentActor.IsTurn = false;
        StartCoroutine(WaitForTurn());
    }

    private IEnumerator WaitForTurn()
    {
        yield return new WaitForSeconds(turnDelay);
        TurnTransition();
    }
    #endregion
}