using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
//using UnityEngine.Rendering;

public class BalaFuego : MonoBehaviour
{
    [SerializeField] float velocidad;

    //Variable que controla el tiempo de vida de las balas de fuego
    private float timerBF;


    //Preparamos la piscina...
    private ObjectPool<BalaFuego> myPoolBF;
    //... y la encapsulamos
    public ObjectPool<BalaFuego> MyPoolBF { get => myPoolBF; set => myPoolBF = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Las balas se mueven en su dirección (1,0,0) porque ya traen la rotación de fábrica
    void Update()
    {
        transform.Translate(new Vector3(1,0,0) * velocidad * Time.deltaTime);
        timerBF += Time.deltaTime;
        if (timerBF >= 4)
        {
            timerBF = 0;
            myPoolBF.Release(this);
        }
    }
} 
