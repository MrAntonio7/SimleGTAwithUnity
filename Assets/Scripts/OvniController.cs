using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OvniController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public GameObject player;
    public GameObject efect;

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void Update()
    {
        // Hacer que el canvas siempre mire al jugador
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // Mantener el canvas en el mismo nivel
        healthSlider.transform.parent.rotation = Quaternion.LookRotation(direction);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
            GameManager.Instance.AddScore(1);
        }
    }

    private void Die()
    {
        //// Lógica para cuando el objeto muere
        //Debug.Log("Objeto destruido");
        //Destroy(gameObject);
        GetComponent<RotateObject>().OcultarObjecto();
        // Instanciar el objeto en la posición y rotación especificadas


        GameObject newObject = Instantiate(efect, transform);

        // Establecer la posición y rotación locales del objeto instanciado
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localRotation = Quaternion.identity;
    }
}
