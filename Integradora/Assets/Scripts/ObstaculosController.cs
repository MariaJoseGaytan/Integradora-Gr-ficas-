using System.Collections;
using UnityEngine;

public class ObstaculosController : MonoBehaviour
{
    public GameObject prefabObstaculo;      // Prefab del obstáculo
    public GameObject prefabMoneda;         // Prefab de la moneda
    public float intervaloSpawnObstaculos = 2f;  // Intervalo entre generación de obstáculos
    public float intervaloSpawnMoneda = 30f;     // Intervalo para generar una moneda

    private bool monedaActiva = false;       // Controla si una moneda está activa

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

    IEnumerator GenerarObstaculos()
    {
        while (true)
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
        while (true)
        {
            // Espera el intervalo establecido para la moneda
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

                // Desactiva monedaActiva al destruir la moneda
                nuevaMoneda.GetComponent<MonedaController>().OnMonedaRecogida += () => monedaActiva = false;
            }
        }
    }
}
