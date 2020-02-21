public static class MathExt
{
    public static int getReverseXPosition(int maxWidth, int x)
    {
        // - 1 because index starts at 0 not at 1
        return (maxWidth - 1) - x;
    }
}
