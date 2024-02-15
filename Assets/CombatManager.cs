using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CombatManager : MonoBehaviour
{
    private const int initiativeDrop = 10;

    private static CombatManager instance;

    private List<Combatant> _combatants = new List<Combatant>();
    
    private void Awake()
    {
        instance = this;
    }

    public static void AddCombatant(Combatant combatant)
    {
        combatant.hasActed = false;
        instance.RollInitiative(combatant);
        // Add to end, no matter initiatve
        instance._combatants.Add(combatant);
    }

    public static void AddCombatants(List<Combatant> combatants)
    {
        // Have each combatant roll a new initiative
        for (int i = 0; i < combatants.Count; i++)
            AddCombatant(combatants[i]);

        // Re-order combatants to accomodate newcomers
        instance.SortByInitiative();
    }

    // Remove a combatant from battle. Either from retreat or death.
    public void OnCombatantRemoved(Combatant combatant)
    {
        _combatants.Remove(combatant);

        if (AllHostilesNeutralized()) OnPlayerVictory();
        else if (AllPlayersNeutralized()) OnPlayerDefeat();
    }

    #region - Initiative -
    private void RollInitiative(Combatant combatant)
    {
        // Roll a d6 (Random.Range excludes last number) and add speed
        combatant.Initiative = UnityEngine.Random.Range(1, 7) + 0;
    }

    /// <summary>
    /// Returns true if all combatants have acted
    /// </summary>
    private bool CheckForInitiativePass()
    {
        for (int i = 0; i < _combatants.Count; i++)
        {
            if (!_combatants[i].hasActed) return false;
        }
        return true;
    }

    private void OnIniativePass()
    {
        for (int i = 0; i < _combatants.Count; i++)
        {
            _combatants[i].Initiative = Mathf.Clamp(_combatants[i].Initiative - initiativeDrop, 0, int.MaxValue);
            if (_combatants[i].Initiative > 0) _combatants[i].hasActed = false;
        }

        SortByInitiative();
    }

    /// <summary>
    /// Sorts combatants by Initiative.
    /// </summary>
    private void SortByInitiative()
    {
        // Sort by player status first so that players with same initiative go before enemies
        _combatants.OrderBy(combatant => combatant.isPlayerControlled).ThenBy(a => a.Initiative);
    }
    #endregion

    private void OnStartTurn(Combatant combatant)
    {
        combatant.isTurn = true;

        // IDK some flashy stuff

        if (combatant.isPlayerControlled)
        {

        }
        else
        {

        }
    }

    private void OnEndTurn(Combatant combatant)
    {
        combatant.isTurn = false;
        combatant.Initiative -= initiativeDrop;
        SortByInitiative();

        // Starts the turn of the next highest initiative combatant
        OnStartTurn(_combatants[0]);
    }

    #region - End State -
    /// <summary>
    /// Returns true if all hostile characters have been defeated.
    /// </summary>
    private bool AllHostilesNeutralized()
    {
        for (int i = 0; i < _combatants.Count; i++)
        {
            if (_combatants[i].isHostile) return false;
        }
        return true;
    }

    /// <summary>
    /// Returns true if all player-controlled characters have been defeated.
    /// </summary>
    private bool AllPlayersNeutralized()
    {
        for (int i = 0; i < _combatants.Count; i++)
        {
            if (_combatants[i].isPlayerControlled) return false;
        }
        return true;
    }

    private void OnPlayerVictory()
    {
        Debug.Log("Player is Victorious!");
        // Grant XP
        // Grant Loot
        // Return to previous scene
    }

    private void OnPlayerDefeat()
    {
        Debug.Log("Player has been Defeated!");
        // Game Over? Or time lost?
    }
    #endregion
}
