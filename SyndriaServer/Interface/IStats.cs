using SyndriaServer.Enums;

namespace SyndriaServer.Interface
{
    public interface IStats
    {
        int Aptitude { get; }
        bool IsTargetable { get; }
        UnitState UnitState { get; }
        IStat Attack { get; }
        IStat Health { get; }
        IStat CriticalChance { get; }
        IStat CriticalDamage { get; }
        IStat Movement { get; }
        IStat Range { get; }
        IStat Size { get; }

        void AddModifier(IStatsModifier modifier);
        void RemoveModifier(IStatsModifier modifier);

        void SetUnitState(UnitState state, bool enabled);
    }
}
