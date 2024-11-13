using UnityEngine;

public class TarjetaMovimiento : MonoBehaviour
{
    public float velocidad = 10f;
    public float limiteZ = -40f; // Posición en Z donde la tarjeta se destruirá

    void Update()
    {
        // Mueve la tarjeta en el eje Z hacia los valores negativos
        transform.position += Vector3.back * velocidad * Time.deltaTime;

        // Verifica si la tarjeta ha alcanzado el límite en Z
        if (transform.position.z <= limiteZ)
        {
            Destroy(gameObject); // Destruye la tarjeta cuando alcanza el límite en Z
        }
    }
}
