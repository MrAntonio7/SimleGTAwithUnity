using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    public Animator anim; // Para habilitar/deshabilitar animaciones
    public Collider col; // El collider principal para el objeto
    public List<RagdollBone> ragdollBones; // Lista de huesos que forman el ragdoll

    // Estructura para almacenar información del ragdoll
    [System.Serializable]
    public class RagdollBone
    {
        public Rigidbody rb; // El Rigidbody para el hueso
        public Collider col; // El Collider para el hueso
    }

    void Start()
    {
        // Activa el ragdoll al inicio si se necesita
        //Active(true);
    }

    void Update()
    {
        // No se requiere nada específico en el Update por ahora
    }

    public void Active(bool active)
    {
        // Activa/desactiva el collider principal
        col.enabled = !active;

        // Activa/desactiva el animator
        anim.enabled = !active;

        // Activa/desactiva el ragdoll para cada hueso
        foreach (var ragdollBone in ragdollBones)
        {
            ragdollBone.rb.isKinematic = !active; // Si es activo, el Rigidbody deja de ser kinematic
            ragdollBone.col.enabled = active; // Habilita o deshabilita el collider
        }
    }
}