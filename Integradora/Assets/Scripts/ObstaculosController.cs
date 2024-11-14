using System.Collections;
using UnityEngine;

public class ObstaculosController : MonoBehaviour
{
    public GameObject prefabObstaculo; // Prefab del obstáculo
    public float intervaloSpawn = 2f;  // Intervalo entre generación de obstáculos

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
    }

    IEnumerator GenerarObstaculos()
    {
        while (true)
        {
            // Verifica que el prefab esté asignado antes de instanciar
            if (prefabObstaculo != null)
            {
                // Genera el obstáculo en una posición aleatoria en X y fija en Z
                Instantiate(prefabObstaculo, new Vector3(Random.Range(-18f, 23f), -9.4f, 22f), Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab del obstáculo es nulo.");
                break;
            }
            yield return new WaitForSeconds(intervaloSpawn);
        }
    }
}
