using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


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


    private int vidas = 5;
    private float temporizador;
    private int score =  0;

    private ObjectPool<Disparo> pool;

    private bool inmune = false;

    public int Score { get => score; set => score = value; }

    private void Awake()
    {
        pool = new ObjectPool<Disparo>(CreateDisparo, null, ReleaseDisparo, DestroyDisparo);

    }


    // Start is called before the first frame update
    void Start()
    {
        escudo.SetActive(false);
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
        if(elOtro.gameObject.CompareTag("DisparoEnemigo") || elOtro.gameObject.CompareTag("Enemigo"))
        {
            if (inmune == false)
            {
                Destroy(elOtro.gameObject);
                if (vidas!=0){
                    vidas -= 1;
                }
                
                textoVidas.text = "Vidas: " + vidas;
                if (vidas == 0)
                {
                    Debug.Log("tengo 0 vidas");
                    Destroy(this.gameObject);
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
            for(int i = 0; i < 2; i++)
            {
                //Instantiate(disparoPrefab, canon1.transform.position, Quaternion.identity);
                //Instantiate(disparoPrefab, canon2.transform.position, Quaternion.identity);
                //temporizador = 0;
                Disparo copiaDisparo = pool.Get();
                copiaDisparo.gameObject.SetActive(true);
                copiaDisparo.transform.position = canones[i].transform.position;
            }
            temporizador = 0;
            
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
    
}
