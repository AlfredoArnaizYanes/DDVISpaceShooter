using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private Transform[] canones;
    [SerializeField] private GameObject apariencia;
    [SerializeField] private TextMeshProUGUI textoVidas;
    [SerializeField] private TextMeshProUGUI textoScore;
    [SerializeField] private GameObject escudo;
    [SerializeField] private AudioClip audioDisparo;
    [SerializeField] private AudioClip audioDano;
    [SerializeField] private Jefe myJefe;

    //Variable para poder instanciar los disparos en tres direcciones distinta cuando se ha recogido el bonus de Disparo Extra
    private Vector3[] rotacionesDisparos =new[] { new Vector3(0, 0, 45), new Vector3(0, 0, -45), new Vector3(0, 0, 0) };

    //Variable para manejar el AudioSource
    private AudioSource componenteAudio;

    //Variable que denota la posición a la que debe ir el player si gana la partida en un intento de pequeña "cinemática" final
    private Vector3 posicionfinal = new Vector3(12, 0, 0);
    
    //Número de vidas del player
    private int vidas = 10;

    //Temporizador para controlar el ratio de disparo
    private float temporizador;

    //Score del player
    private int score =  0;

    //Variable booleana para saber cuando el Player ha cogido el bonus de Disparo Extra
    private bool tengoDisparoExtra = false;

    //ObjectPool para reciclar los disparos
    private ObjectPool<Disparo> pool;

    //Variable booleana para ser inmune durante un corto espacio de tiempo despues de recibir daño
    private bool inmune = false;

    //Variable para reducir la velocidad del player al final del juego en la pequeña "cinemática" y que no apareciera el valor 0.5 como un número fantasma
    private float reductorVelocidad = 0.5f;

    //Encapsulamos la variable Score para que sea accesible desde los Scripts Enemigos y DisparadorEnemigos
    public int Score { get => score; set => score = value; }

    //Encapsulamos el GameObject Escudo para poder acceder a su estado desde el Script Enemigo
    public GameObject Escudo { get => escudo; set => escudo = value; }

    //Encapsulamos la variable TengoDisparoExtra para poder acceder a ella desde el Script Enemigo
    public bool TengoDisparoExtra { get => tengoDisparoExtra; set => tengoDisparoExtra = value; }

   

    //Preparamos el ObjectPool para los Disparos
    private void Awake()
    {
        pool = new ObjectPool<Disparo>(CreateDisparo, null, ReleaseDisparo, DestroyDisparo);
        
    }


    // Al iniciar el juego...
    void Start()
    {
        //... se oculta el escudo,
        escudo.SetActive(false);
        //... se prepara el componente que va a manejar el audio del Player 
        componenteAudio = GetComponent<AudioSource>();
        //y se muestra el texto de vidas del Player, ya que no se actualizará tan a menudo como el texto de Score.
        textoVidas.text = "Vidas: " + vidas;
    }

    // El juego discurre de la siguiente forma...
    void Update()
    {
        //Se llama al método que permite mover al player...
        Movimiento();
        //... y se delimita para no salirnos de la pantalla.
        DelimitadorMovimiento();
        //Se llama al método Disparar...
        Disparar();
        //Se presenta el texto de Score en pantalla.
        textoScore.text = "Score: " + score;

        //Si nos quedamos sin vidas cargamos la escena del MenúGameOver
        if (vidas <= 0)
        {
            SceneManager.LoadScene("MenuGameOver");
        }
        
        //Si ganamos al jefe llamamos a la corutina que maneja la pequeña "cinemática"
        if (myJefe.Ganaste == true) 
        {
            StartCoroutine(TratamientoFinalJuego());
        }

    }

    //MÉTODOS PARA MANEJAR LA PISCINA (Tal y como vimos en clase)

    private Disparo CreateDisparo()
    {
        Disparo copiaDisparo = Instantiate(disparoPrefab, transform.position, Quaternion.identity);
        copiaDisparo.MyPool = pool;
        return copiaDisparo;
    }

    private void ReleaseDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }

    private void DestroyDisparo(Disparo disparo)
    {
        Destroy(disparo.gameObject);
    }
    //FIN DE LOS MÉTODOS PARA MANEJAR LA PISCINA


    //Manejamos las colisiones con el Player
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //Si colisionamos con objetos que nos hacen daño
        if (elOtro.gameObject.CompareTag("DisparoEnemigo") || elOtro.gameObject.CompareTag("Enemigo") || 
            elOtro.gameObject.CompareTag("BalaJefeGrande")|| elOtro.gameObject.CompareTag("BalaFuegoJefe"))
        {
            //Esta condición se explica en la Memoria del Proyecto, pero digamos que es para saber si estamos en condiciones de recibir daño
            if (inmune == false && myJefe.Ganaste==false)
            {
                componenteAudio.PlayOneShot(audioDano);
                Destroy(elOtro.gameObject);
                //Esta condición fue necesaria ya bajo determinadas circunstancias despues de perder se mostraba en la pantalla que teníamos -1 vidas
                //Asi que solo se restan vidas si nuestro valor de vidas es positivo
                if (vidas != 0)
                {
                    vidas -= 1;
                }

                textoVidas.text = "Vidas: " + vidas;
                if (vidas == 0)
                {
                    //Damos tiempo a que finalicen bien todas las acciones que involucran al player, que si se hacía de golpe en algun caso mostraban un error por la consola
                    Destroy(this.gameObject, 2f);
                }
                else
                {
                    //Si las vidas no son cero y estamos en condiciones de recibir daño, lo gestionamos con la Corutina ReciboDanoSoyInmune
                    StartCoroutine(ReciboDanoSoyInmune());
                }

            }
            //Si no estamos en condición de recibir daño, destruimos el objeto con el que colisionamos, y tan tranquilos
            else
            {
                Destroy(elOtro.gameObject);
            }

        }
        //Si colixionamos con Objetos que nos dan Bonus
        //Bonus Escudo
        else if (elOtro.gameObject.CompareTag("Escudo"))
        {
            Destroy(elOtro.gameObject);
            StartCoroutine(TratamientoEscudo());
        }
        //Bonus Disparo Extra
        else if (elOtro.gameObject.CompareTag("DisparoExtra"))
        {
            Destroy(elOtro.gameObject);
            tengoDisparoExtra = true;
            StartCoroutine(TratamientoDisparoExtra());

        }
        //Bonus Vida Extra
        else if (elOtro.gameObject.CompareTag("VidaExtra"))
        {
            Destroy(elOtro.gameObject);
            vidas += 1;
            textoVidas.text = "Vidas: " + vidas;

        }
        
    }

    //GESTIONAMOS EL MOVIMIENTO DEL PLAYER
    void Movimiento()
    {
       //Condición que hace que cuando ganemos dejemos de tener control sobre el Player con el teclado, y este pueda emprender su pequeña "cinemática" que lo lleva hasta el extremo derecho de la pantalla 
       if (myJefe.Ganaste == false)
        {
            float inputH = Input.GetAxisRaw("Horizontal");
            float inputV = Input.GetAxisRaw("Vertical");
            transform.Translate(new Vector2(inputH, inputV).normalized * velocidad * Time.deltaTime);
        }
        
    }

    void DelimitadorMovimiento()
    {
        //Lo mismo que con el movimiento, para que una vez que ganemos, el player pueda desaparecer por el extremo derecho de la pantalla
        if (myJefe.Ganaste == false)
        {
            float restrinigidaX = Mathf.Clamp(transform.position.x, -8f, 8f);
            float restrinigidaY = Mathf.Clamp(transform.position.y, -4.2f, 4.2f);
            transform.position = new Vector3(restrinigidaX, restrinigidaY, 0);
        }
            
    }
    //FIN DE GESTIONAMOS EL MOVIMIENTO DEL PLAYER

    //Método que gestiona los disparos del Player 
    void Disparar()
    {
        temporizador += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && temporizador >= ratioDisparo)
        {
            //Al disparar hacemos ruido
            componenteAudio.PlayOneShot(audioDisparo);

            //Si no tengo Disparo Extra, disparamos por dos cañones
            if (!tengoDisparoExtra)
            {
                for (int i = 0; i < 2; i++)
                {
                    
                    Disparo copiaDisparo = pool.Get();
                    copiaDisparo.gameObject.SetActive(true);
                    copiaDisparo.transform.position = canones[i].transform.position;
                    copiaDisparo.transform.eulerAngles = new Vector3 (0, 0, 0); 
                }
                
                temporizador = 0;
            }
            //Si tengo Disparo Extra, disparamos por 3 cañones, con los cañones laterales disparando en dos direciiones oblicuas a la horizontal
            else if (tengoDisparoExtra)
            {
               
                for (int i = 0; i < 3; i++)
                {
                    
                    Disparo copiaDisparo = pool.Get();
                    copiaDisparo.gameObject.SetActive(true);
                    copiaDisparo.transform.position = canones[i].transform.position;
                    copiaDisparo.transform.eulerAngles = rotacionesDisparos[i];
                    
                }
                
                temporizador = 0;
            }
        }
    }

    //Corrutina que cambia el color de nuestra nave al recibir daño y durante ese tiempo somos inmune al daño
    IEnumerator ReciboDanoSoyInmune ()
    {
        inmune = true;
        apariencia.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        apariencia.GetComponent<SpriteRenderer>().color = Color.white;
        inmune = false;
    }

    //Corrutina para manejar lo que ocurre cuando colisionamos con el Bonus Escudo
    IEnumerator TratamientoEscudo()
    {
        escudo.SetActive(true);
        yield return new WaitForSeconds(6f);
        escudo.SetActive(false);
    }

    //Corrutina para manejar lo que ocurre cuando colisionamos con el Bonus Disparo Extra
    IEnumerator TratamientoDisparoExtra()
    {
        tengoDisparoExtra = true;
        yield return new WaitForSeconds(6f);
        tengoDisparoExtra = false;
    }

    //Corrutina que maneja la pequeña "cinematica" que hace que la nave se mueva librementa hacia la derecah de la pantalla
    //una vez que hemos ganado y despues carga el menú YouWin
    IEnumerator TratamientoFinalJuego() 
    {
        transform.Translate((posicionfinal - transform.position).normalized * velocidad * reductorVelocidad * Time.deltaTime);
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("MenuYouWin");
    }



}
        
        


    

