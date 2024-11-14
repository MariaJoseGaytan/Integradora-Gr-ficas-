using UnityEngine;

public class ObstaculoMovimiento : MonoBehaviour
{
    public float velocidadX = 2f; 
    private float limiteIzquierdo = -18f; 
    private float limiteDerecho = 23f; 

    private float posicionFijaZ; 
    private float posicionFijaY = -9.4f; 

    private GoalController goalController;

    void Start()
    {
        // Asignar una posición aleatoria en Z entre -20 y 20 cuando el obstáculo se genera
        posicionFijaZ = Random.Range(-20f, 20f);
        transform.position = new Vector3(transform.position.x, posicionFijaY, posicionFijaZ);

        // Encontrar el GoalController para verificar si el juego ha terminado
        GameObject goalControllerObject = GameObject.FindWithTag("GoalController");
        if (goalControllerObject != null)
        {
            goalController = goalControllerObject.GetComponent<GoalController>();
        }
    }

    void Update()
    {
        // Detener el movimiento si el juego ha terminado
        if (goalController != null && goalController.juegoTerminado)
        {
            return; // Salir de Update si el juego ha terminado
        }

        // Mover el obstáculo de lado a lado en el eje X
        transform.position += Vector3.right * velocidadX * Time.deltaTime;

        // Verificar los límites y destruir el obstáculo si llega a los bordes
        if (transform.position.x <= limiteIzquierdo || transform.position.x >= limiteDerecho)
        {
            Debug.Log("Obstáculo destruido");
            Destroy(gameObject);
        }
    }
}
