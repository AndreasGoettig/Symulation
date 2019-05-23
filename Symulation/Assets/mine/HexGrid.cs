using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// erstellt das spielfeld
/// </summary>
public class HexGrid : MonoBehaviour
{
    public int width = 6; // breite des Spielfelds
    public int height = 6; // länge des Spielfeld

    public HexCell cellPrefab; // das hexagon prefab als plane
    HexCell[] cells; // array für alle hexagons

    public Text cellLabelPrefab; // prefab text zum zeigen der pos jedes hexagons (zum debuggen)
    Canvas gridCanvas; // parent für text

    HexMesh hexMesh; // das hexagon mesh

    public Color defaultColor = Color.white; // standart farbe


    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>(); 

        cells = new HexCell[height * width]; // setzt die array größe
        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i); // erstellt an jeder stelle das prefab für die hexagons
                i++;
            }
        }
    }
    //start weil es nach der Awake ausgeführt werden MUSS, sonst enstehen errors
    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    /// <summary>
    /// gibt die zelle zurück an der angegebenen position
    /// </summary>
    /// <param name="position"></param>
    public void ColorCell(Vector3 position, Color color)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = color;
        hexMesh.Triangulate(cells);
    }


    /// <summary>
    ///  instanzier das prefab für hexagons und setzt alle nötigen positionen und daten
    /// </summary>
    /// <param name="x"> x coordinate </param>
    /// <param name="z"> z coordinate </param>
    /// <param name="i"> index im array cells </param>
    void CreateCell(int x, int z, int i)
    {
        //pos für jedes instanzierte hexagon + offset damit alle hexagons auch aneinander liegen
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);// instanziert das prefab und zwischenspeicher es als variable cell
        cell.transform.SetParent(transform, false); // setzt den parent für das object
        cell.transform.localPosition = position; // setzt die position
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z); //setzt neue offset coordinaten zum einfacheren navigieren
        cell.color = defaultColor; // die standart farbe wird auf das mesh gesetzt

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]); // hexagon links neben diesem hat den index: index-1
        }
        if (z > 0)
        {
            if ((z & 1) == 0) // & == AND für bits
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]); // hexagon untenrechts hat den index : index - breite des spielfelds
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]); // index - breite - 1 (untenlinks)
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]); // index - breite (untenlinks)
                if (x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]); // index - breite + 1 (untenrechts)
                }
            }
        }


        Text label = Instantiate<Text>(cellLabelPrefab); // instanziert das text prefab 
        label.rectTransform.SetParent(gridCanvas.transform, false); // setzt den parent auf das Canavs
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z); // setzt die position auf jedes Hexagon
        label.text = cell.coordinates.ToStringOnSeparateLines(); // setzt den text auf die position des Hexagons
    }
}
