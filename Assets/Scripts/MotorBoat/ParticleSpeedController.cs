using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeedController : MonoBehaviour
{
    public Rigidbody boatRigidbody;  // Referencia al Rigidbody de la lancha
    public ParticleSystem[] particleSystems;  // Array de sistemas de partículas
    public float maxBoatSpeed = 9f;  // Velocidad máxima esperada de la lancha para escalar

    private ParticleSystem.MainModule[] psMains;  // Array de módulos principales de cada sistema de partículas

    void Start()
    {
        // Inicializar el array de módulos principales
        psMains = new ParticleSystem.MainModule[particleSystems.Length];
        for (int i = 0; i < particleSystems.Length; i++)
        {
            // Obtener el módulo principal de cada sistema de partículas para manipularlo
            psMains[i] = particleSystems[i].main;
        }
    }

    void Update()
    {
        // Calcular el factor de escala de la velocidad de partículas basado en la velocidad actual del barco
        float speedFactor = Mathf.Clamp(boatRigidbody.velocity.magnitude / maxBoatSpeed, 0.1f, 1f);

        // Ajustar el Start Speed de cada sistema de partículas en el array
        for (int i = 0; i < psMains.Length; i++)
        {
            psMains[i].startSpeed = Mathf.Lerp(1f, 10f, speedFactor);
        }
    }
}