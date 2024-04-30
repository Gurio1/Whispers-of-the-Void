namespace WoV;

public interface ICultivable
{
    public float TotalCultivationExp { get; }
    public float CultivationSpeed { get; }
    public bool IsCultivating { get; }

    public Task CultivateAsync();
    public void StopCultivating();
}