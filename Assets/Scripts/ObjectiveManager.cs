using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance; // Singleton for easy access
    public int totalObjectives = 20; // Set this based on level requirements
    private int completedObjectives = 0;

    public Text objectiveText; // Assign a UI Text in Inspector

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CompleteObjective()
    {
        completedObjectives++;
        UpdateUI();

        if (completedObjectives >= totalObjectives)
        {
            LevelComplete();
        }
    }

    void UpdateUI()
    {
        if (objectiveText != null)
        {
            objectiveText.text = $"{completedObjectives}/{totalObjectives}";
        }
    }

    void LevelComplete()
    {
        Debug.Log("Level Completed!");
        // Add logic to transition to the next level or display a win screen
    }
}
