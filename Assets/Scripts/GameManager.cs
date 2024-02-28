using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.EventSystem;
using SD.LocationSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private List<MapCharacter> actors = new List<MapCharacter>();

    private TurnPhase currentPhase;

    private MapCharacter _player;

    [SerializeField] private GameEvent newRoundEvent;

    private bool waitingForPlayer = false;
    private Coroutine playerTurnCoroutine;

    public static List<MapCharacter> Actors => instance.actors;
    public static TurnPhase CurrentPhase => instance.currentPhase;

    private GameEvent locationEncounterEvent;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        currentPhase = TurnPhase.Player_Fast;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MapCharacter>();
    }

    #region - Actor Registration -    
    public static void AddActor(MapCharacter actor)
    {
        instance.actors.Add(actor);
        if (actor.TryGetComponent<PlayerExhaustion>(out _)) // Is player present?
        {
            instance.currentPhase = TurnPhase.NPC_Slow;
            instance.NextPhase();
        }
    }

    public static void RemoveActor(MapCharacter actor)
    {
        instance.actors.Remove(actor);
    }
    #endregion

    #region - Turn Cycle -
    private void NextPhase()
    {
        // Cycle back to start if at end, otherwise iterate phase
        if (currentPhase == TurnPhase.NPC_Slow)
        {
            currentPhase = TurnPhase.Player_Fast;
            newRoundEvent?.Invoke();
        }
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
                //Debug.Log("Check for collisions.");
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
                //Debug.Log("Check for collisions.");
                NextPhase();
                break;
        }
    }

    /// <summary>
    /// Called after the player has taken an action.
    /// </summary>
    public static void EndPlayerTurn()
    {
        instance.waitingForPlayer = false;
    }

    private IEnumerator WaitForPlayerToAct()
    {
        waitingForPlayer = true;
        while (waitingForPlayer)
        {
            yield return null;
        }
        if (TryGetActorCollision(_player, out MapCharacter character))
        {
            BeginCombatEncounter(character);
        }
        else if (LocationManager.TryGetLocation(_player.Node, out var location))
        {
           BeginLocationEncounter(location);
        }
        else NextPhase();
    }
    #endregion

    private bool TryGetActorCollision(MapCharacter origin, out MapCharacter character)
    {
        character = null;
        if (origin == null || origin.Node == null) return false;
        foreach(var actor in actors)
        {
            if (actor == origin) continue;
            if (actor.Node == origin.Node)
            {
                character = actor;
                return true;
            }
        }
        return false;
    }


    #region - Encounters - 
    private void BeginLocationEncounter(MapLocation location)
    {
        print($"Player arrived at {location.gameObject.name}.");
    }

    // Called by GameEventListener on object
    public void EndLocationEncounter()
    {
        NextPhase();
    }

    private void BeginCombatEncounter(MapCharacter character)
    {
        print($"Player encountered {character.gameObject.name}.");
    }

    public void EndCombatEncounter()
    {
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