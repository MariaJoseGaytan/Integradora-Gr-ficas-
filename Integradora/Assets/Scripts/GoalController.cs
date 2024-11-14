using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject YELLOWCARD;
    public GameObject REDCARD;
    public GameObject BalonRival; 

    public float intervalo = 1f;
    public int tarjetasRojasPorFase = 2;
    public int tarjetasAmarillasPorFase2 = 3;

    public float rangoDeSpawnY = 5f;
    public float tamanoFinal = 0.3f;
    public float anchoTarjeta = 0.3f;
    public float largoTarjeta = 0.3f;
    public float posicionFijaZ = 22f;

    private Camera camara;
    private int tarjetasRojasEnEscena = 0;
    private int tarjetasAmarillasEnEscena = 0;
    private int tarjetasAmarillasRecibidas = 0;
    private int tarjetasRojasRecibidas = 0;
    private int obstaculosGolpeados = 0; 
    private const int limiteObstaculosGolpeados = 5; 
    public bool juegoTerminado = false;

    private int goles = 0;
    private const int golesParaGanar = 5;
    private bool esVictoria = false;

    private float minX = -18f;
    private float maxX = 23f;

    private List<TarjetaMovimiento> tarjetasEnEscena = new List<TarjetaMovimiento>();

    void Start()
    {
        camara = Camera.main;
        StartCoroutine(CicloDeTarjetas());
        Invoke("IniciarLanzamientoBalones", 30f); // Iniciar lanzamiento de balones después de 30 segundos
    }

    void Update()
    {
        if (!juegoTerminado)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                CambiarVelocidadTarjetas(0.5f);
            }
            else
            {
                CambiarVelocidadTarjetas(1.0f);
            }
        }
    }

    void IniciarLanzamientoBalones()
    {
        if (!juegoTerminado)
        {
            StartCoroutine(CicloDeBalones());
        }
    }

    IEnumerator CicloDeBalones()
    {
        while (!juegoTerminado)
        {
            for (int i = 0; i < 3; i++) // Lanzar 3 balones
            {
                CrearBalonRival();
                yield return new WaitForSeconds(1f); // Intervalo de 1 segundo entre cada balón
            }
            yield return new WaitForSeconds(10f); // Espera 10 segundos para el siguiente ciclo de 3 balones
        }
    }

    void CrearBalonRival()
    {
        if (BalonRival != null)
        {
            Vector3 posicionBalon = new Vector3(Random.Range(minX, maxX), -9.4f, 22f);
            GameObject nuevoBalonRival = Instantiate(BalonRival, posicionBalon, Quaternion.identity);
            
            // Asigna el GoalController al BalonRival
            BalonRivalMovimiento balonRivalMovimiento = nuevoBalonRival.GetComponent<BalonRivalMovimiento>();
            if (balonRivalMovimiento != null)
            {
                balonRivalMovimiento.AsignarGoalController(this);
            }
        }
    }


    IEnumerator CicloDeTarjetas()
    {
        while (!juegoTerminado)
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
            Random.Range(minX, maxX),
            -9.40f,
            posicionFijaZ);

        GameObject tarjeta = Instantiate(tarjetaPrefab, posicionAleatoria, transform.rotation);
        tarjeta.transform.localScale = new Vector3(anchoTarjeta, tamanoFinal, largoTarjeta);

        TarjetaMovimiento movimiento = tarjeta.AddComponent<TarjetaMovimiento>();
        movimiento.velocidad = velocidad;
        movimiento.AsignarGoalController(this);
        tarjetasEnEscena.Add(movimiento);

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
            if (tarjeta != null)
            {
                tarjeta.VelocidadActualizada(factor);
            }
        }
    }

    public void RegistrarGol()
    {
        if (!juegoTerminado)
        {
            goles++;
            if (goles >= golesParaGanar)
            {
                esVictoria = true;
                TerminarJuego("WIN");
            }
        }
    }

    public void RegistrarImpactoTarjeta(bool esRoja)
    {
        if (!juegoTerminado && !esVictoria)
        {
            if (esRoja)
            {
                tarjetasRojasRecibidas++;
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
    }

    public void RegistrarColisionConObstaculo()
{
    if (!juegoTerminado && !esVictoria)
    {
        obstaculosGolpeados++;
        Debug.Log("Obstáculos golpeados: " + obstaculosGolpeados); // Mensaje para verificar
        if (obstaculosGolpeados >= limiteObstaculosGolpeados)
        {
            TerminarJuego("Has golpeado demasiados obstáculos. ¡Juego terminado!");
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
        tarjetasEnEscena.Remove(tarjeta);
    }

    void TerminarJuego(string mensaje)
    {
        juegoTerminado = true;
        Debug.Log(mensaje);

        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            JugadorMovimiento jugadorMovimiento = jugador.GetComponent<JugadorMovimiento>();
            if (jugadorMovimiento != null)
            {
                jugadorMovimiento.TerminarJuego(esVictoria);
            }
        }

        GameObject obstaculosControllerObject = GameObject.FindWithTag("ObstaculosController");
        if (obstaculosControllerObject != null)
        {
            ObstaculosController obstaculosController = obstaculosControllerObject.GetComponent<ObstaculosController>();
            if (obstaculosController != null)
            {
                obstaculosController.TerminarJuego();
            }
        }
    }

    public void ResetearContadoresDeTarjetas()
    {
        tarjetasAmarillasRecibidas = 0;
        tarjetasRojasRecibidas = 0;
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

        GUIStyle estiloTextoBlanco = new GUIStyle(); 
        estiloTextoBlanco.fontSize = 16;
        estiloTextoBlanco.normal.textColor = Color.white;

        GUI.Label(new Rect(15, 15, 220, 20), "Tarjetas Amarillas en juego: " + tarjetasAmarillasEnEscena, estiloTextoAmarillo);
        GUI.Label(new Rect(15, 35, 220, 20), "Tarjetas Rojas en juego: " + tarjetasRojasEnEscena, estiloTextoRojo);

        GUI.Box(new Rect(Screen.width - 200, Screen.height - 70, 190, 80), "");

        GUI.Label(new Rect(Screen.width - 190, Screen.height - 65, 180, 20), "Tarjetas Amarillas: " + tarjetasAmarillasRecibidas, estiloTextoAmarillo);
        GUI.Label(new Rect(Screen.width - 190, Screen.height - 45, 180, 20), "Tarjetas Rojas: " + tarjetasRojasRecibidas, estiloTextoRojo);
        GUI.Label(new Rect(Screen.width - 190, Screen.height - 25, 180, 20), "Obstáculos golpeados: " + obstaculosGolpeados, estiloTextoBlanco);

        GUI.Box(new Rect(Screen.width - 200, 10, 190, 40), "");
        GUI.Label(new Rect(Screen.width - 190, 15, 180, 20), "Goles: " + goles, estiloTextoAmarillo);

        if (juegoTerminado)
        {
            GUIStyle estiloFinal = new GUIStyle();
            estiloFinal.fontSize = 40;
            estiloFinal.alignment = TextAnchor.MiddleCenter;

            if (esVictoria) estiloFinal.normal.textColor = Color.green;
            else estiloFinal.normal.textColor = Color.red;

            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), esVictoria ? "WIN" : "GAME OVER", estiloFinal);
        }
    }
}
