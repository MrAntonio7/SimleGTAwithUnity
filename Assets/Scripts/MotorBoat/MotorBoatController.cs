using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorBoatController : MonoBehaviour
{
    public float speed = 10f;          // Velocidad de avance de la lancha
    public float turnSpeed = 50f;      // Velocidad de giro de la lancha
    private Rigidbody rb;              // Referencia al componente Rigidbody

    private AudioSource engineAudioSource; // Referencia al AudioSource del motor
    public float minPitch = 0.5f; // Pitch mínimo del motor
    public float maxPitch = 2.0f; // Pitch máximo del motor
    public float multiplier_pitch = 10.0f;   
    void Start()
    {
        engineAudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>(); // Obtener el componente Rigidbody
    }
    private void Update()
    {
        UpdateEngineSound();
    }
    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");   // Entrada para avanzar o retroceder
        float turnInput = Input.GetAxis("Horizontal"); // Entrada para girar

        // Movimiento hacia adelante y hacia atrás
        rb.AddForce(transform.forward * moveInput * speed);

        // Control de giro
        if (moveInput != 0)  // Solo permite girar si la lancha se está moviendo
        {
            rb.AddTorque(0f, turnInput * turnSpeed * moveInput, 0f);
        }
    }

    private void UpdateEngineSound()
    {
        if (engineAudioSource != null)
        {
            float speedPercent = rb.velocity.magnitude / multiplier_pitch; // Convertir km/h a m/s
            engineAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);
        }
    }
}
