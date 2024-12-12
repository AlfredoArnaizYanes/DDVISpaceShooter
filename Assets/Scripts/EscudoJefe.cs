using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscudoJefe : MonoBehaviour
{
    [SerializeField] private float velocidad;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {   
        
        //El escudo se mueve arriba y abajo
        transform.Rotate(0, 0, velocidad * Time.deltaTime);
        //Dejo esta línea porque tuve que estar viendo los valores de la rotación del escudo por consola para delimitar bien su movimiento
        //Debug.Log(transform.eulerAngles.z);
        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 310)// && bajo == true)
        {
            velocidad *= -1;
        }
    }

    //Este maldito escudo protege al Jefe de nuestros disparos
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            Destroy(elOtro.gameObject);
        }
    }
}
