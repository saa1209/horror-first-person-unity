using System;
using UnityEngine;

namespace HorrorGame
{
    [Serializable]
    public sealed class StoryStep
    {
        [TextArea] public string message;
        public float duration = 4f;
        public AudioClip sound;
    }

    public sealed class HorrorGameManager : MonoBehaviour
    {
        [SerializeField] private StoryStep[] story;
        [SerializeField] private bool startStoryOnAwake = true;

        public event Action<string, float> OnSubtitle;

        private int currentStep;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void Start()
        {
            if (startStoryOnAwake)
                PlayStory();
        }

        [ContextMenu("Play Story")]
        public void PlayStory()
        {
            StopAllCoroutines();
            currentStep = 0;
            StartCoroutine(StoryRoutine());
        }

        private System.Collections.IEnumerator StoryRoutine()
        {
            while (currentStep < story.Length)
            {
                StoryStep step = story[currentStep++];

                if (step.sound != null)
                    audioSource.PlayOneShot(step.sound);

                OnSubtitle?.Invoke(step.message, step.duration);
                yield return new WaitForSeconds(step.duration);
            }
        }

        public void SetSubtitleSource(Action<string, float> source)
        {
            OnSubtitle = source;
        }
    }
}
