using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    public bool atacado;
    // Start is called before the first frame update
    void Start()
    {
        atacado = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyController>().isAlive && !atacado)
            {
                other.gameObject.GetComponent<EnemyController>().Hit();
                atacado=true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyController>().isAlive && atacado)
            {
                other.gameObject.GetComponent<EnemyController>().StopHit();
                atacado=false;
            }
        }
    }
}
