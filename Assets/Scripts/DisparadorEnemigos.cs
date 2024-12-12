using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DisparadorEnemigos : MonoBehaviour
{
    [SerializeField] private Enemigo enemigoPrefab;
    //[SerializeField] private float velocidadMov;
    [SerializeField] private TextMeshProUGUI textoOleada;
    [SerializeField] private Jefe myJefe;
    [SerializeField] private Player myPlayer;
   
    

    //private Vector2 direccion = new Vector3(0, 1, 0);

    //Variable donde almacenar la posición aleatoria de salida de las naves enemigas
    private Vector3 posicionSalida;
    
    //private float numeroRandom;
    //private int numNaves = 27;
    //private int tipo = 0;
    
    //Variable booleana que nos indica si estamos en la última oleada de cada nivel
    private bool ultimaOleada = false;
    //La encapsulamos para que pueda acceder a ella la clase Enemigo y así las naves puedan variar su movimiento cuando esté activada
    public bool UltimaOleada { get => ultimaOleada; set => ultimaOleada = value; }

    //Creamos la piscina donde se almacenarán las naves enemigas
    private ObjectPool<Enemigo> enemyPool;


    private void Awake()
    {
        enemyPool = new ObjectPool<Enemigo>(CreateEnemigo, GetEnemigo, ReleaseEnemigo, DestroyEnemigo);
    }

    //MËTODOS PARA MANEJAR LA PISCINA DE NAVES ENEMIGAS
    private Enemigo CreateEnemigo()
    {
        Enemigo copiaEnemigo = Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
        copiaEnemigo.MyEnemyPool = enemyPool;
        return copiaEnemigo;

        
    }

    private void GetEnemigo(Enemigo enemigo)
    {
        enemigo.transform.position = posicionSalida;
        enemigo.gameObject.SetActive(true);
        //Inclusión en el Get() del método DispararR() para que las naves que se rescaten de la piscina tambien disparen.
        //En la Memoria que acompaña a este proyecto se habla de este problema
        enemigo.DispararR();
        
    }

    private void ReleaseEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(false);
    }

    private void DestroyEnemigo(Enemigo enemigo)
    {
        Destroy(enemigo.gameObject);
    }
    //FIN DE MÉTODOS PARA MANEJAR LA PISCINA


    //Nada más empezar el DisparadorEnemigos se pone a trabajar creando enemigos
    void Start()
    {
        StartCoroutine(DispararEnemigos());
    }

    //
    void Update()
    {
        
    }
    //Corrutina que genera los enemigos... y lleva la narrativa del juego
    IEnumerator DispararEnemigos()
    {
        //Fase inicial: 3 niveles con tres oleadas cada uno y 3 naves por oleada.
        for(int i=0; i<3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                textoOleada.text = "Nivel " + (i + 1) + " - Oleada " + (j + 1);  
                for (int k = 0; k < 3; k++)
                {
                    if (j == 2)
                    {
                        ultimaOleada = true;
                    }
                    posicionSalida = new Vector3(transform.position.x, Random.Range(-4.2f, 4.2f), 0);
                    //tipo = i;

                    //Instantiate(enemigoPrefab, posicionSalida, Quaternion.identity);

                    enemyPool.Get();
                    //enemigoNumero += 1;
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(3f);
            ultimaOleada = false;
        }

        //Damos paso a la segunda parte del juego. Mata naves hasta que llegues a los 500 puntos de Score
        if (myPlayer.Score <= 500)
        {
            textoOleada.text = "Creo que tienes que mejorar tu puntería. Consigue 500 puntos";
        }
        

        while (myPlayer.Score<500)
        {
                posicionSalida = new Vector3(transform.position.x, Random.Range(-4.2f, 4.2f), 0);
                enemyPool.Get();
                yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);

        //Y ahora llega el JefeFinal

        textoOleada.text = "Muy bien. Ahora te toca el jefe";
        yield return new WaitForSeconds(2f);
        
        textoOleada.gameObject.SetActive(false);


        myJefe.gameObject.SetActive(true);

        //Pero escondemos las llamas que saldrán cuando lo derrotemos
        for (int i = 7; i <= 10; i++)
        {
            myJefe.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        

    }
}
