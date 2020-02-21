public interface IStatsModifier
{
    IStatModifier Health { get; }
    IStatModifier Attack { get; }
    IStatModifier Movement { get; }
    IStatModifier Range { get; }
    IStatModifier Size { get; }
}