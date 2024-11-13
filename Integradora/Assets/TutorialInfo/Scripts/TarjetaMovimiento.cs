using UnityEngine;

public class TarjetaMovimiento : MonoBehaviour
{
    public float velocidad = 10f;
    public float limiteZ = -40f;

    private GoalController goalController;
    private bool esRoja;

    public void AsignarGoalController(GoalController controller)
    {
        goalController = controller;
        esRoja = gameObject.CompareTag("REDCARD");
    }

    void Update()
    {
        transform.position += Vector3.back * velocidad * Time.deltaTime;

        if (transform.position.z <= limiteZ)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (goalController != null)
        {
            goalController.RegistrarDestruccionTarjeta(esRoja);
        }
    }
}
