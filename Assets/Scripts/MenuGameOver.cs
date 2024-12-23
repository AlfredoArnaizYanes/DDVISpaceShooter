using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuGameOver : MonoBehaviour
{

    private Canvas myCanvas;

    
    public void Start()
    {
        myCanvas = GetComponent<Canvas>();
        StartCoroutine(cambioOrden());
    }
    public void Reiniciar()
    {
        SceneManager.LoadScene("Juego");
    }

    public void MenuInicial(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
        Application.Quit();
    }
    IEnumerator cambioOrden()
    {
        yield return new WaitForSeconds(1.5f);
        myCanvas.sortingOrder = 5;

    }
}
