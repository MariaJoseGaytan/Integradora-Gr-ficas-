using System.Collections;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject YELLOWCARD;
    public GameObject REDCARD;

    public float intervalo = 1f;
    public int tarjetasRojasPorFase = 2;
    public int tarjetasAmarillasPorFase2 = 3;

    public float rangoDeSpawnY = 5f; 
    public float tamanoFinal = 0.05f;

    private Camera camara;

    private int tarjetasRojasEnEscena = 0;
    private int tarjetasAmarillasEnEscena = 0;

   
    private float minX = -18f; // Poste izquierdo 
    private float maxX = 23f; // Poste derecho 
    void Start()
    {
        camara = Camera.main;
        StartCoroutine(CicloDeTarjetas());
    }

    IEnumerator CicloDeTarjetas()
    {
        while (true)
        {
            // Fase 1: solo tarjetas amarillas, velocidad 10
            float duracionFase1 = 10f;
            float tiempoInicio = Time.time;
            int tarjetasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase1 && tarjetasLanzadas < 5)
            {
                CrearTarjeta(YELLOWCARD, 10f);
                tarjetasLanzadas++;
                yield return new WaitForSeconds(intervalo);
            }

            // Fase 2: tarjetas amarillas y rojas, velocidad 15
            float duracionFase2 = 10f;
            tiempoInicio = Time.time;
            int rojasLanzadas = 0;
            int amarillasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase2 && (amarillasLanzadas < tarjetasAmarillasPorFase2 || rojasLanzadas < tarjetasRojasPorFase))
            {
                if (amarillasLanzadas < tarjetasAmarillasPorFase2)
                {
                    CrearTarjeta(YELLOWCARD, 15f);
                    amarillasLanzadas++;
                }
                
                if (rojasLanzadas < tarjetasRojasPorFase)
                {
                    CrearTarjeta(REDCARD, 15f);
                    rojasLanzadas++;
                }
                
                yield return new WaitForSeconds(intervalo);
            }

            // Fase 3: solo tarjetas rojas, velocidad 20
            float duracionFase3 = 10f;
            tiempoInicio = Time.time;
            tarjetasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase3 && tarjetasLanzadas < 5)
            {
                CrearTarjeta(REDCARD, 20f);
                tarjetasLanzadas++;
                yield return new WaitForSeconds(intervalo);
            }
        }
    }

    void CrearTarjeta(GameObject tarjetaPrefab, float velocidad)
    {
        // Genera una posición aleatoria en X entre los límites de los postes y en Y dentro del rango de spawn
        Vector3 posicionAleatoria = new Vector3(
            Random.Range(minX, maxX),  // Rango entre los postes
            -16f + Random.Range(-rangoDeSpawnY, rangoDeSpawnY),  // Rango en Y
            22f); // Posición fija en Z

        GameObject tarjeta = Instantiate(tarjetaPrefab, posicionAleatoria, transform.rotation);
        tarjeta.transform.localScale = new Vector3(tamanoFinal, tamanoFinal, tamanoFinal);

        TarjetaMovimiento movimiento = tarjeta.AddComponent<TarjetaMovimiento>();
        movimiento.velocidad = velocidad;
        movimiento.AsignarGoalController(this); // Asignar referencia de GoalController

        if (tarjetaPrefab == YELLOWCARD)
        {
            tarjetasAmarillasEnEscena++;
        }
        else if (tarjetaPrefab == REDCARD)
        {
            tarjetasRojasEnEscena++;
        }
    }

    public void RegistrarDestruccionTarjeta(bool esRoja)
    {
        if (esRoja)
        {
            tarjetasRojasEnEscena--;
        }
        else
        {
            tarjetasAmarillasEnEscena--;
        }
    }

    void OnGUI()
    {
        
        Color colorOriginal = GUI.color;
        GUI.color = Color.black; 

       
        GUI.Box(new Rect(10, 10, 250, 50), ""); 

        
        GUI.color = colorOriginal;

        
        GUIStyle estiloTextoAmarillo = new GUIStyle();
        estiloTextoAmarillo.fontSize = 16;
        estiloTextoAmarillo.normal.textColor = Color.yellow;

        GUIStyle estiloTextoRojo = new GUIStyle();
        estiloTextoRojo.fontSize = 16;
        estiloTextoRojo.normal.textColor = Color.red;

        
        GUI.Label(new Rect(15, 15, 220, 20), "Tarjetas Amarillas en juego: " + tarjetasAmarillasEnEscena, estiloTextoAmarillo);
        GUI.Label(new Rect(15, 35, 220, 20), "Tarjetas Rojas en juego: " + tarjetasRojasEnEscena, estiloTextoRojo);
    }
}
