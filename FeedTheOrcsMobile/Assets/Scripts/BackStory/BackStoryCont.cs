using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    private float[] lengthOfLines = new float[4];

    public GameObject jughogsImage;
    public GameObject orcsImage;
    public GameObject orcsImageSprite;
    public GameObject continueButton;
    public Text mainLineText;

    #endregion

    #region Audio Sources
    public AudioMixer mixer;
    public AudioSource musicSource;
    public AudioSource speechSource;
    public AudioSource clickGood1;
    public AudioClip[] speechClips;
    public bool isMuted;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        LoadLines();
        LoadNameOfSpeaker();

        // load the correct image for the orc.
        orcsImageSprite.GetComponent<Image>().sprite = spriteOfDoctor;

        // Check to see if the audio should be muted.
        if (isMuted) MuteSound(true);

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
        isMuted = GlobalCont.Instance.isMuted;
    }

    public void SaveData()
    {
        GlobalCont.Instance.isMuted = isMuted;
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
        lines[0] = nameOfDoctor + ", once again it's Goblin Cap Mushroom season. Gonna get big harvest this year! " +
            "You laziest orc, but you watch Slop Hall for me. You better make me lots of GOLD, or I cut off your good ear.";
        lines[1] = "By my troth, I will not let you down!";
        lines[2] = "Haha. Good to hear! I be back 10 days time. Do your best. Or I cut off your bad ear!";
        lines[3] = "Dragon's strength to you " + nameOfDoctor + "!";

        lengthOfLines[0] = 14f;
        lengthOfLines[1] = 3.5f;
        lengthOfLines[2] = 8f;
        lengthOfLines[3] = 2.5f;
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
        clickGood1.Play();
    }

    public void ClickContinue()
    {
        StartCoroutine("Continue");
        clickGood1.Play();
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

            //Play correct speech clip.
            speechSource.clip = speechClips[currentLine];
            speechSource.Play();

            currentLine += 1;

            
        }

        // Suspend execution for 5 seconds
        yield return new WaitForSeconds(lengthOfLines[currentLine - 1]);

        // Set the continue button to visable.
        continueButton.SetActive(true);




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

    #endregion
}
