using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public delegate void OnTurnChangeCallback();
    public OnTurnChangeCallback onTurnStart;
    public OnTurnChangeCallback onTurnEnd;

    [SerializeField] private bool isSentient = false;

    public bool IsSentient => isSentient;

    private bool isTurn = false;
    
    public bool IsTurn
    {
        get => isTurn;
        set
        {
            SetTurn(value);
        }
    }

    public void Start()
    {
        if (GetComponent<PlayerLocomotion>())
        {

        }
    }

    private void SetTurn(bool isTurn)
    {
        if (this.isTurn == isTurn) return;

        this.isTurn = isTurn;

        if (isTurn) onTurnStart?.Invoke();
        else onTurnEnd?.Invoke();
    }

    public void Move(Vector2 direction)
    {
        transform.position += (Vector3)direction;
    }
}
