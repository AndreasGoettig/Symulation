﻿using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// erstellt das grid für die hexagons
/// </summary>
public class HexGrid : MonoBehaviour {

	public int chunkCountX = 4, chunkCountZ = 3; // anzahl der chunks

	public Color defaultColor = Color.white; // standart farbe

	public HexCell cellPrefab; //prefab für die zelle
	public Text cellLabelPrefab; // label das die position jeder hexagons zeigt
	public HexGridChunk chunkPrefab; // das chunk prefab

	public Texture2D noiseSource; // staic noise texture (wegen vorschau, sonst andere noisetexturen möglich)

	public HexGridChunk[] chunks; // array der chunks
	HexCell[] cells; // arra alles cellen

	int cellCountX, cellCountZ; //anzahl der zellen in x unud z richtung

	void Awake () {
		HexMetrics.noiseSource = noiseSource;

		cellCountX = chunkCountX * HexMetrics.chunkSizeX;
		cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

		CreateChunks();
		CreateCells();
	}
    /// <summary>
    /// erstellt die chunks
    /// </summary>
	void CreateChunks () {
		chunks = new HexGridChunk[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++) {
			for (int x = 0; x < chunkCountX; x++) {
				HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
				chunk.transform.SetParent(transform);
			}
		}
	}
    /// <summary>
    /// erstellt die zellen
    /// </summary>
	void CreateCells () {
		cells = new HexCell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++) {
			for (int x = 0; x < cellCountX; x++) {
				CreateCell(x, z, i++);
			}
		}
	}
    /// <summary>
    /// setzt die noise source auf die noisetexture onEnable
    /// </summary>
	void OnEnable () {
		HexMetrics.noiseSource = noiseSource;
	}
    /// <summary>
    /// gibt die zelle an der gegebenen stelle zurück
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
	public HexCell GetCell (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index =
			coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
		return cells[index];
	}
    /// <summary>
    /// gibt die zelle über ihre koordinaten zurück
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
	public HexCell GetCell (HexCoordinates coordinates) {
		int z = coordinates.Z;
		if (z < 0 || z >= cellCountZ) {
			return null;
		}
		int x = coordinates.X + z / 2;
		if (x < 0 || x >= cellCountX) {
			return null;
		}
		return cells[x + z * cellCountX];
	}
    /// <summary>
    /// activiert die ui
    /// </summary>
    /// <param name="visible"></param>
	public void ShowUI (bool visible) {
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i].ShowUI(visible);
		}
	}
    /// <summary>
    /// erstellt die zelle
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="i"></param>
	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.Color = defaultColor;

		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
		cell.uiRect = label.rectTransform;

		cell.Elevation = 0;

		AddCellToChunk(x, z, cell);
	}
    /// <summary>
    /// fügt eine zelle dem chunk hinzu
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="cell"></param>
	void AddCellToChunk (int x, int z, HexCell cell) {
		int chunkX = x / HexMetrics.chunkSizeX;
		int chunkZ = z / HexMetrics.chunkSizeZ;
		HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * HexMetrics.chunkSizeX;
		int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
		chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
	}
}