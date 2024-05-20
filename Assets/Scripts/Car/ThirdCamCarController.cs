using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ThirdCamCarController : MonoBehaviour, IVehicleController
{
    public float brakeTorque;
    public List<AxleInfo> axleInfos;
    public bool IsActiveAndEnabled => isActiveAndEnabled; // Implementación de propiedad de ejemplo
    public SphereCollider col;
    public GameObject camCar;
    public GameObject camPlayer;
    private CarController controller;
    private Rigidbody rb;
    public GameObject canvasControles;
    public Transform initialPoint;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ActivarVehiculo()
    {
        audioSource.Play();
        GameManager.Instance.crosshair.SetActive(false);
        camCar.SetActive(true);
        camPlayer.SetActive(false);
        controller.enabled = true;
        UnBrakeCar();
    }
    public void DesactivarVehiculo()
    {
        audioSource.Stop();
        GameManager.Instance.crosshair.SetActive(true);
        camCar.SetActive(false);
        camPlayer.SetActive(true);
        controller.enabled = false;
        BrakeCar();
    }
    public void ActivarCanvasControles()
    {
        canvasControles.SetActive(true);
    }
    public void DesactivarCanvasControles()
    {
        canvasControles.SetActive(false);
    }
    public void ReiniciarVehiculo()
    {
        gameObject.transform.SetPositionAndRotation(initialPoint.position, initialPoint.rotation);
    }
    public void BrakeCar()
    {
        // Aplicar freno a todas las ruedas si estás usando un sistema de ruedas basado en físicas
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = brakeTorque;
            axleInfo.rightWheel.brakeTorque = brakeTorque;
        }

        // Adicionalmente, puedes aumentar el drag del Rigidbody para asegurar que el vehículo se detiene más rápidamente
        rb.drag = 10;  // Ajusta este valor según sea necesario para obtener una respuesta de frenado adecuada

        // Opcional: Establecer la velocidad angular a cero para evitar que el coche siga girando
        rb.angularVelocity = Vector3.zero;

        // Establecer la velocidad lineal a cero puede ser demasiado abrupto, pero es una opción si necesitas una parada inmediata
        rb.velocity = Vector3.zero;
    }
    public void UnBrakeCar()
    {
        
            // Quitar el freno aplicando un torque de freno de cero a todas las ruedas
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }

            // Restablecer el drag del Rigidbody al valor estándar para la conducción normal
            rb.drag = 0;  // Ajusta este valor según sea necesario basado en el comportamiento deseado

            // Opcional: Restablecer la velocidad angular si fue modificada al desactivar el vehículo
            rb.angularVelocity = Vector3.zero;  // Esto puede ser omitido si no deseas afectar la rotación al reactivar

            // Revisar si es necesario restablecer la velocidad a un valor inicial o permitir que continúe desde cero
            // rb.velocity = algún valor inicial; // Descomentar y ajustar si es necesario
        
    }
}
