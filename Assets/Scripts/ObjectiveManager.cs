using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;
    public int totalObjectives = 20; // Set this based on level requirements
    private int completedObjectives = 0;

    public TMP_Text objectiveText; // Assign a UI Text in Inspector
    public GameObject WinUI;

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
        WinUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

    }
}
