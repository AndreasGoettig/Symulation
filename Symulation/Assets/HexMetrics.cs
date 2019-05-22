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

    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius), // top
        new Vector3(innerRadius, 0f, 0.5f * outerRadius), // topright
        new Vector3(innerRadius, 0f, -0.5f * outerRadius), // topleft
        new Vector3(0f, 0f, -outerRadius), // bottom
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius), // bottomleft
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius) // bottomright
    };
}
