using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Este escudo destruye todo lo malo
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoEnemigo") || elOtro.gameObject.CompareTag("Enemigo") ||
            elOtro.gameObject.CompareTag("BalaJefeGrande") || elOtro.gameObject.CompareTag("BalaFuegoJefe"))
        {
                Destroy(elOtro.gameObject);
        }
    }
}
