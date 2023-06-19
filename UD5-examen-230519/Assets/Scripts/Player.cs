using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private float playerSpeed = 3.5f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private PlayerState state;
    private Animator animator;
    private Transform cameraTransform;

    // referencia al objeto que tiene el player sobre la cabeza que indica cuando agarro algo
    public GameObject LightHouse;
    // script de la puerta de entrada a las escaleras
    public DoorMovement inputDoubleDoor; //Asociar el script correspondiente en la jerarquia
    // variable donde se guardara la thing agarrada del player
    private GameObject thing;
    // control de si tiene algo agarrado
    private bool iHaveThing = false; //Por defecto falso ya que no tenemos nada

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetState(PlayerState.Idle);
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            //playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Transformamos el movimiento en la direccion de la cámara
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0;
        move = move.normalized;


        Vector3 displacement = move * Time.deltaTime * playerSpeed;
        controller.Move(displacement);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            //playerVelocity.y = 0f;
            Debug.Log("Salta Amy, salta");
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            SetState(PlayerState.Jump);
        }
        else if (groundedPlayer)
        {
            if (move == Vector3.zero)
            {
                SetState(PlayerState.Idle);
            }
            else
            {
                SetState(PlayerState.Run);
            }
        }

        /*Cuando el usuario pulse el boton Interaction Amy chequeara si tiene delante un objeto de la clase Shelf. 
        De ser asi le pedira una Thing llamando a un metodo especifico para realizar esta tarea, que se deberá incluir en el comportamiento de Shelf.*/

        // se comprueba si el player pulso la tecla interaccion
        if (Input.GetButtonDown("Interaction"))
        {
            // se llama al metodo que controla las interaciones
            CheckForInteraction();
        }


        playerVelocity.y += gravityValue * Time.deltaTime;

        if (playerVelocity.y < 0 && !groundedPlayer)
        {
            SetState(PlayerState.Fall);
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    /*RAYCAST: Se utiliza para realizar un lanzamiento de rayo y determinar qué objetos o superficies intersecta el rayo. 
    Es una técnica utilizada para realizar detección de colisiones basada en rayos o para realizar operaciones basadas en la intersección de rayos,
    como selección de objetos, apuntado, disparos, entre otros.

    void Update()
    {
        // Lanzar un rayo desde la posición de la cámara hacia adelante
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Almacenar la información del impacto del rayo
        RaycastHit hit;

        // Realizar el raycast y comprobar si ha habido un impacto
        if (Physics.Raycast(ray, out hit))
        {
            // Acceder al objeto golpeado y realizar acciones
            GameObject hitObject = hit.collider.gameObject;
            Debug.Log("Objeto golpeado: " + hitObject.name);
        }
    }
    Para que el raycast funcione correctamente, los objetos deben tener colisionadores apropiados y estar en una capa que permita que el rayo los intersecte
    */

    /*La deteccion de que tiene delante un objeto Shelf, la hará Amy lanzando un RayCast desde un punto situado  una altura de 0.4f m, 0.1f m por delante del eje vertical del personaje, en direccion forward y con un alcance de 0.5fm.*/
    private void CheckForInteraction()
    {
        RaycastHit hit;
        // se hace un raycast
        if (Physics.Raycast(transform.position + Vector3.up * 0.4f + transform.forward * 0.1f, transform.forward, out hit, 0.5f))
        {
            // se compureba que tiene delante un objeto con la tag shelf y el player no tenga ninguna thing agarrada
            if (hit.collider.gameObject.CompareTag("Shelf") && !iHaveThing)
            {
                // se obtiene la ultima thing de mas arriba del shelf
                thing = hit.collider.gameObject.GetComponent<Shelf>().GetThing();
                // en caso de que el shelf no tenga nada ninguna thing no se haria nada
                if (thing != null)
                {
                    // se le asigna a la thing que el padre es el player
                    thing.transform.SetParent(transform);
                    // se le asigna su nueva posicion
                    Vector3 position = new Vector3(0f, 1f, 0.3f);
                    thing.transform.localPosition = position;
                    thing.transform.localRotation = Quaternion.identity;//Util cuando deseas restablecer la rotación de un objeto a su estado original o neutral.
                    // se asgina a la lighthouse el mismo material de la thing
                    LightHouse.GetComponent<Renderer>().material = thing.GetComponentInChildren<Renderer>().material;
                    // se activa la lighthouse
                    LightHouse.SetActive(true);
                    // se marca que el player ya tiene una thing
                    iHaveThing = true;
                    // se llama al metodo que abre la puerta de entrada a las escaleras
                    inputDoubleDoor.OpenDoor();
                    Debug.Log("Cosa Seleccionada");
                }
            }
            // se comprueba si tiene delante el teletransportador de materia y que tiene una thing agarrada
            if (hit.collider.gameObject.CompareTag("TeleTransporter") && iHaveThing)
            {
                // se destrulle la thing
                Destroy(thing);
                // se desactiva la lighthouse
                LightHouse.SetActive(false);
                // se marca que el player no tiene ninguna thing agarrada
                iHaveThing = false;
                Debug.Log("Objeto Eliminado");
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // se controla si el player colisiono con el transportador de las escaleras
        if (other.gameObject.CompareTag("TraslateFloorUp"))
        {
            GetComponent<TranslateFloorUp>().Translate();
            Debug.Log("Subiendo");
        }
        // se controla si el player colisiono con el collider para cerrar puertas
        if (other.gameObject.CompareTag("CloserDoor"))
        {
            other.gameObject.GetComponentInParent<DoorMovement>().CloseDoor();
            Debug.Log("Cierro 1º puerta");
        }
        // se controla si el player colisiono con el collider para abrir puertas
        if (other.gameObject.CompareTag("OpenerDoor"))
        {
            other.gameObject.GetComponentInParent<DoorMovement>().OpenDoor();
            Debug.Log("Abro 2º puerta");
        }
    }

    //OnTriggerEnter: es un método que se utiliza en Unity para detectar cuando un objeto colisiona con un trigger. 
    //Un trigger es un colisionador con la propiedad "Is Trigger" habilitada,
    // que permite que otros objetos atraviesen su área sin generar una colisión física, pero aún así se puede detectar cuando otro objeto entra en su volumen

    /* private void OnTriggerEnter(Collider other)
    {
        // Se ejecuta cuando otro objeto entra en el trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡El jugador ha entrado en el trigger!");
        }
    }
    El objeto que contiene el script debe tener un colisionador con la propiedad "Is Trigger" habilitada,
    y el otro objeto debe tener un colisionador físico que colisione con el trigger.*/

    private void SetState(PlayerState newState)
    {
        if (state != newState)
        {
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Jump");
            animator.ResetTrigger("Fall");
            state = newState;
            animator.SetTrigger($"{newState}");
        }
    }


}

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Fall
}
