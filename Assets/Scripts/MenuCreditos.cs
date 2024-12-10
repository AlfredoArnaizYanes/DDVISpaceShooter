using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCreditos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("FinCreditos", 30);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space)) 
        {
            SceneManager.LoadScene("MenuInicial");
        }
    }

    void FinCreditos()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}
