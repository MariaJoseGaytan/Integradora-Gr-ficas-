using System.Collections;
using UnityEngine;

public class ObstaculosController : MonoBehaviour
{
    public GameObject prefabObstaculo;      // Prefab del obstáculo
    public GameObject prefabMoneda;         // Prefab de la moneda
    public float intervaloSpawnObstaculos = 2f;  // Intervalo entre generación de obstáculos
    public float intervaloSpawnMoneda = 30f;     // Intervalo para generar una moneda

    private bool monedaActiva = false;       // Controla si una moneda está activa
    private bool juegoTerminado = false;     // Controla si el juego ha terminado

    void Start()
    {
        if (prefabObstaculo != null)
        {
            StartCoroutine(GenerarObstaculos());
        }
        else
        {
            Debug.LogWarning("Prefab del obstáculo no asignado en ObstaculosController.");
        }

        if (prefabMoneda != null)
        {
            StartCoroutine(GenerarMoneda());
        }
        else
        {
            Debug.LogWarning("Prefab de la moneda no asignado en ObstaculosController.");
        }
    }

    public void TerminarJuego()
    {
        juegoTerminado = true; // Detiene la generación de obstáculos y monedas
    }

    IEnumerator GenerarObstaculos()
    {
        while (!juegoTerminado) // Solo genera obstáculos si el juego no ha terminado
        {
            if (prefabObstaculo != null)
            {
                Vector3 posicionObstaculo = new Vector3(
                    Random.Range(-18f, 23f), 
                    -9.4f, 
                    Random.Range(-20f, 20f)
                );

                Instantiate(prefabObstaculo, posicionObstaculo, Quaternion.identity);
            }
            yield return new WaitForSeconds(intervaloSpawnObstaculos);
        }
    }

    IEnumerator GenerarMoneda()
    {
        while (!juegoTerminado) // Solo genera monedas si el juego no ha terminado
        {
            yield return new WaitForSeconds(intervaloSpawnMoneda);

            if (prefabMoneda != null && !monedaActiva)
            {
                Vector3 posicionMoneda = new Vector3(
                    Random.Range(-18f, 23f), 
                    -12.95f, 
                    Random.Range(-20f, 20f)
                );

                GameObject nuevaMoneda = Instantiate(prefabMoneda, posicionMoneda, Quaternion.identity);
                monedaActiva = true;

                // Desactiva monedaActiva cuando la moneda se recoja
                MonedaController monedaController = nuevaMoneda.GetComponent<MonedaController>();
                if (monedaController != null)
                {
                    monedaController.OnMonedaRecogida += () => monedaActiva = false;
                }

                // Desactiva monedaActiva automáticamente después de 30 segundos, si la moneda sigue activa
                StartCoroutine(ResetMonedaActiva(30f, nuevaMoneda));
            }
        }
    }

    IEnumerator ResetMonedaActiva(float tiempoEspera, GameObject moneda)
    {
        yield return new WaitForSeconds(tiempoEspera);
        
        if (moneda != null) // Si la moneda aún existe
        {
            Destroy(moneda); // Destruye la moneda si no ha sido recogida
        }

        monedaActiva = false; // Permite que otra moneda sea generada
    }
}
