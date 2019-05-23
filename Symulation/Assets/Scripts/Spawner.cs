using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Mesh mesh;
    List<GameObject> agents;
    public Vector3 goal;
    
    // Start is called before the first frame update
    void Start()
    {
        agents = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())//bei rechtsklick soll ein npc spawnen
        {
            HandleInput();
        }
        if (Input.GetMouseButtonDown(2)) // mit mittlerermaustaste wird ein neues ziel gewählt
        {
            SetGoal();
        }
    }
    void HandleInput()
    {

		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Vector3 newPos = hit.point;
            newPos.y = newPos.y + 1;
            GameObject agent = Instantiate(prefab, newPos, Quaternion.identity, transform);
            agents.Add(agent);
        }
		
    }
    void SetGoal()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Vector3 newPos = hit.point;
            newPos.y = newPos.y + 1;
            goal = newPos;
        }
    }
}
