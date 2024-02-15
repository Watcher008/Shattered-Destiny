using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.ECS;
using SD.EventSystem;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private List<Actor> actors = new List<Actor>();

    [SerializeField] private TurnPhase currentPhase;

    private Actor _player;

    [SerializeField] private Actor currentActor;

    [SerializeField] private GameEvent turnTickEvent;

    private bool waitingForPlayer = false;
    private Coroutine playerTurnCoroutine;

    public static List<Actor> Actors => instance.actors;
    public static TurnPhase CurrentPhase => instance.currentPhase;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        currentPhase = TurnPhase.Player_Fast;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
    }

    #region - Actor Registration -    
    public static void AddActor(Actor actor)
    {
        instance.actors.Add(actor);
    }

    public static void RemoveActor(Actor actor)
    {
        instance.actors.Remove(actor);
    }
    #endregion

    #region - Turn Cycle -
    private void NextPhase()
    {
        // Cycle back to start if at end, otherwise iterate phase
        if (currentPhase == TurnPhase.NPC_Slow)
            currentPhase = TurnPhase.Player_Fast;
        else currentPhase++;

        switch (currentPhase)
        {
            case TurnPhase.Player_Fast:
                // Skip this phase if the player is on foot/slow
                if (_player.Speed == MovementSpeed.Slow) NextPhase();
                else
                {
                    // Wait for player to act
                    if (playerTurnCoroutine != null) StopCoroutine(playerTurnCoroutine);
                    playerTurnCoroutine = StartCoroutine(WaitForPlayerToAct());
                }
                break;
            case TurnPhase.NPC_Fast:
                // Iterate backwards through the list in case one is removed
                for (int i = actors.Count - 1; i >= 0; i--)
                {
                    if (actors[i] == _player) continue;
                    else if (actors[i].Speed == MovementSpeed.Slow) continue;
                    else actors[i].TakeAction();
                }
                NextPhase();
                break;
            case TurnPhase.Player_Slow:
                if (playerTurnCoroutine != null) StopCoroutine(playerTurnCoroutine);
                playerTurnCoroutine = StartCoroutine(WaitForPlayerToAct());
                break;
            case TurnPhase.NPC_Slow:
                for (int i = actors.Count - 1; i >= 0; i--)
                {
                    if (actors[i] == _player) continue;
                    else actors[i].TakeAction();
                }
                NextPhase();
                break;
        }
    }

    public static void EndPlayerTurn()
    {
        instance.waitingForPlayer = false;
    }

    private IEnumerator WaitForPlayerToAct()
    {
        while (waitingForPlayer)
        {
            yield return null;
        }
        NextPhase();
    }
    #endregion
}

// Player always gets to act before NPCs
// Player/NPCs on horseback get two movement actions
public enum TurnPhase
{
    Player_Fast,
    NPC_Fast,
    Player_Slow,
    NPC_Slow,
}

public enum MovementSpeed
{
    Fast,
    Slow,
}