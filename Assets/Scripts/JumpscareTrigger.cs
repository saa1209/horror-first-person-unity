using UnityEngine;

namespace HorrorGame
{
    public sealed class JumpscareTrigger : MonoBehaviour
    {
        [SerializeField] private bool oneShot = true;
        [SerializeField] private GameObject monster;
        [SerializeField] private AudioClip scream;
        [SerializeField] private float scareDuration = 1.2f;
        [SerializeField] private Light cameraFlash;

        private bool triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (triggered || !other.CompareTag("Player"))
                return;

            triggered = true;
            StartCoroutine(ScareRoutine());
        }

        private System.Collections.IEnumerator ScareRoutine()
        {
            if (monster != null)
                monster.SetActive(true);

            if (scream != null)
                AudioSource.PlayClipAtPoint(scream, transform.position);

            if (cameraFlash != null)
            {
                cameraFlash.enabled = true;
                yield return new WaitForSeconds(0.08f);
                cameraFlash.enabled = false;
            }

            yield return new WaitForSeconds(scareDuration);

            if (monster != null)
                monster.SetActive(false);

            if (!oneShot)
                triggered = false;
        }

        [ContextMenu("Test Jumpscare")]
        public void TestJumpscare()
        {
            triggered = false;
            OnTriggerEnter(GetTestCollider());
        }

        private Collider GetTestCollider()
        {
            return GetComponent<Collider>();
        }
    }
}
