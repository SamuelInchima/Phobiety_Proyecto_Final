using System.Collections;
using System.Collections.Generic;
using TMPro; // Importamos la librería de TextMeshPro para poder encender y apagar las letras Q y E en pantalla.
using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar si cae

public class BalanceSystem : MonoBehaviour
{
    // [Header] crea un título en el Inspector de Unity para mantener el código organizado.
    [Header("Configuración del QTE - Desbalanceo")]

    // 'public' permite modificar este valor desde Unity sin abrir el código.
    // Define cuántas veces en total el jugador debe presionar las teclas para salvarse.
    public int pressesNeeded = 10;

    // NUEVO: Tiempo máximo en segundos que el jugador tiene para presionar las 10 teclas.
    public float timeLimit = 4f;

    // Variable interna que llevará la cuenta regresiva del tiempo.
    private float currentTime;

    // 'private' oculta la variable en Unity. El jugador empieza con 0 pulsaciones.
    private int currentPresses = 0;

    // Interruptor principal. Si es 'true', el juego sabe que el jugador está perdiendo el equilibrio.
    public bool isBalancing = false;

    // Alternador lógico. Si es 'true', el sistema espera que presiones Q. Si es 'false', espera la E.
    private bool needQ = true;

    [Header("Referencias de Interfaz y Scripts")]

    // Espacios para arrastrar los textos "UI_TeclaQ" y "UI_TeclaE" desde el Canvas al Inspector.
    public GameObject qteUI_Q;
    public GameObject qteUI_E;

    // Variables para guardar y controlar otros scripts instalados en el jugador.
    private PlayerMovement playerMovement;
    private CameraShake cameraShake;

    // Start() se ejecuta una sola vez en el instante en que el nivel de Unity comienza.
    void Start()
    {
        // GetComponent busca en este mismo objeto (el Jugador) el script de movimiento y lo guarda.
        // Lo necesitamos para poder congelar la velocidad más adelante.
        playerMovement = GetComponent<PlayerMovement>();

        // GetComponentInChildren busca el script de temblor en los objetos "hijos".
        // Como la cámara está dentro del jugador, la encuentra automáticamente.
        cameraShake = GetComponentInChildren<CameraShake>();

        // Medida de seguridad: Nos aseguramos de que las letras Q y E empiecen invisibles.
        // Comprobamos "!= null" (si no está vacío) para evitar que el juego colapse si olvidan asignar el texto en Unity.
        if (qteUI_Q != null) qteUI_Q.SetActive(false);
        if (qteUI_E != null) qteUI_E.SetActive(false);
    }

    // Update() se ejecuta constantemente, docenas de veces por segundo (cada frame).
    void Update()
    {
        // Si el jugador NO está en la viga perdiendo el equilibrio, la función se detiene aquí con 'return'.
        // Esto evita que el código procese pulsaciones de Q y E mientras camina normal.
        if (!isBalancing) return;

        // --- LÓGICA DEL TIEMPO (NUEVO) ---

        // Restamos el tiempo transcurrido (Time.deltaTime) a nuestro cronómetro actual.
        currentTime -= Time.deltaTime;

        // Si el tiempo llega a 0 o menos antes de completar las teclas, el jugador cae.
        if (currentTime <= 0)
        {
            PlayerFalls();
            return; // Detenemos la función aquí para que no siga leyendo teclas.
        }

        // --- LÓGICA DE PULSACIÓN ALTERNA ---

        // Si el sistema exige la Q (needQ == true) Y el jugador acaba de presionar la tecla Q...
        if (needQ && Input.GetKeyDown(KeyCode.Q))
        {
            currentPresses++; // Sumamos 1 al contador de pulsos exitosos.
            needQ = false;    // Cambiamos el alternador a falso para que la próxima tecla exigida sea la E.
        }
        // De lo contrario, si el sistema exige la E (needQ == false) Y el jugador presiona la E...
        else if (!needQ && Input.GetKeyDown(KeyCode.E))
        {
            currentPresses++; // Sumamos 1 al contador.
            needQ = true;     // Cambiamos el alternador a verdadero para volver a exigir la Q.
        }

        // --- CONDICIÓN DE VICTORIA ---

        // Si las pulsaciones actuales alcanzan o superan las necesarias (ej. 10)...
        if (currentPresses >= pressesNeeded)
        {
            // Llamamos a la función que termina el minijuego y libera al jugador.
            CompleteQTE();
        }
    }

    // Esta función es 'public' para que el script del travesaño invisible (BeamTrigger) pueda activarla al chocar.
    public void StartBalanceMiniGame()
    {
        isBalancing = true;  // Encendemos el interruptor del minijuego.
        currentPresses = 0;  // Reiniciamos el contador por si es la segunda vez que pasa por un tubo.
        needQ = true;        // Siempre empezamos pidiendo la tecla Q.

        // REINICIAR EL CRONÓMETRO (NUEVO)
        // Igualamos el tiempo actual al límite establecido en el Inspector (ej. 4 segundos).
        currentTime = timeLimit;

        // CONGELAR AL JUGADOR
        // Accedemos al script de movimiento y ponemos la velocidad a 0 para que no pueda avanzar.
        if (playerMovement != null) playerMovement.speed = 0f;

        // INICIAR TEMBLOR
        // Llamamos a la función del script de la cámara para generar el efecto de desequilibrio.
        if (cameraShake != null) cameraShake.StartShake();

        // MOSTRAR INTERFAZ
        // Encendemos las letras animadas en la pantalla.
        if (qteUI_Q != null) qteUI_Q.SetActive(true);
        if (qteUI_E != null) qteUI_E.SetActive(true);
    }

    // Función que se activa automáticamente desde el Update() al alcanzar las pulsaciones necesarias.
    void CompleteQTE()
    {
        isBalancing = false; // Apagamos el interruptor para que Update() deje de leer las teclas.

        // DESCONGELAR AL JUGADOR
        // Le devolvemos al jugador su velocidad original (5f) para que continúe el parkour.
        if (playerMovement != null) playerMovement.speed = 5f;

        // DETENER TEMBLOR
        // Llamamos a la función que detiene la vibración de la cámara.
        if (cameraShake != null) cameraShake.StopShake();

        // OCULTAR INTERFAZ
        // Apagamos las letras Q y E de la pantalla.
        if (qteUI_Q != null) qteUI_Q.SetActive(false);
        if (qteUI_E != null) qteUI_E.SetActive(false);
    }

    // --- CONDICIÓN DE DERROTA (NUEVO) ---
    void PlayerFalls()
    {
        isBalancing = false;

        // Apagamos los efectos e interfaces antes de reiniciar
        if (cameraShake != null) cameraShake.StopShake();
        if (qteUI_Q != null) qteUI_Q.SetActive(false);
        if (qteUI_E != null) qteUI_E.SetActive(false);

        Debug.Log("¡El tiempo se acabó! El jugador perdió el equilibrio y cayó.");

        // Reiniciamos la escena actual, cumpliendo la regla de "Game Over"
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}