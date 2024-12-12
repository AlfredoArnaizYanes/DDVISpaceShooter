using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuYouWin : MonoBehaviour
{
    private Canvas myCanvas;
    public void Start()
    {
        myCanvas = GetComponent<Canvas>();
        StartCoroutine(cambioOrden());
    }
    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void exit()
    {
        Application.Quit();
    }

    IEnumerator cambioOrden()
    {
        yield return new WaitForSeconds(1.5f);
        myCanvas.sortingOrder = 5;

    }
}
