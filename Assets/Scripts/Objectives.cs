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

    void Start()
    {
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

    void CompleteObjective()
    {
        if (isCompleted) return;

        isCompleted = true;
        ObjectiveManager.Instance.CompleteObjective(); // Update global tracker
        gameObject.SetActive(false); // Hide the object after completion
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