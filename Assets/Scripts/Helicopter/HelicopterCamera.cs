using UnityEngine;

public class HelicopterCamera : MonoBehaviour
{
    public Transform helicopter;  // Referencia al helicóptero
    public GameObject attachPointCam;
    public float heightAboveHelicopter = 5f;  // Altura fija sobre el helicóptero
    public float smoothSpeed = 0.125f;  // Velocidad de suavizado para el seguimiento

    void LateUpdate()
    {
        // Establecer la posición horizontal para seguir al helicóptero
        Vector3 desiredPosition = new Vector3(attachPointCam.transform.position.x, transform.position.y, attachPointCam.transform.position.z);
        // Agregar altura fija sobre el helicóptero para evitar movimientos bruscos verticales
        desiredPosition.y = helicopter.position.y + heightAboveHelicopter;
        // Suavizar la transición de la posición de la cámara para evitar movimientos bruscos
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Fija los ángulos de pitch y roll a 0, manteniendo solo el yaw
        float yRotation = helicopter.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
