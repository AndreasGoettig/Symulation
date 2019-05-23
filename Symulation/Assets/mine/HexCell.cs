using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    [SerializeField]
    HexCell[] neighbors;

    public HexCoordinates coordinates; // die neuen offset coordinaten

    public Color color; // die farbe der celle

    /// <summary>
    /// holt sich die nachbarn
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    /// <summary>
    /// setzt die nachbarn in das Array
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cell"></param>
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
}
