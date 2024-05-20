using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BotonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText1;
    public TextMeshProUGUI buttonText2;
    private string originalText;

    void Start()
    {
        // Obtén el componente de texto del botón
        buttonText1 = GetComponentInChildren<TextMeshProUGUI>();
        originalText = buttonText1.text;
        buttonText2 = GetComponentInChildren<TextMeshProUGUI>();
        originalText = buttonText2.text;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Subraya el texto cuando el ratón pasa por encima
        if (buttonText1 != null)
        {
            buttonText1.text = $"<u>{originalText}</u>";
        }
        // Subraya el texto cuando el ratón pasa por encima
        if (buttonText2 != null)
        {
            buttonText2.text = $"<u>{originalText}</u>";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Quita el subrayado cuando el ratón sale
        if (buttonText1 != null)
        {
            buttonText1.text = originalText;
        }
        // Quita el subrayado cuando el ratón sale
        if (buttonText2 != null)
        {
            buttonText2.text = originalText;
        }
    }
}