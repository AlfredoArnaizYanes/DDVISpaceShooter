using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Disparo : MonoBehaviour
{
    //Este scrpt creo que es identico al que se proponía en los videos de ayuda, aunque se implementó la piscina que recicla los disparos.
    [SerializeField] private float velocidad;
    [SerializeField] private Vector3 direccion;

    private float timer;

    private ObjectPool<Disparo> myPool;
    public ObjectPool<Disparo> MyPool { get => myPool; set => myPool = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= 4) 
        {
            timer = 0;
            myPool.Release(this);
        }
    }
}
