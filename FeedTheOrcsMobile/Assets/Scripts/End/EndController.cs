using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class EndController : MonoBehaviour
{
    #region Variables - Stats
    public string nameOfDoctor;
    public string statusOfDoctor;
    public int day;
    public int patientsHealed;
    public int patientsDeceased;
    public int gameDifficulty;
    public int wealth;
    #endregion

    #region Text objects to update
    public Text rankText;
    public Text patientsSavedText;
    public Text wealthText;
    public GameObject creditsPanel;
    public Text[] highScoreTexts;
    public Slider rankSlider;
    public Slider wealthSlider;
    private string wealthStatus;
    private float sliderTimer = 0;
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
        // Update slider.
        if (sliderTimer < 1)
        {
            rankSlider.value = CalculateSliderValue();
            wealthSlider.value = CalculateWealthSlider();
        }
        
        sliderTimer += Time.deltaTime;
        
    }

    public void LoadData()
    {
        nameOfDoctor = GlobalCont.Instance.nameOfDoctor;
        statusOfDoctor = GlobalCont.Instance.statusOfDoctor;
        day = GlobalCont.Instance.day;
        patientsHealed = GlobalCont.Instance.patientsHealed;
        patientsDeceased = GlobalCont.Instance.patientsDeceased;
        gameDifficulty = GlobalCont.Instance.gameDifficulty;
        wealth = GlobalCont.Instance.wealth;

        for (int i = 0; i < GlobalCont.Instance.highScores.Length; i++)
        {
            highScoreTexts[i].text = GlobalCont.Instance.highScores[i].ToString();
        }

    }

    public void SetTextToEndGameScreen()
    {
        if (wealth <= 25) wealthStatus = "IMPOVERISHED";
        else if (wealth <= 50) wealthStatus = "FINANCIALLY STABLE";
        else if (wealth <= 75) wealthStatus = "WEALTHY";
        else wealthStatus = "FILTHY RICH";

        if (patientsHealed <= 10) rankText.text = "RANK: " + wealthStatus +" GRUNT DISHWASHER";
        else if (patientsHealed <= 20) rankText.text = "RANK: " + wealthStatus + "  DISHWASHER";
        else if (patientsHealed <= 30) rankText.text = "RANK: " + wealthStatus + " MASTER DISHWASHER";
        else if (patientsHealed <= 40) rankText.text = "RANK: " + wealthStatus + " GRUNT COOK";
        else if (patientsHealed <= 50) rankText.text = "RANK: " + wealthStatus + " COOK";
        else if (patientsHealed <= 60) rankText.text = "RANK: " + wealthStatus + " MASTER COOK";
        else if (patientsHealed <= 70) rankText.text = "RANK: " + wealthStatus + " GRUNT CHEF";
        else if (patientsHealed <= 80) rankText.text = "RANK: " + wealthStatus + " CHEF";
        else if (patientsHealed <= 90) rankText.text = "RANK: " + wealthStatus + " MASTER CHEF";
        else if (patientsHealed <= 100) rankText.text = "RANK: " + wealthStatus + " DEMIGOD CHEF";

        patientsSavedText.text = patientsHealed.ToString();
        wealthText.text = wealth.ToString();
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

    float CalculateSliderValue()
    {
        return ((patientsHealed * sliderTimer) / 100);
    }

    float CalculateWealthSlider()
    {
        return (wealth * sliderTimer / 100);
    }

}
