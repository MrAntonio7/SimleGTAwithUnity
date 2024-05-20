using UnityEngine;

public class RotateObject : MonoBehaviour
{

    public float rotationSpeed = 100f; // Velocidad de rotación en grados por segundo
    public bool collectable;
    public bool collected;
    [SerializeField]private Collider col;
    public GameObject[] model;
    public bool rotateZ;
    public bool rotateY;
    public bool rotateX;

    private void Start()
    {
        collected = false;
        col = GetComponent<Collider>(); // Inicializa el collider
    }

    void Update()
    {
        if (rotateX)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        }
        if (rotateY)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
        if (rotateZ)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        // Actualiza la visibilidad y el estado del collider basándose en la variable collectable
        if (collected)
        {
            OcultarObjecto();
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Airplane"))
        {
            if (!collected && collectable)
            {
                collected = true;
                GameManager.Instance.AddScore(1);
                other.gameObject.GetComponentInChildren<AudioSource>().Play();
            }
        }

    }

    public void OcultarObjecto()
    {
        col.enabled = false;
        foreach (var item in model)
        {
            item.SetActive(false);
        }
    }
}