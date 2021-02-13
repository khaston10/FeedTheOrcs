using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Audio;

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
    public Image[] stars;
    private int amountOfStarsEarned; // Needs to be 0 - 5
    private int showStarImageCounter = 0;
    #endregion

    #region Audio Sources
    public AudioMixer mixer;
    public AudioSource musicSource;
    public AudioSource clickGood1;
    public AudioSource clickGood2;
    public AudioSource starHammer;
    public AudioClip[] speechStarsClips;
    public bool isMuted;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LoadData();

        // Set texts objects initially.
        SetTextToEndGameScreen();

        creditsPanel.SetActive(false);

        // Check to see if the audio should be muted.
        if (isMuted) MuteSound(true);

        // This coroutine may need to be activated at a later time.
        StartCoroutine(loadStarImages());
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
        isMuted = GlobalCont.Instance.isMuted;

        for (int i = 0; i < GlobalCont.Instance.highScores.Length; i++)
        {
            highScoreTexts[i].text = GlobalCont.Instance.highScores[i].ToString();
        }

    }

    public void SaveData()
    {
        GlobalCont.Instance.isMuted = isMuted;
        GlobalCont.Instance.patientsHealed = 0;
        GlobalCont.Instance.newHighScore = false;
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
        SaveData();

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

    IEnumerator loadStarImages()
    {
        // This function will be called once at the end of the game to load x stars out of 5.
        // Pick the amount of stars. Stars will be dependent on Orc Served and Wealth.
        if (wealth >= 500 && patientsHealed >= 50) amountOfStarsEarned = 5;
        else if (wealth >= 400 && patientsHealed >= 40) amountOfStarsEarned = 4;
        else if (wealth >= 300 && patientsHealed >= 30) amountOfStarsEarned = 3;
        else if (wealth >= 200 && patientsHealed >= 20) amountOfStarsEarned = 2;
        else amountOfStarsEarned = 1;

        // Suspend execution for 5 seconds
        yield return new WaitForSeconds(1);

        //starHammer.Play();
        if (amountOfStarsEarned > 0) StartCoroutine(showStarImage());
    }

    IEnumerator showStarImage()
    {
        starHammer.Play();
        stars[showStarImageCounter].gameObject.SetActive(true);
        showStarImageCounter += 1;
        // Suspend execution for 5 seconds
        yield return new WaitForSeconds(1);
        if (showStarImageCounter < amountOfStarsEarned) StartCoroutine(showStarImage());
        

    }

    public void SetSoundAtStart()
    {
        if (isMuted)
        {
            clickGood1.Play();
            MuteSound(false);
            isMuted = false;
        }
        else
        {
            MuteSound(true);
            isMuted = true;
        }
    }

    public void MuteSound(bool mute)
    {
        if (mute)
        {
            mixer.SetFloat("Music", -80f);
            mixer.SetFloat("SoundFXs", -80f);
        }

        else
        {
            mixer.SetFloat("Music", 0f);
            mixer.SetFloat("SoundFXs", 0f);
        }
    }

}
