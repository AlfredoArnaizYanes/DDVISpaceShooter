using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Disparo disparoEnemigoPrefab;
    [SerializeField] private GameObject canonEnemigo;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject escudoPrefab;
    [SerializeField] private GameObject disparoExtraPrefab;
    [SerializeField] private GameObject vidaExtraPrefab;
    
   


    private float timerLife = 0;
    private float probBonus;
    private ObjectPool<Disparo> pool;

    private DisparadorEnemigos generator;

    private Player myPlayer;

    //Declaración de la piscina para manejar las naves enemigas
    private ObjectPool<Enemigo> myEnemyPool;
    public ObjectPool<Enemigo> MyEnemyPool { get => myEnemyPool; set => myEnemyPool = value; }

    //Variable para manejar el AudioSource
    private AudioSource componenteAudioEnemigo;

    
    

    private void Awake()
    {
        pool = new ObjectPool<Disparo>(CreateDisparo, GetDisparo, ReleaseDisparo,DestroyDisparo);

        generator = FindObjectOfType<DisparadorEnemigos>();
        myPlayer = FindObjectOfType<Player>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Disparar());
        componenteAudioEnemigo = GetComponent<AudioSource>();

    }




    // Update is called once per frame
    void Update()
    {
        if (generator.UltimaOleada)
        {
            Movimiento();
            DelimitadorMovimiento();
        }
        else
        {
            transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
        }

        timerLife += Time.deltaTime;
        if (timerLife >= 6)
        {
            timerLife = 0;
            myEnemyPool.Release(this);
        }

    }

    //MÉTODOS PARA MANEJAR LA PISCINA DE DISPAROS
    private Disparo CreateDisparo()
    {
        Disparo copiaDisparo = Instantiate(disparoEnemigoPrefab, canonEnemigo.transform.position, Quaternion.identity);
        copiaDisparo.MyPool = pool;
        return copiaDisparo;
    }

    private void GetDisparo(Disparo disparo)
    {
        disparo.transform.position = canonEnemigo.transform.position;
        disparo.gameObject.SetActive(true);
        
    }

    private void ReleaseDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    private void DestroyDisparo(Disparo disparo)
    {
        Destroy(disparo.gameObject);
    }

    //FIN DE MÉTODOS PARA MANEJAR LA PISCINA

    //MÉTODOS PARA EL MOVIMIENTO NO LINEAL DE LOS ENEMIGOS
    void Movimiento()
    {
        
        transform.Translate(new Vector3(-1,Mathf.Cos(transform.position.x), 0) * velocidad * Time.deltaTime);
    }
    void DelimitadorMovimiento()
    {
       
        float restrinigidaY = Mathf.Clamp(transform.position.y, -4.2f, 4.2f);
        transform.position = new Vector3(transform.position.x, restrinigidaY, 0);
    }

    IEnumerator Disparar()
    {
        
        while (true)        
        {
            //Pedimos a la piscina que nos de un disparo, ella sabe si tiene que crearlo porque está vacia, llama al CreateDisparo, o si puede cogerlo porque hay en la
            //piscina, llama al GetDisparo
            pool.Get();
            yield return new WaitForSeconds(1f);
        }
    }

    public void dispararR()
    {
        StartCoroutine(Disparar());
    }


    //COLISIÓN DE LOS DISPAROS DEL JUGADOR CON LAS NAVES ENEMIGAS
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            Destroy(elOtro.gameObject);
            var copy = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(copy, 0.2f);
            
            Destroy(this.gameObject);

            myPlayer.Score += 10;
            generator.NumNaves -= 1;
            Debug.Log("Quedan: " + generator.NumNaves);
            
            
         
           
            probBonus = UnityEngine.Random.Range(0f, 1f);
            if (probBonus <= 0.2f && myPlayer.Escudo.activeInHierarchy==false)
            {
                Instantiate(escudoPrefab, transform.position, Quaternion.identity);
            }
            else if (probBonus < 0.4f && myPlayer.TengoDisparoExtra==false)
            {
                Instantiate(disparoExtraPrefab, transform.position, Quaternion.identity);
            }
            else if (probBonus > 0.9f)
            {
                Instantiate(vidaExtraPrefab, transform.position, Quaternion.identity);
            }

            
                

        }
    }
}
