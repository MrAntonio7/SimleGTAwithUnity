using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogNPCVehiculosManager : MonoBehaviour
{


    public static DialogNPCVehiculosManager Instance { get; private set; }
    public TextMeshProUGUI dialogText; // Referencia al objeto de texto en el Canvas
    private int currentLineIndex = 0; // �ndice de la l�nea de di�logo actual
    public GameObject canvasDialog;
    public bool dialogActive;
    public GameObject imagenTriangulo;
    public List<string> dialogLines; // Array de l�neas de di�logo
    public List<GameObject> vehiculos;

    private void Awake()
    {
        // Aseg�rate de que solo haya un DialogManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Elimina cualquier instancia adicional
        }
    }

    void Start()
    {
        dialogActive = false;
        canvasDialog.SetActive(false);
        // Asegurarse de que el texto de di�logo comienza con la primera l�nea
        if (dialogLines.Count > 0)
        {
            dialogText.text = dialogLines[currentLineIndex];
        }
    }

    void Update()
    {
        if (dialogActive)
        {
            // Detectar la tecla "F" o "Enter" para avanzar en el di�logo
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            {
                AdvanceDialog();
            }
        }

        if (currentLineIndex < dialogLines.Count - 1)
        {
            imagenTriangulo.SetActive(true);
        }
        else
        {
            imagenTriangulo.SetActive(false);
        }
    }

    public void StartDialog()
    {

        
        dialogActive = true;
        canvasDialog.SetActive(true);
        currentLineIndex = 0;
        dialogText.text = dialogLines[currentLineIndex];
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.JugadorBloqueado();
        player.enabled = false;
        
        
    }

    void AdvanceDialog()
    {
        if (!dialogActive) return;

        // Avanzar al siguiente texto si hay m�s l�neas de di�logo
        if (currentLineIndex < dialogLines.Count - 1)
        {
            currentLineIndex++;
        }
        else
        {
            EndDialog();
        }
        dialogText.text = dialogLines[currentLineIndex];
    }

    void EndDialog()
    {
        // Si se alcanza el final del di�logo, ocultar el texto o reiniciar el �ndice
        ReiniciarTodosLosVehiculos();
        canvasDialog.SetActive(false);
        dialogActive = false;
        currentLineIndex = 0; // Reiniciar �ndice solo despu�s de desactivar el di�logo
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.enabled = true;
        player.JugadorDesbloqueado();
            
    }

    void ReiniciarTodosLosVehiculos()
    {
        foreach (var v in vehiculos)
        {
            v.GetComponent<IVehicleController>().ReiniciarVehiculo();
        }
    }

}
