using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class ProfeDisparadoEnemigos : MonoBehaviour
{
    [SerializeField] private Enemigo enemigoPrefab;
    [SerializeField] private float velocidadMov;
    [SerializeField] private TextMeshProUGUI textoOleada;

    private Vector2 direccion = new Vector3(0, 1, 0);
    private Vector3 posicionSalida;
    private float numeroRandom;

    private bool ultimaOleada = false;
    public bool UltimaOleada { get => ultimaOleada; set => ultimaOleada = value; }

    private ObjectPool<Enemigo> enemyPool;


    private void Awake()
    {
        enemyPool = new ObjectPool<Enemigo>(CreateEnemigo, GetEnemigo, ReleaseEnemigo, DestroyEnemigo);
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
        for(int i = 0; i<5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                textoOleada.text = "Nivel " + (i + 1) + " - Oleada " + (j + 1);  
                for (int k = 0; k < 10; k++)
                {
                    if (j == 2)
                    {
                        ultimaOleada = true;
                    }
                    posicionSalida = new Vector3(transform.position.x, Random.Range(-4.2f, 4.2f), 0);
                    
                    //Instantiate(enemigoPrefab, posicionSalida, Quaternion.identity);
                    enemyPool.Get();
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(3f);
            ultimaOleada = false;
        }

        
        
        
    }
}
