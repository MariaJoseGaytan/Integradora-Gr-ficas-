using UnityEngine;

public class MonedaController : MonoBehaviour
{
    public delegate void MonedaRecogida();
    public event MonedaRecogida OnMonedaRecogida;

    public float velocidad = 8f;
    private float limiteDerecho = 23f;
    private GoalController goalController; 

    void Start()
    {
        GameObject goalControllerObject = GameObject.FindWithTag("GoalController");
        if (goalControllerObject != null)
        {
            goalController = goalControllerObject.GetComponent<GoalController>();
        }
        else
        {
            Debug.LogWarning("GoalController no encontrado. Asegúrate de que el Tag esté configurado correctamente.");
        }
        
        transform.position = new Vector3(-18f, -12.95f, 20f);
    }

    void Update()
    {
        float movimientoX = velocidad * Time.deltaTime;
        transform.Translate(movimientoX, 0, 0);

        if (transform.position.x >= limiteDerecho)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnMonedaRecogida?.Invoke();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Balon"))
        {
            if (goalController != null)
            {
                goalController.ResetearContadoresDeTarjetas();
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
