using Klareh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingControl : MonoBehaviour
    {
    [Range(1, 8)]
    public float drag;
    


    public float thrust;
        [Range(1,100)]

        public float thrust_multiplier;
        [Range(1,200)]

        public float yaw_multiplier;
        [Range(1,200)]

        public float pitch_multiplier;
        private Rigidbody rb;
        public float thrustChangeRate = 10f; // Tasa de cambio para thrust_multiplier
        public float dragChangeRate = 1f; // Tasa de cambio para thrust_multiplier
        public float velocityLift;

    public Slider thrustSlider;
    public Slider dragSlider;

    public GameObject[] smokes;

    private AudioSource engineAudioSource; // Referencia al AudioSource del motor
    public float minPitch = 0.5f; // Pitch mínimo del motor
    public float maxPitch = 2.0f; // Pitch máximo del motor
    public float multiplier_pitch = 1.0f;

    //public bool canBack;
    void Awake() 
        {
           rb = GetComponent<Rigidbody>();
           rb.useGravity = true;
        //rb.drag = 1f;
        }
    private void Start()
    {
        engineAudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        thrustSlider.value = thrust_multiplier;
        dragSlider.value = drag;
        UpdateEngineSound();
    }
    void FixedUpdate()
    {
        //if (canBack)
        //{
        //    float reverse = Input.GetAxis("Vertical");
        //    float turnReverse = -Input.GetAxis("Horizontal");
        //    if (reverse < 0)
        //    {
        //        // Aplicar una fuerza hacia atrás en la dirección local 'back' del avión, convertida a coordenadas globales
        //        Vector3 forceDirection = transform.TransformDirection(Vector3.back * 20 * Mathf.Abs(reverse));
        //        rb.AddForce(forceDirection * Time.deltaTime, ForceMode.VelocityChange);

        //        // Aplicar un torque relativo para girar, no necesita transformación ya que es un giro sobre el eje Y global
        //        rb.AddTorque(0f, turnReverse * 10 * Time.deltaTime, 0f, ForceMode.VelocityChange);
        //    }
        //}
        //if (rb.velocity.magnitude < 2f)
        //{
        //    canBack = true;
        //}
        //else
        //{
        //    canBack = false;
        //}
        
        float pitch = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Horizontal");
        float fire1 = -Input.GetAxis("Fire1"); // Asumiendo que Fire1 puede ser positivo o negativo

        // Applying thrust force forward relative to the object's orientation
        rb.AddRelativeForce(0f, 0f, thrust * thrust_multiplier * Time.deltaTime);

        // Applying torque for pitch and yaw control, with an additional factor for yaw-induced roll
        rb.AddRelativeTorque(pitch * pitch_multiplier * Time.deltaTime,
                             yaw * yaw_multiplier * Time.deltaTime,
                             -yaw * yaw_multiplier * 2 * Time.deltaTime);

        if (rb.velocity.magnitude > velocityLift || thrust_multiplier > velocityLift)
        {
            rb.useGravity = false;
            rb.drag = 10f;
            GetComponent<ZeroController>().Wheels = false;
            GetComponent<ZeroController>().Engine = true;

        }
        else
        {
            rb.useGravity = true;
            rb.drag = 1f;
            GetComponent<ZeroController>().Wheels = true;
            GetComponent<ZeroController>().Engine = false;
            
        }

        if (rb.velocity.magnitude > 5)
        {
            for (int i = 0; i < smokes.Length; i++)
            {
                smokes[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < smokes.Length; i++)
            {
                smokes[i].SetActive(false);
            }
        }

        // Ajustar thrust_multiplier basado en la entrada Fire1
        if (fire1 > 0) // Incrementar thrust_multiplier y drag
        {
            thrust_multiplier += thrustChangeRate * Time.deltaTime;
            drag += dragChangeRate * Time.deltaTime;
        }
        else if (fire1 < 0) // Decrementar thrust_multiplier y drag
        {
            thrust_multiplier -= thrustChangeRate * Time.deltaTime;
            drag -= dragChangeRate * Time.deltaTime;
        }

        // Clamp both thrust_multiplier and drag within their respective ranges
        thrust_multiplier = Mathf.Clamp(thrust_multiplier, 1, 100);
        drag = Mathf.Clamp(drag, 1, 10); // Assuming 10 is the max value for drag

        rb.drag = drag;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Blocker"))
        {
            // Rota 180 grados alrededor del eje Y
            transform.Rotate(0, 180, 0);
        }
    }

    private void UpdateEngineSound()
    {
        if (engineAudioSource != null)
        {
            float speedPercent = drag / multiplier_pitch; // Convertir km/h a m/s
            engineAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);
        }
    }
}

