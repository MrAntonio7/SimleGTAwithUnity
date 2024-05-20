using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamHelicopterController : MonoBehaviour, IVehicleController
{
    public bool IsActiveAndEnabled => isActiveAndEnabled; // Implementación de propiedad de ejemplo
    public SphereCollider col;
    public GameObject camHelicoper;
    public GameObject camPlayer;
    private HelicopterController controller;
    private Rigidbody rb;
    public GameObject canvasControles;
    public Transform initialPoint;
    private GameObject[] blockers;
    [SerializeField]private AudioSource audioSource;
    public bool canShoot;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<HelicopterController>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        blockers = GameObject.FindGameObjectsWithTag("Blocker");
        audioSource.Stop();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivarVehiculo()
    {
        audioSource.Play();
        camHelicoper.SetActive(true);
        camPlayer.SetActive(false);
        controller.enabled = true;
        rb.useGravity = false;
        if (canShoot)
        {
            GetComponent<ShootHelicopter>().enabled = true;
        }
        
        BlockLimits();
    }
    public void DesactivarVehiculo()
    {
        audioSource.Stop();
        camHelicoper.SetActive(false);
        camPlayer.SetActive(true);
        controller.enabled = false;
        rb.useGravity = true;
        
        if (canShoot)
        {
            GetComponent<ShootHelicopter>().efectShoot.SetActive(false);
            GetComponent<ShootHelicopter>().enabled = false;
        }
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
        gameObject.transform.SetPositionAndRotation(initialPoint.position, initialPoint.rotation);
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
