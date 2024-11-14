using UnityEngine;

public class ObstaculoMovimiento : MonoBehaviour
{
    public float velocidadX = 2f; // Velocidad de movimiento en el eje X
    private float limiteIzquierdo = -18f; // Límite izquierdo para destruir el obstáculo
    private float limiteDerecho = 23f; // Límite derecho para destruir el obstáculo

    private float posicionFijaZ; // Posición fija en Z
    private float posicionFijaY = -9.4f; // Posición fija en Y

    void Start()
    {
        // Asignar una posición aleatoria en Z entre -20 y 20 cuando el obstáculo se genera
        posicionFijaZ = Random.Range(-20f, 30f);
        transform.position = new Vector3(transform.position.x, posicionFijaY, posicionFijaZ);
    }

    void Update()
    {
        // Mover el obstáculo de lado a lado en el eje X
        transform.position += Vector3.right * velocidadX * Time.deltaTime;

        // Verificar los límites y destruir el obstáculo si llega a los bordes
        if (transform.position.x <= limiteIzquierdo || transform.position.x >= limiteDerecho)
        {
            Destroy(gameObject);
        }
    }
}
