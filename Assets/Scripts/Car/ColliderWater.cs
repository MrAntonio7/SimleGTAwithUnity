using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderWater : MonoBehaviour
{
    public bool cocheInundado = false;
    public GameObject canvasInundado;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("El coche se ha inundado!");
            cocheInundado=true;
            GetComponentInParent<CarController>().inundado = cocheInundado;
            canvasInundado.SetActive(true);
            
        }
    }
}
