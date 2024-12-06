using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
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
        //transform.Translate(new Vector3(-1,0,0)*velocidad*Time.deltaTime);
        //if(transform.position.x <= -22.73f)
        //{

            transform.position = posicionInicial + direccion*((velocidad * Time.time) % anchoImagen);
        //}
    }
}
