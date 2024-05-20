using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimController : MonoBehaviour
{
    public float speed = 2f;          // Velocidad de avance de la lancha
    public float turnSpeed = 20f;      // Velocidad de giro de la lancha
    private Rigidbody rb;              // Referencia al componente Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Obtener el componente Rigidbody
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");   // Entrada para avanzar o retroceder
        //float turnInput = Input.GetAxis("Horizontal"); // Entrada para girar

        // Movimiento hacia adelante y hacia atrás
        rb.AddForce(transform.forward * moveInput * speed);

        // Control de giro
        //if (moveInput != 0)  // Solo permite girar si la lancha se está moviendo
        //{
        //    rb.AddTorque(0f, turnInput * turnSpeed * moveInput, 0f);
        //}
    }
}