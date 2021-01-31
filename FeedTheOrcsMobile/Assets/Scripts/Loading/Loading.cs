using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider loadingSlider;
    public Text UWMText;
    public Text TipText;
    public Image tipImage;
    public Sprite[] tipSprites;
    public string nameOfDoctor;
    public string statusOfDoctor;
    public int day;
    public int waitingRoomFullLimit;
    public int numberOfNewPatients;
    public bool playerHasRunOutOfSuppliesAndMoney;
    private float loadScreenTimer;
    public GameObject continueButton;
    public bool isMuted;
    public AudioMixer mixer;
    public AudioSource clickGood1;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        UpdateTextToDisplayInformation();
        StartCoroutine(BeforeLoadWait());
        loadScreenTimer = 0;

        // Check to see if the audio should be muted.
        if (isMuted) MuteSound(true);

    }

    void Update()
    {
            loadScreenTimer += 1 * Time.deltaTime;
            loadingSlider.value = CalculateSliderValue();
    }

    public void SaveData()
    {
        GlobalCont.Instance.isMuted = isMuted;
    }

    #region Functions - LoadingSlider
    IEnumerator BeforeLoadWait()
    {
        yield return new WaitForSeconds(5);
        continueButton.SetActive(true);
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(4);

        while (gameLevel.progress < 1)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Functions - Display User Warning Message

    public void LoadData()
    {
        nameOfDoctor = GlobalCont.Instance.nameOfDoctor;
        statusOfDoctor = GlobalCont.Instance.statusOfDoctor;
        day = GlobalCont.Instance.day;
        waitingRoomFullLimit = GlobalCont.Instance.waitingRoomFullLimit;
        numberOfNewPatients = GlobalCont.Instance.numberOfNewPatients;
        playerHasRunOutOfSuppliesAndMoney = GlobalCont.Instance.playerHasRunOutOfSuppliesAndMoney;
        isMuted = GlobalCont.Instance.isMuted;
    }

    public void UpdateTextToDisplayInformation()
    {
        // There are 4 cases in which the player could have reached this screen.
        // We check for all of them and then display the correct information.
        // 1. Lobby has filled up.
        if (numberOfNewPatients > waitingRoomFullLimit)
        {
            UWMText.text = "The Orcs have overrun Jughog's Slophall.";
            TipText.text = "TIP: The Lobby Size upgrade will help to ensure the lobby does not fill up.";
            tipImage.sprite = tipSprites[0];
        }

        // 2. Player has run out of money and supplies
        else if (playerHasRunOutOfSuppliesAndMoney)
        {
            UWMText.text = "You have run out of supplies and money.";
            TipText.text = "TIP: The Purse Stings upgrade will help to reduce cost of supplies.";
            tipImage.sprite = tipSprites[1];
        }

        // 3. The player is exhausted.
        else if (statusOfDoctor == "EXHAUSTED")
        {
            UWMText.text = nameOfDoctor + " Is exhausted";
            TipText.text = "TIP: Make time to get a drink!";
            tipImage.sprite = tipSprites[2];
        }

        // 4. The player has reached the end of the 10 day period.
        else
        {
            UWMText.text = "Congratulations, " + nameOfDoctor + " has survived the 10 days!";
            TipText.text = "Nice work!";
            tipImage.sprite = tipSprites[3];
        }

    }

    float CalculateSliderValue()
    {
        return (loadScreenTimer/ 5);
    }

    public void ClickContinue()
    {
        clickGood1.Play();
        StartCoroutine(LoadAsyncOperation());
    }

    #endregion

    #region Functions Audio

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
