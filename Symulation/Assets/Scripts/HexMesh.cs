using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Generiert das Mesh für die Hexagons
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
	static List<Vector3> vertices = new List<Vector3>();  // liste der Verticies
    static List<Color> colors = new List<Color>(); // liste der Farben der dreiecken
    static List<int> triangles = new List<int>(); // Liste der Dreiecken die aus den verticies entstehen

    Mesh hexMesh; // das mesh
    MeshCollider meshCollider; // der collider des Hexagons

    void Awake ()
    {
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh(); // initiallisiert das neue mesh
        meshCollider = gameObject.AddComponent<MeshCollider>(); // initiallisiert den collider
        hexMesh.name = "Hex Mesh"; // setzt den namen des Meshes
    }
    /// <summary>
    /// geht durch das array und setzt alle verticies auf die jeweiligen hexagon-meshes
    /// </summary>
    /// <param name="cells"></param>
    public void Triangulate (HexCell[] cells)
    {
		hexMesh.Clear(); // säubert das mesh das nichts drin ist
        vertices.Clear(); // säubert die verticies liste
        colors.Clear(); // säubert die farb liset
        triangles.Clear(); // säubert die triangles liste
        for (int i = 0; i < cells.Length; i++)//geht alle zellen durch
        {
			Triangulate(cells[i]);
		}
		hexMesh.vertices = vertices.ToArray(); //setzt die verticies damit das mesh ensteht
        hexMesh.colors = colors.ToArray(); //setzt den dreiecksindex
        hexMesh.triangles = triangles.ToArray(); // setzt die farbe auf das mesh
        hexMesh.RecalculateNormals(); // updated die meschnormals
        meshCollider.sharedMesh = hexMesh; //setzt den mesh auf den collider 
    }
    /// <summary>
    /// rotiert im uhrzeigersin angefangen von oben rechts
    /// </summary>
    /// <param name="cell"></param>
	void Triangulate (HexCell cell) {
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
			Triangulate(d, cell);
		}
	}
    /// <summary>
    /// erstellt alle mesches
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cell"></param>
	void Triangulate (HexDirection direction, HexCell cell)
    {
		Vector3 center = cell.Position;
		EdgeVertices e = new EdgeVertices(
			center + HexMetrics.GetFirstSolidCorner(direction),
			center + HexMetrics.GetSecondSolidCorner(direction));

		TriangulateEdgeFan(center, e, cell.Color);

		if (direction <= HexDirection.SE)
        {
			TriangulateConnection(direction, cell, e);
		}
	}
    /// <summary>
    /// bei höhenunterschied werden die vertiecies mit filler triangles gefüllt
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="cell"></param>
    /// <param name="e1"></param>
	void TriangulateConnection (
		HexDirection direction, HexCell cell, EdgeVertices e1)
    {
		HexCell neighbor = cell.GetNeighbor(direction);
		if (neighbor == null)
        {
			return;
		}

		Vector3 bridge = HexMetrics.GetBridge(direction);
		bridge.y = neighbor.Position.y - cell.Position.y;
		EdgeVertices e2 = new EdgeVertices(
			e1.v1 + bridge,
			e1.v4 + bridge);

		if (cell.GetEdgeType(direction) == HexEdgeType.Slope)
        {
			TriangulateEdgeTerraces(e1, cell, e2, neighbor);
		}
		else
        {
			TriangulateEdgeStrip(e1, cell.Color, e2, neighbor.Color);
		}

		HexCell nextNeighbor = cell.GetNeighbor(direction.Next());
		if (direction <= HexDirection.E && nextNeighbor != null)
        {
			Vector3 v5 = e1.v4 + HexMetrics.GetBridge(direction.Next());
			v5.y = nextNeighbor.Position.y;

			if (cell.Elevation <= neighbor.Elevation)
            {
				if (cell.Elevation <= nextNeighbor.Elevation)
                {
					TriangulateCorner(
						e1.v4, cell, e2.v4, neighbor, v5, nextNeighbor
					);
				}
				else
                {
					TriangulateCorner(
						v5, nextNeighbor, e1.v4, cell, e2.v4, neighbor);
				}
			}
			else if (neighbor.Elevation <= nextNeighbor.Elevation)
            {
				TriangulateCorner(
					e2.v4, neighbor, v5, nextNeighbor, e1.v4, cell);
			}
			else
            {
				TriangulateCorner(
					v5, nextNeighbor, e1.v4, cell, e2.v4, neighbor);
			}
		}
	}
    /// <summary>
    /// trianguliert zwischen 2 eckpunkten von hexagons ohne höhenunterschied
    /// </summary>
    /// <param name="bottom"></param>
    /// <param name="bottomCell"></param>
    /// <param name="left"></param>
    /// <param name="leftCell"></param>
    /// <param name="right"></param>
    /// <param name="rightCell"></param>
	void TriangulateCorner (
		Vector3 bottom, HexCell bottomCell,
		Vector3 left, HexCell leftCell,
		Vector3 right, HexCell rightCell)
    {
		HexEdgeType leftEdgeType = bottomCell.GetEdgeType(leftCell);
		HexEdgeType rightEdgeType = bottomCell.GetEdgeType(rightCell);

		if (leftEdgeType == HexEdgeType.Slope)
        {
			if (rightEdgeType == HexEdgeType.Slope)
            {
				TriangulateCornerTerraces(
					bottom, bottomCell, left, leftCell, right, rightCell);
			}
			else if (rightEdgeType == HexEdgeType.Flat)
            {
				TriangulateCornerTerraces(
					left, leftCell, right, rightCell, bottom, bottomCell);
			}
			else
            {
				TriangulateCornerTerracesCliff(
					bottom, bottomCell, left, leftCell, right, rightCell);
			}
		}
		else if (rightEdgeType == HexEdgeType.Slope)
        {
			if (leftEdgeType == HexEdgeType.Flat)
            {
				TriangulateCornerTerraces(
					right, rightCell, bottom, bottomCell, left, leftCell);
			}
			else
            {
				TriangulateCornerCliffTerraces(
					bottom, bottomCell, left, leftCell, right, rightCell);
			}
		}
		else if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope)
        {
			if (leftCell.Elevation < rightCell.Elevation) {
				TriangulateCornerCliffTerraces(
					right, rightCell, bottom, bottomCell, left, leftCell);
			}
			else
            {
				TriangulateCornerTerracesCliff(
					left, leftCell, right, rightCell, bottom, bottomCell);
			}
		}
		else
        {
			AddTriangle(bottom, left, right);
			AddTriangleColor(bottomCell.Color, leftCell.Color, rightCell.Color);
		}
	}
    /// <summary>
    /// interpolliert die beiden kanten von zwei hexagons mit höhenunterschied
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="beginCell"></param>
    /// <param name="end"></param>
    /// <param name="endCell"></param>
	void TriangulateEdgeTerraces (
		EdgeVertices begin, HexCell beginCell,
		EdgeVertices end, HexCell endCell)
    {
		EdgeVertices e2 = EdgeVertices.TerraceLerp(begin, end, 1);
		Color c2 = HexMetrics.TerraceLerp(beginCell.Color, endCell.Color, 1);

		TriangulateEdgeStrip(begin, beginCell.Color, e2, c2);

		for (int i = 2; i < HexMetrics.terraceSteps; i++)
        {
			EdgeVertices e1 = e2;
			Color c1 = c2;
			e2 = EdgeVertices.TerraceLerp(begin, end, i);
			c2 = HexMetrics.TerraceLerp(beginCell.Color, endCell.Color, i);
			TriangulateEdgeStrip(e1, c1, e2, c2);
		}

		TriangulateEdgeStrip(e2, c2, end, endCell.Color);
	}
    /// <summary>
    /// interpolliert die beiden ekecn von 2 hexagons
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="beginCell"></param>
    /// <param name="left"></param>
    /// <param name="leftCell"></param>
    /// <param name="right"></param>
    /// <param name="rightCell"></param>
	void TriangulateCornerTerraces (
		Vector3 begin, HexCell beginCell,
		Vector3 left, HexCell leftCell,
		Vector3 right, HexCell rightCell)
    {
		Vector3 v3 = HexMetrics.TerraceLerp(begin, left, 1);
		Vector3 v4 = HexMetrics.TerraceLerp(begin, right, 1);
		Color c3 = HexMetrics.TerraceLerp(beginCell.Color, leftCell.Color, 1);
		Color c4 = HexMetrics.TerraceLerp(beginCell.Color, rightCell.Color, 1);

		AddTriangle(begin, v3, v4);
		AddTriangleColor(beginCell.Color, c3, c4);

		for (int i = 2; i < HexMetrics.terraceSteps; i++)
        {
			Vector3 v1 = v3;
			Vector3 v2 = v4;
			Color c1 = c3;
			Color c2 = c4;
			v3 = HexMetrics.TerraceLerp(begin, left, i);
			v4 = HexMetrics.TerraceLerp(begin, right, i);
			c3 = HexMetrics.TerraceLerp(beginCell.Color, leftCell.Color, i);
			c4 = HexMetrics.TerraceLerp(beginCell.Color, rightCell.Color, i);
			AddQuad(v1, v2, v3, v4);
			AddQuadColor(c1, c2, c3, c4);
		}

		AddQuad(v3, v4, left, right);
		AddQuadColor(c3, c4, leftCell.Color, rightCell.Color);
	}
    /// <summary>
    /// interpoliert zwischen dem linken und dem rechten rand einer klippe
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="beginCell"></param>
    /// <param name="left"></param>
    /// <param name="leftCell"></param>
    /// <param name="right"></param>
    /// <param name="rightCell"></param>
	void TriangulateCornerTerracesCliff (
		Vector3 begin, HexCell beginCell,
		Vector3 left, HexCell leftCell,
		Vector3 right, HexCell rightCell)
    {
		float b = 1f / (rightCell.Elevation - beginCell.Elevation);
		if (b < 0)
        {
			b = -b;
		}
		Vector3 boundary = Vector3.Lerp(Perturb(begin), Perturb(right), b);
		Color boundaryColor = Color.Lerp(beginCell.Color, rightCell.Color, b);

		TriangulateBoundaryTriangle(
			begin, beginCell, left, leftCell, boundary, boundaryColor);

		if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope)
        {
			TriangulateBoundaryTriangle(
				left, leftCell, right, rightCell, boundary, boundaryColor);
		}
		else
        {
			AddTriangleUnperturbed(Perturb(left), Perturb(right), boundary);
			AddTriangleColor(leftCell.Color, rightCell.Color, boundaryColor);
		}
	}
    /// <summary>
    /// trianguliert einen quad zwischen den einzelnen kleinen treppen
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="beginCell"></param>
    /// <param name="left"></param>
    /// <param name="leftCell"></param>
    /// <param name="right"></param>
    /// <param name="rightCell"></param>
	void TriangulateCornerCliffTerraces (
		Vector3 begin, HexCell beginCell,
		Vector3 left, HexCell leftCell,
		Vector3 right, HexCell rightCell)
    {
		float b = 1f / (leftCell.Elevation - beginCell.Elevation);
		if (b < 0)
        {
			b = -b;
		}
		Vector3 boundary = Vector3.Lerp(Perturb(begin), Perturb(left), b);
		Color boundaryColor = Color.Lerp(beginCell.Color, leftCell.Color, b);

		TriangulateBoundaryTriangle(
			right, rightCell, begin, beginCell, boundary, boundaryColor);

		if (leftCell.GetEdgeType(rightCell) == HexEdgeType.Slope)
        {
			TriangulateBoundaryTriangle(
				left, leftCell, right, rightCell, boundary, boundaryColor);
		}
		else
        {
			AddTriangleUnperturbed(Perturb(left), Perturb(right), boundary);
			AddTriangleColor(leftCell.Color, rightCell.Color, boundaryColor);
		}
	}
    /// <summary>
    /// trianguliert die hexagons am rande der map die keine 6 nachbarn haben
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="beginCell"></param>
    /// <param name="left"></param>
    /// <param name="leftCell"></param>
    /// <param name="boundary"></param>
    /// <param name="boundaryColor"></param>
	void TriangulateBoundaryTriangle (
		Vector3 begin, HexCell beginCell,
		Vector3 left, HexCell leftCell,
		Vector3 boundary, Color boundaryColor)
    {
		Vector3 v2 = Perturb(HexMetrics.TerraceLerp(begin, left, 1));
		Color c2 = HexMetrics.TerraceLerp(beginCell.Color, leftCell.Color, 1);

		AddTriangleUnperturbed(Perturb(begin), v2, boundary);
		AddTriangleColor(beginCell.Color, c2, boundaryColor);

		for (int i = 2; i < HexMetrics.terraceSteps; i++)
        {
			Vector3 v1 = v2;
			Color c1 = c2;
			v2 = Perturb(HexMetrics.TerraceLerp(begin, left, i));
			c2 = HexMetrics.TerraceLerp(beginCell.Color, leftCell.Color, i);
			AddTriangleUnperturbed(v1, v2, boundary);
			AddTriangleColor(c1, c2, boundaryColor);
		}

		AddTriangleUnperturbed(v2, Perturb(left), boundary);
		AddTriangleColor(c2, leftCell.Color, boundaryColor);
	}
    /// <summary>
    /// trianguliert zwischen dem centrum des hexagons und einem ihrer kanten
    /// </summary>
    /// <param name="center"></param>
    /// <param name="edge"></param>
    /// <param name="color"></param>
	void TriangulateEdgeFan (Vector3 center, EdgeVertices edge, Color color)
    {
		AddTriangle(center, edge.v1, edge.v2);
		AddTriangleColor(color);
		AddTriangle(center, edge.v2, edge.v3);
		AddTriangleColor(color);
		AddTriangle(center, edge.v3, edge.v4);
		AddTriangleColor(color);
	}
    /// <summary>
    /// trianguliert einen streifen von quads zwischen zwei kanten
    /// </summary>
    /// <param name="e1"></param>
    /// <param name="c1"></param>
    /// <param name="e2"></param>
    /// <param name="c2"></param>
	void TriangulateEdgeStrip (
		EdgeVertices e1, Color c1,
		EdgeVertices e2, Color c2)
    {
		AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
		AddQuadColor(c1, c2);
		AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
		AddQuadColor(c1, c2);
		AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
		AddQuadColor(c1, c2);
	}
    /// <summary>
    /// standart dreieckte werden erstellt
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3)
    {
		int vertexIndex = vertices.Count;
		vertices.Add(Perturb(v1));
		vertices.Add(Perturb(v2));
		vertices.Add(Perturb(v3));
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}
    /// <summary>
    /// unveränderte dreiecke werden ersellt
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
	void AddTriangleUnperturbed (Vector3 v1, Vector3 v2, Vector3 v3)
    {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}
    /// <summary>
    /// setzt die fare des triangles auf 1 feste
    /// </summary>
    /// <param name="color"></param>
	void AddTriangleColor (Color color)
    {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}
    /// <summary>
    /// setzt dem triangle eine color pro verticie
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
	void AddTriangleColor (Color c1, Color c2, Color c3)
    {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
	}
    /// <summary>
    /// fügt das quad für die farbmischung hinzu
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="v3"></param>
    /// <param name="v4"></param>
	void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
		int vertexIndex = vertices.Count;
		vertices.Add(Perturb(v1));
		vertices.Add(Perturb(v2));
		vertices.Add(Perturb(v3));
		vertices.Add(Perturb(v4));
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}
    /// <summary>
    /// quadcolor variante mit 2 farben. 1 für jedes dreieck
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
	void AddQuadColor (Color c1, Color c2)
    {
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c2);
	}
    /// <summary>
    /// setzt farben für jede verticie des quads
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="c3"></param>
    /// <param name="c4"></param>
	void AddQuadColor (Color c1, Color c2, Color c3, Color c4)
    {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
		colors.Add(c4);
	}
    /// <summary>
    /// setzt die noise ein im bezu auf die turbulenz
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
	Vector3 Perturb (Vector3 position)
    {
		Vector4 sample = HexMetrics.SampleNoise(position);
		position.x += (sample.x * 2f - 1f) * HexMetrics.cellPerturbStrength;
		position.z += (sample.z * 2f - 1f) * HexMetrics.cellPerturbStrength;
		return position;
	}
}