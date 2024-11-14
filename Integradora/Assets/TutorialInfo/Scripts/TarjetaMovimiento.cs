using UnityEngine;

public class TarjetaMovimiento : MonoBehaviour
{
    public float velocidad = 10f;
    private float velocidadOriginal;
    private Vector3 direccion = Vector3.back;

    // Referencia al GoalController
    private GoalController goalController;

    void Start()
    {
        velocidadOriginal = velocidad;
    }

    public void AsignarGoalController(GoalController controller)
    {
        goalController = controller; // Asignar referencia al GoalController
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
                // Llama a goalController para registrar la destrucción de la tarjeta
                goalController.RegistrarDestruccionTarjeta(gameObject.CompareTag("REDCARD"), this);
            }
            Destroy(gameObject);
        }
    }
}
