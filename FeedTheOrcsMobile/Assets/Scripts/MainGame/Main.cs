﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Main : MonoBehaviour
{
    #region Variables - Panels and Buttons

    public Button[] newPatientButtons;
    public Button[] patientPanelButtons;
    public GameObject[] patientPanels;
    public GameObject DischargePanel;
    public GameObject SpeechBubbleGood;
    private GameObject tempSpeechBubbleGood;
    public GameObject SpeechBubbleBad;
    private GameObject tempSpeechBubbleBad;
    public GameObject[] foodOnTableImgs;
    public Sprite[] foodOnTableSprites;

    public GameObject newUpgradePanel;
    public Text newUpgradeInformationText;
    public Image newUpgradeInformationImg;


    public GameObject RedTrollUseButton;
    private Image RedTrollUseImage;
    public Sprite RedBullActiveSprite;
    public Sprite RedBullInActiveSprite;
    public GameObject RedTrollBuyButton;

    public GameObject OrcBanquetUseButton;
    private Image OrcBanquetUseImage;
    public Sprite OrcBanquetActiveSprite;
    public Sprite OrcBanquetInActiveSprite;
    public GameObject OrcBanquetBuyButton;

    public GameObject DragonsWealthUseButton;
    private Image DragonsWealthUseImage;
    public Sprite DragonsWealthActiveSprite;
    public Sprite DragonsWealthInActiveSprite;
    public GameObject DragonsWealthBuyButton;


    #endregion

    #region Variables - Text objects to update
    public Text numberOfNewPatientsText;
    public Text lobbyLimitText;
    public Text currentDayText;
    public Text[] nameOfPatientsText;
    public Text[] ageOfPatientsText;
    public Text[] sexOfPatientsText;
    public Text[] statusOfPatientsText;
    public Image[] picOfPatientImages;
    public Text nameOfDoxtorText;
    public Text statusOfDoctorText;
    public Image imageOfDoctor;
    #endregion

    #region Variables - Patients
    public GameObject patientPrefab;
    // This array will be filled with current patients. 0 - patient in bed 1, 1 - patient in bed 2 ...
    public GameObject[] currentPatients = new GameObject[6];

    // Each Bed-Panel has 4 buttons that can only be pressed when the doctor is in reach - TODO.
    // The lights around the button indicate if a user can press the button. Here is where we store the
    // images to update when a button needs to be updated.
    public Sprite greenLightImage;
    public Sprite redLightImage;
    public Sprite blankImage;
    // LayoutForBedButtons [maskLight, IVLight, pneumoniaLight, criticalLight]
    public Image[] bedButtons1;
    public Image[] bedButtons2;
    public Image[] bedButtons3;
    public Image[] bedButtons4;
    public Image[] bedButtons5;
    public Image[] bedButtons6;
    public Image[] handWashButtons;
    // Layout For bedButtonsForAllBeds [bedButtons1[], bedButtons2[], bedButtons3[], bedButtons4[], bedButtons5[], bedButtons6[], handWashButtons]
    public Image[][] bedButtonsForAllBeds = new Image[7][];
    public Image[] newPatientButtonImages;

    // Warning Bubbles and warning bubble location objects.
    public GameObject[] warningBubblePrefab; // 0 - Healthy 1 - Mask, 2 - IV, 3 - Cough, 4 - Ven,  5 - Deceased 
    public GameObject[] warningBubbleLocations; // 0 - Sink, 1 - Bed 1, 2 - Bed 2, ...
    public GameObject[] activeWarningBubbles = new GameObject[7]; // This is used to keep track of active bubbles for deletion.
    public GameObject t; // Used when creating new Warning Bubbles to temporaly hold the object;

    // This deals with putting an image of the patient in the bad.
    public GameObject[] physicalPatientPrefabs;
    public GameObject[] activePhysicalPatients = new GameObject[6]; // This is to keep track of the objects so we can delete later.
    public GameObject[] physicalBeds; // So we have the location to put the patient at.
    public GameObject physicalPatientTemp;
    #endregion

    #region Variables Doctor

    public string nameOfDoctor;
    public int ageOfDoctor;
    public string sexOfDoctor;
    public string statusOfDoctor;
    public Sprite spriteOfDoctor;
    public int doctorsCurrentBed; // 0 -Sink, 1- bed1, ...

    // This section hold the physical doctor and animations.
    public GameObject doctorPrefab1;
    public GameObject doctorPrefab2;
    public GameObject doctorPrefab3;
    public GameObject doctorPrefab4;
    public GameObject doctor;
    public Animator docAnim;
    public float doctorSpeed;
    public Vector3 targetPos;
    public GameObject[] roomTriggers; //Layout [Room, Bed1, Bed2, Bed3, Bed4, Bed5, Bed6, Sink]

    // Deals with hand washing.
    public float handWashingTimer;
    public float timeDoctorHasToWashHands; // Once the player is prompted, this is the time they have to respond.
    public float timeBetweenHandWashMin; // For an element of randomness, the time between washings will be a random.
    public float timeBetweenHandWashMax; // number between these two variables.
    public float timeBetweenHandWash;
    private bool needsToWashHands;
    public float warningTime; // The warning bubble will turn more serious if there is only this amount of time left.
    private bool handWashWarning;
    private bool movingDisabledForHandWashing;

    // Deals with discharging.
    private bool dischargeButtonIsAvailable = true;

    // Handles soap animation.
    public GameObject soapBubblePrefab;
    public GameObject BeerSprayPrefab;
    private GameObject beerSpray;
    public GameObject sink;
    private GameObject tempSoapBubble;
    private GameObject[] soapBubbles = new GameObject[10];


    #endregion

    #region Variables - SupplyPanel

    public GameObject supplyPanel;
    public int[] numberOfSupplies; // 0 - quantity of masks, 1 - quantity of IVs, ...
    public Text[] quantityOfMaskText;
    public Text[] quantityOfIVText;
    public Text[] quantityOfCoughText;
    public Text[] quantityOfVenText;
    public Text[][] quantityOfSuppliesText = new Text[4][];
    public int currentMoney;
    public Text currentMoneyText;
    public int[] costOfSupplies;

    #endregion

    #region Variables - Consumables

    // Red Troll - Increases the player's speed for 10 seconds.
    // OrcBanquet - Feeds all orcs that are seated instantly.
    // Dragons Wealth - THis makes all orcs pay 4 times the amount.
    private int redTrollTimerLength = 10;
    private float redTrollTimer;
    private bool redTrollUpgradeActive;
    //private bool orcBanquetUpgradeActive;
    private int dragonsWealthTimerLength = 10;
    private float dragonsWealthTimer;
    private bool dragonsWealthUpgradeActive;
    public Slider redTrollSlider;
    public Slider dragonsWealthSlider;


    #endregion

    #region Variables - Upgrades
    public GameObject UpgradePanel;
    private int UpgradeGems;
    private int upgradeGemRate;
    public Text upgradeGemsText;

    public GameObject[] upgradeLobbySizeBacklights;
    public GameObject[] upgradePopularityBacklights;
    public GameObject[] upgradePurseStringsBacklights;
    public GameObject[] upgradeChemistryBacklights;

    private int upgradeLobbySizeCurrentValue; // This value should be between 0 - 10
    private int upgradePopularityCurrentValue; // This value should be between 0 - 10
    private int upgradePurseStringsCurrentValue; // This value should be between 0 - 10
    private int upgradeChemistryCurrentValue; // This value should be between 0 - 10


    // Upgrade Lobby Size
    private int waitingRoomFullLimit; // How many patients can be in the waiting room before game over.

    //Upgrade Popularity
    public int timeBetweenNewPatients; // In seconds

    //Upgrade Frugality


    //upgrade Efficiency


    #endregion

    #region Variables - Coin Animation
    public GameObject coinAnimationPreFab;
    public GameObject dragonsWealthCoinAnimationPreFab;
    private GameObject coinAnimation;
    private GameObject dragonsWealthCoinAnimation;
    private Vector3 startCoinLocation;
    private Vector3 hideCoinLocation = new Vector3(15f, 15f, 0f);
    private Vector3 endCoinLocation = new Vector3(5.7f, 6.3f, 0f); // This is the location of the Coin Icon.
    private Vector3 errorCoinLocation = new Vector3(1f, 1f, 0f); // This vector is used to see if the coin is close enough to the end location.
    private bool coinAnimationActive;
    private bool dragonsWealthCoinAnimationActive;

    # endregion

    #region Audio Sources
    public AudioMixer mixer;
    public AudioSource musicSource;
    public AudioClip[] musicClips;
    public AudioSource clickGood1;
    public AudioSource clickGood2;
    public AudioSource clickBad1;
    public AudioSource newDay;
    public AudioSource newCustomer;
    public AudioSource spendMoney;
    public AudioSource getMoney;
    public AudioSource turnPage;
    public AudioSource useConsumable;
    public AudioSource warning1;
    public AudioSource serverDrink;
    public AudioSource unhappy;
    public AudioClip[] unhappyClips;
    public AudioSource happy;
    public AudioClip[] happyClips;
    public AudioSource achievementUnlockedAudio;
    public bool isMuted;
    public Slider musicSlider;
    public Slider SFXSlider;
    #endregion

    #region Variables - Stats
    public int day;
    public int patientsHealed;
    public int patientsDeceased;
    public int gameDifficulty;
    public int numberOfNewPatients;
    private int lowestHighScore;
    private int lowestHighScoreIndex;
    public int[] highScores;
    private bool playerHasRunOutOfSuppliesAndMoney;
    public bool[] achievementsUnlocked;

    // Variables to keep each day moving.
    public float mainTimer;
    public float newPatientTimer;
    public int lenghtOfDay; // In seconds.
    public Slider timeOfDaySlider;

    private bool gameIsPaused;
    #endregion

    #region Variables - Tips&Tricks
    public GameObject tipsAndTricksPanel;
    public GameObject ConsumableSubPanel;
    public GameObject foodSubPanel;
    public GameObject upgradeSubPanel;
    public GameObject settingsSubPanel;
    public GameObject achievementSubPanel;
    public Text consumableName;
    public Text foodName;
    public Text upgradeName;
    public Text consumableDescriptionText;
    public Text foodDescriptionText;
    public Text upgradeDescriptionText;
    public Text consumablesCurrentStatText;
    public Text foodCurrentStatText;
    public Text upgradeCurrentStatText;
    public Text currentTrackNumberText;
    public Text achievementDescription;
    
    private int currentTrackNumber = 0;

    #endregion

    #region Variables - Achievements

    public GameObject achievementPopPanel;
    public Image[] achievementImgs;
    public Sprite[] achievementSprites;
    private int orcsServedInOneDayACH0203;
    private bool[] consumablesUsedACH01 = new bool[3];
    private int goldEarnedInOneDay;
    private int orcsRefusedACH04;

    #endregion

    #region Variables - EndOfDay
    public GameObject EndOFDayPanel;
    public Text numberOFGemsText;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        LoadGameDifficulty();
        timeBetweenHandWash = SetNewTimeBetweenHandWash(timeBetweenHandWashMin, timeBetweenHandWashMax);

        // Disable all panels/buttons.
        HidePatientPanels();
        ClickPatientPanel(0);
        HideDischargePanel();

        // Set text objects to inital values.
        numberOfNewPatientsText.text = "0";
        lobbyLimitText.text = "/ " + waitingRoomFullLimit.ToString();
        currentMoneyText.text = currentMoney.ToString();
        upgradeGemsText.text = UpgradeGems.ToString();

        // Load Doctor Data to screen.
        LoadDoctorsPage();

        // Check to see if the audio should be muted.
        if (isMuted)
        {
            MuteSound(true);
            isMuted = true;
        }

        // Set All Buttons Images to the appropriate availability.
        bedButtonsForAllBeds[0] = bedButtons1;
        bedButtonsForAllBeds[1] = bedButtons2;
        bedButtonsForAllBeds[2] = bedButtons3;
        bedButtonsForAllBeds[3] = bedButtons4;
        bedButtonsForAllBeds[4] = bedButtons5;
        bedButtonsForAllBeds[5] = bedButtons6;
        bedButtonsForAllBeds[6] = handWashButtons;

        // Setup the supplyText array.
        quantityOfSuppliesText[0] = quantityOfMaskText;
        quantityOfSuppliesText[1] = quantityOfIVText;
        quantityOfSuppliesText[2] = quantityOfCoughText;
        quantityOfSuppliesText[3] = quantityOfVenText;

        for (int i = 0; i < 6; i++)
        {
            SetBedButtonsOnOff(i, false);
            SetNewPatientButtonsOnOff(i, true);

            // Clear Patient Information From Screen.
            //ClearPatientDataFromScreen(i);
        }
        SetBedButtonsOnOff(6, false);

        // Create Doctor from Prefab that matches.
        if (nameOfDoctor == "Nar") doctor = Instantiate(doctorPrefab1);
        else if (nameOfDoctor == "Gnurl") doctor = Instantiate(doctorPrefab2);
        else if (nameOfDoctor == "Tarfu") doctor = Instantiate(doctorPrefab3);
        else doctor = Instantiate(doctorPrefab4);
        doctor.transform.position = Vector3.zero;

        //Initialize Doctor Animator.
        docAnim = doctor.GetComponentInChildren<Animator>();

        // Initialize coin animation.
        coinAnimation = Instantiate(coinAnimationPreFab);
        coinAnimation.transform.position = hideCoinLocation;
        dragonsWealthCoinAnimation = Instantiate(dragonsWealthCoinAnimationPreFab);
        dragonsWealthCoinAnimation.transform.position = hideCoinLocation;

        // Initialize images for instant upgrades and timer values.
        RedTrollUseImage = RedTrollUseButton.GetComponent<Image>();
        OrcBanquetUseImage = OrcBanquetUseButton.GetComponent<Image>();
        DragonsWealthUseImage = DragonsWealthUseButton.GetComponent<Image>();

        // Set initial value for achievements.
        orcsServedInOneDayACH0203 = 0;
        goldEarnedInOneDay = 0;
        orcsRefusedACH04 = 0;
        for (int i = 0; i < 3; i++)
        {
            consumablesUsedACH01[i] = false;
        }

        LoadAchievementSpritesAtStart();
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is paused we do not want to update anything.
        if (!gameIsPaused)
        {
            if (movingDisabledForHandWashing == false)
            {
                // Move Doctor Towards Trigger if Doc Is Moving.
                doctor.transform.position = Vector3.MoveTowards(doctor.transform.position, targetPos, doctorSpeed * Time.deltaTime);
            }


            // Main Game Loop - THINGS PUT HERE NEED TO BE CLEAN AND STREAM LINED!!!
            mainTimer += Time.deltaTime; // Update Timer
            timeOfDaySlider.value = CalculateTimeOfDaySliderValue();
            newPatientTimer += Time.deltaTime;
            handWashingTimer += Time.deltaTime;

            // New Patient Update
            if (newPatientTimer > timeBetweenNewPatients)
            {
                numberOfNewPatients += 1;
                UpdateStatsToScreen();
                newCustomer.Play();

                // Check to see if waiting room is full - GAMEOVER
                if (numberOfNewPatients > waitingRoomFullLimit)
                {
                    SaveData();
                    SceneManager.LoadScene(3);
                }

                //Reset Timer
                newPatientTimer = 0;
            }

            // Hand Wash Update
            if (needsToWashHands == false && handWashingTimer > timeBetweenHandWash)
            {
                // Create warning bubble at hand wash station.
                CreateWarningBubbleAtBed(0, 6);

                // Set bool so doctor needs to wash hands.
                needsToWashHands = true;

                // Set doctors status to "CONTAMINATED"
                statusOfDoctor = "EXHAUSTED";
                statusOfDoctorText.text = statusOfDoctor;


                // reset timer.
                handWashingTimer = 0;
            }

            else if (handWashWarning == false && needsToWashHands && handWashingTimer > timeDoctorHasToWashHands - warningTime)
            {
                // Replace the warning bubble with a RED warning bubble so the user knows there is only a few seconds to infection.
                DestroyWarningBubbleAtBed(0);
                CreateWarningBubbleAtBed(0, 7);

                handWashWarning = true;
            }

            else if (handWashWarning && handWashingTimer > timeDoctorHasToWashHands)
            {
                statusOfDoctor = "EXHAUSTED";
                SaveData();
                SceneManager.LoadScene(3);
            }

            // If the coin animation bool is true - run the moveCoinFunction.
            if (coinAnimationActive || dragonsWealthCoinAnimationActive)
            {
                moveCoinAnimation();
            }


            // End Of Day Tasks
            if (mainTimer > lenghtOfDay)
            {
                // Reset Timer, Advance Day, and Update Stats On Screen.
                day += 1;
                mainTimer = 0;
                UpgradeGems += upgradeGemRate;
                newDay.Play();
                UpdateStatsToScreen();

                // Reset Achievement trackers.
                orcsServedInOneDayACH0203 = 0;
                goldEarnedInOneDay = 0;

                // Open the End Of Day Panel and pause game.
                OpenCloseEndOfDayPanel(true);

                // Check to see if player still has money and supplies.
                if (CheckToSeeIfPlayerIsOutOfMoney())
                {
                    playerHasRunOutOfSuppliesAndMoney = true;
                    SaveData();
                    SceneManager.LoadScene(3);
                }

                // Check to see if the 10 day time limit is up.
                if (day == 11)
                {
                    SaveData();
                    SceneManager.LoadScene(3);
                }

                // Check to see if it is time to make the next upgrade available.
                if (day == 2 && RedTrollUseButton.activeInHierarchy == false)
                {
                    RedTrollUseButton.SetActive(true);
                    LoadNewUpgradePanel(0);
                }

                else if (day == 3 && OrcBanquetUseButton.activeInHierarchy == false)
                {
                    OrcBanquetUseButton.SetActive(true);
                    LoadNewUpgradePanel(1);
                }

                else if (day == 4 && DragonsWealthUseButton.activeInHierarchy == false)
                {
                    DragonsWealthUseButton.SetActive(true);
                    LoadNewUpgradePanel(2);
                }
            }

            // Check to if the RedTroll is active. If it is, update the timer and disable the speed upgrade at the appropriate time.
            if (redTrollUpgradeActive)
            {
                if (redTrollTimer > 0)
                {
                    redTrollTimer -= 1 * Time.deltaTime;
                    redTrollSlider.value = CalculateSliderValue("RedTroll");
                }

                else
                {
                    redTrollUpgradeActive = false;
                    doctorSpeed = 4;
                }
            }

            if (dragonsWealthUpgradeActive)
            {
                if (dragonsWealthTimer > 0)
                {
                    dragonsWealthTimer -= 1 * Time.deltaTime;
                    dragonsWealthSlider.value = CalculateSliderValue("DragonsWealth");
                }

                else
                {
                    dragonsWealthUpgradeActive = false;
                }
            }
        }
    }

    #region Functions

    #region Load-Save Functions
    public void LoadData()
    {
        nameOfDoctor = GlobalCont.Instance.nameOfDoctor;
        ageOfDoctor = GlobalCont.Instance.ageOfDoctor;
        sexOfDoctor = GlobalCont.Instance.sexOfDoctor;
        statusOfDoctor = GlobalCont.Instance.statusOfDoctor;
        day = GlobalCont.Instance.day;
        patientsHealed = GlobalCont.Instance.patientsHealed;
        patientsDeceased = GlobalCont.Instance.patientsDeceased;
        spriteOfDoctor = GlobalCont.Instance.spriteOfDoctor;
        gameDifficulty = GlobalCont.Instance.gameDifficulty;
        GlobalCont.Instance.gameDifficulty = gameDifficulty;
        highScores = GlobalCont.Instance.highScores;
        isMuted = GlobalCont.Instance.isMuted;
        achievementsUnlocked = GlobalCont.Instance.achievementsUnlocked;

        LoadHighScoreFromPlayerPrefs();
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
        GlobalCont.Instance.wealth = currentMoney;
        GlobalCont.Instance.numberOfNewPatients = numberOfNewPatients;
        GlobalCont.Instance.waitingRoomFullLimit = waitingRoomFullLimit;
        GlobalCont.Instance.playerHasRunOutOfSuppliesAndMoney = playerHasRunOutOfSuppliesAndMoney;
        GlobalCont.Instance.isMuted = isMuted;

        SaveHighScoreToPlayerPrefs();

    }

    public void SaveHighScoreToPlayerPrefs()
    {
        Debug.Log("Saving High Scores from Player Prefs ...");

        // Check to see if the player's score is a New High Score.
        // 1. Find the lowest high score.
        lowestHighScore = highScores[0];
        lowestHighScoreIndex = 0;

        for (int i = 1; i < highScores.Length; i++)
        {
            if (highScores[i] < lowestHighScore)
            {
                lowestHighScore = highScores[i];
                lowestHighScoreIndex = i;
            } 
        }

        // 2. Compare the lowest high score to the player's score.
        //    If the player's score is greater then we will replace the score and set newHighScore bool true.
        if (patientsHealed > lowestHighScore)
        {
            highScores[lowestHighScoreIndex] = patientsHealed;
            GlobalCont.Instance.newHighScore = true;
        }

        // Save this data to player prefs.
        PlayerPrefs.SetInt("HighScore01", highScores[0]);
        PlayerPrefs.SetInt("HighScore02", highScores[1]);
        PlayerPrefs.SetInt("HighScore03", highScores[2]);

        // Save achievement information.
        for (int i = 0; i < 16; i++)
        {
            if (achievementsUnlocked[i])
            {
                PlayerPrefs.SetInt("ACH" + i.ToString(), 1);
            }

            else
            {
                PlayerPrefs.SetInt("ACH" + i.ToString(), 0);
            }
        }
        
    }

    public void LoadHighScoreFromPlayerPrefs()
    {
        Debug.Log("Loading High Scores from Player Prefs ...");

        // Load data if it exists.
        highScores[0] = PlayerPrefs.GetInt("HighScore01", 0);
        highScores[1] = PlayerPrefs.GetInt("HighScore02", 0);
        highScores[2] = PlayerPrefs.GetInt("HighScore03", 0);

        // Load Achievement Information.
        for (int i = 0; i < 16; i++)
        {
            if (PlayerPrefs.GetInt("ACH" + i.ToString()) == 1)
            {
                achievementsUnlocked[i] = true;
            }

        }
    }

    public void LoadGameDifficulty()
    {
        // This function will need to be re worked once upgrades are implementaed.
            timeDoctorHasToWashHands = 30;
            timeBetweenHandWashMin = 20;
            timeBetweenHandWashMax = 120;
            warningTime = 10;

        // Set the dificulty values.
        if (gameDifficulty == 0)
        {
            // Set initial upgrade levels and then call the update function.
            upgradeLobbySizeCurrentValue = 3;
            upgradePopularityCurrentValue = 3;
            upgradePurseStringsCurrentValue = 3;
            upgradeChemistryCurrentValue = 3;
            currentMoney = 50;
            UpgradeGems = 2;
            upgradeGemRate = 3;
            lenghtOfDay = 60;
        }

        else if (gameDifficulty == 1)
        {
            // Set initial upgrade levels and then call the update function.
            upgradeLobbySizeCurrentValue = 2;
            upgradePopularityCurrentValue = 2;
            upgradePurseStringsCurrentValue = 2;
            upgradeChemistryCurrentValue = 2;
            currentMoney = 10;
            UpgradeGems = 1;
            upgradeGemRate = 3;
            lenghtOfDay = 50;
        }

        else if (gameDifficulty == 2)
        {
            // Set initial upgrade levels and then call the update function.
            upgradeLobbySizeCurrentValue = 1;
            upgradePopularityCurrentValue = 1;
            upgradePurseStringsCurrentValue = 1;
            upgradeChemistryCurrentValue = 1;
            currentMoney = 0;
            UpgradeGems = 0;
            upgradeGemRate = 3;
            lenghtOfDay = 40;
        }

        UpdateUpgradeValues();

    }
    #endregion


    #region Mics - Functions
    public void HidePatientPanels()
    {
        for (int i = 0; i < patientPanels.Length; i++)
        {
            patientPanels[i].SetActive(false);
        }
    }

    public void HideDischargePanel()
    {
        DischargePanel.SetActive(false);
    }

    IEnumerator WaitForOrcToSpeak(int i)
    {

        dischargeButtonIsAvailable = false;

        // Play the audio for unhappy if the int i == 0 and happy if i == 1.
        if (i == 1)
        {
            // Set the movement ability.
            movingDisabledForHandWashing = true;

            // Create a speech bubble at the orc's location.
            tempSpeechBubbleGood = Instantiate(SpeechBubbleGood);
            tempSpeechBubbleGood.transform.position = warningBubbleLocations[doctorsCurrentBed + 1].transform.position;

            // Check to see if achievement 01 or 02 have been unlocked.
            orcsServedInOneDayACH0203 += 1;
            CheckForAchievement(2);
            CheckForAchievement(3);

            //Play a random happy audio clip.
            var randAudio = Random.Range(0, happyClips.Length);
            happy.clip = happyClips[randAudio];
            happy.Play();
            getMoney.Play();

            // Suspend execution for 5 seconds
            yield return new WaitForSeconds(2);

            // Set the movement ability.
            movingDisabledForHandWashing = false;

            // Delete the patient and update stat.
            Destroy(currentPatients[doctorsCurrentBed]);
            currentPatients[doctorsCurrentBed] = null;
            patientsHealed += 1;

            // Update Patient Data and reset bed button
            //UpdatePatientDataToScreen(doctorsCurrentBed);
            SetNewPatientButtonsOnOff(doctorsCurrentBed, true);

            // Delete Warning Bubble if it is still there.
            if (activeWarningBubbles[doctorsCurrentBed + 1] != null)
            {
                DestroyWarningBubbleAtBed(doctorsCurrentBed + 1);
            }

            // Destroy the physical patient image.
            RemovePhysicalPatientFromBed(doctorsCurrentBed);

            // Player gets MONEY$$$$ - If the player has Dragons Wealth active then the 
            if (dragonsWealthUpgradeActive)
            {
                currentMoney += 20;
                goldEarnedInOneDay += 20;
            }
            else
            {
                currentMoney += 5;
                goldEarnedInOneDay += 5;
            }
            currentMoneyText.text = currentMoney.ToString();

            // Play Click Sound
            clickGood1.Play();

            // Check to see if achievements 05 or 06 have been unlocked.
            CheckForAchievement(5);
            CheckForAchievement(6);

            // Check to see if achievements 07 or 08 have been unlocked.
            CheckForAchievement(7);
            CheckForAchievement(8);

            // Hide Panel and reset the discharge button.
            HideDischargePanel();
            dischargeButtonIsAvailable = true;

            // Delete the speech bubble.
            Destroy(tempSpeechBubbleGood);

            // Update player's score to screen.
            UpdateStatsToScreen();
        }

        else
        {
            // Create a speech bubble at the orc's location.
            tempSpeechBubbleBad = Instantiate(SpeechBubbleBad);
            tempSpeechBubbleBad.transform.position = warningBubbleLocations[doctorsCurrentBed + 1].transform.position;

            //Play a random angry audio clip.
            var randAudio = Random.Range(0, unhappyClips.Length);
            unhappy.clip = unhappyClips[randAudio];
            unhappy.Play();

            // Suspend execution for 5 seconds
            yield return new WaitForSeconds(2);

            // Delete the patient and update stat.
            Destroy(currentPatients[doctorsCurrentBed]);
            currentPatients[doctorsCurrentBed] = null;
            patientsDeceased += 1;

            // Update Patient Data and reset bed button
            //UpdatePatientDataToScreen(doctorsCurrentBed);
            SetNewPatientButtonsOnOff(doctorsCurrentBed, true);

            // Delete Warning Bubble if it is still there.
            if (activeWarningBubbles[doctorsCurrentBed + 1] != null)
            {
                DestroyWarningBubbleAtBed(doctorsCurrentBed + 1);
            }

            // Destroy the physical patient image.
            RemovePhysicalPatientFromBed(doctorsCurrentBed);

            // Play Click Sound
            clickGood1.Play();

            // Hide Panel and reset the discharge button
            HideDischargePanel();
            dischargeButtonIsAvailable = true;

            // Delete the speech bubble.
            Destroy(tempSpeechBubbleBad);

            // Increase achievment tracker and check for achievement reached.
            orcsRefusedACH04 += 1;
            CheckForAchievement(4);
        }

        
    }

    public void ClickDischargePanelDischarge(int location)
    {

        if (location == 1 && dischargeButtonIsAvailable)
        {
            // Check to see if patient is healthy.
            if (currentPatients[doctorsCurrentBed].GetComponent<PatientData>().statusOfPatient == "FED")
            {
                // Play audio clip and start co routine to wait for clip to finish.
                StartCoroutine(WaitForOrcToSpeak(1));

                // Play coin animation.
                startCoinLocation = doctor.transform.position;
                Debug.Log(startCoinLocation);
                if (dragonsWealthUpgradeActive)
                {
                    dragonsWealthCoinAnimationActive = true;
                    dragonsWealthCoinAnimation.transform.position = startCoinLocation;
                }

                else
                {
                    coinAnimationActive = true;
                    coinAnimation.transform.position = startCoinLocation;
                }
            }

            else
            {
                Debug.Log("Patient is not fed");
                clickBad1.Play();
            }
        }

        else if (location == 2 && dischargeButtonIsAvailable)
        {
            // Check to see if patient is dead.
            if (currentPatients[doctorsCurrentBed].GetComponent<PatientData>().statusOfPatient == "DISSATISFIED")
            {
                // Play audio clip and start co routine to wait for clip to finish.
                StartCoroutine(WaitForOrcToSpeak(0));
            }

            else
            {
                Debug.Log("Patient is not dissatisfied");
                // Play Click Sound
                clickBad1.Play();
            }
        }
        
        else clickBad1.Play(); ;


    }

    public void UpdateStatsToScreen()
    {
        // Currently only for Day and New Patients.
        numberOfNewPatientsText.text = numberOfNewPatients.ToString();
        currentDayText.text = day.ToString();
        lobbyLimitText.text = "/ " + waitingRoomFullLimit.ToString();
        upgradeGemsText.text = UpgradeGems.ToString();
    }

    float CalculateTimeOfDaySliderValue()
    {
        return (mainTimer / lenghtOfDay);
    }

    bool CheckToSeeIfPlayerIsOutOfMoney()
    {
        if (currentMoney < 5 && numberOfSupplies[0] == 0 && numberOfSupplies[1] == 0 && numberOfSupplies[2] == 0 && numberOfSupplies[3] == 0)
        {
            return true;
        }

        return false;
    }

    #endregion


    #region Functions - Set Lights For Buttons
    public void SetButtonImgAvailable(Image buttonImg)
    {
        buttonImg.sprite = greenLightImage;
    }

    public void SetButtonImgUnAvailable(Image buttonImg)
    {
        buttonImg.sprite = redLightImage;
    }

    public void SetBedButtonsOnOff(int bed, bool on)
    {
        //This function is used to make handeling turing on and off lights for
        // the bed panels easy.
        if (on)
        {
            for (int i = 0; i < bedButtonsForAllBeds[bed].Length; i++)
            {
                bedButtonsForAllBeds[bed][i].sprite = greenLightImage;
            }
        } 

        else
        {
            for (int i = 0; i < bedButtonsForAllBeds[bed].Length; i++)
            {
                bedButtonsForAllBeds[bed][i].sprite = redLightImage;
            }
        }
            
        

    }

    public void SetNewPatientButtonsOnOff(int bed, bool on)
    {
        if (on)
        {
            newPatientButtonImages[bed].sprite = greenLightImage;
        }

        else
        {
            newPatientButtonImages[bed].sprite = redLightImage;
        }
    }
    #endregion


    #region Functions - Patient Update Functions
    public void ClickNewPatient(int bed)
    {
        // Check to see if there is any new patients.
        if(numberOfNewPatients > 0)
        {
            // Check to see if the table is available.
            if (currentPatients[bed] == null)
            {
                bool providerLoaded = CreatePatientAndPopulateBed(bed);
                //UpdatePatientDataToScreen(bed);

                if (providerLoaded) SetNewPatientButtonsOnOff(bed, false);

                numberOfNewPatients -= 1;
                // Update Text object on screen.
                UpdateStatsToScreen();

                // Place the image of the patient at the bed.
                PlacePhysicalPatientOnBed(bed);

                // Play Click Sound
                clickGood1.Play();

                Debug.Log("Orc has been seated");
            }

            else Debug.Log("Table is full!");
        }

        else
        {
            Debug.Log("TODO Handle No New Patient");
            // Play Click Sound
            clickBad1.Play();
        }
        

    }
    
    public bool CreatePatientAndPopulateBed(int bed)
    {
        //Returns True if is successfully loads the patient.
        GameObject temp = Instantiate(patientPrefab);
        currentPatients[bed] = temp;
        return true;

    }

    public void PauseUnpausePatients(bool isPaused)
    {
        for (int i = 0; i < currentPatients.Length; i++)
        {
            if (currentPatients[i] != null) currentPatients[i].GetComponent<PatientData>().gameIsPaused = isPaused;
        }
    }
 
    public void ClickPatientPanel(int bedNumber)
    {
        HidePatientPanels();

        patientPanels[bedNumber].SetActive(true);

        // Play Click Sound
        clickGood1.Play();
    }

    public void CreateWarningBubbleAtBed(int bed, int warning)
    {
        // Play Warning Sound
        warning1.Play();

        // Create bubble and place it at location.
        t = Instantiate(warningBubblePrefab[warning], warningBubbleLocations[bed].transform);

        // Save bubble to array for deletion latter.
        activeWarningBubbles[bed] = t;

        // Clear the food on table image if it is not empty already.
        if (bed != 0)
        {
            var tempSprite = foodOnTableImgs[bed - 1].GetComponent<SpriteRenderer>();
            tempSprite.sprite = foodOnTableSprites[4];
        }
        
  

    }

    public void DestroyWarningBubbleAtBed(int bed)
    {
        Destroy(activeWarningBubbles[bed]);
        activeWarningBubbles[bed] = null;
    }

    public void PlacePhysicalPatientOnBed(int bed)
    {
        // Pic a physical patient image depending on the age and gender.
        var indexTemp = Random.Range(0, physicalPatientPrefabs.Length);
        physicalPatientTemp = Instantiate(physicalPatientPrefabs[indexTemp]);
        activePhysicalPatients[bed] = physicalPatientTemp;

        // Place patient on bed.
        physicalPatientTemp.transform.position = physicalBeds[bed].transform.position + (Vector3.up * 1f);
    }

    public void RemovePhysicalPatientFromBed(int bed)
    {
        // Destory the game object.
        Destroy(activePhysicalPatients[bed]);
    }

    #region Functions - Treatment For Individual Beds.
    public void TreatPB1(int treatment)
    {
        TreatPatientAtBed(0, treatment);
    }

    public void TreatPB2(int treatment)
    {
        TreatPatientAtBed(1, treatment);
    }

    public void TreatPB3(int treatment)
    {
        TreatPatientAtBed(2, treatment);
    }

    public void TreatPB4(int treatment)
    {
        TreatPatientAtBed(3, treatment);
    }

    public void TreatPB5(int treatment)
    {
        TreatPatientAtBed(4, treatment);
    }

    public void TreatPB6(int treatment)
    {
        TreatPatientAtBed(5, treatment);
    }
    #endregion

    public void TreatPatientAtBed(int bed, int treatment)
    {
        // Check to see if doctor is in range to treat.
        if (bedButtonsForAllBeds[bed][treatment].sprite.name == "GreenCircle" && currentPatients[bed] != null)
        {
            // Check To see if the user selected the correct treatment and if there is supply.
            if (currentPatients[bed].GetComponent<PatientData>().statusOfPatient == GlobalPatientData.statusOfPatients[treatment + 1] && numberOfSupplies[treatment] > 0)
            {
                // Play Click Sound
                clickGood1.Play();

                // Place food on table by loading in the correct image.
                var tempSprite = foodOnTableImgs[bed].GetComponent<SpriteRenderer>();
                tempSprite.sprite = foodOnTableSprites[treatment];

                // Destory the warning Message and change patients bool, so they get healther.
                DestroyWarningBubbleAtBed(bed + 1);
                currentPatients[bed].GetComponent<PatientData>().needsTreatment = false;

                // Update supply information.
                UpdateSupplyIntAndText(treatment, -1);
            }

            else clickBad1.Play();

        }

        else 
        {
            // Play Click Sound
            clickBad1.Play();
        }
        
    }
    #endregion


    #region Functions - Doctor Update Function

    public void LoadDoctorsPage()
    {
        nameOfDoxtorText.text = nameOfDoctor;
        statusOfDoctorText.text = statusOfDoctor;
        imageOfDoctor.sprite = spriteOfDoctor;

    }

    public void MoveDocToTrigger(int triggerNum)
    {
        // This function should move the doctor to a trigger and play the correct animations.
        // When the doctor reachers the trigger the doctor should stop and the idle anim plays.

        // Decide if the trigger is left or right of the doctor to play the correct anim.
        if (doctor.transform.position.x - roomTriggers[triggerNum].transform.position.x < 0 && movingDisabledForHandWashing == false)
        {
            // Play Right Animation
            docAnim.Play("MWalkRight");
        }

        else if (movingDisabledForHandWashing == false)
        {
            // Play Left Animation TODO.
            docAnim.Play("MWalkLeft");
        }

        // Set the target.
        targetPos = roomTriggers[triggerNum].transform.position;
    }

    public float SetNewTimeBetweenHandWash(float min, float max)
    {
        return Random.Range(min, max);
    }

    public void ClickWashHands()
    {
        // Check to see if doctor is in range to wash.
        if (bedButtonsForAllBeds[6][0].sprite.name == "GreenCircle" && needsToWashHands)
        {
            handWashingTimer = 0;

            // Play Click Sound.
            clickGood1.Play();

            // Wait For Hands To Wash
            StartCoroutine(WaitForHandsToWash());

        }

        else
        {
            clickBad1.Play();
            Debug.Log("Trying to wash hands");
        }
        
    }

    IEnumerator WaitForHandsToWash()
    {
        // Play the audio of the server drinking.
        serverDrink.Play();

        // Play the animation and switch the bool so the doctor can not walk away.
        movingDisabledForHandWashing = true;
        docAnim.Play("MWash");
        CreateBeerBubbles();

        // Suspend execution for 5 seconds
        yield return new WaitForSeconds(5);

        // Reset the bool and set the doctors target back to the sink. Incase the user clicked another 
        // location during hand washing.
        movingDisabledForHandWashing = false;
        MoveDocToTrigger(0);
        docAnim.Play("MIdle");

        // Reset all data back so game can continue
        RemoveSoapBubbles();
        DestroyWarningBubbleAtBed(0);
        needsToWashHands = false;
        handWashWarning = false;
        SetNewTimeBetweenHandWash(timeBetweenHandWashMin, timeBetweenHandWashMax);
        statusOfDoctor = "HEALTHY";
        statusOfDoctorText.text = statusOfDoctor;


    }

    public void CreateBeerBubbles()
    {
        // Create Beer Spay
        beerSpray = Instantiate(BeerSprayPrefab);
        var tempVector = new Vector3(-3.87f, .71f, 0f);
        beerSpray.transform.position = tempVector;

        // Create Beer Bubbles
        for (int i = 0; i < soapBubbles.Length; i++)
        {
            var randUp = Random.Range(-1f, 0f);
            var randRight = Random.Range(3f, 3.5f);
            tempSoapBubble = Instantiate(soapBubblePrefab);
            tempSoapBubble.transform.position = sink.transform.position + (Vector3.up * randUp) + (Vector3.right * randRight);
            soapBubbles[i] = tempSoapBubble;
        }
    }
     
    public void RemoveSoapBubbles()
    {
        Destroy(beerSpray);

        for (int i = 0; i < soapBubbles.Length; i++)
        {

            Destroy(soapBubbles[i]);
        }
    }

    #endregion


    #region Functions - Supply Quanity

    public void ClickOpenCloseSupplyPanel()
    {
        // Check to see if panel is closed. If it is, open it.
        if (supplyPanel.activeSelf)
        {
            supplyPanel.SetActive(false);
        }

        else supplyPanel.SetActive(true);
    }

    public void UpdateSupplyIntAndText(int supplyType, int quanity)
    {
        // supply type - 0: masks, 1: Ivs

        numberOfSupplies[supplyType] += quanity;

        for (int i = 0; i < quantityOfSuppliesText[supplyType].Length; i++)
            quantityOfSuppliesText[supplyType][i].text = numberOfSupplies[supplyType].ToString();
    }

    public void ClickBuySupplies(int supplyType)
    {
        // Check to see if player has money.
        if (currentMoney > costOfSupplies[supplyType])
        {
            spendMoney.Play();
            currentMoney -= costOfSupplies[supplyType];
            currentMoneyText.text = currentMoney.ToString();
            UpdateSupplyIntAndText(supplyType, 5);

            ClickOpenCloseSupplyPanel();
        }

        else
        {
            clickBad1.Play();
        }


    }

    #endregion


    #region Functions - Coin Animation

    public void moveCoinAnimation()
    {
        // Check to see if the coin is at (close to) the end location.
        // If it is then move the coin to a hidden location.
        if (coinAnimationActive)
        {
            if ((coinAnimation.transform.position - endCoinLocation).magnitude < errorCoinLocation.magnitude)
            {
                coinAnimation.transform.position = hideCoinLocation;
                coinAnimationActive = false;
            }

            else
            {
                coinAnimation.transform.position = Vector3.MoveTowards(coinAnimation.transform.position, endCoinLocation, 10f * Time.deltaTime);
            }
        }
        
        else if (dragonsWealthCoinAnimationActive)
        {
            if ((dragonsWealthCoinAnimation.transform.position - endCoinLocation).magnitude < errorCoinLocation.magnitude)
            {
                dragonsWealthCoinAnimationActive = false;
                dragonsWealthCoinAnimation.transform.position = hideCoinLocation;
            }

            else
            {
                dragonsWealthCoinAnimation.transform.position = Vector3.MoveTowards(dragonsWealthCoinAnimation.transform.position, endCoinLocation, 10f * Time.deltaTime);
            }
        }

    }


    #endregion


    #region Functions - Consumables

    public void LoadNewUpgradePanel(int upgrade)
    {
        // Display the new upgrade panel.
        newUpgradePanel.SetActive(true);

        // Load the correct information and picture.
        if (upgrade == 0)
        {
            newUpgradeInformationText.text = "Fizzy Drink";
            newUpgradeInformationImg.sprite = RedBullActiveSprite;
        }

        else if (upgrade == 1)
        {
            newUpgradeInformationText.text = "Feed the Orcs";
            newUpgradeInformationImg.sprite = OrcBanquetActiveSprite;
        }

        else
        {
            newUpgradeInformationText.text = "Dragons Wealth";
            newUpgradeInformationImg.sprite = DragonsWealthActiveSprite;
        }

    }

    public void ClickCloseNewUpgradePanel()
    {
        newUpgradePanel.SetActive(false);
    }

    public void ClickUseRedTroll()
    {
        // Check to see if the Red Troll is active - available for use.
        if (RedTrollUseImage.sprite == RedBullActiveSprite)
        {
            Debug.Log("Using Red Troll");

            // Set the Red Troll image to "In Active".
            RedTrollUseImage.sprite = RedBullInActiveSprite;

            // Set up the upgrade.
            doctorSpeed = 10;
            redTrollTimer = redTrollTimerLength;
            redTrollUpgradeActive = true;

            // If this is the first time this consumable has been used, we need to track that for achievements.
            if (consumablesUsedACH01[0] == false) consumablesUsedACH01[0] = true;

            // Check to see if all consumanles have been used for achievement unlocked.
            CheckForAchievement(1);

            useConsumable.Play();
        }

        else
        {
            Debug.Log("Need to buy more Red Troll");

            // Bring up the Buy Button.
            RedTrollBuyButton.SetActive(true);

        }
    }

    public void ClickBuyRedTroll()
    {

        if (currentMoney > 200)
        {
            currentMoney -= 200;

            // Set the Red Troll image to "In Active".
            RedTrollUseImage.sprite = RedBullActiveSprite;

            // Update current money text on screen.
            currentMoneyText.text = currentMoney.ToString();

            // Hide the Buy Button.
            RedTrollBuyButton.SetActive(false);
        }

        else
        {
            clickBad1.Play();

            // Hide the Buy Button.
            RedTrollBuyButton.SetActive(false);
        }

    }

    public void ClickUseOrcBanquet()
    {
        // Check to see if the OrcBanquet is active - available for use.
        if (OrcBanquetUseImage.sprite == OrcBanquetActiveSprite)
        {
            Debug.Log("Using OrcBanquet");
            useConsumable.Play();

            for (int i = 0; i < currentPatients.Length; i++)
            {
                if (currentPatients[i] != null)
                {
                    // Delete the patient and update stat.
                    Destroy(currentPatients[i]);
                    currentPatients[i] = null;
                    patientsHealed += 1;

                    // Update Patient Data and reset bed button
                    //UpdatePatientDataToScreen(doctorsCurrentBed);
                    SetNewPatientButtonsOnOff(i, true);

                    // Delete Warning Bubble if it is still there.
                    if (activeWarningBubbles[i + 1] != null)
                    {
                        DestroyWarningBubbleAtBed(i + 1);
                    }

                    // Destroy the physical patient image.
                    RemovePhysicalPatientFromBed(i);

                    // Player gets MONEY$$$$ - If the player has Dragons Wealth active then the 
                    if (dragonsWealthUpgradeActive)
                    {
                        currentMoney += 20;
                        goldEarnedInOneDay += 20;
                    }
                    else
                    {
                        currentMoney += 5;
                        goldEarnedInOneDay += 5;
                    }
                    currentMoneyText.text = currentMoney.ToString();

                    // Update the achievement tracker,
                    orcsServedInOneDayACH0203 += 1;
                }
            }

            // Set the Orc Banquet image to "In Active".
            OrcBanquetUseImage.sprite = OrcBanquetInActiveSprite;

            // If this is the first time this consumable has been used, we need to track that for achievements.
            if (consumablesUsedACH01[1] == false) consumablesUsedACH01[1] = true;

            // Check to see if all consumanles have been used for achievement unlocked.
            CheckForAchievement(1);

            // Check Achievements 2, 3, and 5 - 8;
            CheckForAchievement(2);
            CheckForAchievement(3);

            for (int i = 5; i < 9; i++)
            {
                CheckForAchievement(i);
            }
            
           
        }

        else
        {
            Debug.Log("Need to buy more OrcBanquet");

            // Bring up the Buy Button.
            OrcBanquetBuyButton.SetActive(true);

        }
    }

    public void ClickBuyOrcBanquet()
    {

        if (currentMoney > 200)
        {
            currentMoney -= 200;

            useConsumable.Play();

            // Set the OrcBanquet image to "In Active".
            OrcBanquetUseImage.sprite = OrcBanquetActiveSprite;

            // Update current money text on screen.
            currentMoneyText.text = currentMoney.ToString();

            // Hide the Buy Button.
            OrcBanquetBuyButton.SetActive(false);
        }

        else
        {
            clickBad1.Play();

            // Hide the Buy Button.
            OrcBanquetBuyButton.SetActive(false);
        }
    }

    public void ClickUseDragonsWealth()
    {
        // Check to see if the DragonsWealth is active - available for use.
        if (DragonsWealthUseImage.sprite == DragonsWealthActiveSprite)
        {
            Debug.Log("Using DragonsWealth");

            // Set the Dragons Wealth image to "In Active".
            DragonsWealthUseImage.sprite = DragonsWealthInActiveSprite;

            // Set up the upgrade.
            dragonsWealthTimer = dragonsWealthTimerLength;
            dragonsWealthUpgradeActive = true;

            // If this is the first time this consumable has been used, we need to track that for achievements.
            if (consumablesUsedACH01[2] == false) consumablesUsedACH01[2] = true;

            // Check to see if all consumanles have been used for achievement unlocked.
            CheckForAchievement(1);

        }

        else
        {
            Debug.Log("Need to buy more DragonsWealth");

            // Bring up the Buy Button.
            DragonsWealthBuyButton.SetActive(true);

        }
    }

    public void ClickBuyDragonsWealth()
    {

        if (currentMoney > 200)
        {
            currentMoney -= 200;

            // Set the DragonsWealth image to "In Active".
            DragonsWealthUseImage.sprite = DragonsWealthActiveSprite;

            // Update current money text on screen.
            currentMoneyText.text = currentMoney.ToString();

            // Hide the Buy Button.
            DragonsWealthBuyButton.SetActive(false);
        }

        else
        {
            clickBad1.Play();

            // Hide the Buy Button.
            DragonsWealthBuyButton.SetActive(false);
        }

    }

    float CalculateSliderValue(string nameOfUpgrade)
    {
        if (nameOfUpgrade == "RedTroll")
        {
            return (redTrollTimer / redTrollTimerLength);
        }

        else if (nameOfUpgrade == "DragonsWealth")
        {
            return (dragonsWealthTimer / dragonsWealthTimerLength);
        }

        else
        {
            Debug.Log("Upgrade was not names correctly when calling CalculateSliderValue");
            return 0f;
        }
    }

    #endregion


    #region Functions - Achievements

    public void LoadAchievementSprite(int achievementNumber)
    {
        if (achievementNumber < 16 && achievementNumber >= 0)
        {
            achievementImgs[achievementNumber].sprite = achievementSprites[achievementNumber];
        }

        else Debug.Log("Invalid Input function LoadAchievementSprite");
    }

    public void LoadAchievementSpritesAtStart()
    {
        for (int i = 0; i < 16; i++)
        {
            if (achievementsUnlocked[i]) LoadAchievementSprite(i);
        }
    }

    public void CheckForAchievement(int achievementNumber)
    {
        // There are 16 achievements all together. So if the input to this functions is 1 - 16 we will check 
        //to see of that acheivment has been met. If the input is 17, we will check all achievments. And if 
        //it is anything else we will print to debug the error.

        if (achievementNumber == 17)
        {

        }

        else if (achievementNumber == 1) //Use all consumables.
        {
            
            if (consumablesUsedACH01[0] && consumablesUsedACH01[1] && consumablesUsedACH01[2] && achievementsUnlocked[0] == false && achievementsUnlocked[0] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(0);
                achievementsUnlocked[0] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 2) //Serve 5 orces in 1 day.
        {
            if (orcsServedInOneDayACH0203 >= 5 && achievementsUnlocked[1] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(1);
                achievementsUnlocked[1] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 3) //Serve 10 orces in 1 day.
        {
            if (orcsServedInOneDayACH0203 >= 10 && achievementsUnlocked[2] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(2);
                achievementsUnlocked[2] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 4) //Refuse service to 5 orcs.
        {
            if (orcsRefusedACH04 >= 5 && achievementsUnlocked[3] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(3);
                achievementsUnlocked[3] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 5) //Earn 100 gold in 1 day.
        {
            if (achievementsUnlocked[4] == false && goldEarnedInOneDay >= 100)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(4);
                achievementsUnlocked[4] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 6) //Earn 200 gold in 1 day.
        {
            if (achievementsUnlocked[5] == false && goldEarnedInOneDay >= 200)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(5);
                achievementsUnlocked[5] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 7) //Earn 500 gold.
        {
            if (achievementsUnlocked[6] == false && currentMoney >= 500)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(6);
                achievementsUnlocked[6] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 8) //Earn 1000 gold.
        {
            if (achievementsUnlocked[7] == false && currentMoney >= 1000)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(7);
                achievementsUnlocked[7] = true;
                achievementPopPanel.SetActive(true);
            }

            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 9) //Max out the Lobby Size Upgrade.
        {
            if (upgradeLobbySizeCurrentValue == 10 && achievementsUnlocked[8] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(8);
                achievementsUnlocked[8] = true;
                achievementPopPanel.SetActive(true);
            }
            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 10) //Max out the Popularity Upgrade.
        {
            if (upgradePopularityCurrentValue == 10 && achievementsUnlocked[9] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(9);
                achievementsUnlocked[9] = true;
                achievementPopPanel.SetActive(true);
            }
            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 11) //Max out the Purse Strings Upgrade.
        {
            if (upgradePurseStringsCurrentValue == 10 && achievementsUnlocked[10] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(10);
                achievementsUnlocked[10] = true;
                achievementPopPanel.SetActive(true);
            }
            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 12) //Max out the Chemistry Upgrade.
        {
            if (upgradeChemistryCurrentValue == 10 && achievementsUnlocked[11] == false)
            {
                achievementUnlockedAudio.Play();
                LoadAchievementSprite(11);
                achievementsUnlocked[11] = true;
                achievementPopPanel.SetActive(true);
            }
            else Debug.Log("Checking for an achievment that is already unlocked or has not been met.");
        }

        else if (achievementNumber == 13) //TBA01
        {

        }

        else if (achievementNumber == 14) //TBA02
        {

        }

        else if (achievementNumber == 15) //TBA03
        {

        }

        else if (achievementNumber == 16) //TBA04
        {

        }

        else
        {
            Debug.Log("Invalid input for CheckAchievement function.");
        }
    }
    #endregion


    #region Functions - Tips&Ticks

    public void OpenCloseTipsPanel()
    {
        if (tipsAndTricksPanel.activeInHierarchy)
        {
            tipsAndTricksPanel.SetActive(false);
            gameIsPaused = false;
            PauseUnpausePatients(false);
            clickGood1.Play();
        }
        else
        {
            tipsAndTricksPanel.SetActive(true);
            consumablesCurrentStatText.text = "";
            foodCurrentStatText.text = "";
            upgradeCurrentStatText.text = "";
            gameIsPaused = true;
            PauseUnpausePatients(true);
            clickGood1.Play();
        }
    }

    public void OpenTipsSubPanel(string nameOfPanel)
    {
        if (nameOfPanel == "Consumables")
        {
            ConsumableSubPanel.SetActive(true);
            foodSubPanel.SetActive(false);
            upgradeSubPanel.SetActive(false);
            achievementSubPanel.SetActive(false);
            settingsSubPanel.SetActive(false);
            clickGood1.Play();
        }

        else if (nameOfPanel == "Food")
        {
            ConsumableSubPanel.SetActive(false);
            foodSubPanel.SetActive(true);
            upgradeSubPanel.SetActive(false);
            achievementSubPanel.SetActive(false);
            settingsSubPanel.SetActive(false);
            clickGood1.Play();
        }

        else if (nameOfPanel == "Upgrades")
        {
            ConsumableSubPanel.SetActive(false);
            foodSubPanel.SetActive(false);
            upgradeSubPanel.SetActive(true);
            achievementSubPanel.SetActive(false);
            settingsSubPanel.SetActive(false);
            clickGood1.Play();
        }

        else if (nameOfPanel == "Achievements")
        {
            ConsumableSubPanel.SetActive(false);
            foodSubPanel.SetActive(false);
            upgradeSubPanel.SetActive(false);
            achievementSubPanel.SetActive(true);
            settingsSubPanel.SetActive(false);
            clickGood1.Play();
        }

        else if (nameOfPanel == "Settings")
        {
            ConsumableSubPanel.SetActive(false);
            foodSubPanel.SetActive(false);
            upgradeSubPanel.SetActive(false);
            achievementSubPanel.SetActive(false);
            settingsSubPanel.SetActive(true);
            clickGood1.Play();
        }

        else Debug.Log("Invalid use of OpenTipsSubPanel");
    }

    public void ChangeSoundVolume(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void ChangeSFXVolume(float sliderValue)
    {
        mixer.SetFloat("SoundFXs", Mathf.Log10(sliderValue) * 20);
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
            musicSlider.value = 0f;
            SFXSlider.value = 0f;
            ChangeSoundVolume(musicSlider.value);
            ChangeSFXVolume(SFXSlider.value);
            isMuted = false;
        }

        else
        {
            musicSlider.value = 1f;
            SFXSlider.value = 1f;
            ChangeSoundVolume(musicSlider.value);
            ChangeSFXVolume(SFXSlider.value);
            isMuted = true;
        }
    }

    public void GetDescriptionForConsumables(int description)
    {
        // RedTroll - 1, OrcBanquet - 2, DragonsWealth - 3
        if (description == 1)
        {
            consumableName.text = "RED TROLL";
            consumableDescriptionText.text = "A yummy, sugary energy drink that will have you running circles around the Slop Hall!";
            consumablesCurrentStatText.text = "Red Troll's effects last " + redTrollTimerLength.ToString() + " seconds.";
            turnPage.Play();
        }

        else if (description == 2)
        {
            consumableName.text = "ORC BANQUET";
            consumableDescriptionText.text = "All seated Orcs will gobble their food, pay, and leave in less than 0.05 seconds! Amazing for turnaround times.";
            consumablesCurrentStatText.text = "This upgrade has no duration.";
            turnPage.Play();
        }

        else if (description == 3)
        {
            consumableName.text = "DRAON'S WEALTH";
            consumableDescriptionText.text = "Give me gold. Give me gold! GIVE ME GOLD! All customers pay 4 times the normal price for their meals.";
            consumablesCurrentStatText.text = "Dragons Wealth's effects last " + dragonsWealthTimerLength.ToString() + " seconds.";
            turnPage.Play();
        }

        else
        {
            Debug.Log("Invalid function call");
        }
    }

    public void GetDescriptionForFood(string nameOfFood)
    {
        if (nameOfFood == "Ale")
        {
            foodName.text = "Ale";
            foodDescriptionText.text = "Aggressively bold with hints of mushroom.";
            foodCurrentStatText.text = "Current Cost per 5 units: " + costOfSupplies[0].ToString() + " gold.";
            turnPage.Play();
        }

        else if (nameOfFood == "Classic Combo")
        {
            foodName.text = "Classic Combo";
            foodDescriptionText.text = "Nothing beats the Classic Combo!";
            foodCurrentStatText.text = "Current Cost per 5 units: " + costOfSupplies[1].ToString() + " gold.";
            turnPage.Play();
        }

        else if (nameOfFood == "The Hungry Orc")
        {
            foodName.text = "The Hungry Orc";
            foodDescriptionText.text = "Shove this down your gullet, it cures hunger!";
            foodCurrentStatText.text = "Current Cost per 5 units: " + costOfSupplies[2].ToString() + " gold.";
            turnPage.Play();
        }

        else if (nameOfFood == "Gut Buster Deluxe")
        {
            foodName.text = "Gut Buster Deluxe";
            foodDescriptionText.text = "Ever had your gut busted?";
            foodCurrentStatText.text = "Current Cost per 5 units: " + costOfSupplies[3].ToString() + " gold.";
            turnPage.Play();
        }

        else Debug.Log("Invalid use of GetDescriptionForFood");
    }

    public void GetDescriptionForUpgrade(string nameOfUpgrade)
    {
        if (nameOfUpgrade == "Upgrade01")
        {
            upgradeName.text = "Lobby Size";
            upgradeDescriptionText.text = "Hungry Orcs in large numbers spell trouble! If the lobby fills up the Orcs will overun the Slop Hall.\n\nINCREASE THE LOBBY SIZE BY 0NE.";
            upgradeCurrentStatText.text = "Current lobby size: " + waitingRoomFullLimit.ToString() + ".";
            turnPage.Play();
        }

        else if (nameOfUpgrade == "Upgrade02")
        {
            upgradeName.text = "Popularity";
            upgradeDescriptionText.text = "Rubbing elbows and shaking hands with influential Orcs is a great way to become Mr. Popular. \n\nINCREASE THE RATE OF CUSTOMERS.";
            upgradeCurrentStatText.text = "An orc will arrive to eat every: " + timeBetweenNewPatients.ToString() + " seconds.";
            turnPage.Play();
        }

        else if (nameOfUpgrade == "Upgrade03")
        {
            upgradeName.text = "Purse Strings";
            upgradeDescriptionText.text = "The frugal Orc spends less on ingredents.\n\nFOOD COST LESS TO PURCHASE.";
            upgradeCurrentStatText.text = "Food pricing varies, see FOOD for more information.";
            turnPage.Play();
        }

        else if (nameOfUpgrade == "Upgrade04")
        {
            upgradeName.text = "Chemistry";
            upgradeDescriptionText.text = "Recent advances in modern Orcish technology allowing you to do more with less.\n\nCONSUMABLES LAST LONGER.\nLENGTH OF DAY INCREASES";
            upgradeCurrentStatText.text = "Consumable durations vary, see CONSUMABLES for more information.";
            turnPage.Play();
        }

        else Debug.Log("Invalid use of GetDescriptionForUpgrade");
    }

    public void GetDescriptionForAchievements(int achievement)
    {
        if (achievement == 1) achievementDescription.text = "Use all consumables.";
        else if (achievement == 2) achievementDescription.text = "Serve 5 orces in 1 day.";
        else if (achievement == 3) achievementDescription.text = "Serve 10 orces in 1 day.";
        else if (achievement == 4) achievementDescription.text = "Refuse service to 5 orcs.";
        else if (achievement == 5) achievementDescription.text = "Earn 100 gold in 1 day.";
        else if (achievement == 6) achievementDescription.text = "Earn 200 gold in 1 day.";
        else if (achievement == 7) achievementDescription.text = "Earn 500 gold.";
        else if (achievement == 8) achievementDescription.text = "Earn 1000 gold.";
        else if (achievement == 9) achievementDescription.text = "Max out the Lobby Size Upgrade.";
        else if (achievement == 10) achievementDescription.text = "Max out the Popularity Upgrade.";
        else if (achievement == 11) achievementDescription.text = "Max out the Purse Strings Upgrade.";
        else if (achievement == 12) achievementDescription.text = "Max out the Chemistry Upgrade.";
        else if (achievement == 13) achievementDescription.text = "TBA01";
        else if (achievement == 14) achievementDescription.text = "TBA02";
        else if (achievement == 15) achievementDescription.text = "TBA03";
        else if (achievement == 16) achievementDescription.text = "TBA04";
        else Debug.Log("Invalid value used in GetDescriptionForAchievements");

        clickGood1.Play();
    }

    public void ChangeAudioTrack(string upOrDown)
    {
        // Inputs for this function should be "Up" or "Down"

        if (upOrDown == "Up")
        {
            if (currentTrackNumber == 3)
            {
                currentTrackNumber = 0;
                currentTrackNumberText.text = "01"; 
            }

            else
            {
                currentTrackNumber += 1;
                currentTrackNumberText.text = "0" + (currentTrackNumber + 1).ToString();
            }

            clickGood1.Play();
            musicSource.clip = musicClips[currentTrackNumber];
            musicSource.Play();
        }

        else if (upOrDown == "Down")
        {
            if (currentTrackNumber == 0)
            {
                currentTrackNumber = 3;
                currentTrackNumberText.text = "04";
            }

            else
            {
                currentTrackNumber -= 1;
                currentTrackNumberText.text = "0" + (currentTrackNumber + 1).ToString();
            }

            clickGood1.Play();
            musicSource.clip = musicClips[currentTrackNumber];
            musicSource.Play();
        }

        else Debug.Log("Input to function ChangeAudoTrack is invalid");
    }
    #endregion


    #region Functions - Upgrades

    public void OpenCloseUpgradePanel()
    {
        if (UpgradePanel.activeInHierarchy)
        {
            UpgradePanel.SetActive(false);
            gameIsPaused = false;
            PauseUnpausePatients(false);
            clickGood1.Play();
        }
        else
        {
            UpgradePanel.SetActive(true);
            gameIsPaused = true;
            PauseUnpausePatients(true);
            clickGood1.Play();
        }
    }

    public void IncreaseUpgrade(string nameOfUpgrade)
    {
        if (UpgradeGems >= 1)
        {
            clickGood1.Play();

            if (nameOfUpgrade == "Lobby Size" && upgradeLobbySizeCurrentValue < 10)
            {
                upgradeLobbySizeCurrentValue += 1;
                UpdateUpgradeValues();
                lobbyLimitText.text = waitingRoomFullLimit.ToString();
                UpgradeGems -= 1;
                UpdateStatsToScreen();
                CheckForAchievement(9);
            }

            else if (nameOfUpgrade == "Popularity" && upgradePopularityCurrentValue < 10)
            {
                upgradePopularityCurrentValue += 1;
                UpdateUpgradeValues();
                UpgradeGems -= 1;
                UpdateStatsToScreen();
                CheckForAchievement(10);
            }

            else if (nameOfUpgrade == "Purse Strings" && upgradePurseStringsCurrentValue < 10)
            {
                upgradePurseStringsCurrentValue += 1;
                UpdateUpgradeValues();
                UpgradeGems -= 1;
                UpdateStatsToScreen();
                CheckForAchievement(11);
            }

            else if (nameOfUpgrade == "Chemistry" && upgradeChemistryCurrentValue < 10)
            {
                upgradeChemistryCurrentValue += 1;
                lenghtOfDay += 20;
                UpdateUpgradeValues();
                UpgradeGems -= 1;
                UpdateStatsToScreen();
                CheckForAchievement(12);
            }

            else Debug.Log("Function IncreaseUpgrade has been called with invalid argument.");
        }

        else
        {
            clickBad1.Play();
        }
    }

    public void UpdateUpgradeBacklights()
    {
        // Set all backlights to off.
        for (int i = 0; i < 10; i++)
        {
            upgradeLobbySizeBacklights[i].SetActive(false);
            upgradePopularityBacklights[i].SetActive(false);
            upgradePurseStringsBacklights[i].SetActive(false);
            upgradeChemistryBacklights[i].SetActive(false);
        }
        // Set the correct backlights on.
         for (int i = 0; i < upgradeLobbySizeCurrentValue; i++)
        {
            upgradeLobbySizeBacklights[i].SetActive(true);
        }
         for (int i = 0; i < upgradePopularityCurrentValue; i++)
        {
            upgradePopularityBacklights[i].SetActive(true);
        }
         for (int i = 0; i < upgradePurseStringsCurrentValue; i++)
        {
            upgradePurseStringsBacklights[i].SetActive(true);
        }
         for (int i = 0; i < upgradeChemistryCurrentValue; i++)
        {
            upgradeChemistryBacklights[i].SetActive(true);
        }

    }

    public void UpdateUpgradeValues()
    {
        // Deals with the Lobby Size Upgrade.
        // Lobby size should always fall between 3 - 12
        #region Lobby Size Ifs
        if (upgradeLobbySizeCurrentValue == 1) waitingRoomFullLimit = 3;
        else if (upgradeLobbySizeCurrentValue == 2) waitingRoomFullLimit = 4;
        else if (upgradeLobbySizeCurrentValue == 3) waitingRoomFullLimit = 5;
        else if (upgradeLobbySizeCurrentValue == 4) waitingRoomFullLimit = 6;
        else if (upgradeLobbySizeCurrentValue == 5) waitingRoomFullLimit = 7;
        else if (upgradeLobbySizeCurrentValue == 6) waitingRoomFullLimit = 8;
        else if (upgradeLobbySizeCurrentValue == 7) waitingRoomFullLimit = 9;
        else if (upgradeLobbySizeCurrentValue == 8) waitingRoomFullLimit = 10;
        else if (upgradeLobbySizeCurrentValue == 9) waitingRoomFullLimit = 11;
        else if (upgradeLobbySizeCurrentValue == 10) waitingRoomFullLimit = 12;
        else Debug.Log("The Lobby size value is out of bounds");
        #endregion

        // Deals with the Popularity Upgrade.
        // timeBetweenNewPatients should always fall between 20 secs - 3secs
        #region Popularity Ifs
        if (upgradePopularityCurrentValue == 1) timeBetweenNewPatients = 20;
        else if (upgradePopularityCurrentValue == 2) timeBetweenNewPatients = 18;
        else if (upgradePopularityCurrentValue == 3) timeBetweenNewPatients = 16;
        else if (upgradePopularityCurrentValue == 4) timeBetweenNewPatients = 14;
        else if (upgradePopularityCurrentValue == 5) timeBetweenNewPatients = 12;
        else if (upgradePopularityCurrentValue == 6) timeBetweenNewPatients = 10;
        else if (upgradePopularityCurrentValue == 7) timeBetweenNewPatients = 8;
        else if (upgradePopularityCurrentValue == 8) timeBetweenNewPatients = 6;
        else if (upgradePopularityCurrentValue == 9) timeBetweenNewPatients = 4;
        else if (upgradePopularityCurrentValue == 10) timeBetweenNewPatients = 3;
        else Debug.Log("upgradePopularity is out of bounds");
        #endregion

        // Deals with the Purse Strings Upgrade.
        // Cost of supplies varies with level and type of food.
        // Ale 1 - 10;
        // Classic Combo 2 - 20;
        // The Hungry Orc 3 - 25;
        // Gut Buster Deluxe 4 - 30 
        #region  Purse Strings Ifs
        if (upgradePurseStringsCurrentValue == 1)
        {
            costOfSupplies[0] = 10;
            costOfSupplies[1] = 20;
            costOfSupplies[2] = 30;
            costOfSupplies[3] = 40;
        }

        else if (upgradePurseStringsCurrentValue == 2)
        {
            costOfSupplies[0] = 9;
            costOfSupplies[1] = 18;
            costOfSupplies[2] = 26;
            costOfSupplies[3] = 36;
        }

        else if (upgradePurseStringsCurrentValue == 3)
        {
            costOfSupplies[0] = 8;
            costOfSupplies[1] = 16;
            costOfSupplies[2] = 23;
            costOfSupplies[3] = 32;
        }

        else if (upgradePurseStringsCurrentValue == 4)
        {
            costOfSupplies[0] = 7;
            costOfSupplies[1] = 14;
            costOfSupplies[2] = 20;
            costOfSupplies[3] = 28;
        }

        else if (upgradePurseStringsCurrentValue == 5)
        {
            costOfSupplies[0] = 6;
            costOfSupplies[1] = 12;
            costOfSupplies[2] = 18;
            costOfSupplies[3] = 24;
        }

        else if (upgradePurseStringsCurrentValue == 6)
        {
            costOfSupplies[0] = 5;
            costOfSupplies[1] = 10;
            costOfSupplies[2] = 15;
            costOfSupplies[3] = 20;
        }

        else if (upgradePurseStringsCurrentValue == 7)
        {
            costOfSupplies[0] = 4;
            costOfSupplies[1] = 8;
            costOfSupplies[2] = 12;
            costOfSupplies[3] = 16;
        }

        else if (upgradePurseStringsCurrentValue == 8)
        {
            costOfSupplies[0] = 3;
            costOfSupplies[1] = 6;
            costOfSupplies[2] = 9;
            costOfSupplies[3] = 12;
        }

        else if (upgradePurseStringsCurrentValue == 9)
        {
            costOfSupplies[0] = 2;
            costOfSupplies[1] = 4;
            costOfSupplies[2] = 6;
            costOfSupplies[3] = 8;
        }

        else if (upgradePurseStringsCurrentValue == 10)
        {
            costOfSupplies[0] = 1;
            costOfSupplies[1] = 2;
            costOfSupplies[2] = 3;
            costOfSupplies[3] = 4;
        }
        else Debug.Log("Invalid arg for UpdateUpgradeValues Purse Stings");

        #endregion

        // Deals with the Chemistry Upgrade.
        // Length 10 - 100;
        #region  Chemistry Ifs
        redTrollTimerLength = upgradeChemistryCurrentValue * 10;
        dragonsWealthTimerLength = upgradeChemistryCurrentValue * 10;
        #endregion


        UpdateUpgradeBacklights();

    }

    #endregion


    #region Functions - EndOfDay

    public void OpenCloseEndOfDayPanel(bool open)
    {
        if (open)
        {
            EndOFDayPanel.SetActive(true);
            gameIsPaused = true;
            numberOFGemsText.text = upgradeGemRate.ToString() + " Gems Received!";

            // Remove the ORCS that are still in the Slop Hall.
            for (int i = 0; i < currentPatients.Length; i++)
            {
                if (currentPatients[i] != null)
                {
                    // Delete the images of food at the table.
                    if(foodOnTableImgs[i] != null)
                    {
                        foodOnTableImgs[i].GetComponent<SpriteRenderer>().sprite = null;
                    }

                    // Delete the patient and update stat.
                    Destroy(currentPatients[i]);
                    currentPatients[i] = null;
                    patientsHealed += 1;

                    
                    

                    // Update Patient Data and reset bed button
                    //UpdatePatientDataToScreen(doctorsCurrentBed);
                    SetNewPatientButtonsOnOff(i, true);

                    // Delete Warning Bubble if it is still there.
                    if (activeWarningBubbles[i + 1] != null)
                    {
                        DestroyWarningBubbleAtBed(i + 1);
                    }

                    // Destroy the physical patient image.
                    RemovePhysicalPatientFromBed(i);

                    // Player gets MONEY$$$$ - If the player has Dragons Wealth active then the 
                    if (dragonsWealthUpgradeActive)
                    {
                        currentMoney += 20;
                    }
                    else
                    {
                        currentMoney += 5;
                    }
                    currentMoneyText.text = currentMoney.ToString();
                }
            }
        }

        else
        {
            EndOFDayPanel.SetActive(false);
            gameIsPaused = false;
        }
    }

    #endregion

    #endregion
}
