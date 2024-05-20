using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public Animator animator;
    public GameObject hit;
    private RaycastShooter shooter;
    private Vector3 lockedPosition; // Guarda la posición cuando el jugador empieza a disparar
    private ActiveAndDesactivePlayer activeAndDesactivePlayer;
    public GameObject canvaEntryVehicule;
    public GameObject currentVehicle;
    public GameObject vehicleController;
    public bool subidoVehiculo;
    public GameObject colliderSwim;
    private Rigidbody rb;
    public GameObject canvasInundado;
    public int maxHealth;
    public int currentHealth;
    public Slider healthSlider;
    private AudioSource audioSource;
    public AudioSource swinSource;
    private void Awake()
    {

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        rb = GetComponent<Rigidbody>();
        subidoVehiculo = false;
        activeAndDesactivePlayer = GetComponent<ActiveAndDesactivePlayer>();
        shooter = GetComponent<RaycastShooter>();
        characterController = GetComponent<CharacterController>();

        maxHealth = 20;
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        HandleShooting();

        if (shooter.isShooting)
        {
            BlockMovement();
        }
        else
        {
            HandleMovement();
            lockedPosition = transform.position; // Actualiza la posición bloqueada cada vez que el jugador no está disparando
        }

        HandleJumpAndCursor();
        healthSlider.value = currentHealth;

    }

    private void LateUpdate()
    {
        // Maneja la interacción con los vehiculos
        if (currentVehicle && Input.GetKeyDown(KeyCode.F))
        {
            HandleVehicleInteraction(currentVehicle);

        }
        // Maneja la interacción con los vehiculos
        if (currentVehicle && Input.GetKeyDown(KeyCode.LeftControl))
        {
            ReinciarVehiculo(currentVehicle);

        }
        if (subidoVehiculo)
        {
            transform.position = currentVehicle.transform.position + new Vector3(0f, 2f, 2f);
            canvaEntryVehicule.SetActive(false);
        }

        
    }
    public void JugadorBloqueado()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CharController_Motor>().enabled = false;
        GetComponent<RaycastShooter>().enabled = false;
        animator.SetBool("Run", false);
        animator.SetBool("Back", false);
        animator.SetBool("WalkL", false);
        animator.SetBool("WalkR", false);
        animator.SetBool("Jump", false);
        animator.SetBool("Attack", false);
    }
    public void JugadorDesbloqueado()
    {
        GetComponent<CharacterController>().enabled = true;
        GetComponent<CharController_Motor>().enabled = true;
        GetComponent<RaycastShooter>().enabled = true;
    }
    public void BlockMovement()
    {
        // Bloquea la posición actualizando constantemente la posición del jugador a la posición bloqueada
        //Debug.Log("Jugador bloqueado");
        characterController.enabled = false;
        transform.position = lockedPosition;
        characterController.enabled = true;
    }
    public void UnlockMovement()
    {
        // Bloquea la posición actualizando constantemente la posición del jugador a la posición bloqueada
        //Debug.Log("Jugador bloqueado");
        characterController.enabled = true;
        characterController.enabled = false;
    }
    public void HandleMovement()
    {
        //Debug.Log("Jugador desbloqueado");
        float moveY = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveX, 0.0f, moveY);
        characterController.Move(movement * Time.deltaTime);
        // Update animations based on movement
        animator.SetBool("Run", moveY > 0f);
        animator.SetBool("Back", moveY < 0f);
        animator.SetBool("WalkR", moveX > 0f);
        animator.SetBool("WalkL", moveX < 0f);
        if ((animator.GetBool("Run")|| animator.GetBool("Back") || animator.GetBool("WalkL") || animator.GetBool("WalkR"))&& !animator.GetBool("Swim"))
        {

                if (!audioSource.isPlaying & !subidoVehiculo)
                {
                    audioSource.Play();
                }

        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        if (animator.GetBool("Swim") & !subidoVehiculo)
        {
            if (!swinSource.isPlaying)
            {
                swinSource.Play();
            }
        }
        else
        {
            if (swinSource.isPlaying)
            {
                swinSource.Stop();
            }
            swinSource.Stop();
        }
    }

    public void HandleShooting()
    {
        if (Input.GetButton("Fire1"))
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    private void HandleJumpAndCursor()
    {
        if (Input.GetButton("Jump"))
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EntryVehicle") && !subidoVehiculo)
        {
            canvaEntryVehicule.SetActive(true);
            currentVehicle = other.gameObject; // Asignar el helicóptero actual
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EntryVehicle") && !subidoVehiculo)
        {
            canvaEntryVehicule.SetActive(false);
            currentVehicle = null; // Limpiar la referencia al helicóptero
        }
    }

    private void HandleVehicleInteraction(GameObject currentVehicle)
    {
        //IVehicleController vehicleController = currentVehicle.GetComponentInParent<IVehicleController>();
        vehicleController = currentVehicle;
        if (vehicleController != null)
        {
            if (vehicleController.GetComponentInParent<IVehicleController>().IsActiveAndEnabled && !subidoVehiculo)
            {
                SubirAlVehiculo();
            }
            else if (vehicleController.GetComponentInParent<IVehicleController>().IsActiveAndEnabled && subidoVehiculo)
            {
                BajarDelVehiculo();
            }
        }
    }
    public void SubirAlVehiculo()
    {
        DesactivarAnimaciones();
        subidoVehiculo = true;
        vehicleController.GetComponentInParent<IVehicleController>().ActivarVehiculo();
        vehicleController.GetComponentInParent<IVehicleController>().ActivarCanvasControles();
        colliderSwim.SetActive(false);
        rb.isKinematic = false;
        activeAndDesactivePlayer.DesactivarJugador();
        if (currentVehicle.GetComponentInParent<CarController>() != null)
        {
            if (currentVehicle.GetComponentInParent<CarController>().inundado)
            {
                canvasInundado.SetActive(true);
            }
            else
            {
                canvasInundado.SetActive(false);
            }
        }
        else
        {

        }

        //gameObject.transform.SetParent(currentVehicle.transform);

        //gameObject.transform.SetPositionAndRotation(new Vector3(currentVehicle.transform.position.x, currentVehicle.transform.position.y + 2f, currentVehicle.transform.position.z+2f),gameObject.transform.rotation);
    }
    public void BajarDelVehiculo()
    {
        if (vehicleController == null)
        {
            colliderSwim.SetActive(true);
            rb.isKinematic = true;
            activeAndDesactivePlayer.ActivarJugador();
            canvasInundado.SetActive(false);
            //transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            subidoVehiculo = false;
            vehicleController.GetComponentInParent<IVehicleController>().DesactivarVehiculo();
            vehicleController.GetComponentInParent<IVehicleController>().DesactivarCanvasControles();
            colliderSwim.SetActive(true);
            rb.isKinematic = true;
            activeAndDesactivePlayer.ActivarJugador();
            canvasInundado.SetActive(false);
            //gameObject.transform.SetParent(null);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            vehicleController = null;
        }
        
    }
    public void ReinciarVehiculo(GameObject currentVehicle)
    {
        IVehicleController vehicleController = currentVehicle.GetComponentInParent<IVehicleController>();
        if (vehicleController != null && vehicleController.IsActiveAndEnabled)
        {
            vehicleController.ReiniciarVehiculo();

        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player defeated!");
        GameManager.Instance.ResetPlayer(GameManager.Instance.puntoInicial);
        GameManager.Instance.ResetVehiculos();
        currentHealth = maxHealth;
        // Implementa la lógica de muerte del jugador aquí
    }
    public void DesactivarAnimaciones()
    {
        animator.SetBool("Run",false);
        animator.SetBool("Back", false);
        animator.SetBool("WalkL", false);
        animator.SetBool("WalkR", false);
    }

}
