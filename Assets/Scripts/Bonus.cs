using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    [SerializeField] private float velocidad;
    private Vector3 direccion = Vector3.right * (-1);
    private float timer = 0;
    private bool parpadeando = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
