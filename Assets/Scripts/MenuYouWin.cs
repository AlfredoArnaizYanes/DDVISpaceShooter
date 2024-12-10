using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuYouWin : MonoBehaviour
{
    private Canvas transicion;
    public void Start()
    {
        transicion = GetComponent<Canvas>();
        StartCoroutine(cambioOrden());
    }
    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void exit()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    IEnumerator cambioOrden()
    {
        yield return new WaitForSeconds(1.5f);
        transicion.sortingOrder = 5;

    }
}
