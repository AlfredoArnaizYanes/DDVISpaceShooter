using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BalasNegras : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Transform[] direccionesDisparo;
    [SerializeField] private BalaFuego balaFuegoPrefab;

    //Variable donde recogeremos la trayectoria de la BalaNegra
    private Vector3 direccion;

    //Variable que lleva el tiempo que lleva viviendo la BalaNegra
    private float timer = 0;

    //Varaible para gestionar el parpadeo antes de la explosión 
    private bool parpadeando = false;

    //Variable donde recogeremos el tiempo de vida de cada BalaNegra
    private float tiempoRandom;

    //Variable donde recogeremos la componente "y" de la dirección que tomará la BalaNegra 
    private float componenteYRandom;


    //Creamos la piscina donde se almacenarán las balas de fuego
    private ObjectPool<BalaFuego> poolBF;


    //Preparamos el ObjectPool para las balas de fuego
    private void Awake()
    {
        poolBF = new ObjectPool<BalaFuego>(CreateBalaFuego, null, ReleaseBalaFuego, DestroyBalaFuego);
    }
    

    //MÉTODOS PARA MANEJAR LA PISCINA DE LAS BOLAS DE FUEGO
    private BalaFuego CreateBalaFuego()
    {
        BalaFuego copiaBalaFuego = Instantiate(balaFuegoPrefab, transform.position, Quaternion.identity);
        copiaBalaFuego.MyPoolBF = poolBF;
        return copiaBalaFuego;
    }

    private void ReleaseBalaFuego(BalaFuego bFuego)
    {
        bFuego.gameObject.SetActive(false);
    }

    private void DestroyBalaFuego(BalaFuego bFuego)
    {
        Destroy(bFuego.gameObject);
    }
    // FIN DE LOS MÉTODOS PARA MANEJAR LA PISCINA DE LAS BOLAS DE FUEGO

    // Start is called before the first frame update
    void Start()
    {
        //Determinamos el tiempo de vida y la dirección de la bala negra
        tiempoRandom = Random.Range(0.5f, 1.5f);
        componenteYRandom = Random.Range(-1f, 1f);
        direccion = new Vector3(-1, componenteYRandom, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //La bala negra se mueve, parpadea y explota
        transform.Translate(direccion * velocidad * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= tiempoRandom-0.5 && parpadeando == false)
        {
            StartCoroutine(Intermitencia());

        }
        if (timer >= tiempoRandom)
        {
            Destroy(this.gameObject);
            Explosion();
            timer = 0;
        }
    }

    //Manejamos la intermitencia de la bala negra antes de explotar
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


   
    private void Explosion()
    {
        //La bala negra explota e instancia 7 pequeñas balas que van hacia el Player()
        for (int i = 0; i <= 6; i++)
        {
            //Dejo esta linea de código porque creo que pueda ser útil en el futuro, para instanciar objetos con una rotación que no sea Quaternion.Identity
            //Instantiate(balaFuegoPrefab, direccionesDisparo[i].transform.position, Quaternion.Euler(direccionesDisparo[i].transform.eulerAngles.x, direccionesDisparo[i].transform.eulerAngles.y, direccionesDisparo[i].transform.eulerAngles.z));
            BalaFuego copiaBalaFuego = poolBF.Get();
            copiaBalaFuego.gameObject.SetActive(true);
            copiaBalaFuego.transform.position = direccionesDisparo[i].transform.position;
            copiaBalaFuego.transform.eulerAngles = new Vector3 (direccionesDisparo[i].transform.eulerAngles.x, direccionesDisparo[i].transform.eulerAngles.y, direccionesDisparo[i].transform.eulerAngles.z);

        }
    }
    
}
