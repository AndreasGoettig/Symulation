using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors; //color array für die user auswahl

	public HexGrid hexGrid; //referenz zu dem grid

	int activeElevation; //stufe der erhöhung

	Color activeColor; // die aktive farbe

	int brushSize; // pinselgröße

	bool applyColor; // bool ob farbe hinzugefügt wird
	bool applyElevation = true; // bool ob höhe verändert werden soll
    /// <summary>
    /// farbe wird auf den wert der togglebar gesetzt
    /// </summary>
    /// <param name="index"></param>
	public void SelectColor (int index) {
		applyColor = index >= 0;
		if (applyColor) {
			activeColor = colors[index];
		}
	}
    /// <summary>
    /// toggle zwischen benutze hhenunterschiede und benutze keine höhenunterschiede
    /// </summary>
    /// <param name="toggle"></param>
	public void SetApplyElevation (bool toggle) {
		applyElevation = toggle;
	}
    /// <summary>
    /// setzt die elevation auf die ausgewählte elevationsttufe
    /// </summary>
    /// <param name="elevation"></param>
	public void SetElevation (float elevation) {
		activeElevation = (int)elevation;
	}
    /// <summary>
    /// setzt die pinselgröße
    /// </summary>
    /// <param name="size"></param>
	public void SetBrushSize (float size) {
		brushSize = (int)size;
	}
    /// <summary>
    /// bool ob die positionen der hexagons als label angezeigt werden sollen
    /// </summary>
    /// <param name="visible"></param>
	public void ShowUI (bool visible) {
		hexGrid.ShowUI(visible);
	}
    /// <summary>
    /// setzt beim start die start farbe auf die erste stelle im farb array
    /// </summary>
	void Awake () {
		SelectColor(0);
	}
    /// <summary>
    /// inpu abfrage ob die linke moustaste gedrückt wurde
    /// </summary>
	void Update () {
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
			HandleInput();
		}
	}
    /// <summary>
    /// wählt den angeklickten hexagon und bearbeitet ihn
    /// </summary>
	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			EditCells(hexGrid.GetCell(hit.point));
		}
	}
    /// <summary>
    /// bearbeited den ausgewählten hexagon
    /// </summary>
    /// <param name="center"></param>
	void EditCells (HexCell center) {
		int centerX = center.coordinates.X;
		int centerZ = center.coordinates.Z;

		for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++) {
			for (int x = centerX - r; x <= centerX + brushSize; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
		for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++) {
			for (int x = centerX - brushSize; x <= centerX + r; x++) {
				EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
			}
		}
	}
    /// <summary>
    /// setzt die hhe und die farbe des hexagons
    /// </summary>
    /// <param name="cell"></param>
	void EditCell (HexCell cell) {
		if (cell) {
			if (applyColor) {
				cell.Color = activeColor;
			}
			if (applyElevation) {
				cell.Elevation = activeElevation;
			}
		}
	}
}