/// <summary>
/// enum für alle möglichen nachbar hexagons
/// </summary>
public enum HexDirection
{
    NE,
    E,
    SE,
    SW,
    W,
    NW
}
/// <summary>
/// extensio zum enum
/// </summary>
public static class HexDirectionExtensions
{
    /// <summary>
    /// gibt die gegenüberliegende seite zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static HexDirection Opposite(this HexDirection direction)
    { 
        return (int)direction < 3 ? (direction + 3) : (direction - 3); // if direction < 3 : direction +3 else -3
    }

    /// <summary>
    /// gibt die direction -1 zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    /// <summary>
    /// gibt die direction +1 zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
}