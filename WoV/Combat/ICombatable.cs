namespace WoV.Combat;

public interface ICombatable
{
    public float HP { get; }
    public float Mana { get; }
    public float Damage { get; }
    public float Defence { get; }

    public void Attack(ICombatable opponent);
}