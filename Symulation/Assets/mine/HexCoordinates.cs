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
    /// <summary>
    /// gibt die neuen hexagon coordinaten zurück wenn ein hexagon per input ausgewählt wurde
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static HexCoordinates FromPosition(Vector3 position) 
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;

        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0) //das sollte nicht passieren dürfen
        {
            //aber wenn es passiert... :
            float dX = Mathf.Abs(x - iX); // abstand von x zu dem gerundeten X
            float dY = Mathf.Abs(y - iY); // abstand von y zu dem gerundeten Y
            float dZ = Mathf.Abs(-x - y - iZ); // abstand von z zu dem gerundeten Z

            if (dX > dY && dX > dZ)  // wenn der abstand von den X werten grüßer ist als der von den y werten und den z werten 
            {
                iX = -iY - iZ; // wird ix auf -den abstand von den y werten - den abstand von den z werten gesetzt
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY; // wie bei x 
            }

            //jetzt muss 0 rauskommen
        }

        return new HexCoordinates(iX, iZ);
    }
}
