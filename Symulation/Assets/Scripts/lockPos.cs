using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// löst das problem das die generierte map in die luft flieg´t beim backen wenn chunk 1  nur die  höhe 0 hat
/// </summary>
public class lockPos : MonoBehaviour
{
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = pos; //lockt die position von der map
    }
}
