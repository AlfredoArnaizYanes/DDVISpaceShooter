using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    [SerializeField] private float velocidad;

    //Variable que nos indica la dirección del movimiento de los Bonus
    private Vector3 direccion = Vector3.right * (-1);

    //Variable que controla el tiempo en pantalla de los Bonus
    private float timer = 0;

    //Variable que me indica si el Bonus tiene que parpadear porque va a desaparecer
    private bool parpadeando = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Los Bonus aparecen y se mueven hacia la izquierda, están dos segundos en pantalla y desaparecen.
    //El último medio segundo antes de desaparecer están parpadeando
    void Update()
    {

        transform.Translate(direccion* velocidad * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= 1.5  && parpadeando==false)
        {
            StartCoroutine(Intermitencia());
           
        }
        if (timer >= 2)
        {
            Destroy(this.gameObject);
            timer = 0;
        }
    }

    //Corrutina que hace que los Bonus parpadeen
    IEnumerator Intermitencia()
    {
        parpadeando = true;
        while (true)
        {
            GetComponent<SpriteRenderer>().GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
