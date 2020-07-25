using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public AudioSource clickGood1;
    public AudioSource clickGood2;
    #endregion

    #region Variables - Text and Images to Update
    public Dropdown difficultyDropDown;
    public GameObject tutorialPanel;
    public Text doctorsNameText;
    public Text doctorsTaxText;
    public Image[] buttonLights;
    public Sprite geenLightImage;
    public Sprite blankImage;
    public Sprite[] spriteOfDoctors;
    public string[] doctorsNames = new string[4] { "D1", "D2", "D3", "D4" };
    public int[] agesOfDoctors = new int[4] { 40, 29, 32, 56 };
    public string[] genderOfDoctors = new string[4] { "M", "M", "M", "M" };
    public string[] taxonomyOfDoctors = new string[4] { "Nurse Practitioner", "Registered Nurse", "MD Pulmonary", "MD Family" };
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

        tutorialPanel.SetActive(false);

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

    public void ClickTutorial()
    {
        tutorialPanel.SetActive(true);
        clickGood1.Play();
    }

    public void ExitTutorialPanel()
    {
        tutorialPanel.SetActive(false);
        clickGood2.Play();
    }

    #endregion
}
