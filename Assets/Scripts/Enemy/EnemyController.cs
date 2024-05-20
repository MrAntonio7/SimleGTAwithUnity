using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int lives = 6;
    public bool isAlive = true;
    public bool isMision;
    private Collider mainCollider;
    private Animator anim;
    public ActiveRagdoll ragdoll;
    private NavMeshAgent agent; // Referencia al NavMeshAgent
    private GameObject player;
    private Rigidbody rb;
    [SerializeField]private GameObject cursor;
    private bool atacando;
    public bool follow;
    public bool deanbulando;
    public float wanderRadius; // Radio de deambulación
    public float wanderTimer; // Tiempo entre cada cambio de dirección
    private float timer;

    void Start()
    {
        cursor = transform.Find("Cursor").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        // Inicializa el enemigo (si es necesario)
        // Obtener referencias a los componentes
        mainCollider = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        ragdoll = GetComponent<ActiveRagdoll>();
        ragdoll.Active(false);
        wanderRadius = 5f;
        wanderTimer = 0.1f;
        timer = wanderTimer;
    }

    private void Update()
    {

        if (isAlive)
        {
            if (agent.enabled && player != null && follow)
            {
                FollowPlayer();
                
            }
            else if (!follow)
            {
                //agent.SetDestination(transform.position);
                anim.SetBool("Run", false);
                Wander();
            }
            if (atacando)
            {
                RotateTowardsPlayer();
            }
            anim.SetBool("Attack", atacando);
        }

        if (lives <= 0 && isAlive)
        {
            Die();
        }


        cursor.SetActive(isAlive && isMision);
    }

    public void TakeDamage()
    {
        if (!isAlive) return;

        lives--;
        Debug.Log("Enemy hit! Lives remaining: " + lives);
    }

    public void Hit()
    {
        if (isAlive)
        {
            //Quitar daño al jugador cada dos segundos
            atacando = true;
            StartCoroutine(DamagePlayerOverTime());
        }
        
    }

    public void StopHit()
    {
        atacando = false;
        StopCoroutine(DamagePlayerOverTime());
    }

    private void Die()
    {
        isAlive = false;
        StopHit();
        // Activa el ragdoll
        if (agent.enabled)
        {
            agent.enabled = false;
        }
        ragdoll.Active(true);
        Debug.Log("Enemy defeated!");

        if (isMision)
        {
            GameManager.Instance.AddScore(1);
        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 2f)
            {
                collision.gameObject.GetComponent<SoundManagerCar>().PlaySound();
                lives = 0;
            }
        }
    }

    private IEnumerator DamagePlayerOverTime()
    {
        while (atacando)
        {
            player.GetComponent<PlayerController>().TakeDamage(1); // Asumiendo que el jugador tiene un componente PlayerHealth con un método TakeDamage(int)
            Debug.Log("Player hit! Health remaining: " + player.GetComponent<PlayerController>().currentHealth);
            yield return new WaitForSeconds(2f);
        }
    }
    private void FollowPlayer()
    {
        float minimumDistance = 1f; // Distancia mínima que el enemigo debe mantener con el jugador

        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > minimumDistance)
        {
            // Calcula una posición en línea recta hacia el jugador pero a la distancia mínima
            Vector3 targetPosition = player.transform.position - directionToPlayer.normalized * minimumDistance;
            agent.SetDestination(targetPosition);
        }
        else
        {
            // Si está dentro de la distancia mínima, detén al enemigo
            agent.SetDestination(transform.position);
        }

        anim.SetBool("Walk", false);
        deanbulando = false;
        anim.SetBool("Run", true);
    }
    private void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            deanbulando = true;
            anim.SetBool("Walk",true);
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Ajusta la velocidad de rotación según sea necesario
    }
}
