using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingLights : MonoBehaviour
{
    public Light[] lights;  // Array de luces que ser�n controladas
    public float flickerInterval = 1.0f;  // Intervalo en segundos para cada cambio de luces

    private float nextFlickerTime = 0f;  // Siguiente momento para cambiar las luces
    private int currentLightIndex = 0;   // �ndice de la luz actualmente encendida

    void Update()
    {
        // Comprobar si es tiempo de cambiar las luces
        if (Time.time >= nextFlickerTime)
        {
            // Apagar la luz actual
            lights[currentLightIndex].enabled = false;

            // Calcular el �ndice de la siguiente luz a encender
            currentLightIndex = (currentLightIndex + 1) % lights.Length;

            // Encender la siguiente luz
            lights[currentLightIndex].enabled = true;

            // Actualizar el tiempo para el pr�ximo cambio
            nextFlickerTime = Time.time + flickerInterval;
        }
    }
}
