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

    private Vector3[] rotacionesDisparos =new[] { new Vector3(0, 0, 45), new Vector3(0, 0, -45), new Vector3(0, 0, 0) };

    //Variable para manejar el AudioSource
    private AudioSource componenteAudio;

    private int vidas = 5;
    private float temporizador;
    private int score =  0;
    private bool tengoDisparoExtra = false;
    private ObjectPool<Disparo> pool;

    private bool inmune = false;

    public int Score { get => score; set => score = value; }
    public GameObject Escudo { get => escudo; set => escudo = value; }
    public bool TengoDisparoExtra { get => tengoDisparoExtra; set => tengoDisparoExtra = value; }

   


    private void Awake()
    {
        pool = new ObjectPool<Disparo>(CreateDisparo, null, ReleaseDisparo, DestroyDisparo);
        
    }


    // Start is called before the first frame update
    void Start()
    {
        escudo.SetActive(false);
        componenteAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
        DelimitadorMovimiento();
        Disparar();
        textoScore.text = "Score: " + score;
        
    }

    //MÉTODOS PARA MANEJAR LA PISCINA

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

    private void OnTriggerEnter2D (Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoEnemigo") || elOtro.gameObject.CompareTag("Enemigo"))
        {
            if (inmune == false)
            {
                componenteAudio.PlayOneShot(audioDano);
                Destroy(elOtro.gameObject);
                if (vidas != 0) {
                   
                    vidas -= 1;
                }

                textoVidas.text = "Vidas: " + vidas;
                if (vidas == 0)
                {
                    Destroy(this.gameObject, 2f);
                    SceneManager.LoadScene("MenuGameOver");

                }
                else
                {
                    StartCoroutine(ReciboDanoSoyInmune());
                }

            }
            else
            {
                Destroy(elOtro.gameObject);
            }

        }
        else if (elOtro.gameObject.CompareTag("Escudo"))
        {
            Destroy(elOtro.gameObject);
            StartCoroutine(TratamientoEscudo());
        }
        else if (elOtro.gameObject.CompareTag("DisparoExtra"))
        {
            Destroy(elOtro.gameObject);
            tengoDisparoExtra = true;
            StartCoroutine(TratamientoDisparoExtra());
             
        }

    }

    void Movimiento()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(inputH, inputV).normalized * velocidad * Time.deltaTime);
    }

    void DelimitadorMovimiento()
    {
        float restrinigidaX = Mathf.Clamp(transform.position.x, -8f, 8f);
        float restrinigidaY = Mathf.Clamp(transform.position.y, -4.2f, 4.2f);
        transform.position = new Vector3(restrinigidaX, restrinigidaY, 0);
    }

    void Disparar()
    {
        temporizador += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && temporizador >= ratioDisparo)
        {
            componenteAudio.PlayOneShot(audioDisparo);
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

    //Corrutina que cambia el color de nuestra nave al recibir daño
    IEnumerator ReciboDanoSoyInmune ()
    {
        inmune = true;
        apariencia.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        apariencia.GetComponent<SpriteRenderer>().color = Color.white;
        inmune = false;
    }

    IEnumerator TratamientoEscudo()
    {
        escudo.SetActive(true);
        yield return new WaitForSeconds(6f);
        escudo.SetActive(false);
    }
    IEnumerator TratamientoDisparoExtra()
    {
        tengoDisparoExtra = true;
        yield return new WaitForSeconds(6f);
        tengoDisparoExtra = false;
    }
}
        
        


    

