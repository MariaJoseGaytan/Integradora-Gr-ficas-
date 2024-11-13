using System.Collections;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public GameObject YELLOWCARD;
    public GameObject REDCARD;

    public float intervalo = 1f;
    public int tarjetasRojasPorFase = 5;
    
    public float rangoDeSpawn = 5f;
    public float velocidadTarjeta = 10f; // Velocidad de las tarjetas
    public float tamanoFinal = 0.05f; // Tamaño final de las tarjetas

    private Camera camara;

    void Start()
    {
        camara = Camera.main; // Obtiene la cámara principal
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
    Vector3 posicionAleatoria = new Vector3(
        transform.position.x + Random.Range(-rangoDeSpawn, rangoDeSpawn),
        transform.position.y + Random.Range(-rangoDeSpawn, rangoDeSpawn),
        transform.position.z);

    // Instanciar la tarjeta en la posición aleatoria con el tamaño final directamente
    GameObject tarjeta = Instantiate(tarjetaPrefab, posicionAleatoria, transform.rotation); // Usa la rotación de la portería
    tarjeta.transform.localScale = new Vector3(tamanoFinal, tamanoFinal, tamanoFinal); // Asignar tamaño final de inmediato

    // Agregar el script de movimiento sin dirección a la cámara
    TarjetaMovimiento movimiento = tarjeta.AddComponent<TarjetaMovimiento>();
    movimiento.velocidad = velocidadTarjeta;
}

}
