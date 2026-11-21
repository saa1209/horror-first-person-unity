using UnityEngine;

namespace HorrorGame
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class FirstPersonController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float sprintSpeed = 6f;
        [SerializeField] private float crouchSpeed = 1.8f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -18f;

        [Header("Look")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField, Range(-90f, 0f)] private float minPitch = -88f;
        [SerializeField, Range(0f, 90f)] private float maxPitch = 88f;

        [Header("Crouch")]
        [SerializeField] private float standingHeight = 1.8f;
        [SerializeField] private float crouchingHeight = 1f;

        private CharacterController controller;
        private float pitch;
        private float verticalVelocity;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (cameraTransform == null && Camera.main != null)
                cameraTransform = Camera.main.transform;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleLook();
            HandleMovement();
        }

        private void HandleLook()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            pitch = Mathf.Clamp(pitch - mouseY, minPitch, maxPitch);
            cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void HandleMovement()
        {
            bool crouching = Input.GetKey(KeyCode.LeftControl);
            float targetHeight = crouching ? crouchingHeight : standingHeight;
            controller.height = Mathf.MoveTowards(controller.height, targetHeight, 6f * Time.deltaTime);

            float speed = crouching
                ? crouchSpeed
                : Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            Vector3 horizontal = transform.right * Input.GetAxisRaw("Horizontal")
                               + transform.forward * Input.GetAxisRaw("Vertical");
            horizontal = Vector3.ClampMagnitude(horizontal, 1f) * speed;

            if (controller.isGrounded)
            {
                verticalVelocity = -2f;
                if (Input.GetButtonDown("Jump") && !crouching)
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            Vector3 velocity = horizontal + Vector3.up * verticalVelocity;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
