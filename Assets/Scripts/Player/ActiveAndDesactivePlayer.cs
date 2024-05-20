using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActiveAndDesactivePlayer : MonoBehaviour
{
    public GameObject modelPlayer;
    public GameObject modelRifle;
    public GameObject hit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DesactivarJugador()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<RaycastShooter>().enabled = false;
        GetComponent<SwimController>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        modelPlayer.SetActive(false);
        modelRifle.SetActive(false);
        hit.SetActive(false);
    }
    public void ActivarJugador() 
    {
        GetComponent<CharacterController>().enabled = true;
        GetComponent<RaycastShooter>().enabled = true;
        modelPlayer.SetActive(true);
        modelRifle.SetActive(true);
        hit.SetActive(true);
        GetComponent<PlayerController>().animator.SetBool("Swim", false);
    }

}
