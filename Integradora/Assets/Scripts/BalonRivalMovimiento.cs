using UnityEngine;

public class BalonRivalMovimiento : MonoBehaviour
{
    public float velocidad = 10f;
    private Vector3 direccion = Vector3.back;
    private GoalController goalController;

    void Start()
    {
        // Configura la dirección inicial
        direccion = Vector3.back;
    }

    public void AsignarGoalController(GoalController controller)
    {
        goalController = controller;
    }

    void Update()
    {
        // Mueve el balón rival hacia el jugador
        transform.position += direccion * velocidad * Time.deltaTime;

        // Destruye el balón si sale del límite del campo de juego
        if (transform.position.z <= -40f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Balon"))
    {
        // Si el balón del jugador colisiona con el balón rival
        Destroy(other.gameObject); // Destruye el balón del jugador
        Destroy(gameObject);       // Destruye el balón rival
    }
    else if (other.CompareTag("Player"))
    {
        // Si el balón rival colisiona con el jugador
        if (goalController != null)
        {
            goalController.RegistrarColisionConObstaculo(); 
        }

        // Destruye el balón rival tras la colisión
        Destroy(gameObject);
    }
}

}
