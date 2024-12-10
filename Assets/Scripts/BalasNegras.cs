using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalasNegras : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Transform[] direccionesDisparo;
    [SerializeField] private GameObject balaFuegoPrefab;
    private Vector3 direccion;
    private float timer = 0;
    private bool parpadeando = false;
    private float tiempoRandom;
    private float componenteYRandom;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        tiempoRandom = Random.Range(1.5f, 3.5f);
        componenteYRandom = Random.Range(-3f, 3f);
        direccion = new Vector3(-1, componenteYRandom, 0);

        transform.Translate(direccion * velocidad * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= tiempoRandom-0.5 && parpadeando == false)
        {
            StartCoroutine(Intermitencia());

        }
        if (timer >= tiempoRandom)
        {
            Destroy(this.gameObject);
            
            StartCoroutine(ExplosionBomba());
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
    IEnumerator ExplosionBomba()
    {
        for (int i = 0; i <= 6; i++)
        {
            Instantiate(balaFuegoPrefab, direccionesDisparo[i].transform.position, Quaternion.Euler(direccionesDisparo[i].transform.eulerAngles.x, direccionesDisparo[i].transform.eulerAngles.y, direccionesDisparo[i].transform.eulerAngles.z));
           
        }
        yield return new WaitForSeconds(2f);

    }
    
}
