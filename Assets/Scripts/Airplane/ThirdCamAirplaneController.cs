using Klareh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamAirplaneController : MonoBehaviour, IVehicleController
{
    public bool IsActiveAndEnabled => isActiveAndEnabled; // Implementación de propiedad de ejemplo
    public SphereCollider col;
    public GameObject camAirplane;
    public GameObject camPlayer;
    private FlyingControl controller;
    private Rigidbody rb;
    public GameObject canvasControles;
    public Transform initialPoint;
    private GameObject[] blockers;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<FlyingControl>();
        rb = GetComponent<Rigidbody>();
        blockers = GameObject.FindGameObjectsWithTag("Blocker");
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
        foreach (var smoke in GetComponent<FlyingControl>().smokes) {
        smoke.SetActive(true);
        }
        GameManager.Instance.crosshair.SetActive(false);
        camAirplane.SetActive(true);
        camPlayer.SetActive(false);
        controller.enabled = true;
        rb.useGravity = false;
        BlockLimits();
    }
    public void DesactivarVehiculo()
    {
        audioSource.Stop();
        foreach (var smoke in GetComponent<FlyingControl>().smokes)
        {
            smoke.SetActive(false);
        }
        GetComponentInParent<FlyingControl>().thrust_multiplier = 1;
        GetComponentInParent<FlyingControl>().drag = 1;
        GameManager.Instance.crosshair.SetActive(true);
        camAirplane.SetActive(false);
        camPlayer.SetActive(true);
        controller.enabled = false;
        rb.useGravity = true;
        UnBlockLimits();
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
        GetComponentInParent<ZeroController>().Engine = false;
        GetComponentInParent<ZeroController>().Wheels = true;
        gameObject.transform.SetPositionAndRotation(initialPoint.position,initialPoint.rotation);
    }
    public void BlockLimits()
    {
        foreach (var blocker in blockers)
        {
            blocker.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
    public void UnBlockLimits()
    {
        foreach (var blocker in blockers)
        {
            blocker.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
