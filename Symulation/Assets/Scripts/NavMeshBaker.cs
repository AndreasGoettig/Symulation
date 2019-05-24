using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// backt den navmesh für die agent auf knopfdruck
/// </summary>
public class NavMeshBaker : MonoBehaviour
{
    HexGrid reference; //reference zum Grid
    NavMeshSurface[] navs;

    /// <summary>
    /// init und holt sich die mesches für die generierte map;
    /// </summary>
    private void Start()
    {
        reference = GetComponent<HexGrid>();
        navs = new NavMeshSurface[reference.chunks.Length];
        for (int i = 0; i < reference.chunks.Length; i++)
        {
            NavMeshSurface surface = reference.chunks[i].gameObject.GetComponentInChildren<NavMeshSurface>();
            navs[i] = surface;           
        }
    }
    /// <summary>
    /// auf knopfDruck wird das NavMesh Gebacken
    /// </summary>
    public void PressToBake()
    {
        for (int i = 0; i < navs.Length; i++)
        {
            navs[i].BuildNavMesh();         
        }
    }
}
