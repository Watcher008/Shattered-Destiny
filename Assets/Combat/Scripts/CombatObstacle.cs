using SD.Combat;
using SD.Grids;
using UnityEngine;

public class CombatObstacle : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    private PathNode _node;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = _maxHealth;
    }

    public void SetNode(PathNode node)
    {
        _node = node;
        _node.SetOccupant(Occupant.Object);
        CombatManager.Instance.Obstacles.Add(this);
    }

    public PathNode GetNode() => _node;

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} destroyed.");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        CombatManager.Instance.Obstacles.Remove(this);
    }
}
