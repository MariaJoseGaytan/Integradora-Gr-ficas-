using UnityEngine;

public class JugadorMovimiento : MonoBehaviour
{
    public float velocidadMovimiento = 10f; // Velocidad de movimiento horizontal, valor por defecto
    public float limiteIzquierdo = -7f; // Límite izquierdo en el eje X, valor por defecto
    public float limiteDerecho = 11f; // Límite derecho en el eje X, valor por defecto
    public GameObject prefabBalon; // Prefab del balón
    public float fuerzaLanzamiento = 15f; // Fuerza con la que se lanza el balón

    private int balonesRestantes = 50; // Número máximo de balones

    void Update()
    {
        // Movimiento horizontal con las flechas izquierda y derecha
        float movimientoHorizontal = Input.GetAxis("Horizontal") * velocidadMovimiento * Time.deltaTime;
        float nuevaPosX = Mathf.Clamp(transform.position.x + movimientoHorizontal, limiteIzquierdo, limiteDerecho);
        transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);

        // Lanzar balón con la flecha hacia arriba si quedan balones
        if (Input.GetKeyDown(KeyCode.UpArrow) && balonesRestantes > 0)
        {
            LanzarBalon();
        }
    }

    void LanzarBalon()
    {
        if (prefabBalon != null && balonesRestantes > 0)
        {
            // Instanciar el balón en la posición de la jugadora y un poco hacia adelante
            GameObject balon = Instantiate(prefabBalon, transform.position + Vector3.forward, Quaternion.identity);
            
            // Aplicar una fuerza hacia adelante en el eje Z para que el balón se mueva hacia la portería
            Rigidbody rb = balon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.forward * fuerzaLanzamiento, ForceMode.Impulse); // Fuerza en el eje Z positivo
            }

            // Reducir el contador de balones
            balonesRestantes--;
        }
        else if (balonesRestantes <= 0)
        {
            Debug.LogWarning("No quedan balones restantes.");
        }
    }

    void OnGUI()
{
    // Estilo para el texto de balones restantes
    GUIStyle estiloTexto = new GUIStyle();
    estiloTexto.fontSize = 16;
    estiloTexto.normal.textColor = Color.white;

    // Posición y tamaño de la caja de fondo
    float xPos = 10f;
    float yPos = Screen.height - 50f; // Un poco más alto para que el cuadro abarque el texto
    float ancho = 200f;
    float alto = 30f;

    // Dibujar la caja de fondo negra
    Color colorOriginal = GUI.color; // Guardar el color original de la GUI
    GUI.color = Color.black; // Cambiar el color de la GUI a negro
    GUI.Box(new Rect(xPos, yPos, ancho, alto), ""); // Dibujar la caja vacía como fondo
    GUI.color = colorOriginal; // Restaurar el color original de la GUI

    // Dibujar el texto sobre la caja de fondo
    GUI.Label(new Rect(xPos + 5f, yPos + 5f, ancho, alto), "Balones Restantes: " + balonesRestantes, estiloTexto);
}

}
