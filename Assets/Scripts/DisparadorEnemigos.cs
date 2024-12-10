using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DisparadorEnemigos : MonoBehaviour
{
    [SerializeField] private Enemigo enemigoPrefab;
    [SerializeField] private float velocidadMov;
    [SerializeField] private TextMeshProUGUI textoOleada;
    [SerializeField] private Jefe myJefe;
    [SerializeField] private Player myPlayer;
    

    private Vector2 direccion = new Vector3(0, 1, 0);
    private Vector3 posicionSalida;
    private float numeroRandom;
    private int numNaves = 27;
    





    private bool ultimaOleada = false;
    public bool UltimaOleada { get => ultimaOleada; set => ultimaOleada = value; }
    public int NumNaves { get => numNaves; set => numNaves = value; }
    //public int EnemigoNumero { get => enemigoNumero; set => enemigoNumero = value; }

    private ObjectPool<Enemigo> enemyPool;


    private void Awake()
    {
        enemyPool = new ObjectPool<Enemigo>(CreateEnemigo, GetEnemigo, ReleaseEnemigo, DestroyEnemigo);
        //myJefe = FindObjectOfType<Jefe>();
    }

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
        enemigo.dispararR();
        
    }

    private void ReleaseEnemigo(Enemigo enemigo)
    {
        enemigo.gameObject.SetActive(false);
    }

    private void DestroyEnemigo(Enemigo enemigo)
    {
        Destroy(enemigo.gameObject);
    }



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DispararEnemigos());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DispararEnemigos()
    {
        for(int i = 0; i<3; i++)
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

        yield return new WaitForSeconds(2f);

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

        yield return new WaitForSeconds(4f);

        textoOleada.text = "Muy bien. Ahora te toca el jefe";
        yield return new WaitForSeconds(2f);
        
        textoOleada.gameObject.SetActive(false);


        myJefe.gameObject.SetActive(true);
        for (int i = 6; i <= 9; i++)
        {
            myJefe.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        

    }
}
