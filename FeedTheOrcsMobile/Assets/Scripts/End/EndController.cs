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
        patientsHealed = 0;

        // Play Click Sound
        clickGood1.Play();

        SceneManager.LoadScene(1);
    }

    public void ExitCreditsPanel()
    {
        creditsPanel.SetActive(false);
        clickGood2.Play();
    }
}
