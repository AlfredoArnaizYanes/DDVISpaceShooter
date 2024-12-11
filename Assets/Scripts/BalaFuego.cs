using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class BalaFuego : MonoBehaviour
{
    [SerializeField] float velocidad;

    private UnityEngine.Pool.ObjectPool<BalaFuego> myPoolBF;

    private float timerBF;

    public UnityEngine.Pool.ObjectPool<BalaFuego> MyPoolBF { get => myPoolBF; set => myPoolBF = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
