using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generiert das Mesh für die Hexagons
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh hexMesh; // das mesh
    List<Vector3> vertices; // liste der Verticies
    List<int> triangles; // Liste der Dreiecken die aus den verticies entstehen
    List<Color> colors; // liste der Farben

    MeshCollider meshCollider; // der collider des Hexagons

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh(); // erstellt das neue mesh
        meshCollider = gameObject.AddComponent<MeshCollider>(); // erstellt den collider
        hexMesh.name = "Hex Mesh"; // setzt den namen des Meshes
        vertices = new List<Vector3>(); // initiallisiert die verticies liste
        triangles = new List<int>(); // initiallisiert die triangles liste
        colors = new List<Color>(); // initiallisiert die farb liste
    }
    /// <summary>
    /// geht durch das array und setzt alle verticies auf die jeweiligen hexagon-meshes
    /// </summary>
    /// <param name="cells"></param>
    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear(); // säubert das mesh das nichts drin ist
        vertices.Clear();  // säubert die verticies liste
        triangles.Clear(); // säubert die triangles liste
        colors.Clear(); // säubert die farb liset
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray(); //setzt die verticies damit das mesh ensteht
        hexMesh.triangles = triangles.ToArray(); //setzt den dreiecksindex
        hexMesh.colors = colors.ToArray(); // setzt die farbe auf das mesh
        hexMesh.RecalculateNormals(); // updated die meschnormals
        meshCollider.sharedMesh = hexMesh; //setzt den mesh auf den collider 
    }

    /// <summary>
    /// rotiert durch alle nachbarn im uhrzeigersin und erstellt dreiecke
    /// </summary>
    /// <param name="cell"></param>
    void Triangulate(HexCell cell)
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            Triangulate(d, cell);
        }
    }
    /// <summary>
    /// erstellt die dreiecke
    /// </summary>
    /// <param name="cell"></param>
    void Triangulate(HexDirection direction, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(direction);
        Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(direction);

        AddTriangle(center, v1, v2);
        AddTriangleColor(cell.color);

        if (direction <= HexDirection.SE)
        {
            TriangulateConnection(direction, cell, v1, v2);
        }

    }

    void TriangulateConnection(HexDirection direction, HexCell cell, Vector3 v1, Vector3 v2)
    {
        HexCell neighbor = cell.GetNeighbor(direction);
        if (neighbor == null)
        {
            return;
        }

        Vector3 bridge = HexMetrics.GetBridge(direction);
        Vector3 v3 = v1 + bridge;
        Vector3 v4 = v2 + bridge;

        AddQuad(v1, v2, v3, v4);
        AddQuadColor(cell.color, neighbor.color);

        HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
        if (direction <= HexDirection.E && nextNeighbor != null)
        {
            AddTriangle(v2, v4, v2 + HexMetrics.GetBridge(direction.Next()));
            AddTriangleColor(cell.color, neighbor.color, nextNeighbor.color);
        }

    }

    /// <summary>
    /// Fügt der verticies und triangles listen die daten hinzu
    /// </summary>
    /// <param name="v1">verticie nr 1</param>
    /// <param name="v2">verticie nr 2</param>
    /// <param name="v3">verticie nr 3</param>
    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count; // die aktuelle anzahl der verticies in der liste
        vertices.Add(v1);   //fügt die verticies der liste hinzu in richtiger reihenfolge
        vertices.Add(v2);   
        vertices.Add(v3);
        triangles.Add(vertexIndex);     //fügt der liste die dreieckindexe hinzu
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }
    /// <summary>
    /// fügt jedem dreieck eine farbe hinzu
    /// </summary>
    /// <param name="color"></param>
    void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
    /// <summary>
    /// alternative farbvergabe mit 3 farben
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    void AddTriangleColor(Color c1, Color c2, Color c3)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }
    /// <summary>
    /// quad für das vermischen der farbe
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="v4"></param>
    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }
    /// <summary>
    /// die farben für den quad
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    /// <param name="c4"></param>
    void AddQuadColor(Color c1, Color c2, Color c3, Color c4)
    {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }
    /// <summary>
    /// AddQuadColor variante mit nur 2 farben
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    void AddQuadColor(Color c1, Color c2)
    {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);
    }
}
