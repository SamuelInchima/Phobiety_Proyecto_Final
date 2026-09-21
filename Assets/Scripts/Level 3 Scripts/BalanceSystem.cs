using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar si cae

public class BalanceSystem : MonoBehaviour
{
    [Header("Configuración de Balance")]
    public float currentBalance = 100f;
    public float balanceDepletionRate = 40f; // Qué tan rápido pierde el balance
    public float recoveryAmount = 20f; // Cuánto recupera al presionar la tecla correcta
    public bool isBalancing = false;

    [Header("Interfaz de Usuario")]
    public TextMeshProUGUI qteText; // Referencia al texto en pantalla
    private bool needQ = true; // Alternador para saber si toca presionar Q o E
    private PlayerMovement playerMovement;

    void Start()
    {
        // Buscamos el script de movimiento del jugador para ralentizarlo durante el balance
        playerMovement = GetComponent<PlayerMovement>();

        // Asegurarnos de que el texto empiece apagado
        if (qteText != null) qteText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Si no está en el minijuego de balance, no hacemos nada
        if (!isBalancing) return;

        // El balance cae constantemente simulando que se va de lado
        currentBalance -= balanceDepletionRate * Time.deltaTime;

        // Detectar teclas alternas (Q y E)
        if (needQ && Input.GetKeyDown(KeyCode.Q))
        {
            RecoverBalance();
            needQ = false; // Ahora el jugador debe presionar E
        }
        else if (!needQ && Input.GetKeyDown(KeyCode.E))
        {
            RecoverBalance();
            needQ = true; // Ahora el jugador debe presionar Q
        }

        // Si presiona la tecla equivocada, penalizamos quitando más balance
        else if ((needQ && Input.GetKeyDown(KeyCode.E)) || (!needQ && Input.GetKeyDown(KeyCode.Q)))
        {
            currentBalance -= 10f;
        }

        // Condición de derrota: el balance llega a 0 y el jugador cae
        if (currentBalance <= 0)
        {
            PlayerFalls();
        }
    }

    void RecoverBalance()
    {
        currentBalance += recoveryAmount;
        if (currentBalance > 100f) currentBalance = 100f; // Límite máximo
    }

    public void StartBalanceMiniGame()
    {
        isBalancing = true;
        currentBalance = 100f;

        // Ralentizamos al jugador para generar tensión mientras intenta cruzar
        if (playerMovement != null) playerMovement.speed = 1.5f;
        Debug.Log("¡Perdiendo el balance! Presiona Q y E.");

        // Encendemos el texto de advertencia
        if (qteText != null) qteText.gameObject.SetActive(true);
    }

    public void StopBalanceMiniGame()
    {
        isBalancing = false;

        // Restauramos la velocidad normal al salir del tubo
        if (playerMovement != null) playerMovement.speed = 5f;

        // Apagamos el texto al salir del tubo
        if (qteText != null) qteText.gameObject.SetActive(false);
    }

    void PlayerFalls()
    {
        isBalancing = false;
        Debug.Log("¡Caíste de la viga! Game Over.");

        if (qteText != null) qteText.gameObject.SetActive(false);
        
        // Reinicia el nivel al caer
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
