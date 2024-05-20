using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAlignment : MonoBehaviour
{
    public Animator animPlayer;           // Animator del jugador
    public Transform hand;                // Mano que sostiene el rifle

    public Vector3 localOffsetPositionAttack; // Desplazamiento de posición local para ataque
    public Vector3 localOffsetRotationAttack; // Desplazamiento de rotación local para ataque

    public Vector3 localOffsetPositionWalkL;  // Desplazamiento de posición local para caminar a la izquierda
    public Vector3 localOffsetRotationWalkL;  // Desplazamiento de rotación local para caminar a la izquierda

    private Vector3 originalLocalPosition; // Posición local original del rifle
    private Quaternion originalLocalRotation; // Rotación local original del rifle

    void Start()
    {
        // Guardar la posición y rotación locales originales
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
    }

    void FixedUpdate()
    {
        // Aplicar primero la posición y rotación originales para asegurar una base consistente
        transform.localPosition = originalLocalPosition;
        transform.localRotation = originalLocalRotation;

        // Evaluar las condiciones de las animaciones y aplicar offsets correspondientes
        if (animPlayer.GetBool("Attack"))
        {
            ApplyLocalOffset(localOffsetPositionAttack, localOffsetRotationAttack);
        }
        else if (animPlayer.GetBool("WalkL"))
        {
            ApplyLocalOffset(localOffsetPositionWalkL, localOffsetRotationWalkL);
        }
    }

    void ApplyLocalOffset(Vector3 positionOffset, Vector3 rotationOffset)
    {
        transform.localPosition += positionOffset;
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + rotationOffset);
    }
}
