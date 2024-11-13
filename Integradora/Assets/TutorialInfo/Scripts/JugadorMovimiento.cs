using UnityEngine;

public class JugadorMovimiento : MonoBehaviour
{
    public float velocidadMovimiento = 10f; // Ajusta la velocidad según tus preferencias
    public float limiteIzquierdo = -10f; // Límite izquierdo en el eje X (ajusta según tu escena)
    public float limiteDerecho = 10f; // Límite derecho en el eje X (ajusta según tu escena)

    void Update()
    {
        // Obtener la entrada del jugador en el eje horizontal (flechas izquierda y derecha)
        float movimientoHorizontal = Input.GetAxis("Horizontal") * velocidadMovimiento * Time.deltaTime;

        // Nueva posición en X con límite para que no salga de los bordes
        float nuevaPosX = Mathf.Clamp(transform.position.x + movimientoHorizontal, limiteIzquierdo, limiteDerecho);

        // Actualizar la posición de la jugadora
        transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);
    }
}
