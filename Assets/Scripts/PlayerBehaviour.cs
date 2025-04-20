using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerType { FirstPerson, ThirdPerson}
public class PlayerBehaviour : MonoBehaviour, Damageable
{
    [SerializeField] private PlayerType playerType;

    [Header("Player Characteristics")]
    [SerializeField] private float health;
    private float maxHealth;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private InputManagerSO inputManager;

    [Header("Third Person Settings")]
    [SerializeField] private Transform cameraThirdPerson;
    [SerializeField] private GameObject cameraThirdContainer;

    [Header("First Person Settings")]
    [SerializeField] private Transform cameraFirstPerson;
    [SerializeField] private GameObject cameraFirstContainer;

    [Header("Floor Detection")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask layerToDetect;

    [Header("Crosshair")]
    [SerializeField] private GameObject crosshair;

    private CharacterController controller;
    private Animator animator;
    private Vector3 directionCamera;
    private Vector3 directionInput;
    private Vector3 verticalVelocity;

    [Header("Flags")]
    public bool IsAiming = false;
    public bool CanShoot = true;
    public bool IsZombieMinigame = false;

    private void OnEnable()
    {
        inputManager.OnJump += Jump;
        inputManager.OnMove += Move;
        inputManager.OnAim += Aim;
        inputManager.OnShoot += Shoot;
    }

    private void Awake()
    {
        maxHealth = health;
    }

    private void Aim(bool x)
    {
        IsAiming = x;
        if (IsZombieMinigame)
        {
            animator.SetBool("IsAiming", x);
        }
        else
        {
            animator.SetBool("IsAiming", false);
        }
        crosshair.SetActive(x && IsZombieMinigame);
        TogglePlayerType(!x || !IsZombieMinigame);
    }

    private void Shoot()
    {
        if (IsAiming && CanShoot && IsZombieMinigame)
        {
            animator.SetTrigger("Shoot");
            CanShoot = false;
            GetComponent<RaycastWeapon>().ShootLaser();
        }
       
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

            if (playerType == PlayerType.ThirdPerson) animator.SetTrigger("JumpAction");
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
        if (playerType == PlayerType.ThirdPerson) ThirdPersonMovement();

        if (playerType == PlayerType.FirstPerson) FirstPersonMovement();
    }

    private void ThirdPersonMovement()
    {
        animator.SetBool("HasGrounded", PlayerGrounded());
        directionCamera = cameraThirdPerson.forward * directionInput.z + cameraThirdPerson.right * directionInput.x;
        directionCamera.y = 0;
        controller.Move(directionCamera * moveSpeed * Time.deltaTime);

        animator.SetFloat("Velocity", controller.velocity.magnitude);

        if (directionCamera.sqrMagnitude > 0)
        {
            RotateToDestination();
        }

        if (PlayerGrounded() && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0;
            animator.ResetTrigger("JumpAction"); // Resetea el trigger para que evitar posible lista de triggers
        }
        ApplyGravity();
    }

    private void CanShootAgain()
    {
        CanShoot = true;
    }

    private void FirstPersonMovement()
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = cameraFirstPerson.eulerAngles.y;
        transform.eulerAngles = euler;
        directionCamera = cameraFirstPerson.forward * directionInput.z + cameraFirstPerson.right * directionInput.x;
        directionCamera.y = 0;
        controller.Move(directionCamera * moveSpeed * Time.deltaTime);

        if (PlayerGrounded() && verticalVelocity.y < 0)
        {
            verticalVelocity.y = 0;
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
        Collider[] colliders = Physics.OverlapSphere(playerTransform.position + new Vector3(0f, 0.1f, 0f), detectionRadius, layerToDetect);

        foreach (Collider collider in colliders)
        {
            if (!collider.isTrigger)
            {
                return true; // Está tocando el suelo (collider no es trigger)
            }
        }

        return false; // No se encontró ningún collider válido
    }

    public void TogglePlayerType(bool changeToThirdPerson)
    {
        if (changeToThirdPerson)
        {
            playerType = PlayerType.ThirdPerson;
            cameraFirstContainer.SetActive(false);
            cameraThirdContainer.SetActive(true);
        }
        else
        {
            playerType = PlayerType.FirstPerson;
            cameraThirdContainer.SetActive(false);
            cameraFirstContainer.SetActive(true);
        }
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

    public void DamageTarget(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            FindAnyObjectByType<MazeSpawner>().CollectedRewards += 1;
            Destroy(other.gameObject);
        }
    }
}
