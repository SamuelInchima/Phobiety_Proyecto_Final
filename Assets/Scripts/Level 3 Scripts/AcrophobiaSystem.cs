using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Obligatorio para manipular Sliders y elementos de UI

public class AcrophobiaSystem : MonoBehaviour
{
    [Header("Configuración de Miedo")]
    public float maxTime = 30f; // Tiempo en segundos antes de que el jugador se desmaye
    private float currentTime;
    private bool isGameOver = false;

    [Header("Interfaz Visual")]
    public Slider fearBar; // Referencia a la barra creada en el Canvas
    void Start()
    {
        // Al iniciar el nivel, el cronómetro invisible empieza desde 0
        currentTime = 0f;

        // Sincronizamos el valor máximo de la barra visual con el tiempo límite del nivel
        if (fearBar != null)
        {
            fearBar.maxValue = maxTime;
            fearBar.value = 0f;
        }
    }

    void Update()
    {
        if (isGameOver) return; // Si ya perdió, detenemos el cronómetro

        // El tiempo de miedo va aumentando cada segundo
        currentTime += Time.deltaTime;

        // Aquí podrías conectar 'currentTime' a la UI de la barra "Phobiety" en el futuro
        // Llenamos la barra de miedo en tiempo real basándonos en el cronómetro
        if (fearBar != null)
        {
            fearBar.value = currentTime;
        }
        // Condición de derrota: la barra de miedo se llena (el tiempo se agota)
        if (currentTime >= maxTime)
        {
            PlayerFaints();
        }
    }

    void PlayerFaints()
    {
        isGameOver = true;
        Debug.Log("¡El jugador se desmayó por acrofobia! Game Over.");
        // Reinicia el nivel actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}