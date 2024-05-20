using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SmokeEmissionController : MonoBehaviour
{

    public ParticleSystem exhaustSmoke;
    public float maxEmissionRate = 50f;

    void Update()
    {
        if (GetComponent<CarController>().enabled)
        {
            var emissionModule = exhaustSmoke.emission;
            float rate = maxEmissionRate * Input.GetAxis("Vertical");
            emissionModule.rateOverTime = rate;
        }
        
    }
}
