using UnityEngine;
/// <summary>
/// alle daten rund um das Hexagon
/// </summary>
public static class HexMetrics {
    //der innere und äußere kreis aus dem das Hexagon erstellt wird
    public const float outerRadius = 10f;
	public const float innerRadius = outerRadius * 0.866025404f;// laut wikipedia die perfecte abstand zwischen den beiden kreisen. <- sqrt(3)/5
    
    //blendradius in % und der blendfactor
    public const float solidFactor = 0.8f;
	public const float blendFactor = 1f - solidFactor;

    // höhe zur nächsten anhöhe (unterschied pro schritt)
    public const float elevationStep = 3f;

    // wie viele stufen es beim höhenausgleich gibt
	public const int terracesPerSlope = 2;
	public const int terraceSteps = terracesPerSlope * 2 + 1;
	public const float horizontalTerraceStepSize = 1f / terraceSteps;
	public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);

    //wie stark die tuurbulenzen im mesh auftreten
	public const float cellPerturbStrength = 4f;
	public const float elevationPerturbStrength = 1.5f;

    //passt die noise der map an
	public const float noiseScale = 0.003f;

    //die größe der chunks (hier 5 * 5 also 25 hexagons pro chunk)
	public const int chunkSizeX = 5, chunkSizeZ = 5;

	static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),// top
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),// topright
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),// topleft
		new Vector3(0f, 0f, -outerRadius),// bottom
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),// bottomleft
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),// bottomright
		new Vector3(0f, 0f, outerRadius)// nochmal top wegen der out of range exception
	};

    //noise can beliebig ersetzt werden (statische noise hier zum vorführen)
	public static Texture2D noiseSource;

    //mesh wird leicht deformiert
	public static Vector4 SampleNoise (Vector3 position) {
		return noiseSource.GetPixelBilinear(
			position.x * noiseScale,
			position.z * noiseScale
		);
	}

    /// <summary>
    /// gibt die erste ecke zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetFirstCorner (HexDirection direction) {
		return corners[(int)direction];
	}
    /// <summary>
    /// gibt die zweite ecke zurück
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetSecondCorner (HexDirection direction) {
		return corners[(int)direction + 1];
	}
    /// <summary>
    /// gibt die erste ecke zurück * den SolidFactor
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetFirstSolidCorner (HexDirection direction) {
		return corners[(int)direction] * solidFactor;
	}
    /// <summary>
    /// gibt die zweite ecke zurück * den solidFactor
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetSecondSolidCorner (HexDirection direction) {
		return corners[(int)direction + 1] * solidFactor;
	}
    /// <summary>
    /// zwischenbereich zwischen den hexagons
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 GetBridge (HexDirection direction) {
		return (corners[(int)direction] + corners[(int)direction + 1]) *
			blendFactor;
	}
    /// <summary>
    /// interpoliert zwischen anfang a und ende b in step schritten
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="step"></param>
    /// <returns></returns>
	public static Vector3 TerraceLerp (Vector3 a, Vector3 b, int step) {
		float h = step * HexMetrics.horizontalTerraceStepSize;
		a.x += (b.x - a.x) * h;
		a.z += (b.z - a.z) * h;
		float v = ((step + 1) / 2) * HexMetrics.verticalTerraceStepSize;
		a.y += (b.y - a.y) * v;
		return a;
	}
    /// <summary>
    /// simuliert den farbverlauf 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="step"></param>
    /// <returns></returns>
	public static Color TerraceLerp (Color a, Color b, int step) {
		float h = step * HexMetrics.horizontalTerraceStepSize;
		return Color.Lerp(a, b, h);
	}
    /// <summary>
    /// gibt zurück um was für eine kante es sich handlet ()
    /// </summary>
    /// <param name="elevation1"></param>
    /// <param name="elevation2"></param>
    /// <returns></returns>
	public static HexEdgeType GetEdgeType (int elevation1, int elevation2) {
		if (elevation1 == elevation2) {
			return HexEdgeType.Flat;
		}
		int delta = elevation2 - elevation1;
		if (delta == 1 || delta == -1) {
			return HexEdgeType.Slope;
		}
		return HexEdgeType.Cliff;
	}
}