using UnityEngine;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates; //referenz zu den hexcoordinaten

	public RectTransform uiRect; //Transform der UI

	public HexGridChunk chunk; // referenz zu HexGridChunk

	public Color Color
    {
		get
        {
			return color;
		}
		set
        {
			if (color == value)
            {
				return;
			}
			color = value;
			Refresh();
		}
	}
    /// <summary>
    ///gibt elevation und setzt die position des hexagons auf die gewünschte höhe
    /// </summary>
	public int Elevation {
		get {
			return elevation;
		}
		set {
			if (elevation == value) {
				return;
			}
			elevation = value;
			Vector3 position = transform.localPosition;
			position.y = value * HexMetrics.elevationStep;
			position.y +=
				(HexMetrics.SampleNoise(position).y * 2f - 1f) *
				HexMetrics.elevationPerturbStrength;
			transform.localPosition = position;

			Vector3 uiPosition = uiRect.localPosition;
			uiPosition.z = -position.y;
			uiRect.localPosition = uiPosition;
			Refresh();
		}
	}
    /// <summary>
    /// gibt die position der zelle zurück
    /// </summary>
	public Vector3 Position {
		get {
			return transform.localPosition;
		}
	}

	Color color;

	int elevation = int.MinValue; // elevation ist standartmäßig auf dem niedrigsten wert

	[SerializeField]
	HexCell[] neighbors; //liste aller 6 nachbarn
    /// <summary>
    /// returnt den nachbarn in der gewünschten richtung
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}
    /// <summary>
    /// setzt die nachbarn im array
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cell"></param>
    public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}
    /// <summary>
    /// returnt den typ der kante über die gewünschte richtung
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
	public HexEdgeType GetEdgeType (HexDirection direction) {
		return HexMetrics.GetEdgeType(
			elevation, neighbors[(int)direction].elevation
		);
	}
    /// <summary>
    /// gibt die gewünschte kante zu dem nachbarn
    /// </summary>
    /// <param name="otherCell"></param>
    /// <returns></returns>
	public HexEdgeType GetEdgeType (HexCell otherCell) {
		return HexMetrics.GetEdgeType(
			elevation, otherCell.elevation
		);
	}
    /// <summary>
    /// updated die zellen und die chunks
    /// </summary>
	void Refresh () {
		if (chunk) {
			chunk.Refresh();
			for (int i = 0; i < neighbors.Length; i++) {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.chunk != chunk) {
					neighbor.chunk.Refresh();
				}
			}
		}
	}
}