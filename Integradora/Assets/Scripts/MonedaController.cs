using UnityEngine;

public class MonedaController : MonoBehaviour
{
    public delegate void MonedaRecogida();
    public event MonedaRecogida OnMonedaRecogida;

    public float velocidad = 2f; // Velocidad de movimiento en el eje X
    private float limiteDerecho = 23f;

    void Start()
    {
        // Posiciona la moneda en el punto de inicio en X, con Y y Z fijos
        transform.position = new Vector3(-18f, -12.95f, 20f);
    }

    void Update()
    {
        // Mueve la moneda en el eje X hacia la derecha
        float movimientoX = velocidad * Time.deltaTime;
        transform.Translate(movimientoX, 0, 0);

        // Destruye la moneda si llega al límite derecho en X
        if (transform.position.x >= limiteDerecho)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador recoge la moneda
        {
            OnMonedaRecogida?.Invoke(); // Notifica que la moneda ha sido recogida
            Destroy(gameObject);        // Destruye la moneda
        }
        else if (other.CompareTag("Balon")) // Si un balón colisiona con la moneda
        {
            Destroy(other.gameObject); // Destruye el balón
            Destroy(gameObject);       // Destruye la moneda
        }
    }
}
