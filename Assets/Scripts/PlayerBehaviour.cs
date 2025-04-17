using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform cameraPlayer;
    [SerializeField] private InputManagerSO inputManager;

    [Header("Floor Detection")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask layerToDetect;

    private CharacterController controller;
    private Animator animator;
    private Vector3 directionCamera;
    private Vector3 directionInput;
    private Vector3 verticalVelocity;

    private void OnEnable()
    {
        inputManager.OnJump += Jump;
        inputManager.OnMove += Move;
    }

    private void Move(Vector2 obj)
    {
        directionInput = new Vector3(obj.x, 0, obj.y);
    }

    private void Jump()
    {
        if (PlayerGrounded())
        {
            verticalVelocity.y = Mathf.Sqrt(-2 * gravityForce * jumpHeight);
            animator.SetTrigger("JumpAction");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("HasGrounded", PlayerGrounded());
        directionCamera = cameraPlayer.forward * directionInput.z + cameraPlayer.right * directionInput.x;
        directionCamera.y = 0;
        controller.Move(directionCamera * moveSpeed * Time.deltaTime);
        animator.SetFloat("Velocity", controller.velocity.magnitude);

        if (directionCamera.sqrMagnitude > 0)
        {
            RotateToDestination();
        }

        if (PlayerGrounded() && verticalVelocity.y <0)
        {
            verticalVelocity.y = 0;
            animator.ResetTrigger("JumpAction"); // Resetea el trigger para que evitar posible lista de triggers
        }
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        verticalVelocity.y += gravityForce * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private bool PlayerGrounded()
    {
        return Physics.CheckSphere(playerTransform.position + new Vector3(0f,0.1f,0f), detectionRadius, layerToDetect);
    }

    private void RotateToDestination()
    {
        Quaternion rotationObjetivo = Quaternion.LookRotation(directionCamera);
        transform.rotation = rotationObjetivo;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(playerTransform.position + new Vector3(0f, 0.1f, 0f), detectionRadius);
    }
}
