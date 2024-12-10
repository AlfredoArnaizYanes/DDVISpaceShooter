//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Jefe : MonoBehaviour
{
    [SerializeField] Transform[] canonesJefe;
    [SerializeField] GameObject balaNegraPrefab;
    [SerializeField] AudioClip explosion;
    [SerializeField] private Image barraVida;
    [SerializeField] private Image barraVidaColor;

    private float vidaJefe = 500;
    private float vidaMaxima = 500; 
    private int indice1;
    private int indice2;
    private AudioSource componenteAudio;

    // Start is called before the first frame update
    void Start()
    {
        componenteAudio = GetComponent<AudioSource>();
        StartCoroutine(DisparosJefe());
        barraVida.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (vidaJefe <= 0)
        {
            StartCoroutine(GranExplosion());    
        }

        barraVidaColor.fillAmount = vidaJefe / vidaMaxima;
    }

    IEnumerator DisparosJefe()
    {
        while(vidaJefe > 0)
        {
            indice1 = Random.Range(0, 4);
            indice2 = Random.Range(0, 4);
            while (indice2 == indice1)
            {
                indice2 = Random.Range(0, 4);
            }
            Instantiate(balaNegraPrefab, canonesJefe[indice1].transform.position, Quaternion.identity);
            Instantiate(balaNegraPrefab, canonesJefe[indice2].transform.position, Quaternion.identity);

            yield return new WaitForSeconds(2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            Destroy(elOtro.gameObject);
            vidaJefe -= 5;
            Debug.Log(vidaJefe);
        }
        
    }

    IEnumerator GranExplosion()
    {   
        for(int j=6;j<=9; j++)
        {
            transform.GetChild(j).transform.gameObject.SetActive(true);
        }
        for(int i= 0; i < 6; i++)
        {
            componenteAudio.PlayOneShot(explosion);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        if (vidaJefe <= 0)
        {
            SceneManager.LoadScene("MenuYouWin");
        }


    }
}
