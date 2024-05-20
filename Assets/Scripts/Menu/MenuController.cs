using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button loadButton;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadGameScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.allowSceneActivation = false; // Para controlar cuándo se activa la escena

        // Mientras la escena se está cargando
        while (!asyncLoad.isDone)
        {
            // Comprobar el progreso de la carga
            Debug.Log("Cargando escena: " + asyncLoad.progress * 100 + "%");

            if (asyncLoad.progress >= 0.90f)
            {
                // Cuando está casi completo
                asyncLoad.allowSceneActivation = true; // Activa la escena
            }

            yield return null; // Esperar hasta el siguiente frame
        }
    }
    public void DesactivarBoton()
    {
        loadButton.interactable = false;
        text1.text = "Cargando...";
        text2.text = "Cargando...";
    }
}
