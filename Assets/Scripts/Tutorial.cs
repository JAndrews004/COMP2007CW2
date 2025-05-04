using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialPanel; // Your whole tutorial UI
    public Image tutorialImage;      // UI Image to display slides
    public Sprite[] tutorialSlides;  // Your array of tutorial images
    public Button nextButton;
    public Button closeButton;

    private int currentSlide = 0;

    public void CheckTutorial()
    {
        if (PlayerPrefs.GetInt("HasSeenTutorial", 0) == 0)
        {
            ShowTutorial();
        }
        else
        {
            tutorialPanel.SetActive(false);
            SceneManager.LoadScene("SampleScene");

        }
    }

    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        currentSlide = 0;
        tutorialImage.sprite = tutorialSlides[currentSlide];
        nextButton.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    public void NextSlide()
    {
        currentSlide++;
        if (currentSlide < tutorialSlides.Length)
        {
            tutorialImage.sprite = tutorialSlides[currentSlide];
        }
        else
        {
            EndTutorial();
        }

        if (currentSlide == tutorialSlides.Length - 1)
        {
            nextButton.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(true);
        }
    }

    public void EndTutorial()
    {
        PlayerPrefs.SetInt("HasSeenTutorial", 1);
        PlayerPrefs.Save();
        tutorialPanel.SetActive(false);
    }
}
