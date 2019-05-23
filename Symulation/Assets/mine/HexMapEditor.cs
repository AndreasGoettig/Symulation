using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;

    public HexGrid hexGrid;

    private Color activeColor;

    void Awake()
    {
        SelectColor(0); //wählt eine farbe
    }

    void Update()
    {
        //user input
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
    }
    /// <summary>
    /// holt sich die zelle auf die der spieler geklickt hat
    /// </summary>
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            hexGrid.ColorCell(hit.point, activeColor);
        }
    }

    /// <summary>
    /// setzt die acive farbe aus der liste
    /// </summary>
    /// <param name="index"></param>
    public void SelectColor(int index)
    {
        activeColor = colors[index];
    }
}

