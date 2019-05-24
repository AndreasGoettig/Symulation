using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// gibt random position über der generierten map und schießt anschließend einen raycast auf die Map ab um dort eine random position zu bekommen
/// </summary>
public class RandomPosOnMesh : MonoBehaviour
{
    /// <summary>
    /// die Random position über der map
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPos()
    {
        Vector3 pos = new Vector3(Random.Range(0, 300), 60f, Random.Range(0, 200));
        return pos;
    }
    /// <summary>
    /// die Random Position auf der Map
    /// </summary>
    /// <param name="tempPos"></param>
    /// <returns></returns>
    public Vector3 GetPosOnMesh(Vector3 tempPos)
    {
        Vector3 meshPos = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(tempPos, Vector3.down, out hit))
        {
            meshPos = hit.point;
        }

           return meshPos;
       
    }
}
