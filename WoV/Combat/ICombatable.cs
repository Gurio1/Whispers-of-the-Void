namespace WoV.Combat;

public interface ICombatable
{
    public double HP { get; }
    public double Mana { get; }
    public double Damage { get; }
    public double Defence { get; }

    public void Attack(ICombatable opponent);
}