//using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    //Variable que contabiliza la vida del Jefe
    private float vidaJefe = 500;

    //Variable que toma el valor de la vida máxima del jefe para poder gestionar la barra de vida
    private float vidaMaxima = 500; 

    //Variables que recogerán un valor aleatorio para que los disparos del jefe se instancien en diferentes puntos
    private int indice1;
    private int indice2;
    
    //Variable para manejar el sonido de destrucción del Jefe
    private AudioSource componenteAudio;

    //Variable que avisa de que el jefe ha sido derrotado al script Player 
    private bool ganaste = false;

    //Posición en la que se detiene el jefe al aparecer en pantalla
    private Vector3 posFinal = new Vector3 (2.75f,-0.7f,0f);

    //Velocidad de desplazamiento del jefe al aparecer en pantalla
    private float velocidadJefe = 2f;

    //Encapsulamos la varable Ganaste para que puedan acceder a ella el script Player
    public bool Ganaste { get => ganaste; set => ganaste = value; }

    // Start is called before the first frame update
    void Start()
    {
        //Al aparecer el jefe, accedemos a la componente de audio del mismo...
        componenteAudio = GetComponent<AudioSource>();
        //... empiezan los disparos...
        StartCoroutine(DisparosJefe());
        //... y se muestra la barra de vida
        barraVida.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //El jefe se mueves desde fuera de la pantalla a su posición final y allí se para.
        if((posFinal - transform.position).magnitude > 0.1){
            transform.Translate((posFinal - transform.position).normalized * velocidadJefe * Time.deltaTime);
        }
            
        
        //Cuanod el jefe se queda sin vida llamamos a la corrutina Gran Explosión
        if (vidaJefe <= 0)
        {
            StartCoroutine(GranExplosion());    
        }

        //Vamos actualizando la barra de vida
        barraVidaColor.fillAmount = vidaJefe / vidaMaxima;
    }

    IEnumerator DisparosJefe()
    {
        //Mientras le quede un aliento de vida el jefe está disparando
        while(vidaJefe > 0)
        {
            //Una bala se instancia en el cañón 0 o 1, mitad superior de la pantalla
            indice1 = Random.Range(0, 2);
            //Otra bala se instancia en los cañones 2 o 3, mitad inferior de la pantalla
            indice2 = Random.Range(2, 4);
            //Y siempre se instancia una bala en el cañón 4 que está justo en el ecuador del jefe 
            Instantiate(balaNegraPrefab, canonesJefe[indice1].transform.position, Quaternion.identity);
            Instantiate(balaNegraPrefab, canonesJefe[4].transform.position, Quaternion.identity);
            Instantiate(balaNegraPrefab, canonesJefe[indice2].transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);
        }
    }

    //El jefe recibe 5 de daño por cada disparo del Player
    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("DisparoPlayer"))
        {
            Destroy(elOtro.gameObject);
            vidaJefe -= 5;
        }
        
    }
    //La gran explosión
    IEnumerator GranExplosion()
    {   
        for(int j=6;j<=9; j++)
        {
            //Aparecen las cuatro llamas sobre el jefe, que son imágenes hijas en la jerarquía.
            //No sé qué es mejor, si esto, o haberlas instancido como prefabs en este momento
            transform.GetChild(j).transform.gameObject.SetActive(true);
        }

        //sonido de explosión
        componenteAudio.PlayOneShot(explosion);
        //Ligera espera
        yield return new WaitForSeconds(1f);
        //Ganó el Player
        ganaste = true;
        //Desaparecemos la barra de vida de la escena final
        barraVida.gameObject.SetActive(false);
        //Destruimos al jefe
        Destroy(this.gameObject,1.5f);


    }
}
