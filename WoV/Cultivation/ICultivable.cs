namespace WoV.Cultivation;

public interface ICultivable
{
    public double TotalCultivationExp { get; }
    public double CurrentCultivationExp { get; }
    public double CultivationSpeedPerSecond { get; }
    public double Cultivate();
}