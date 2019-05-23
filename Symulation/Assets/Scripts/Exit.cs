using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// die app schließungs bedingung
/// </summary>
public class Exit : MonoBehaviour
{
    public void EndGame()
    {
        Application.Quit();
    }
}
