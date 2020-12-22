using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

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
    public Text numberOfPatientsHealed;
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
    private bool orcBanquetUpgradeActive;
    private int dragonsWealthTimerLength = 10;
    private float dragonsWealthTimer;
    private bool dragonsWealthUpgradeActive;
    public Slider redTrollSlider;
    public Slider dragonsWealthSlider;


    #endregion

    #region Variables - Upgrades
    public GameObject UpgradePanel;
    private int UpgradeGems;
    public Text upgradeGemsText;

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
    public AudioSource musicSource;
    public AudioSource clickGood1;
    public AudioSource clickGood2;
    public AudioSource clickBad1;
    public AudioSource warning1;
    public AudioSource serverDrink;
    public AudioSource unhappy;
    public AudioClip[] unhappyClips;
    public AudioSource happy;
    public AudioClip[] happyClips;
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

    // Variables to keep each day moving.
    public float mainTimer;
    public float newPatientTimer;
    public int lenghtOfDay; // In seconds.

    private bool gameIsPaused;
    #endregion

    #region Variables - Tips&Tricks
    public GameObject tipsAndTricksPanel;
    public GameObject ConsumableSubPanel;
    public GameObject foodSubPanel;
    public GameObject upgradeSubPanel;
    public Text consumableName;
    public Text foodName;
    public Text upgradeName;
    public Text consumableDescriptionText;
    public Text foodDescriptionText;
    public Text upgradeDescriptionText;

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
        numberOfPatientsHealed.text = "0";
        lobbyLimitText.text = "/ " + waitingRoomFullLimit.ToString();
        currentMoneyText.text = currentMoney.ToString();
        upgradeGemsText.text = UpgradeGems.ToString();

        // Load Doctor Data to screen.
        LoadDoctorsPage();

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
            newPatientTimer += Time.deltaTime;
            handWashingTimer += Time.deltaTime;

            // New Patient Update
            if (newPatientTimer > timeBetweenNewPatients)
            {
                numberOfNewPatients += 1;
                UpdateStatsToScreen();

                // Check to see if waiting room is full - GAMEOVER
                if (numberOfNewPatients > waitingRoomFullLimit)
                {
                    SaveData();
                    SceneManager.LoadScene(2);
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
                SceneManager.LoadScene(2);
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
                UpdateStatsToScreen();
                mainTimer = 0;
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

            // Check to see if it is time to make the next upgrade available.
            if (patientsHealed == 2 && RedTrollUseButton.activeInHierarchy == false)
            {
                RedTrollUseButton.SetActive(true);
                LoadNewUpgradePanel(0);
            }

            else if (patientsHealed == 4 && OrcBanquetUseButton.activeInHierarchy == false)
            {
                OrcBanquetUseButton.SetActive(true);
                LoadNewUpgradePanel(1);
            }

            else if (patientsHealed == 6 && DragonsWealthUseButton.activeInHierarchy == false)
            {
                DragonsWealthUseButton.SetActive(true);
                LoadNewUpgradePanel(2);
            }

        }
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
        spriteOfDoctor = GlobalCont.Instance.spriteOfDoctor;
        gameDifficulty = GlobalCont.Instance.gameDifficulty;
        GlobalCont.Instance.gameDifficulty = gameDifficulty;
        highScores = GlobalCont.Instance.highScores;

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
    }

    public void LoadHighScoreFromPlayerPrefs()
    {
        Debug.Log("Loading High Scores from Player Prefs ...");

        // Load data if it exists.
        highScores[0] = PlayerPrefs.GetInt("HighScore01", 0);
        highScores[1] = PlayerPrefs.GetInt("HighScore02", 0);
        highScores[2] = PlayerPrefs.GetInt("HighScore03", 0);
    }

    public void LoadGameDifficulty()
    {
        // This function will need to be re worked once upgrades are implementaed.
            timeBetweenNewPatients = 10;
            waitingRoomFullLimit = 5;
            timeDoctorHasToWashHands = 30;
            timeBetweenHandWashMin = 20;
            timeBetweenHandWashMax = 120;
            warningTime = 10;
            currentMoney = 50;
            UpgradeGems = 2;

    }

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

            //Play a random happy audio clip.
            var randAudio = Random.Range(0, happyClips.Length);
            happy.clip = happyClips[randAudio];
            happy.Play();

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
            }
            else
            {
                currentMoney += 5;
            }
            currentMoneyText.text = currentMoney.ToString();

            // Play Click Sound
            clickGood1.Play();

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
        numberOfPatientsHealed.text = patientsHealed.ToString();
        lobbyLimitText.text = "/ " + waitingRoomFullLimit.ToString();
        upgradeGemsText.text = UpgradeGems.ToString();
    }


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


    //public void UpdatePatientDataToScreen(int bed)
    //{
    //    if (currentPatients[bed] != null)
    //    {
    //        nameOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().nameOfPatient;
    //        ageOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().ageOfPatient.ToString();
    //        sexOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().sexOfPatient;
    //        statusOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().statusOfPatient;
    //        picOfPatientImages[bed].sprite = currentPatients[bed].GetComponent<PatientData>().picOfPatient;
    //    }

    //    else
    //    {
    //        nameOfPatientsText[bed].text = "";
    //        ageOfPatientsText[bed].text = "";
    //        sexOfPatientsText[bed].text = "";
    //        statusOfPatientsText[bed].text = "";
    //        picOfPatientImages[bed].sprite = blankImage;
    //    }
    //}

     
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

        else clickBad1.Play();
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
            clickGood1.Play();
            currentMoney -= costOfSupplies[supplyType];
            currentMoneyText.text = currentMoney.ToString();
            UpdateSupplyIntAndText(supplyType, 10);

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

        if (currentMoney > 20)
        {
            currentMoney -= 20;

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

            for(int i = 0; i < currentPatients.Length; i++)
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
                    }
                    else
                    {
                        currentMoney += 5;
                    }
                    currentMoneyText.text = currentMoney.ToString();
                }
            }

            // Set the Orc Banquet image to "In Active".
            OrcBanquetUseImage.sprite = OrcBanquetInActiveSprite;


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

        if (currentMoney > 20)
        {
            currentMoney -= 20;

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

        if (currentMoney > 20)
        {
            currentMoney -= 20;

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


    #region Functions - Tips&Ticks

    public void OpenCloseTipsPanel()
    {
        if (tipsAndTricksPanel.activeInHierarchy)
        {
            tipsAndTricksPanel.SetActive(false);
            gameIsPaused = false;
            PauseUnpausePatients(false);
        }
        else
        {
            tipsAndTricksPanel.SetActive(true);
            gameIsPaused = true;
            PauseUnpausePatients(true);
        }
    }

    public void OpenTipsSubPanel(string nameOfPanel)
    {
        if (nameOfPanel == "Consumables")
        {
            ConsumableSubPanel.SetActive(true);
            foodSubPanel.SetActive(false);
            upgradeSubPanel.SetActive(false);
        }

        else if (nameOfPanel == "Food")
        {
            ConsumableSubPanel.SetActive(false);
            foodSubPanel.SetActive(true);
            upgradeSubPanel.SetActive(false);
        }

        else if (nameOfPanel == "Upgrades")
        {
            ConsumableSubPanel.SetActive(false);
            foodSubPanel.SetActive(false);
            upgradeSubPanel.SetActive(true);
        }

        else Debug.Log("Invalid use of OpenTipsSubPanel");
    }

    public void GetDescriptionForConsumables(int description)
    {
        // RedTroll - 1, OrcBanquet - 2, DragonsWealth - 3
        if (description == 1)
        {
            consumableName.text = "RED TROLL";
            consumableDescriptionText.text = "Yummy Energy Drink. Yummy Energy Drink. Yummy Energy Drink. Yummy Energy Drink. Yummy Energy Drink.";
        }

        else if (description == 2)
        {
            consumableName.text = "ORC BANQUET";
            consumableDescriptionText.text = "Let's throw a party!";
        }

        else if (description == 3)
        {
            consumableName.text = "DRAON'S WEALTH";
            consumableDescriptionText.text = "Give me money. Give me money. Give me money. Give me money. Give me money.";
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
            foodDescriptionText.text = "Aggressively bold, .... ";
        }

        else if (nameOfFood == "Classic Combo")
        {
            foodName.text = "Classic Combo";
            foodDescriptionText.text = "Aggressively bo, .... ";
        }

        else if (nameOfFood == "The Hungry Orc")
        {
            foodName.text = "The Hungry Orc";
            foodDescriptionText.text = "Aggressively , .... ";
        }

        else if (nameOfFood == "Gut Buster Deluxe")
        {
            foodName.text = "Gut Buster Deluxe";
            foodDescriptionText.text = "The Gut Buster Deluxe, .... ";
        }

        else Debug.Log("Invalid use of GetDescriptionForFood");
    }

    public void GetDescriptionForUpgrade(string nameOfUpgrade)
    {
        if (nameOfUpgrade == "Upgrade01")
        {
            upgradeName.text = "Lobby Size";
            upgradeDescriptionText.text = "If the lobby fills full of hungry Orcs, they will overrun the diner.\nIncrease the size of the lobby.";
        }

        else if (nameOfUpgrade == "Upgrade02")
        {
            upgradeName.text = "Popularity";
            upgradeDescriptionText.text = "Increase Jughog's Diner popularity within the Orc community.\nMore Orcs will come.";
        }

        else if (nameOfUpgrade == "Upgrade03")
        {
            upgradeName.text = "Frugality";
            upgradeDescriptionText.text = "The frugal Orc spends less on ingredents.\nFood costs less to make.";
        }

        else if (nameOfUpgrade == "Upgrade04")
        {
            upgradeName.text = "Efficiency";
            upgradeDescriptionText.text = "Efficient, Efficient, Efficient\nTimed consumables will last longer.";
        }

        else Debug.Log("Invalid use of GetDescriptionForUpgrade");
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
        }
        else
        {
            UpgradePanel.SetActive(true);
            gameIsPaused = true;
            PauseUnpausePatients(true);
        }
    }

    #endregion

    #endregion
}
