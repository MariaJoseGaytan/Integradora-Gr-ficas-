using UnityEngine;

public class JugadorMovimiento : MonoBehaviour
{
    public float velocidadMovimiento = 10f; 
    public float limiteIzquierdo = -7f;
    public float limiteDerecho = 11f;
    public GameObject prefabBalon;
    public float fuerzaLanzamiento = 15f;

    private int balonesRestantes = 50; 
    private bool juegoTerminado = false; 

    public AudioClip sonidoPerdida;
    private AudioSource audioSource; 
    public AudioSource audioFondo; 

    private int tarjetasAmarillas = 0; // Contador de tarjetas amarillas recibidas

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoPerdida;

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
            float movimientoHorizontal = Input.GetAxis("Horizontal") * velocidadMovimiento * Time.deltaTime;
            float nuevaPosX = Mathf.Clamp(transform.position.x + movimientoHorizontal, limiteIzquierdo, limiteDerecho);
            transform.position = new Vector3(nuevaPosX, transform.position.y, transform.position.z);

            if (Input.GetKeyDown(KeyCode.UpArrow) && balonesRestantes > 0)
            {
                LanzarBalon();
            }

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

    public void RecibirTarjetaAmarilla()
    {
        tarjetasAmarillas++;
        if (tarjetasAmarillas >= 2)
        {
            TerminarJuego();
        }
    }

    public void RecibirTarjetaRoja()
    {
        TerminarJuego();
    }

    public void TerminarJuego()
    {
        juegoTerminado = true;

        GameObject goalControllerObject = GameObject.FindWithTag("GoalController");
        if (goalControllerObject != null)
        {
            GoalController goalController = goalControllerObject.GetComponent<GoalController>();
            if (goalController != null)
            {
                goalController.juegoTerminado = true;
            }
        }

        if (audioFondo != null)
        {
            audioFondo.Stop();
        }

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

        float xPos = 10f;
        float yPos = Screen.height - 50f;
        GUI.Box(new Rect(xPos, yPos, 200, 30), ""); 
        GUI.Label(new Rect(xPos + 5f, yPos + 5f, 200, 20), "Balones Restantes: " + balonesRestantes, estiloTexto);

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
