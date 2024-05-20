using UnityEngine;
using UnityEngine.AI;

public class DetectWaterCollision : MonoBehaviour
{
    public SwimController swimController;
    public CharacterController characterController;
    public RaycastShooter shooter;
    public PlayerController playerController;
    public NavMeshAgent navMeshAgent;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            swimController.enabled = true;
            characterController.enabled = false;
            shooter.enabled = false ;
            player.GetComponent<Rigidbody>().isKinematic = false;
            navMeshAgent.enabled = true ;
            playerController.animator.SetBool("Swim", true);
            Debug.Log("Triggered with water!");
        }
        if (other.gameObject.CompareTag("Terrain"))
        {
            swimController.enabled = false;
            characterController.enabled = true;
            player.GetComponent<Rigidbody>().isKinematic = true;
            shooter.enabled = true;
            navMeshAgent.enabled = false ;
            playerController.animator.SetBool("Swim", false);
            
            Debug.Log("Triggered with terrein!");
        }
    }

}