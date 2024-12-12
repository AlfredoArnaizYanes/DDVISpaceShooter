using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    //Este scrpt creo que es identico al que se proponía en los videos de ayuda.
    [SerializeField] private float velocidad;
    [SerializeField] private float anchoImagen;

    private Vector3 posicionInicial;
    private Vector3 direccion = new Vector3(-1,0,0);

    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = posicionInicial + direccion*((velocidad * Time.time) % anchoImagen);        
    }
}
