using UnityEngine;
/// <summary>
/// erstellt die neuen offset coordinaten damit man leichter mit den hexagons arbeiten kann
/// </summary>
[System.Serializable]
public struct HexCoordinates {

	[SerializeField]
	private int x, z;

	public int X {
		get {
			return x;
		}
	}

	public int Z {
		get {
			return z;
		}
	}

	public int Y {
		get {
			return -X - Z;
		}
	}

	public HexCoordinates (int x, int z) {
		this.x = x;
		this.z = z;
	}
    //gibt die neuen koordinaten zurück
	public static HexCoordinates FromOffsetCoordinates (int x, int z) {
		return new HexCoordinates(x - z / 2, z);
	}
    /// <summary>
    /// gibt die neuen hexagon coordinaten zurück wenn ein hexagon per input ausgewählt wurde
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static HexCoordinates FromPosition (Vector3 position) {
		float x = position.x / (HexMetrics.innerRadius * 2f);
		float y = -x;

		float offset = position.z / (HexMetrics.outerRadius * 3f);
		x -= offset;
		y -= offset;

		int iX = Mathf.RoundToInt(x);
		int iY = Mathf.RoundToInt(y);
		int iZ = Mathf.RoundToInt(-x -y);

		if (iX + iY + iZ != 0)  //das sollte nicht passieren dürfen
        {
            //aber wenn es passiert... :
            float dX = Mathf.Abs(x - iX); // abstand von x zu dem gerundeten X
            float dY = Mathf.Abs(y - iY); // abstand von y zu dem gerundeten Y
            float dZ = Mathf.Abs(-x -y - iZ); // abstand von z zu dem gerundeten Z

            if (dX > dY && dX > dZ) // wenn der abstand von den X werten grüßer ist als der von den y werten und den z werten
            { 
				iX = -iY - iZ; // wird ix auf -den abstand von den y werten - den abstand von den z werten gesetzt
            }
			else if (dZ > dY) {
				iZ = -iX - iY; // wie bei x 
            }

		}

		return new HexCoordinates(iX, iZ);
	}

	public override string ToString ()// string convertierung für die coordinaten
    {
		return "(" +
			X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines ()// auch string convertierung aber für das label der Hexagons
    {
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}
}