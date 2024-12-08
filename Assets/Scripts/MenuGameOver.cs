using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuGameOver : MonoBehaviour
{

    [SerializeField] private GameObject elMenu;
    private Player myPlayer;

    private void Start()
    {
        //Debug.Log("Lleg� hasta qu�1");
        //myPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Debug.Log("Lleg� hasta qu�");
        //myPlayer.MuerteJugador += ActivarMenu;
    }

    //private void ActivarMenu(object sender, EventArgs e)
    //{
    //    menuGameOver.SetActive(true);
    //}

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
}
