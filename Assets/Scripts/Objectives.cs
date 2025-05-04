using UnityEngine.UI;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    public float holdDuration = 5f;
    private float holdTime = 0f;
    private bool isHolding = false;
    private bool isCompleted = false;
    private bool playerInRange = false; // Tracks if the player is close

    public Slider progressBar; // Assign in Inspector
    public GameObject promptUI; // Assign a "Press E" UI element
    public GameObject handle;
    public AudioClip soundClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // Makes the sound 3D
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
            progressBar.maxValue = holdDuration;
            progressBar.value = 0;
        }

        if (promptUI != null)
        {
            promptUI.SetActive(false); // Hide prompt initially
        }
    }

    void Update()
    {
        if (isCompleted || !playerInRange) return; // Only work if player is near

        if (Input.GetKey(KeyCode.E))
        {
            if (!isHolding)
            {
                isHolding = true;
                if (progressBar != null) progressBar.gameObject.SetActive(true);
            }

            holdTime += Time.deltaTime;
            if (progressBar != null) progressBar.value = holdTime;

            if (holdTime >= holdDuration)
            {
                CompleteObjective();
            }
        }
        else
        {
            if (isHolding)
            {
                isHolding = false;
                holdTime = 0f;
                if (progressBar != null)
                {
                    progressBar.value = 0;
                    progressBar.gameObject.SetActive(false);
                }
            }
        }
    }

    public void CompleteObjective()
    {
        if (isCompleted) return;

        isCompleted = true;
        ObjectiveManager.Instance.CompleteObjective(); // Update global tracker

        // Play animation
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("PlayCompleteAnimation"); // Replace with your animation's actual name
            audioSource.PlayOneShot(soundClip);
        }
        else
        {
            Debug.LogWarning("No Animator found on " + gameObject.name);
        }

        // Move the object to a new position (example position, customize as needed)
        handle.transform.position = new Vector3(0.742f, 3.862f, -0.5541f); // Set to whatever position you want
        handle.transform.rotation = Quaternion.Euler(new Vector3(-90, 90, 90));
        // Hide UI elements
        if (progressBar != null) progressBar.gameObject.SetActive(false);
        if (promptUI != null) promptUI.SetActive(false);
    }

    // Detect when the player enters the interaction range
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    // Detect when the player leaves the interaction range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
            holdTime = 0f; // Reset hold time if player leaves early
            if (progressBar != null) progressBar.value = 0;
        }
    }
}