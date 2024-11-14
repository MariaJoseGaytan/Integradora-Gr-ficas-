using UnityEngine;

public class JugadorMovimiento : MonoBehaviour
{
    public float velocidadMovimiento = 10f; // Velocidad de movimiento horizontal
    public float limiteIzquierdo = -7f; // Límite izquierdo en el eje X
    public float limiteDerecho = 11f; // Límite derecho en el eje X
    public GameObject prefabBalon; // Prefab del balón
    public float fuerzaLanzamiento = 15f; // Fuerza con la que se lanza el balón

    private int balonesRestantes = 50; // Número máximo de balones
    private bool juegoTerminado = false; // Indica si el juego ha terminado

    public AudioClip sonidoPerdida; // Sonido que se reproducirá al perder
    private AudioSource audioSource; // AudioSource para el sonido de pérdida
    public AudioSource audioFondo; // AudioSource del sonido de fondo en loop

    void Start()
    {
        // Configurar el AudioSource para el sonido de pérdida
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoPerdida;
        
        // Asegurarse de que el sonido de fondo esté en loop (si no lo estaba)
        if (audioFondo != null)
        {
            audioFondo.loop = true;
            audioFondo.Play();
        }
    }

    void Update()
    {
        if (!juegoTerminado)
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

            // Verificar si se han acabado los balones
            if (balonesRestantes <= 0)
            {
                TerminarJuego();
            }
        }
    }

    void LanzarBalon()
    {
        if (prefabBalon != null && balonesRestantes > 0)
        {
            GameObject balon = Instantiate(prefabBalon, transform.position + Vector3.forward, Quaternion.identity);
            Rigidbody rb = balon.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.forward * fuerzaLanzamiento, ForceMode.Impulse);
            }
            balonesRestantes--;
        }
    }

    void TerminarJuego()
{
    juegoTerminado = true; // Detener el juego

    // Buscar el objeto con el tag "GoalController" y obtener el componente GoalController
    GameObject goalControllerObject = GameObject.FindWithTag("GoalController");
    if (goalControllerObject != null)
    {
        GoalController goalController = goalControllerObject.GetComponent<GoalController>();
        if (goalController != null)
        {
            goalController.juegoTerminado = true;
        }
    }

    // Detener el sonido de fondo si está asignado
    if (audioFondo != null)
    {
        audioFondo.Stop();
    }

    // Reproducir el sonido de pérdida
    if (sonidoPerdida != null)
    {
        audioSource.Play();
    }
}


    void OnGUI()
    {
        GUIStyle estiloTexto = new GUIStyle();
        estiloTexto.fontSize = 16;
        estiloTexto.normal.textColor = Color.white;

        // Mostrar la cantidad de balones restantes en la esquina inferior izquierda
        float xPos = 10f;
        float yPos = Screen.height - 50f;
        GUI.Box(new Rect(xPos, yPos, 200, 30), ""); // Fondo negro
        GUI.Label(new Rect(xPos + 5f, yPos + 5f, 200, 20), "Balones Restantes: " + balonesRestantes, estiloTexto);

        // Mostrar "Perdiste" en el centro de la pantalla si el juego ha terminado
        if (juegoTerminado)
        {
            GUIStyle estiloPerdiste = new GUIStyle();
            estiloPerdiste.fontSize = 40;
            estiloPerdiste.normal.textColor = Color.red;
            estiloPerdiste.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "GAME OVER", estiloPerdiste);
        }
    }
}
