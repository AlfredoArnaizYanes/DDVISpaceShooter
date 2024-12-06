using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{

    [SerializeField] private float velocidad;
    private Vector3 direccion = Vector3.right * (-1);
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direccion* velocidad * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= 1.5)
        {
            Destroy(this.gameObject);
            timer = 0;
        }
    }
}
