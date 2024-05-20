using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float floatSpeed = 3f; // Velocidad de subida y bajada
    public float floatHeight = 0.2f; // Altura m�xima de la flotaci�n
    public float offset;

    void Start()
    {
    }

    void Update()
    {

        // Obtener las coordenadas globales del padre para X y Z
        Vector3 parentGlobalPosition = transform.parent.position;
        float positionY = transform.parent.position.y + offset;

        // Calcular la nueva posici�n utilizando una onda senoidal
        float newY = positionY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Actualizar la posici�n del objeto
        transform.position = new Vector3(parentGlobalPosition.x, newY, parentGlobalPosition.z);
    }
}
