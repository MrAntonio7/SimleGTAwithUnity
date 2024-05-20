using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    public float forwardSpeed = 10000f;
    public float strafeSpeed = 10000f;
    public float turnSpeed = 1f;
    public float liftSpeed = 3000f;
    public float liftDamping = 500f;  // Fuerza de frenado aplicada para detener la elevación
    public float tiltAngle = 10f;     // Máximo ángulo de inclinación al moverse

    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Hacer que el helicóptero no rote de manera errática
        rb.angularDrag = 5f;
        rb.mass = 1000f;
        
    }

    void FixedUpdate()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float strafeInput = Input.GetAxis("Horizontal");
        float liftInput = Input.GetAxis("Jump");
        float turnInput = Input.GetAxis("Fire1");
        float descendInput = Input.GetAxis("Fire3");

        Vector3 movementForce = transform.forward * forwardInput * forwardSpeed
                                + transform.right * strafeInput * strafeSpeed
                                + Vector3.up * (liftInput * liftSpeed - descendInput * liftSpeed);

        if (Mathf.Approximately(liftInput, 0) && descendInput == 0)
        {
            movementForce += Vector3.down * liftDamping;  // Frenado para mantener la altura
        }

        rb.AddForce(movementForce);

        // Manejar la rotación de giro
        rb.angularVelocity = Vector3.up * turnInput * turnSpeed;

        // Ajustar la inclinación del helicóptero basada en el movimiento
        Vector3 currentAngles = transform.eulerAngles;
        float tiltX = Mathf.LerpAngle(currentAngles.x, forwardInput * tiltAngle, Time.deltaTime * 5);
        float tiltZ = Mathf.LerpAngle(currentAngles.z, -strafeInput * tiltAngle, Time.deltaTime * 5);
        Quaternion targetRotation = Quaternion.Euler(tiltX, currentAngles.y, tiltZ);

        rb.MoveRotation(targetRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Blocker"))
        {
            // Rota 180 grados alrededor del eje Y
            transform.Rotate(0, 180, 0);
        }
    }
}
