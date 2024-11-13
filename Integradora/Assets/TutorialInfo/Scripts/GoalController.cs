using System.Collections;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject YELLOWCARD;
    public GameObject REDCARD;

    public float intervalo = 1f;
    public int tarjetasRojasPorFase = 2; // Número máximo de tarjetas rojas en fase 2
    public int tarjetasAmarillasPorFase2 = 3; // Número máximo de tarjetas amarillas en fase 2

    public float rangoDeSpawnX = 30f;
    public float rangoDeSpawnY = 5f;
    public float tamanoFinal = 0.05f;

    private Camera camara;

    void Start()
    {
        camara = Camera.main; 
        StartCoroutine(CicloDeTarjetas());
    }

    IEnumerator CicloDeTarjetas()
    {
        while (true)
        {
            // Fase 1: 10 segundos de solo tarjetas amarillas, velocidad 10
            float duracionFase1 = 10f;
            float tiempoInicio = Time.time;
            int tarjetasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase1 && tarjetasLanzadas < 5)
            {
                CrearTarjeta(YELLOWCARD, 10f); // Velocidad inicial para fase 1
                tarjetasLanzadas++;
                yield return new WaitForSeconds(intervalo);
            }

            // Fase 2: 10 segundos de tarjetas amarillas y algunas rojas, velocidad 15
            float duracionFase2 = 10f;
            tiempoInicio = Time.time;
            int rojasLanzadas = 0;
            int amarillasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase2 && (amarillasLanzadas < tarjetasAmarillasPorFase2 || rojasLanzadas < tarjetasRojasPorFase))
            {
                if (amarillasLanzadas < tarjetasAmarillasPorFase2)
                {
                    CrearTarjeta(YELLOWCARD, 15f); // Velocidad aumentada para fase 2
                    amarillasLanzadas++;
                }
                
                if (rojasLanzadas < tarjetasRojasPorFase)
                {
                    CrearTarjeta(REDCARD, 15f); // Tarjetas rojas en fase 2 también con velocidad 15
                    rojasLanzadas++;
                }
                
                yield return new WaitForSeconds(intervalo);
            }

            // Fase 3: 10 segundos de solo tarjetas rojas, velocidad 20
            float duracionFase3 = 10f;
            tiempoInicio = Time.time;
            tarjetasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase3 && tarjetasLanzadas < 5)
            {
                CrearTarjeta(REDCARD, 20f); // Velocidad más alta para fase 3
                tarjetasLanzadas++;
                yield return new WaitForSeconds(intervalo);
            }
        }
    }

    void CrearTarjeta(GameObject tarjetaPrefab, float velocidad)
    {
        Vector3 posicionAleatoria = new Vector3(
            4.3f + Random.Range(-rangoDeSpawnX, rangoDeSpawnX),
            -16f + Random.Range(-rangoDeSpawnY, rangoDeSpawnY),
            22f);

        GameObject tarjeta = Instantiate(tarjetaPrefab, posicionAleatoria, transform.rotation);
        tarjeta.transform.localScale = new Vector3(tamanoFinal, tamanoFinal, tamanoFinal);

        TarjetaMovimiento movimiento = tarjeta.AddComponent<TarjetaMovimiento>();
        movimiento.velocidad = velocidad; // Asigna la velocidad específica para cada fase
    }
}
