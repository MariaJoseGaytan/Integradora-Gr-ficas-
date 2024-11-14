using UnityEngine;

public class BalonMovimiento : MonoBehaviour
{
    private GoalController goalController;
    private float posicionY;

    void Start()
    {
        GameObject goalControllerObject = GameObject.FindWithTag("GoalController");
        if (goalControllerObject != null)
        {
            goalController = goalControllerObject.GetComponent<GoalController>();
        }
        else
        {
            Debug.LogWarning("GoalController no encontrado.");
        }
    }

    void Update()
    {
        posicionY = transform.position.y;

        // Revisa si el balón ha alcanzado la posición de gol y está entre los límites de la portería
        if (transform.position.z >= 22f && transform.position.x >= -18f && transform.position.x <= 23f)
        {
            if (goalController != null)
            {
                goalController.RegistrarGol();
            }
            Destroy(gameObject); // Destruye el balón al alcanzar la portería
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica si colisiona con un obstáculo
        if (other.CompareTag("Obstaculo"))
        {
            if (goalController != null)
            {
                goalController.RegistrarColisionConObstaculo();
            }
            Destroy(other.gameObject); // Destruye el obstáculo
            Destroy(gameObject);       // Destruye el balón
        }
    }
}
