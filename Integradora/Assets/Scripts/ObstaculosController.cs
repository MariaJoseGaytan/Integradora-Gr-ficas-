using System.Collections;
using UnityEngine;

public class ObstaculosController : MonoBehaviour
{
    public GameObject prefabObstaculo;     
    public GameObject prefabMoneda;        
    public float intervaloSpawnObstaculos = 2f;  
    public float intervaloSpawnMoneda = 30f;    

    private bool monedaActiva = false;       
    private bool juegoTerminado = false;     

    private void Start()
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
        juegoTerminado = true; 
        StopAllCoroutines(); 

        foreach (GameObject obstaculo in GameObject.FindGameObjectsWithTag("Obstaculo"))
        {
            Destroy(obstaculo);
        }

        GameObject moneda = GameObject.FindWithTag("Moneda");
        if (moneda != null)
        {
            Destroy(moneda);
        }

        monedaActiva = false; 
    }

    IEnumerator GenerarObstaculos()
    {
        while (!juegoTerminado) 
        {
            if (prefabObstaculo != null)
            {
                Vector3 posicionObstaculo = new Vector3(
                    Random.Range(-18f, 23f), 
                    -9.4f, 
                    Random.Range(-20f, 20f)
                );

                GameObject obstaculo = Instantiate(prefabObstaculo, posicionObstaculo, Quaternion.identity);
                obstaculo.tag = "Obstaculo"; 
            }
            yield return new WaitForSeconds(intervaloSpawnObstaculos);
        }
    }

    IEnumerator GenerarMoneda()
    {
        while (!juegoTerminado) 
        {
            yield return new WaitForSeconds(intervaloSpawnMoneda);

            if (prefabMoneda != null && !monedaActiva && !juegoTerminado)
            {
                Vector3 posicionMoneda = new Vector3(
                    Random.Range(-18f, 23f), 
                    -12.95f, 
                    Random.Range(-20f, 20f)
                );

                GameObject nuevaMoneda = Instantiate(prefabMoneda, posicionMoneda, Quaternion.identity);
                nuevaMoneda.tag = "Moneda"; 
                monedaActiva = true;

                MonedaController monedaController = nuevaMoneda.GetComponent<MonedaController>();
                if (monedaController != null)
                {
                    monedaController.OnMonedaRecogida += () => monedaActiva = false;
                }

                StartCoroutine(ResetMonedaActiva(30f, nuevaMoneda));
            }
        }
    }

    IEnumerator ResetMonedaActiva(float tiempoEspera, GameObject moneda)
    {
        yield return new WaitForSeconds(tiempoEspera);
        
        if (!juegoTerminado && moneda != null) 
        {
            Destroy(moneda); 
        }

        monedaActiva = false; 
    }
}
