using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackStoryCont : MonoBehaviour
{

    #region Variables
    public string nameOfDoctor;
    public Sprite spriteOfDoctor;
    int numberOfLines = 4;
    string[] lines = new string[4];
    string[] nameOfSpeaker = new string[4];
    int currentLine = 0;

    public GameObject jughogsImage;
    public GameObject orcsImage;
    public GameObject orcsImageSprite;
    public GameObject continueButton;
    public Text mainLineText;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        LoadLines();
        LoadNameOfSpeaker();

        // load the correct image for the orc.
        orcsImageSprite.GetComponent<Image>().sprite = spriteOfDoctor;

        ClickContinue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Functions

    public void LoadData()
    {
        nameOfDoctor = GlobalCont.Instance.nameOfDoctor;
        spriteOfDoctor = GlobalCont.Instance.spriteOfDoctor;
    }

    public void UpdateText()
    {
        mainLineText.text = lines[currentLine];
    }

    public void UpdateSpeakerImage()
    {
        if (nameOfSpeaker[currentLine] == "JUGHOG")
        {
            jughogsImage.SetActive(true);
            orcsImage.SetActive(false);
        }

        else
        {
            jughogsImage.SetActive(false);
            orcsImage.SetActive(true);
        }
    }

    public void LoadLines()
    {
        lines[0] = "Okay " + nameOfDoctor + ", it is once again King Mushroom seasons and this year I am thinking " +
            "I may get the biggest harvest yet. Of course, this means I can either shut down the Slop Hall " +
            "or leave it in capable hands. I would rather not shut down, but unfortunately the only hands I " +
            "see are attached to the laziest Orc I have ever hired!";
        lines[1] = "By my troth, I will not let you down!";
        lines[2] = "Haha, that is what I like to hear " + nameOfDoctor + "! I will return in 10 days time, do your best. " +
            "Unless your best is not good enough, then do better. And do not let the lobby fill up! Large number " +
            "of Orcs with empty stomach will only result in a orc mob!";
        lines[3] = "Fare thee well " + nameOfDoctor + ", may dragons bless you!";
    }

    public void LoadNameOfSpeaker()
    {
        nameOfSpeaker[0] = "JUGHOG";
        nameOfSpeaker[1] = nameOfDoctor;
        nameOfSpeaker[2] = "JUGHOG";
        nameOfSpeaker[3] = "JUGHOG";
    }

    public void ClickSkipButton()
    {
        SceneManager.LoadScene(2);
    }

    public void ClickContinue()
    {
        StartCoroutine("Continue");
    }
    
    IEnumerator Continue()
    {
        // Set the continue button to not visable.
        continueButton.SetActive(false);

        if (currentLine == (numberOfLines))
        {
            SceneManager.LoadScene(2);
        }

        else
        {
            UpdateText();
            UpdateSpeakerImage();
            currentLine += 1;
        }

        // Suspend execution for 5 seconds
        yield return new WaitForSeconds(5);

        // Set the continue button to visable.
        continueButton.SetActive(true);




    }

    #endregion
}
