using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamMotorBoatController : MonoBehaviour, IVehicleController
{
    public float brake;
    public bool IsActiveAndEnabled => isActiveAndEnabled; // Implementación de propiedad de ejemplo
    public SphereCollider col;
    public GameObject camBoat;
    public GameObject camPlayer;
    private MotorBoatController controller;
    public GameObject modelSitting;
    public GameObject canvasControles;
    public Transform initialPoint;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<MotorBoatController>();
        audioSource = GetComponent<AudioSource>();
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
        camBoat.SetActive(true);
        camPlayer.SetActive(false);
        controller.enabled = true;
        modelSitting.SetActive(true);
    }
    public void DesactivarVehiculo()
    {
        audioSource.Stop();
        GameManager.Instance.crosshair.SetActive(true);
        camBoat.SetActive(false);
        camPlayer.SetActive(true);
        controller.enabled = false;
        modelSitting.SetActive(false);
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
}
