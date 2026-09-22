using System.Collections;  // Necesario para usar las Corrutinas (tiempos de espera)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatRescue : MonoBehaviour
{
    [Header("Configuración de Nivel")]
    // Escribe aquí el nombre exacto de la escena de la oficina del jefe
    public string bossOfficeSceneName = "Level_BossOffice";

    [Header("Animación de Transición")]
    public Animator fadeAnimator; // Referencia al Animator de la pantalla negra

    private bool isRescuing = false; // Evita que se active varias veces si chocas rápido

    // Esta función se activa cuando otro objeto entra en el "Trigger" de la esfera
    void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que chocó fue el jugador
        if (other.CompareTag("Player") && !isRescuing)
        {
            isRescuing = true;
            Debug.Log("¡Gato rescatado! Iniciando fundido a negro... Cambiando a la oficina del jefe...");
            // Iniciamos la corrutina que maneja el tiempo del fundido
            StartCoroutine(LoadNextLevelWithFade());
        }
    }


    // Aquí la Persona 3 podría añadir el fundido a negro (cambio cinemático)
    // Por ahora, simplemente cargamos la siguiente escena:
    IEnumerator LoadNextLevelWithFade()    
    {
        // 1. Reproducir la animación de fundido a negro
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("IniciarFundido");                           
        }
        else
        {
            Debug.LogWarning("No asignaste el Animator del fundido en el gato.");
        }

        // 2. Esperar 1 segundo (el tiempo que dura la animación de la pantalla poniéndose negra)
        yield return new WaitForSeconds(1f);

        // 3. Cargar la nueva escena
        SceneManager.LoadScene(bossOfficeSceneName);
    }
}