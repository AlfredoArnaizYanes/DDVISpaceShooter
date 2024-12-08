using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuGameOver : MonoBehaviour
{

    private Canvas transicion;

    
    public void Start()
    {
        transicion = GetComponent<Canvas>();
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
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    IEnumerator cambioOrden()
    {
        yield return new WaitForSeconds(1.5f);
        transicion.sortingOrder = 5;

    }
}
