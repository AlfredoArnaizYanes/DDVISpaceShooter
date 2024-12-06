using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("Juego");
    }
    public void exit()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}
