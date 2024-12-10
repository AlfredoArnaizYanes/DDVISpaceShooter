using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscudoJefe : MonoBehaviour
{
    [SerializeField] private float velocidad;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool bajo = true;
        transform.Rotate(0, 0, velocidad * Time.deltaTime);
        //Debug.Log(transform.eulerAngles.z);
        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 310 && bajo == true)
        {
            bajo = false;
            velocidad *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            Destroy(elOtro.gameObject);
        }
    }
}
