using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// struct um einfacher durch die hexagons zu navigieren
/// </summary>
[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;
    public int X
    {
        get
        {
            return x;
        }
    } // x coordinate

    public int Z
    {
        get
        {
            return z;
        }
    } // z coordinate

    public HexCoordinates(int x, int z) //init
    {
        this.x = x;
        this.z = z;
    }
    /// <summary>
    /// Methode um offset coordinaten zu erstellen
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public int Y // die fehlende 3. dimmension
    {
        get
        {
            return -X - Z;
        }
    }

    public override string ToString() // string convertierung für die coordinaten
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines() // auch string convertierung aber mit trennung für das debug canvas
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}
