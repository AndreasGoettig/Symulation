using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// erstellt die chunks 
/// </summary>
public class HexGridChunk : MonoBehaviour {

	HexCell[] cells;

	HexMesh hexMesh;
	Canvas gridCanvas;

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ]; // standart 5 x 5  
		ShowUI(false);
	}
    /// <summary>
    /// fügt die zelle dem chunk hinzu
    /// </summary>
    /// <param name="index"></param>
    /// <param name="cell"></param>
	public void AddCell (int index, HexCell cell) {
		cells[index] = cell;
		cell.chunk = this;
		cell.transform.SetParent(transform, false);
		cell.uiRect.SetParent(gridCanvas.transform, false);
	}
    /// <summary>
    /// setzt den refrechbool auf true
    /// </summary>
	public void Refresh () {
		enabled = true;
	}

    /// <summary>
    /// activiert die UI
    /// </summary>
    /// <param name="visible"></param>
	public void ShowUI (bool visible) {
		gridCanvas.gameObject.SetActive(visible);
	}

	void LateUpdate () {
		hexMesh.Triangulate(cells);
		enabled = false;
	}
}