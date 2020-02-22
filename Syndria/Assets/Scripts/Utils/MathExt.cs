using UnityEngine;

public static class MathExt
{
    public static int getReverseXPosition(int maxWidth, int x)
    {
        // - 1 because index starts at 0 not at 1
        return (maxWidth - 1) - x;
    }

    public static Color getColorByRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.R:
                return Color.grey;
            case Rarity.SR:
                return new Color(255, 184, 0);
            case Rarity.SSR:
                return Color.blue;
            case Rarity.UR:
                return Color.magenta;
            default:
                return Color.white;
        }
    }
}
