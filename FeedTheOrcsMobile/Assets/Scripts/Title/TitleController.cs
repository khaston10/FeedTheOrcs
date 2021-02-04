﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    #region Variables - Doctor
    public string nameOfDoctor;
    public int ageOfDoctor;
    public string sexOfDoctor;
    public string statusOfDoctor;
    public Sprite spriteOfDoctor;
    #endregion

    #region Variables - Stats
    public int day;
    public int patientsHealed;
    public int patientsDeceased;
    public int gameDifficulty;
    #endregion

    #region Audio Sources
    public AudioMixer mixer;
    public AudioSource clickGood1;
    public AudioSource clickGood2;
    public bool isMuted;
    #endregion

    #region Variables - Text and Images to Update
    public Dropdown difficultyDropDown;
    public GameObject settingsPanel;
    public Text doctorsNameText;
    public Text doctorsTaxText;
    public Image[] buttonLights;
    public Sprite geenLightImage;
    public Sprite blankImage;
    public Sprite[] spriteOfDoctors;
    public string[] doctorsNames = new string[4] { "D1", "D2", "D3", "D4" };
    public int[] agesOfDoctors = new int[4] { 40, 29, 32, 56 };
    public string[] genderOfDoctors = new string[4] { "M", "M", "M", "M" };
    public string[] taxonomyOfDoctors = new string[4] { "Reading", "Blogging", "Pizza", "Video Gaming" };
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LoadData();

        // Set Doc 0 as selection.
        // Update text on screen.
        doctorsNameText.text = doctorsNames[0];
        doctorsTaxText.text = taxonomyOfDoctors[0];

        // Load variables to pass on to next screen.
        nameOfDoctor = doctorsNames[0];
        ageOfDoctor = agesOfDoctors[0];
        sexOfDoctor = genderOfDoctors[0];
        spriteOfDoctor = spriteOfDoctors[0];


        // Update Select Box.
        TurnLightOnOff(0);

        settingsPanel.SetActive(false);

        // Check to see if the audio should be muted.
        if (isMuted) MuteSound(true);


    }

    // Update is called once per frame
    void Update()
    {
        // Update difficulty.
        gameDifficulty = difficultyDropDown.value;
    }

    #region Functions

    public void LoadData()
    {
        nameOfDoctor = GlobalCont.Instance.nameOfDoctor;
        ageOfDoctor = GlobalCont.Instance.ageOfDoctor;
        sexOfDoctor = GlobalCont.Instance.sexOfDoctor;
        statusOfDoctor = GlobalCont.Instance.statusOfDoctor;
        day = GlobalCont.Instance.day;
        patientsHealed = GlobalCont.Instance.patientsHealed;
        patientsDeceased = GlobalCont.Instance.patientsDeceased;
        gameDifficulty = GlobalCont.Instance.gameDifficulty;
        isMuted = GlobalCont.Instance.isMuted;
    }

    public void SaveData()
    {
        GlobalCont.Instance.nameOfDoctor = nameOfDoctor;
        GlobalCont.Instance.ageOfDoctor = ageOfDoctor;
        GlobalCont.Instance.sexOfDoctor = sexOfDoctor;
        GlobalCont.Instance.statusOfDoctor = statusOfDoctor;
        GlobalCont.Instance.day = day;
        GlobalCont.Instance.patientsHealed = patientsHealed;
        GlobalCont.Instance.patientsDeceased = patientsDeceased;
        GlobalCont.Instance.spriteOfDoctor = spriteOfDoctor;
        GlobalCont.Instance.gameDifficulty = gameDifficulty;
        GlobalCont.Instance.isMuted = isMuted;


    }

    public void ClickStart()
    {
        clickGood1.Play();
        SaveData();
        SceneManager.LoadScene(1);
    }

    public void ClickDoctor(int doc)
    {
        // Update text on screen.
        doctorsNameText.text = doctorsNames[doc];
        doctorsTaxText.text = taxonomyOfDoctors[doc];

        // Play audio.
        clickGood2.Play();

        // Load variables to pass on to next screen.
        nameOfDoctor = doctorsNames[doc];
        ageOfDoctor = agesOfDoctors[doc];
        sexOfDoctor = genderOfDoctors[doc];
        spriteOfDoctor = spriteOfDoctors[doc];


        // Update Select Box.
        TurnLightOnOff(doc);
    }

    public void TurnLightOnOff(int doc)
    {
        for(int i = 0; i < buttonLights.Length; i++)
        {
            buttonLights[i].sprite = blankImage;
        }

        buttonLights[doc].sprite = geenLightImage;
    }

    public void ClickSettings()
    {
        settingsPanel.SetActive(true);
        clickGood1.Play();
    }

    public void ExitSettingsPanel()
    {
        settingsPanel.SetActive(false);
        clickGood2.Play();
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

    public void ResetHighScores()
    {
        // Save this data to player prefs.
        PlayerPrefs.SetInt("HighScore01", 0);
        PlayerPrefs.SetInt("HighScore02", 0);
        PlayerPrefs.SetInt("HighScore03", 0);

        clickGood2.Play();
    }

    public void ResetAchievements()
    {
        // Save achievement information.
        for (int i = 0; i < 16; i++)
        {
            PlayerPrefs.SetInt("ACH" + i.ToString(), 0);

            clickGood2.Play();
        }
    }

    #endregion
}
