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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balon"))
        {
            // Colisión con un balón
            if (goalController != null)
            {
                bool esRoja = gameObject.CompareTag("REDCARD");
                goalController.RegistrarImpactoTarjeta(esRoja);
            }

            // Destruir tanto el balón como la tarjeta
            Destroy(other.gameObject); // Destruir el balón
            Destroy(gameObject);       // Destruir la tarjeta
        }
        else if (other.CompareTag("Player"))
        {
            // Colisión con la jugadora
            if (goalController != null)
            {
                bool esRoja = gameObject.CompareTag("REDCARD");
                goalController.RegistrarImpactoTarjeta(esRoja);
            }

            // Destruir la tarjeta al tocar a la jugadora
            Destroy(gameObject);
        }
    }
}
