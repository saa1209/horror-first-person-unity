using UnityEngine;

namespace HorrorGame
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class InteractionController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private KeyCode interactKey = KeyCode.E;

        private void Update()
        {
            if (playerCamera == null)
                return;

            if (!Input.GetKeyDown(interactKey))
                return;

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                interactable?.Interact(gameObject);
            }
        }
    }

    public interface IInteractable
    {
        void Interact(GameObject activator);
    }

    public sealed class DoorInteraction : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform door;
        [SerializeField] private float openAngle = 95f;
        [SerializeField] private float speed = 3f;

        private Quaternion closedRotation;
        private Quaternion targetRotation;
        private bool open;

        private void Awake()
        {
            closedRotation = door != null ? door.localRotation : transform.localRotation;
            targetRotation = closedRotation;
        }

        private void Update()
        {
            Transform target = door != null ? door : transform;
            target.localRotation = Quaternion.Slerp(target.localRotation, targetRotation, speed * Time.deltaTime);
        }

        public void Interact(GameObject activator)
        {
            open = !open;
            targetRotation = closedRotation * Quaternion.Euler(0f, open ? openAngle : 0f, 0f);
        }
    }
}
