using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// alle daten die das Hexagon benötigt
/// </summary>
public class HexMetrics : MonoBehaviour
{
    //der innere und äußere kreis aus dem das Hexagon erstellt wird
    public const float outerRadius = 10f; 
    public const float innerRadius = outerRadius * 0.866025404f; // laut wikipedia die perfecte abstand zwischen den beiden kreisen. <- sqrt(3)/5

    //blendradius in % und der blendfactor
    public const float solidFactor = 0.75f; 
    public const float blendFactor = 1f - solidFactor;

    //alle eckpunkte des hexagons
    private static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius), // top
        new Vector3(innerRadius, 0f, 0.5f * outerRadius), // topright
        new Vector3(innerRadius, 0f, -0.5f * outerRadius), // topleft
        new Vector3(0f, 0f, -outerRadius), // bottom
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius), // bottomleft
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius), // bottomright
        new Vector3(0f, 0f, outerRadius), // nochmal top wegen der out of range exception
    };
    /// <summary>
    /// gibt die erste ecke zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int)direction];
    }

    /// <summary>
    /// gibt die zweite ecke zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1];
    }

    /// <summary>
    /// gibt die erste ecke zurück * den SolidFactor
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return corners[(int)direction] * solidFactor;
    }

    /// <summary>
    /// gibt die zweite ecke zurück * den solidFactor
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return corners[(int)direction + 1] * solidFactor;
    }

    /// <summary>
    /// zwischenbereich zwischen den hexagons
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetBridge(HexDirection direction)
    {
        return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
    }
}
