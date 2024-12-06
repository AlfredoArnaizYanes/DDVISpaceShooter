using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DisparadorEnemigos : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float velocidad;
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private float ratioCreacion;
    private float temporizador = 0f;


    private Vector2 direccion = new Vector3(0, 1, 0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movimiento();
     //   DelimitadorMovimiento();
        CambioDireccion();
        GeneraEnemigo();
    }
    void Movimiento()
    {
      
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    //void DelimitadorMovimiento()
    //{
    //    float restrinigidaY = Mathf.Clamp(transform.position.y, -4.2f, 4.2f);
    //    transform.position = new Vector3(transform.position.x, restrinigidaY, 0);
    //}
    void CambioDireccion()
    {
        if (transform.position.y > 4.2f || transform.position.y < -4.2f)
        {
            direccion *= -1;
        }
    }

    void GeneraEnemigo() 
    {
        temporizador += 1 * Time.deltaTime;
        Debug.Log(temporizador);

        if (temporizador >= ratioCreacion)
        {
            Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
            temporizador = 0;
        }
    }
}
