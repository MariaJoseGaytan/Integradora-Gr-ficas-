using UnityEngine;

public class TarjetaMovimiento : MonoBehaviour
{
    public float velocidad = 10f;
    private float velocidadOriginal;
    private Vector3 direccion = Vector3.back;

    private GoalController goalController;

    void Start()
    {
        velocidadOriginal = velocidad;
    }

    public void AsignarGoalController(GoalController controller)
    {
        goalController = controller;
    }

    public void VelocidadActualizada(float factor)
    {
        velocidad = velocidadOriginal * factor;
    }

    void Update()
    {
        transform.position += direccion * velocidad * Time.deltaTime;

        if (transform.position.z <= -40f)
        {
            if (goalController != null)
            {
                goalController.RegistrarDestruccionTarjeta(gameObject.CompareTag("REDCARD"), this);
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga el tag "Player"
        {
            JugadorMovimiento jugador = other.GetComponent<JugadorMovimiento>();
            if (jugador != null)
            {
                if (gameObject.CompareTag("YELLOWCARD"))
                {
                    jugador.RecibirTarjetaAmarilla();
                }
                else if (gameObject.CompareTag("REDCARD"))
                {
                    jugador.RecibirTarjetaRoja();
                }
                Destroy(gameObject); // Destruye la tarjeta al hacer contacto
            }
        }
    }
}
