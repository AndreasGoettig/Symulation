using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Agent Kontroller
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    public bool destinationReached = false; // bool die zwischen den beiden states wechselt
    [HideInInspector]
    public RandomPosOnMesh rpm; // random destination point generator
    public float destinationRadius = 5; // minimale distance die der Agent erreichen muss
    NavMeshAgent agent; // navmesch component des Agents

    /// <summary>
    /// init und falls agents vorhanden es ihnen ihre erste destination setzt
    /// </summary>
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        RandomPos();
    }

    /// <summary>
    /// update die jeweils nach dem bool wert zwischen 2 states wechselt
    /// </summary>
    private void FixedUpdate()
    {
        if (!destinationReached)
        {
            CheckDestinationReached(); // checkt ob ziel erreicht wurde
        }

        if (destinationReached)
        {
            RandomPos(); //wenn ziel erreicht gib neue pos und setzte destinationReached auf false
            destinationReached = false;
        }


    }
    /// <summary>
    /// gibt true zurück wenn der agent min destinationRadius vom ziel entfernt ist
    /// </summary>
    void CheckDestinationReached()
    {
        destinationReached = Vector3.Distance(transform.position, agent.destination) <= destinationRadius;     
    }

    /// <summary>
    /// setzt die Random Position auf den agent
    /// </summary>
    void RandomPos()
    {
        Vector3 temp = rpm.GetPos();
        agent.SetDestination(rpm.GetPosOnMesh(temp));

    }
    /// <summary>
    /// methode die dem agent die destination setzt
    /// </summary>
    /// <param name="newDestination"></param>
    public void SetDestination(Vector3 newDestination)
    {
        agent.SetDestination(newDestination);
    }
}
