namespace WoV.Cultivation;

public interface ICultivable
{
    public float TotalCultivationExp { get; }
    public float CurrentCultivationExp { get; }
    public float CultivationSpeedPerSecond { get; }
    public float Cultivate();
}