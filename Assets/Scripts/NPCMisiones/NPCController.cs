using UnityEngine;

public class NPCController : MonoBehaviour
{
    public bool npcMisiones;
    public GameObject canvasInteraction;
    private bool isPlayerInRange = false; // Variable para verificar si el jugador está en el rango del trigger

    void Start()
    {
        canvasInteraction.SetActive(false); // Asegurarse de que el canvas está desactivado al inicio
    }

    void Update()
    {
        if (npcMisiones)
        {
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !DialogManager.Instance.dialogActive && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentVehicle == null)
            {
                StartDialog();
            }

            if (DialogManager.Instance.dialogActive)
            {
                canvasInteraction.SetActive(false);
            }
            else if (isPlayerInRange)
            {
                canvasInteraction.SetActive(true);
            }
            GetComponent<Animator>().SetBool("Speaking", DialogManager.Instance.dialogActive);
        }
        else
        {
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !DialogNPCVehiculosManager.Instance.dialogActive && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentVehicle == null)
            {
                StartDialog();
            }
            if (DialogNPCVehiculosManager.Instance.dialogActive)
            {
                canvasInteraction.SetActive(false);
            }
            else if (isPlayerInRange)
            {
                canvasInteraction.SetActive(true);
            }
            GetComponent<Animator>().SetBool("Speaking", DialogNPCVehiculosManager.Instance.dialogActive);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentVehicle == null)
            {
                canvasInteraction.SetActive(true);
                isPlayerInRange = true; // El jugador está en rango
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasInteraction.SetActive(false);
            isPlayerInRange = false; // El jugador salió del rango
        }
    }

    private void StartDialog()
    {
        if (npcMisiones)
        {
            DialogManager.Instance.StartDialog();
        }
        else
        {
            DialogNPCVehiculosManager.Instance.StartDialog();
        }
        GetComponent<Animator>().SetBool("Speaking", true);
        RotateTowardsPlayer();
        
        isPlayerInRange = false; // Descativar esto para que cuando este en el dialogo no pueda empezar o reiniciar el dialogo
    }
    private void RotateTowardsPlayer()
    {
        Vector3 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 200f); // Ajusta la velocidad de rotación según sea necesario
    }
}
