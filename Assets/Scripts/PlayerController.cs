using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction putEchoAction;
    private InputAction restartLevelAction;
    private float speed = 6f;
    private float jump = 7.5f;
    
    public GameObject sceneManager;
    private SceneManager sceneManagerScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sceneManagerScript = sceneManager.GetComponent<SceneManager>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        putEchoAction = InputSystem.actions.FindAction("PutEcho");
        restartLevelAction = InputSystem.actions.FindAction("RestartLevel");

        jumpAction.performed += Jump_Performed;
        putEchoAction.performed += PutEcho_Performed;
        restartLevelAction.performed += RestartLevel_Performed;
    }

    private void PutEcho_Performed(InputAction.CallbackContext context)
    {
        sceneManagerScript.RespawnPlayer();
    }

    private void RestartLevel_Performed(InputAction.CallbackContext context)
    {
        sceneManagerScript.RestartLevel();
    }

    private void Jump_Performed(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3(input.x * speed, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FinishLevel"))
        {
            sceneManagerScript.WinGame();
        }

        if (collision.gameObject.CompareTag("DeadZone"))
        {
            sceneManagerScript.RespawnPlayer(isDeath: true);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f))
        {
            return true;
        }
        return false;
    }
}
