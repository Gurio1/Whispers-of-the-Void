namespace WoV;

public interface ICombatable
{
    public int HP { get; }
    public int Mana { get; }
    public float Damage { get; }
    public float Defence { get; }

    public void Attack(ICombatable opponent);
}