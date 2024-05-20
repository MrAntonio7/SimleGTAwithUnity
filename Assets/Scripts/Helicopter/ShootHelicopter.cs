using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHelicopter : MonoBehaviour
{
    public Transform firePoint;  // Punto desde donde se emitirá el rayo
    public float range = 100f;   // Alcance del rayo
    public float shootRate = 1f; // Balas por segundo, controla la frecuencia de disparo
    public AudioClip shotSound;  // Clip de sonido del disparo
    public AudioSource audioSource; // Componente AudioSource para reproducir sonidos
    private float shootRateTimer; // Temporizador para controlar el intervalo de disparo
    public bool isShooting;
    public GameObject efectShoot;
    public LayerMask ignoreLayers; // Capas a ignorar

    void Start()
    {

    }

    void Update()
    {
        if (shootRateTimer > 0)
        {
            shootRateTimer -= Time.deltaTime;
        }

        if (Input.GetButton("Fire2"))
        {
            isShooting = true; // El jugador está disparando
            if (shootRateTimer <= 0)
            {
                Shoot();
                shootRateTimer = 1f / shootRate; // Reinicia el temporizador
            }

        }
        else
        {
            isShooting = false; // El jugador ha terminado de disparar
        }
        if (isShooting)
        {
            efectShoot.SetActive(true);
        }
        else
        {
            efectShoot.SetActive(false);
        }

    }

    private void Shoot()
    {
        RaycastHit hit;
        Vector3 direction = firePoint.forward;

        // Dibujar el raycast en la escena para depuración
        Debug.DrawRay(firePoint.position, direction * range, Color.red, 1f);
        if (Physics.Raycast(firePoint.position, direction, out hit, range, ~ignoreLayers))
        {
            //Debug.Log($"Hit {hit.transform.name} at {Time.time}");
            // Verificar si el objeto golpeado es un enemigo
            OvniController ovni = hit.transform.GetComponent<OvniController>();
            if (ovni != null)
            {
                ovni.TakeDamage(20);
            }
        }
        else
        {
            Debug.Log($"Miss at {Time.time}");
        }
        // Reproduce el sonido de disparo
        audioSource.PlayOneShot(shotSound);
    }
}
