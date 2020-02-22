namespace SyndriaServer.Interface
{
    public interface IStat
    {
        bool Modified { get; }
        float BaseBonus { get; }
        float BaseValue { get; set; }
        float PercentBonus { get; set; }
        float PercentBaseBonus { get; set; }
        float Total { get; set; }

        bool ApplyStatModificator(IStatModifier statModifier);
        bool RemoveStatModificator(IStatModifier statModifier);
    }
}
