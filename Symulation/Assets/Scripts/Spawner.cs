using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

/// <summary>
/// spawn agents und gibt ihnen befehle
/// </summary>
public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    List<Agent> agents;
    public Vector3 goal;
    RandomPosOnMesh rpm;
    bool abbruch = true;
    public GameObject randomPosCube;
    Vector3 helpVar;
    
    // Start is called before the first frame update
    void Start()
    {
        agents = new List<Agent>();
        rpm = new RandomPosOnMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))//bei rechtsklick soll ein agent spawnen
        {
            HandleInput();
        }
        if (Input.GetMouseButtonDown(0)) // bei linksklick sollen alle agents zum klickort laufen;
        {
            GroupAgents();
        }
        if (!abbruch)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                if (agents[i].transform.position == helpVar)
                {

                }
            }
        }

    }

    /// <summary>
    /// verarbeitet das den Spawn der neuen Agents
    /// </summary>
    void HandleInput()
    {

		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Vector3 newPos = hit.point;
            newPos.y = newPos.y + 1;
            Agent agent = Instantiate(prefab, newPos, Quaternion.identity, transform).GetComponent<Agent>();
            agent.GetComponent<Agent>().rpm = rpm;
            agents.Add(agent);
        }
		
    }
    /// <summary>
    /// sammelt alle agents an die mousklick position
    /// </summary>
    void GroupAgents()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Vector3 newPos = hit.point;
            abbruch = false;
            for (int i = 0; i < agents.Count; i++)
            {
                helpVar = hit.point;
                agents[i].SetDestination(helpVar);
            }
        }
    }
}
