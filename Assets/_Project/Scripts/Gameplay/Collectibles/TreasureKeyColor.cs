namespace MonsterTreasureHunt.Gameplay
{
    public enum TreasureKeyColor
    {
        Yellow = 0,
        Red = 1,
        Green = 2,
        Blue = 3
    }

    public static class TreasureKeyColorUtility
    {
        public static string GetDisplayName(TreasureKeyColor color)
        {
            return color switch
            {
                TreasureKeyColor.Red => "Red",
                TreasureKeyColor.Green => "Green",
                TreasureKeyColor.Blue => "Blue",
                _ => "Yellow"
            };
        }
    }
}
