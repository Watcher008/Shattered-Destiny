using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.ECS;
using SD.EventSystem;

public class GameManager : MonoBehaviour
{
    public const int pointsToAct = 1000;

    private static GameManager instance;

    [SerializeField] private float turnDelay = 0.1f;

    [SerializeField] private List<Actor> actors = new List<Actor>();
    [SerializeField] private Actor currentActor;

    [SerializeField] private GameEvent turnTickEvent;

    private bool runTurnCounter = true;

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
    public static void AddSentinel(Sentinel sentinel)
    {
        instance.actors.Remove(sentinel.Actor);
        instance.actors.Insert(0, sentinel.Actor);
        instance.currentActor = sentinel.Actor;
        instance.StartTurn();
    }
    
    public static void AddActor(Actor actor)
    {
        instance.actors.Add(actor);
    }

    public static void RemoveActor(Actor actor)
    {
        instance.actors.Remove(actor);
        if (instance.currentActor == actor) instance.GetNextActor();
    }
    #endregion

    #region - Turn Cycle -
    //Called from GameEventListener
    //Prevents the transitioning to the next actor's turn
    public void PauseTurnCycle()
    {
        runTurnCounter = false;

    }

    //Called from GameEventListener
    //Resumes the turn cycle and Transitions to next actor's turn
    public void ResumeTurnCycle()
    {
        runTurnCounter = true;
        TurnTransition();
    }

    private void TurnTransition()
    {
        if (!runTurnCounter) return;

        GetNextActor();
        StartTurn();
    }

    private void GetNextActor()
    {
        //Debug.Log("GetNextActor");
        while (FindAbleActor() == null)
        {
            Debug.Log("GetNextActor");
            ReplenishEnergy();
        }
        currentActor = FindAbleActor();
    }

    private Actor FindAbleActor()
    {
        //Debug.Log("FindAbleActor");
        for (int i = 0; i < actors.Count; i++)
        {
            if (ActorCanAct(actors[i])) return actors[i];
        }
        return null;
    }

    private bool ActorCanAct(Actor actor)
    {
        return actor.ActionPoints >= pointsToAct;
    }

    //Passes time by 1/10th of a round
    private void ReplenishEnergy()
    {
        for (int i = 0; i < actors.Count; i++)
        {
            actors[i].RegainActionPoints();
        }
        turnTickEvent.Invoke();
    }

    private void StartTurn()
    {
        //Debug.Log("StartTurn for " + currentActor.name);
        currentActor.IsTurn = true;

        if (!currentActor.Entity.IsSentient) Action.SkipAction(currentActor);
        else if (!currentActor.Entity.HasBehavior) Action.SkipAction(currentActor);
    }

    public static void EndTurn()
    {
        instance.currentActor.IsTurn = false;
        instance.StartCoroutine(instance.WaitForTurn());
    }

    private IEnumerator WaitForTurn()
    {
        yield return new WaitForSeconds(turnDelay);
        TurnTransition();
    }
    #endregion
}