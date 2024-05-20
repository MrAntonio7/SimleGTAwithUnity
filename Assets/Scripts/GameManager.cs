using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static GameManager;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public class Misiones
    {
        public int idMision;
        public string nombre;
        public string texto;
        public GameObject[] objetivo;
        public bool activa;
        public bool terminada;
    }

    // Variable para almacenar la puntuación
    public int indexMision = -1;
    [SerializeField] private int objetivoFinal;
    [SerializeField] private int progresoMision;
    [SerializeField] private bool misionTerminada;
    [SerializeField] private GameObject texto;
    [SerializeField] private GameObject textoMision;
    [SerializeField] private GameObject textoObjetivo;
    [SerializeField] private GameObject cursorObject;
    [SerializeField] private bool algunaMisionActiva;
    public GameObject puntoInicial;
    private bool isPaused = false;
    public GameObject menuPausa;
    public List<Misiones> misiones;
    public List<GameObject> vehiculos;
    public GameObject crosshair;
    public GameObject npcMisiones;
    public GameObject npcPuntoFinal;
    public GameObject jugadorPuntoFinal;
    public GameObject[] enemys;

    void Awake()
    {
        // Asegúrate de que solo haya un GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Elimina cualquier instancia adicional
        }
        // Bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        // Hacer que el cursor sea invisible
        Cursor.visible = false;
        // Registrar el método OnSceneLoaded para que se llame cada vez que una escena se carga
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Inicializar el objetivo
        algunaMisionActiva = false;
        enemys = GameObject.FindGameObjectsWithTag("Enemy");


        // Asegúrate de que los objetos de texto estén asignados
        AssignTextObjects();
        UpdateObjetivoText();
        DesactivarObjetosMision();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Comprobar si la escena cargada es la escena del juego
        if (scene.name == "GameScene") // Cambia "GameScene" por el nombre real de tu escena del juego
        {
            // Intentar encontrar el objeto "Cursor" en la escena
            cursorObject = GameObject.Find("Cursor");
            crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            // Verificar si el objeto "Cursor" fue encontrado
            if (cursorObject != null)
            {
                // Realizar acciones con el objeto cursor
            }
            else
            {
                // Manejar el caso en el que el objeto cursor no está presente
            }

            AssignTextObjects();
        }
    }

    void OnDestroy()
    {
        // Desregistrar el método OnSceneLoaded cuando este objeto se destruya
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método para agregar puntos
    public void AddScore(int points)
    {
        progresoMision += points;
        UpdateObjetivoText();
    }

    // Método para restar puntos
    public void SubtractScore(int points)
    {
        progresoMision -= points;
        Debug.Log("Score: " + progresoMision); // Imprime la puntuación actual
        UpdateObjetivoText();
    }

    // Método para obtener la puntuación actual
    public int GetScore()
    {
        return progresoMision;
    }

    private void Update()
    {
        if (isPaused)
        {
            // Bloquear el cursor al centro de la pantalla
            Cursor.lockState = CursorLockMode.None;
            // Hacer que el cursor sea invisible
            Cursor.visible = true;
        }
        else
        {
            // Bloquear el cursor al centro de la pantalla
            Cursor.lockState = CursorLockMode.Locked;
            // Hacer que el cursor sea invisible
            Cursor.visible = false;
        }
        // Verificar si se presiona la tecla "Escape" para pausar/reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }


        // Activar la primera misión no activa
        foreach (var m in misiones)
        {
            if (m.activa)
            {
                algunaMisionActiva = true;
                break;
            }
        }

        UpdateObjetivoText();

        if (DialogManager.Instance.primeraMision && !algunaMisionActiva)
        {
            ActivarMision(0);
            textoMision.SetActive(true);
        }

        if (misionTerminada && ConsutarActivaMision(indexMision))
        {
            misionTerminada = false;
            DesactivarMision(indexMision);
            cursorObject.SetActive(true);
            textoMision.GetComponent<TextMeshProUGUI>().text = "Vuelve y habla con el Jefe";
            textoObjetivo.SetActive(false);
            TerminarMision(indexMision);
            DialogManager.Instance.SelectDialog(indexMision+1);
            progresoMision = 0;
            //ActivarMision(indexMision+1);
            if (indexMision == 3)
            {
                
                ResetVehiculos();
                ResetPlayer(jugadorPuntoFinal);
                ResetNPC(npcMisiones, npcPuntoFinal);
            }
            else if (indexMision == 4)
            {
                
                ResetVehiculos();
                ResetPlayer(jugadorPuntoFinal);
                ResetNPC(npcMisiones, npcPuntoFinal);
                npcMisiones.transform.Find("Cursor").gameObject.SetActive(false);
                textoMision.GetComponent<TextMeshProUGUI>().text = "Has terminado todas las misiones.";
            }
            else if(indexMision == 5) 
            {
                ResetVehiculos();
                ResetPlayer(jugadorPuntoFinal);
                npcMisiones.transform.Find("Cursor").gameObject.SetActive(false);
            }
        }
    }

    private void UpdateObjetivoText()
    {
        if (textoObjetivo != null && indexMision >= 0)
        {
            int finMision = objetivoFinal - progresoMision;
            textoObjetivo.GetComponent<TextMeshProUGUI>().text = finMision.ToString();
            if (finMision <= 0)
            {
                misionTerminada = true;
            }
        }
    }

    public void ActivarMision(int mision)
    {
        if (indexMision < misiones.Count - 1)
        {
            cursorObject.SetActive(false);
            indexMision++;
            textoObjetivo.SetActive(true);
            texto.SetActive(true);
            textoMision.SetActive(true);
            misiones[mision].activa = true;
            objetivoFinal = misiones[mision].objetivo.Length;
            textoMision.GetComponent<TextMeshProUGUI>().text = misiones[mision].texto;
            UpdateObjetivoText();

            foreach (var item in misiones[mision].objetivo)
            {
                item.SetActive(true);
            }
        }
        else
        {
            textoMision.SetActive(true);
            textoMision.GetComponent<TextMeshProUGUI>().text = "Has terminado el juego, no hay mas misiones disponibles por ahora";
            textoObjetivo.SetActive(false);
        }

        
    }

    public void DesactivarMision(int mision)
    {
        misiones[mision].activa = false;
    }

    public void TerminarMision(int mision)
    {
        misiones[mision].terminada = true;
        DesactivarMision(mision);
    }
    public bool ConsutarActivaMision(int index)
    {
        return misiones[index].activa;
    }

    public void DesactivarObjetosMision()
    {
        foreach (var mision in misiones)
        {
            foreach (var item in mision.objetivo)
            {
                item.SetActive(false);
            }
        }
    }
    private void AssignTextObjects()
    {
        if (textoObjetivo == null)
        {
            textoObjetivo = GameObject.Find("Objetivo");
        }
        if (texto == null)
        {
            texto = GameObject.Find("Texto");
        }
        if (textoMision == null)
        {
            textoMision = GameObject.Find("TextoMision");
        }
    }

    // Método para pausar el juego
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        // Bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.None;
        // Hacer que el cursor sea invisible
        Cursor.visible = true;
        // Mostrar el menú de pausa aquí si tienes uno
        menuPausa.SetActive(true);
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        // Bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        // Hacer que el cursor sea invisible
        Cursor.visible = false;
        // Ocultar el menú de pausa aquí si tienes uno
        menuPausa.SetActive(false);
    }
    public void ResetNPC(GameObject npc, GameObject punto)
    {
        npc.transform.SetPositionAndRotation(punto.transform.position, punto.transform.rotation);
        
    }
    public void ResetPlayer(GameObject punto)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        // Desactivar temporalmente el CharacterController

        playerController.BajarDelVehiculo();

        player.GetComponent<RaycastShooter>().enabled = false;
        player.GetComponent<CharController_Motor>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        playerController.enabled = false;
        player.SetActive(false);

        //// Mover al jugador al puntoInicial
        player.transform.SetPositionAndRotation(punto.transform.position, punto.transform.rotation);

        //// Reactivar el CharacterController
        player.SetActive(true);
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<CharController_Motor>().enabled = true;
        player.GetComponent<RaycastShooter>().enabled = true;
        playerController.enabled = true;
        player.GetComponent<SwimController>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<NavMeshAgent>().enabled = false;
        playerController.animator.SetBool("Swim", false);
        playerController.currentVehicle = null;
        playerController.canvaEntryVehicule.SetActive(false);
        playerController.currentHealth = playerController.maxHealth;
        player.GetComponentInChildren<HitController>().atacado = false;
        foreach(var enemy in enemys)
        {
            enemy.GetComponent<EnemyController>().StopHit();
            enemy.GetComponent<EnemyController>().follow = false;
        }

        ResumeGame();
    }

    public void ResetVehiculos()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.BajarDelVehiculo();
        foreach (var v in vehiculos)
        {
            v.GetComponent<IVehicleController>().ReiniciarVehiculo();
        }
        ResumeGame();
    }

    public void SalirMenu()
    {
        isPaused = true;
        // Bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.None;
        // Hacer que el cursor sea invisible
        Cursor.visible = true;
        Debug.Log("Cambiando a la escena menu");
        SceneManager.LoadScene("MenuScene");
    }
    public void SalirJuego()
    {
#if UNITY_EDITOR
        // Si está en el editor, no hacer nada
        Debug.Log("Salir del juego está deshabilitado en el modo Editor.");
#else
            // Si está en una aplicación compilada, cerrar el juego
            Application.Quit();
#endif
    }
}
