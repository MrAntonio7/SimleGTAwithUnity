using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeedController : MonoBehaviour
{
    public Rigidbody boatRigidbody;  // Referencia al Rigidbody de la lancha
    public ParticleSystem[] particleSystems;  // Array de sistemas de part�culas
    public float maxBoatSpeed = 9f;  // Velocidad m�xima esperada de la lancha para escalar

    private ParticleSystem.MainModule[] psMains;  // Array de m�dulos principales de cada sistema de part�culas

    void Start()
    {
        // Inicializar el array de m�dulos principales
        psMains = new ParticleSystem.MainModule[particleSystems.Length];
        for (int i = 0; i < particleSystems.Length; i++)
        {
            // Obtener el m�dulo principal de cada sistema de part�culas para manipularlo
            psMains[i] = particleSystems[i].main;
        }
    }

    void Update()
    {
        // Calcular el factor de escala de la velocidad de part�culas basado en la velocidad actual del barco
        float speedFactor = Mathf.Clamp(boatRigidbody.velocity.magnitude / maxBoatSpeed, 0.1f, 1f);

        // Ajustar el Start Speed de cada sistema de part�culas en el array
        for (int i = 0; i < psMains.Length; i++)
        {
            psMains[i].startSpeed = Mathf.Lerp(1f, 10f, speedFactor);
        }
    }
}