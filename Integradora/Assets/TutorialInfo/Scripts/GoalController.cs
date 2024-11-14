using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject YELLOWCARD;
    public GameObject REDCARD;

    public float intervalo = 1f;
    public int tarjetasRojasPorFase = 2;
    public int tarjetasAmarillasPorFase2 = 3;

    public float rangoDeSpawnY = 5f; 
    public float tamanoFinal = 0.3f;
    public float anchoTarjeta = 0.3f; // Tamaño para el ancho de la tarjeta (eje X)
    public float largoTarjeta = 0.3f; // Tamaño para la profundidad de la tarjeta (eje Z)
    public float posicionFijaZ = 22f; // Posición fija en Z

    private Camera camara;
    private int tarjetasRojasEnEscena = 0;
    private int tarjetasAmarillasEnEscena = 0;
    private int tarjetasAmarillasRecibidas = 0; // Contador de tarjetas amarillas recibidas
    public bool juegoTerminado = false; // Variable para detener el ciclo

    private float minX = -18f; // Poste izquierdo 
    private float maxX = 23f; // Poste derecho 

    // Lista para almacenar las tarjetas activas
    private List<TarjetaMovimiento> tarjetasEnEscena = new List<TarjetaMovimiento>();

    void Start()
    {
        camara = Camera.main;
        StartCoroutine(CicloDeTarjetas());
    }

    void Update()
    {
        if (!juegoTerminado)
        {
            // Monitorear la tecla Espacio para ajustar la velocidad de las tarjetas
            if (Input.GetKey(KeyCode.Space))
            {
                CambiarVelocidadTarjetas(0.5f); // Reduce la velocidad al 50%
            }
            else
            {
                CambiarVelocidadTarjetas(1.0f); // Restaura la velocidad normal
            }
        }
    }

    IEnumerator CicloDeTarjetas()
    {
        while (!juegoTerminado) // Ciclo solo continúa si el juego no ha terminado
        {
            float duracionFase1 = 10f;
            float tiempoInicio = Time.time;
            int tarjetasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase1 && tarjetasLanzadas < 5 && !juegoTerminado)
            {
                CrearTarjeta(YELLOWCARD, 10f);
                tarjetasLanzadas++;
                yield return new WaitForSeconds(intervalo);
            }

            float duracionFase2 = 10f;
            tiempoInicio = Time.time;
            int rojasLanzadas = 0;
            int amarillasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase2 && (amarillasLanzadas < tarjetasAmarillasPorFase2 || rojasLanzadas < tarjetasRojasPorFase) && !juegoTerminado)
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

            float duracionFase3 = 10f;
            tiempoInicio = Time.time;
            tarjetasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase3 && tarjetasLanzadas < 5 && !juegoTerminado)
            {
                CrearTarjeta(REDCARD, 20f);
                tarjetasLanzadas++;
                yield return new WaitForSeconds(intervalo);
            }
        }
    }

    void CrearTarjeta(GameObject tarjetaPrefab, float velocidad)
    {
        Vector3 posicionAleatoria = new Vector3(
            Random.Range(minX, maxX),  // Rango entre los postes en el eje X
            -9.40f,                    // Fijar la posición Y en -9.40
            posicionFijaZ);            // Usar la variable para la posición fija en Z

        GameObject tarjeta = Instantiate(tarjetaPrefab, posicionAleatoria, transform.rotation);
        tarjeta.transform.localScale = new Vector3(anchoTarjeta, tamanoFinal, largoTarjeta); // Asigna el tamaño en X, Y, Z

        TarjetaMovimiento movimiento = tarjeta.AddComponent<TarjetaMovimiento>();
        movimiento.velocidad = velocidad;
        movimiento.AsignarGoalController(this); // Asignar referencia de GoalController
        tarjetasEnEscena.Add(movimiento); // Agregar tarjeta a la lista de tarjetas activas

        if (tarjetaPrefab == YELLOWCARD)
        {
            tarjetasAmarillasEnEscena++;
        }
        else if (tarjetaPrefab == REDCARD)
        {
            tarjetasRojasEnEscena++;
        }
    }

    void CambiarVelocidadTarjetas(float factor)
    {
        foreach (TarjetaMovimiento tarjeta in tarjetasEnEscena)
        {
            if (tarjeta != null) // Asegurarse de que la tarjeta siga existiendo
            {
                tarjeta.VelocidadActualizada(factor);
            }
        }
    }

    public void RegistrarImpactoTarjeta(bool esRoja)
    {
        if (esRoja)
        {
            TerminarJuego("Recibiste una tarjeta roja. ¡Juego terminado!");
        }
        else
        {
            tarjetasAmarillasRecibidas++;
            if (tarjetasAmarillasRecibidas >= 2)
            {
                TerminarJuego("Recibiste dos tarjetas amarillas. ¡Juego terminado!");
            }
        }
    }

    public void RegistrarDestruccionTarjeta(bool esRoja, TarjetaMovimiento tarjeta)
    {
        if (esRoja)
        {
            tarjetasRojasEnEscena--;
        }
        else
        {
            tarjetasAmarillasEnEscena--;
        }
        tarjetasEnEscena.Remove(tarjeta); // Eliminar la tarjeta de la lista al destruirse
    }

    void TerminarJuego(string mensaje)
    {
        juegoTerminado = true;

        Debug.Log(mensaje);

        // Llama al método de `JugadorMovimiento` para manejar la música y el mensaje de "GAME OVER"
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            JugadorMovimiento jugadorMovimiento = jugador.GetComponent<JugadorMovimiento>();
            if (jugadorMovimiento != null)
            {
                jugadorMovimiento.TerminarJuego();
            }
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
