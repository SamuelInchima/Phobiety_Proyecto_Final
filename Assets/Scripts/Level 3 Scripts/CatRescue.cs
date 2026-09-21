using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatRescue : MonoBehaviour
{
    [Header("Configuración de Nivel")]
    // Escribe aquí el nombre exacto de la escena de la oficina del jefe
    public string bossOfficeSceneName = "Level_BossOffice";

    // Esta función se activa cuando otro objeto entra en el "Trigger" de la esfera
    void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que chocó fue el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Gato rescatado! Cambiando a la oficina del jefe...");
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        // Aquí la Persona 3 podría añadir el fundido a negro (cambio cinemático)
        // Por ahora, simplemente cargamos la siguiente escena:
        SceneManager.LoadScene(bossOfficeSceneName);
    }
}