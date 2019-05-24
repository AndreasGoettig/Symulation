public enum HexDirection //enum zu den verschiedenen nachbarn jedes Hexagons
{
	NE,
    E,
    SE,
    SW,
    W,
    NW
}
/// <summary>
/// extension Methoden 
/// </summary>
public static class HexDirectionExtensions {

    /// <summary>
    /// gibt die gegenüberliegendeseite zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
	public static HexDirection Opposite (this HexDirection direction) {
		return (int)direction < 3 ? (direction + 3) : (direction - 3);
	}
    /// <summary>
    /// gibt die vorherige richtung zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
	public static HexDirection Previous (this HexDirection direction) {
		return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
	}

    /// <summary>
    /// gibt die nächste richtung zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
	public static HexDirection Next (this HexDirection direction) {
		return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
	}
}