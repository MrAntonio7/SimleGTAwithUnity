using System.Collections.Generic;
using UnityEngine;

public class SoundManagerCar : MonoBehaviour
{
    public AudioClip carcrach; // El sonido que se va a reproducir
    public int maxAudioSources = 5; // Máximo número de AudioSources para el solapamiento
    public float volume = 0.5f; // Volumen del sonido
    public float spatialBlend = 1.0f; // 0 = 2D, 1 = 3D

    private List<AudioSource> audioSources;

    void Start()
    {
        audioSources = new List<AudioSource>();

        // Crear múltiples AudioSources
        for (int i = 0; i < maxAudioSources; i++)
        {
            AudioSource newSource = CreateNewAudioSource();
            audioSources.Add(newSource);
        }
    }

    public void PlaySound()
    {
        // Buscar un AudioSource disponible
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.Play();
                return;
            }
        }

        // Si no hay AudioSources disponibles, reproducir en uno nuevo
        AudioSource newSource = CreateNewAudioSource();
        newSource.Play();
        audioSources.Add(newSource);

        // Mantener el número de AudioSources dentro del límite
        if (audioSources.Count > maxAudioSources)
        {
            Destroy(audioSources[0]);
            audioSources.RemoveAt(0);
        }
    }

    private AudioSource CreateNewAudioSource()
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = carcrach;
        newSource.volume = volume;
        newSource.spatialBlend = spatialBlend; // Convertir el sonido a 3D
        newSource.rolloffMode = AudioRolloffMode.Logarithmic; // Ajuste estándar para sonidos 3D
        newSource.minDistance = 1f; // Distancia mínima para el volumen completo
        newSource.maxDistance = 500f; // Distancia máxima a la que se escucha el sonido
        return newSource;
    }
}