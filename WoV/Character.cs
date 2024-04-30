namespace WoV;

public class Character(ILogger logger) : ICombatable, ICultivable
{
    public int HP { get; private set; }
    public int Mana { get; private set; }
    public float Damage { get; private set; }
    public float Defence { get; private set; }

    public float TotalCultivationExp { get; private set;}
    public float CultivationSpeed { get; private set; } = 100.0f;
    public bool IsCultivating { get; private set; }


    public void Attack(ICombatable opponent)
    {
        throw new NotImplementedException();
    }

    public async Task CultivateAsync()
    {
        IsCultivating = true;
        while (IsCultivating)
        {
            TotalCultivationExp += CultivationSpeed;
            logger.LogInformation("Total cultivation exp : {TotalCultivationExp}",TotalCultivationExp);
            await Task.Delay(1000);
        }

    }

    public void StopCultivating()
    {
        IsCultivating = false;
    }
}