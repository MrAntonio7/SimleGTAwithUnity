using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light targetLight;  // Referencia a la luz que debe parpadear
    public float flickerInterval = 0.5f;  // Intervalo en segundos para cada parpadeo

    private float nextFlickerTime = 0f;  // Momento en el tiempo cuando la luz debe cambiar de estado nuevamente

    void Update()
    {
        if (Time.time >= nextFlickerTime)
        {
            // Alternar el estado de la luz
            targetLight.enabled = !targetLight.enabled;

            // Actualizar el próximo momento para cambiar el estado
            nextFlickerTime = Time.time + flickerInterval;
        }
    }
}