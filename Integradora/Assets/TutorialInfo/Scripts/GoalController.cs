using System.Collections;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    // Prefabs de las tarjetas
    public GameObject YELLOWCARD;
    public GameObject REDCARD;

    // Intervalo de tiempo entre lanzamientos
    public float intervalo = 1f;
    public int tarjetasRojasPorFase = 5;
    
    // Rango de posición aleatoria alrededor de la portería
    public float rangoDeSpawn = 5f;

    void Start()
    {
        StartCoroutine(CicloDeTarjetas());
    }

    IEnumerator CicloDeTarjetas()
    {
        while (true)
        {
            // Fase 1: 15 segundos de solo tarjetas amarillas
            float duracionFase1 = 15f;
            float tiempoInicio = Time.time;
            while (Time.time - tiempoInicio < duracionFase1)
            {
                CrearTarjeta(YELLOWCARD);
                yield return new WaitForSeconds(intervalo);
            }

            // Fase 2: 15 segundos de tarjetas amarillas y 5 rojas
            float duracionFase2 = 15f;
            tiempoInicio = Time.time;
            int rojasLanzadas = 0;
            while (Time.time - tiempoInicio < duracionFase2)
            {
                CrearTarjeta(YELLOWCARD);
                if (rojasLanzadas < tarjetasRojasPorFase)
                {
                    CrearTarjeta(REDCARD);
                    rojasLanzadas++;
                }
                yield return new WaitForSeconds(intervalo);
            }

            // Fase 3: 15 segundos de solo tarjetas rojas
            float duracionFase3 = 15f;
            tiempoInicio = Time.time;
            while (Time.time - tiempoInicio < duracionFase3)
            {
                CrearTarjeta(REDCARD);
                yield return new WaitForSeconds(intervalo);
            }
        }
    }

    void CrearTarjeta(GameObject tarjetaPrefab)
    {
        // Generar posición aleatoria cerca de la portería
        Vector3 posicionAleatoria = new Vector3(
            transform.position.x + Random.Range(-rangoDeSpawn, rangoDeSpawn),
            transform.position.y + Random.Range(-rangoDeSpawn, rangoDeSpawn),
            transform.position.z);

        // Instanciar la tarjeta y hacerla pequeña
        GameObject tarjeta = Instantiate(tarjetaPrefab, posicionAleatoria, Quaternion.identity);
        tarjeta.transform.localScale = Vector3.zero; // Empezar desde tamaño 0
        StartCoroutine(CrecerTarjeta(tarjeta));
    }

    IEnumerator CrecerTarjeta(GameObject tarjeta)
    {
        float duracion = 2f; // Duración del crecimiento
        float tiempoTranscurrido = 0f;
        Vector3 tamanoFinal = new Vector3(1f, 1f, 1f); // Tamaño final de la tarjeta

        while (tiempoTranscurrido < duracion)
        {
            tarjeta.transform.localScale = Vector3.Lerp(Vector3.zero, tamanoFinal, tiempoTranscurrido / duracion);
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        tarjeta.transform.localScale = tamanoFinal; // Asegurarse de que llegue al tamaño final
    }
}
