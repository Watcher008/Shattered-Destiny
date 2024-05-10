using SD.Grids;

public interface IDamageable
{
    public PathNode GetNode();
    public void TakeDamage(int damage);
}