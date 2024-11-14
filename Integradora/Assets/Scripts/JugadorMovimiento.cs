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
    public AudioClip sonidoVictoria;
    private AudioSource audioSource; 
    public AudioSource audioFondo; 

    private int tarjetasAmarillas = 0; 
    private bool esVictoria = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

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
                TerminarJuego(false);
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
            TerminarJuego(false);
        }
    }

    public void RecibirTarjetaRoja()
    {
        TerminarJuego(false);
    }

    public void TerminarJuego(bool esVictoria)
    {
        this.esVictoria = esVictoria;
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

        if (esVictoria)
        {
            if (sonidoVictoria != null)
            {
                audioSource.PlayOneShot(sonidoVictoria);
            }
        }
        else
        {
            if (sonidoPerdida != null)
            {
                audioSource.PlayOneShot(sonidoPerdida);
            }
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
            GUIStyle estiloFinal = new GUIStyle();
            estiloFinal.fontSize = 40;
            estiloFinal.alignment = TextAnchor.MiddleCenter;

            estiloFinal.normal.textColor = esVictoria ? Color.green : Color.red;

            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), esVictoria ? "WIN" : "GAME OVER", estiloFinal);
        }
    }
}
