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
    
    //Variable para controlar el tiempo de vida de los enemigos, si lo sobrepasamos los metemos en la piscina
    private float timerLife = 0;

    //Variable en la que guardaremos un valor Random que nos dirá si la nave enemiga nos proporciona un Bonus
    private float probBonus;

    //Creamos la piscina donde se almacenarán los disparos
    private ObjectPool<Disparo> pool;

    //Instancia de la clase DisparadorEnemigos para podernos comunicar con el DispardorEnemigos.
    //No me permitía añadirlo como un [SerialiField] al tratarse de un prefab que no está en la escena de juego 
    private DisparadorEnemigos generator;

    //Instancia de la clase Player para poder comunicarnos con el Player. Ocurre lo mismo que con el DisparadosEnemigos de la linea anterior
    private Player myPlayer;

    //Declaración de la piscina para manejar las naves enemigas...
    private ObjectPool<Enemigo> myEnemyPool;
    //... y la encapsulamos
    public ObjectPool<Enemigo> MyEnemyPool { get => myEnemyPool; set => myEnemyPool = value; }


    
    private void Awake()
    {
        //Preparamos la piscina de los disparos...
        pool = new ObjectPool<Disparo>(CreateDisparo, GetDisparo, ReleaseDisparo,DestroyDisparo);

        //... y localizamos los Objetos tipo DisparadorEnemigos y Player, que al ser únicos en el juego no da problemas, según leí por algún lado.
        //Me imagino que si hubiera varios objetos de ese tipo en el juego habrá otra manera de buscar uno en concreto mediante Tags o algo así.
        generator = FindObjectOfType<DisparadorEnemigos>();
        myPlayer = FindObjectOfType<Player>();
        
    }
    
    //Los enemigos no se lo piensan y nos disparan desde que aparecen en pantalla
    void Start()
    {
        StartCoroutine(Disparar());
    }




    // Update is called once per frame
    void Update()
    {
        //Si estamos en la última oleada de cada nivel, las naves cambian su forma de moverse. De ahí la necesidad de comunicarnos con el DisparadorEnemigos
        //Lo hacemos mediante la propiedad UltimaOleada
        if (generator.UltimaOleada)
        {
            //Método que implementa el movimiento sinusoidal de la última oleada de cada nivel
            Movimiento();
            DelimitadorMovimiento();
        }
        else
        {
            //Si no estamos en la última oleada las naves enemigas se mueven horizontalmente de derecha a izquierda
            transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
        }
        //Controlamos el tiempo de vida de los enemigos, para mandarlos a la piscina
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
        //Usamos la función matemáticas coseno  de manera que la componente y sea el coseno de la componente x
        //Así en su movimiento dibujan la gráfica de la función
        transform.Translate(new Vector3(-1,Mathf.Cos(transform.position.x), 0) * velocidad * Time.deltaTime);
    }
    void DelimitadorMovimiento()
    {
       
        float restrinigidaY = Mathf.Clamp(transform.position.y, -4.2f, 4.2f);
        transform.position = new Vector3(transform.position.x, restrinigidaY, 0);
    }
    //FIN DE MÉTODOS PARA EL MOVIMIENTO NO LINEAL DE LOS ENEMIGOS

    //A los enemigos les gusta dispararnos... y además reciclan sus disparos.
    IEnumerator Disparar()
    {
        while (true)        
        {
            pool.Get();
            yield return new WaitForSeconds(1f);
        }
    }

    //Las naves enemigas que volvían de la piscina de naves no disparaban, y está es la única solución que encontré para que lo hiciesen.
    //En la memoria del Proyecto describo con detalle el problema
    public void DispararR()
    {
        StartCoroutine(Disparar());
    }


    //COLISIÓN DE LOS DISPAROS DEL JUGADOR CON LAS NAVES ENEMIGAS
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //Si los disparos del Player colisonan con los enemigos...
        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            //... se destruyen...
            Destroy(elOtro.gameObject);
            //... instanciamos una explosión...
            var copy = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            //... que destruimos a los 0.2 segundos...
            Destroy(copy, 0.2f);
            //... y destruimos a la nave enemiga
            Destroy(this.gameObject);

            //Aumentamos la puntuación en 10 accediendo a la propiedad Score de la clase Player 
            myPlayer.Score += 10;
            
            //Generamos un float Random entre 0 y 1
            probBonus = UnityEngine.Random.Range(0f, 1f);

            //Dependiendo del valor de probBonus, instanciamos un bonus de algún tipo o no obtenemos bonus.
            //La necesidad de la segunda condición de este if se explica en la Memoria que se adjunta con este proyecto 
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
