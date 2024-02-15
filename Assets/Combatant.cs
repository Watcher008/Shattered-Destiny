using UnityEngine;

public class Combatant : MonoBehaviour
{
    public bool isHostile;
    public bool isPlayerControlled;

    public int Initiative;
    public bool isTurn = false;
    public bool hasActed = false;
}
