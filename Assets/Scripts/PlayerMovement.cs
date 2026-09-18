using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // VARIABLES DEL MOVIMIENTO
    [Header("Configuracion de Movimiento")]
    // Velocidad a la que caminara el personaje. 'public' permite cambiarla desde el Inspector de Unity.
    public float speed = 5f;

    // VARIABLES DE LA CAMARA
    [Header("Configuracion de Camara")]
    // Sensibilidad del raton al mirar alrededor.
    public float mouseSensitivity = 100f;
    // Referencia a la camara que esta dentro del jugador. Tenemos que arrastrarla aqui desde el Inspector.
    public Transform playerCamera;
    // Variable para guardar la rotacion arriba/abajo y limitarla.
    private float xRotation = 0f;

    // VARIABLE DEL CONTROLADOR
    // Referencia al componente que agregamos al jugador para que no atraviese paredes.
    private CharacterController controller;

    // GRAVEDAD
    [Header("Configuracion de Gravedad")]
    // Fuerza con la que caemos al suelo.
    public float gravity = -9.81f;
    // Variable para guardar la velocidad de caida actual.
    public float jumpHeight = 3f; // Altura del salto
    private Vector3 velocity;
    // Referencia a un objeto vacio en los pies del jugador para saber si toca el suelo.
    public Transform groundCheck;
    // Distancia del radio invisible en los pies para detectar el suelo.
    public float groundDistance = 0.4f;
    // Capa de los objetos que consideramos "suelo".
    public LayerMask groundMask;
    // Variable para saber si estamos tocando el suelo o no.
    private bool isGrounded;

    // START: Se ejecuta una sola vez al iniciar el juego.
    void Start()
    {
        // Le decimos al script que busque el componente CharacterController que le pusimos al Player.
        controller = GetComponent<CharacterController>();

        // Bloqueamos el cursor del raton en el centro de la pantalla y lo ocultamos.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // UPDATE: Se ejecuta constantemente, en cada frame del juego.
    void Update()
    {
        // 1. COMPROBAR SI TOCAMOS EL SUELO
        // Crea una esfera invisible en 'groundCheck' y revisa si choca con algo que tenga la capa 'groundMask'.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Si tocamos el suelo y estábamos cayendo, reseteamos la velocidad de caida para no acumularla.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 2. MOVIMIENTO DEL JUGADOR (WASD o Flechas)
        // Lee los ejes de movimiento (A/D para izquierda/derecha, W/S para adelante/atras).
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calcula la direccion en la que nos moveremos basandonos en hacia donde mira el jugador.
        Vector3 move = transform.right * x + transform.forward * z;

        // Le decimos al CharacterController que nos mueva usando la direccion, la velocidad y el tiempo.
        controller.Move(move * speed * Time.deltaTime);

        // 3. MOVIMIENTO DE LA CAMARA (Raton)
        // Lee los movimientos del raton (izquierda/derecha y arriba/abajo).
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Restamos el movimiento arriba/abajo a nuestra variable (si sumamos, los controles se invierten).
        xRotation -= mouseY;
        // Limitamos la vista para no mirar mas de 90 grados hacia arriba o abajo (no dar vueltas la cabeza).
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicamos la rotacion arriba/abajo SOLO a la camara.
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Aplicamos la rotacion izquierda/derecha a TODO el cuerpo del jugador (para que al caminar adelante, vayamos hacia donde miramos).
        transform.Rotate(Vector3.up * mouseX);

        // 4. APLICAR GRAVEDAD
        // Si no estamos en el suelo, caemos constantemente (la gravedad afecta la velocidad Y).
        velocity.y += gravity * Time.deltaTime;
        // Le decimos al CharacterController que nos mueva hacia abajo.
        controller.Move(velocity * Time.deltaTime);

        // 5. SALTO
        // Si el jugador presiona Espacio (Jump) y está tocando el suelo...
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Fórmula física para calcular la fuerza necesaria del salto
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}