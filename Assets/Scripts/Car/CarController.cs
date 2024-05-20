using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float brakeTorque;
    public float maxSpeed = 100f; // Velocidad máxima en km/h
    public float reverseAccelerationFactor = 0.5f; // Factor de aceleración en marcha atrás
    public GameObject brakeLights; // Referencia a las luces de freno
    public GameObject reverseLights; // Referencia a las luces de marcha atrás

    private Rigidbody carRigidbody;
    public bool inundado;
    private AudioSource engineAudioSource; // Referencia al AudioSource del motor
    public float minPitch = 0.5f; // Pitch mínimo del motor
    public float maxPitch = 2.0f; // Pitch máximo del motor
    public float multiplier_pitch = 10.0f;
    void Start()
    {
        engineAudioSource = GetComponent<AudioSource>();
        carRigidbody = GetComponent<Rigidbody>();
        SetBrakeLights(false);
        SetReverseLights(false);
    }

    // Enciende o apaga las luces de freno
    void SetBrakeLights(bool state)
    {
        brakeLights.SetActive(state);
    }

    // Enciende o apaga las luces de marcha atrás
    void SetReverseLights(bool state)
    {
        reverseLights.SetActive(state);
    }

    void Update()
    {
        UpdateEngineSound();
    }
    public void FixedUpdate()
    {


        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        bool isBraking = Input.GetKey(KeyCode.Space) || (Input.GetAxis("Vertical") < 0 && carRigidbody.velocity.sqrMagnitude > 0.1f && Vector3.Dot(transform.forward, carRigidbody.velocity) > 0);
        bool isReversing = Input.GetAxis("Vertical") < 0 && Vector3.Dot(transform.forward, carRigidbody.velocity) < 0;

        SetReverseLights(isReversing);
        SetBrakeLights(isBraking && !isReversing); // Asegura que las luces de freno no se activen cuando se está en reversa

        if (carRigidbody.velocity.magnitude > maxSpeed / 3.6f)
        { // Convertir km/h a m/s
            carRigidbody.velocity = carRigidbody.velocity.normalized * maxSpeed / 3.6f;
        }

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            if (axleInfo.motor)
            {
                if (isReversing)
                { // Manejo de la marcha atrás
                    motor *= reverseAccelerationFactor;
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                else
                {
                    axleInfo.leftWheel.motorTorque = isBraking ? 0 : motor;
                    axleInfo.rightWheel.motorTorque = isBraking ? 0 : motor;
                }
            }

            if (isBraking)
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        //
        if (GetComponentInChildren<ColliderWater>().cocheInundado)
        {
            motor = 0f;
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
            }
        }
        //
        
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {

        }
    }
    private void UpdateEngineSound()
    {
        if (engineAudioSource != null)
        {
            float speedPercent = carRigidbody.velocity.magnitude / multiplier_pitch; // Convertir km/h a m/s
            engineAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);
        }
    }
}
