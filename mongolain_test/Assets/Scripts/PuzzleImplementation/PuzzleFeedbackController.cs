using System.Collections;
using UnityEngine;
    //review this chatgpt made it

    /// <summary>
    /// Controls visual and audio feedback for puzzle interactions in the game.
    /// Provides methods to display success, failure, and hint feedback.
    /// </summary>
    public class PuzzleFeedbackController : MonoBehaviour
    {
        [Header("Visual Feedback")]
        [SerializeField] private GameObject successFX;
        [SerializeField] private GameObject failureFX;
        [SerializeField] private GameObject hintFX;
        
        [Header("Audio Feedback")]
        [SerializeField] private AudioClip successSound;
        [SerializeField] private AudioClip failureSound;
        [SerializeField] private AudioClip hintSound;
        
        [Header("Settings")]
        [SerializeField] private float feedbackDuration = 2f;
        
        private AudioSource audioSource;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Initialize FX objects
            if (successFX) successFX.SetActive(false);
            if (failureFX) failureFX.SetActive(false);
            if (hintFX) hintFX.SetActive(false);
        }
        
        /// <summary>
        /// Display success feedback when a puzzle is solved correctly
        /// </summary>
        /// <param name="position">World position to show the feedback</param>
        public void ShowSuccessFeedback(Vector3 position = default)
        {
            StopAllCoroutines();
            StartCoroutine(ShowFeedbackRoutine(successFX, successSound, position));
        }
        
        /// <summary>
        /// Display failure feedback when a puzzle attempt is incorrect
        /// </summary>
        /// <param name="position">World position to show the feedback</param>
        public void ShowFailureFeedback(Vector3 position = default)
        {
            StopAllCoroutines();
            StartCoroutine(ShowFeedbackRoutine(failureFX, failureSound, position));
        }
        
        /// <summary>
        /// Display hint feedback to guide the player
        /// </summary>
        /// <param name="position">World position to show the feedback</param>
        public void ShowHintFeedback(Vector3 position = default)
        {
            StopAllCoroutines();
            StartCoroutine(ShowFeedbackRoutine(hintFX, hintSound, position));
        }
        
        private IEnumerator ShowFeedbackRoutine(GameObject visualFX, AudioClip sound, Vector3 position)
        {
            if (visualFX != null)
            {
                visualFX.transform.position = position;
                visualFX.SetActive(true);
            }
            
            if (sound != null && audioSource != null)
            {
                audioSource.PlayOneShot(sound);
            }
            
            yield return new WaitForSeconds(feedbackDuration);
            
            if (visualFX != null)
            {
                visualFX.SetActive(false);
            }
        }
        
        /// <summary>
        /// Displays a temporary text message near a puzzle
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="position">World position to show the message</param>
        /// <param name="duration">How long to show the message</param>
        public void ShowTextFeedback(string message, Vector3 position, float duration = 2f)
        {
            // This could be implemented with a UI element, floating text prefab, etc.
            Debug.Log($"Puzzle Feedback: {message}");
            
            // Implementation would depend on your UI system
            // Example: Instantiate a text prefab at the position and destroy after duration
        }
    }
