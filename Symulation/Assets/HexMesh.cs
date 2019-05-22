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

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh(); // erstellt das neue mesh
        hexMesh.name = "Hex Mesh"; // setzt den namen des Meshes
        vertices = new List<Vector3>(); // initiallisiert die verticies liste
        triangles = new List<int>(); // initiallisiert die triangles liste
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
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray(); //setzt die verticies damit das mesh ensteht
        hexMesh.triangles = triangles.ToArray(); //set den dreieckindex
        hexMesh.RecalculateNormals(); // updated das mesh
    }

    /// <summary>
    /// erstellt die dreiecke
    /// </summary>
    /// <param name="cell"></param>
    void Triangulate(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition; // verticie am zentrum des hexagons
        for (int i = 0; i < 6; i++) // 6 die anzahl der dreiecke des hexagons
        {
            AddTriangle(center, center + HexMetrics.corners[i], center + HexMetrics.corners[i+1]);
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
}
