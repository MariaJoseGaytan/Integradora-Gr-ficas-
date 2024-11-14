using UnityEngine;

public class BalonMovimiento : MonoBehaviour
{
    private float posicionY;

    void Update()
    {
        // Obtén la posición en Y del balón
        posicionY = transform.position.y;
    }
}
