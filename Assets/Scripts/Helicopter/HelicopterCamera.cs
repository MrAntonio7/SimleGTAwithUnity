using UnityEngine;

public class HelicopterCamera : MonoBehaviour
{
    public Transform helicopter;  // Referencia al helic�ptero
    public GameObject attachPointCam;
    public float heightAboveHelicopter = 5f;  // Altura fija sobre el helic�ptero
    public float smoothSpeed = 0.125f;  // Velocidad de suavizado para el seguimiento

    void LateUpdate()
    {
        // Establecer la posici�n horizontal para seguir al helic�ptero
        Vector3 desiredPosition = new Vector3(attachPointCam.transform.position.x, transform.position.y, attachPointCam.transform.position.z);
        // Agregar altura fija sobre el helic�ptero para evitar movimientos bruscos verticales
        desiredPosition.y = helicopter.position.y + heightAboveHelicopter;
        // Suavizar la transici�n de la posici�n de la c�mara para evitar movimientos bruscos
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Fija los �ngulos de pitch y roll a 0, manteniendo solo el yaw
        float yRotation = helicopter.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
