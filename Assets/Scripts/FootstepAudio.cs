using UnityEngine;

namespace HorrorGame
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class FootstepAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip[] stepSounds;
        [SerializeField] private float walkInterval = 0.55f;
        [SerializeField] private float sprintInterval = 0.38f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.25f;
        [SerializeField] private LayerMask groundMask = ~0;

        private Rigidbody body;
        private AudioSource audioSource;
        private float stepTimer;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void Update()
        {
            if (stepSounds == null || stepSounds.Length == 0)
                return;

            bool grounded = groundCheck == null
                ? Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask)
                : Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            float planarSpeed = new Vector2(body.linearVelocity.x, body.linearVelocity.z).magnitude;
            if (!grounded || planarSpeed < 0.2f)
            {
                stepTimer = 0f;
                return;
            }

            float interval = Input.GetKey(KeyCode.LeftShift) ? sprintInterval : walkInterval;
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                AudioClip clip = stepSounds[Random.Range(0, stepSounds.Length)];
                audioSource.PlayOneShot(clip);
                stepTimer = interval;
            }
        }
    }
}
