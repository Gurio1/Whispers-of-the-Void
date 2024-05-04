namespace WoV;

public class Character(string userId) : ICombatable, ICultivable
{
    public string UserId { get; private set; } = userId;
    public Guid Id { get; private set; } = Guid.NewGuid();

    public float HP { get; private set; } = 100;
    public float Mana { get; private set; } = 100;
    public float Damage { get; private set; } = 10;
    public float Defence { get; private set; } = 40;

    public float TotalCultivationExp { get; private set;}
    public float CurrentCultivationExp { get; private set; }
    public float CultivationSpeedPerSecond { get; private set; } = 100.0f;
    
    public bool IsCultivating { get; private set; }

    public void Attack(ICombatable opponent)
    {
        throw new NotImplementedException();
    }

    public float Cultivate()
    {
        if (!IsCultivating) return CurrentCultivationExp;
        
        TotalCultivationExp += CultivationSpeedPerSecond;
        CurrentCultivationExp += CultivationSpeedPerSecond;

        return CurrentCultivationExp;
    }
}