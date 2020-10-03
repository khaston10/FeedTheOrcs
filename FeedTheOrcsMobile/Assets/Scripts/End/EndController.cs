using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    #region Variables - Stats
    public string nameOfDoctor;
    public string statusOfDoctor;
    public int day;
    public int patientsHealed;
    public int patientsDeceased;
    public int gameDifficulty;
    #endregion

    #region Text objects to update
    public Text titeText;
    public Text patientsSavedText;
    public GameObject creditsPanel;
    public Text[] highScoreTexts;
    public Text newHighScoreText;
    #endregion

    #region Audio Sources
    public AudioSource musicSource;
    public AudioSource clickGood1;
    public AudioSource clickGood2;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LoadData();

        // Set texts objects initially.
        SetTextToEndGameScreen();

        creditsPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData()
    {
        nameOfDoctor = GlobalCont.Instance.nameOfDoctor;
        statusOfDoctor = GlobalCont.Instance.statusOfDoctor;
        day = GlobalCont.Instance.day;
        patientsHealed = GlobalCont.Instance.patientsHealed;
        patientsDeceased = GlobalCont.Instance.patientsDeceased;
        gameDifficulty = GlobalCont.Instance.gameDifficulty;

        for (int i = 0; i < GlobalCont.Instance.highScores.Length; i++)
        {
            highScoreTexts[i].text = GlobalCont.Instance.highScores[i].ToString();
        }

        // If the player has achieved a high score we will make the "New High Score" object active.
        if (GlobalCont.Instance.newHighScore) newHighScoreText.text = "New High Score";
        else newHighScoreText.text = "";
}

    public void SetTextToEndGameScreen()
    {
        if (statusOfDoctor == "EXHAUSTED") titeText.text = nameOfDoctor + " is EXHAUSTED";
        else titeText.text = "Too Many Customers Waiting!";
        patientsSavedText.text = patientsHealed.ToString();
    }

    public void ClickCredits()
    {
        creditsPanel.SetActive(true);

        // Play Click Sound
        clickGood1.Play();
    }

    public void ClickMainMenu()
    {
        GlobalCont.Instance.patientsHealed = 0;
        GlobalCont.Instance.newHighScore = false;

        // Play Click Sound
        clickGood1.Play();

        SceneManager.LoadScene(0);
    }

    public void ExitCreditsPanel()
    {
        creditsPanel.SetActive(false);
        clickGood2.Play();
    }
}
