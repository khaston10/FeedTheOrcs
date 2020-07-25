using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    #region Variables - Panels and Buttons

    public Button[] newPatientButtons;
    public Button[] patientPanelButtons;
    public GameObject[] patientPanels;
    public GameObject DischargePanel;

    #endregion

    #region Variables - Text objects to update
    public Text dayText;
    public Text numberOfNewPatientsText;
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

    public int waitingRoomFullLimit; // How many patients can be in the waiting room before game over.

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

    // Handles soap animation.
    public GameObject soapBubblePrefab;
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

#region Audio Sources
public AudioSource musicSource;
    public AudioSource clickGood1;
    public AudioSource clickGood2;
    public AudioSource clickBad1;
    public AudioSource warning1;
    #endregion

    #region Variables - Stats
    public int day;
    public int patientsHealed;
    public int patientsDeceased;
    public int gameDifficulty;
    public int numberOfNewPatients;
    public int timeBetweenNewPatients; // In seconds

    // Variables to keep each day moving.
    public float mainTimer;
    public float newPatientTimer;
    public int lenghtOfDay; // In seconds.
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
        dayText.text = "1";
        numberOfNewPatientsText.text = "0";
        currentMoneyText.text = currentMoney.ToString();

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
            ClearPatientDataFromScreen(i);
        }
        SetBedButtonsOnOff(6, false);

        // Create Doctor from Prefab that matches.
        if (nameOfDoctor == "Nate") doctor = Instantiate(doctorPrefab1);
        else if (nameOfDoctor == "Ariko") doctor = Instantiate(doctorPrefab2);
        else if (nameOfDoctor == "Nataly") doctor = Instantiate(doctorPrefab3);
        else doctor = Instantiate(doctorPrefab4);
        doctor.transform.position = Vector3.zero;

        //Initialize Doctor Animator.
        docAnim = doctor.GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
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
        if(newPatientTimer > timeBetweenNewPatients)
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
            statusOfDoctor = "CONTAMINATED";
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
            statusOfDoctor = GlobalPatientData.statusOfPatients[1];
            SaveData();
            SceneManager.LoadScene(2);
        }




        // End Of Day Tasks
        if(mainTimer > lenghtOfDay)
        {
            // Reset Timer, Advance Day, and Update Stats On Screen.
            day += 1;
            UpdateStatsToScreen();
            mainTimer = 0;
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

    }

    public void LoadGameDifficulty()
    {
        if (gameDifficulty == 0)
        {
            timeBetweenNewPatients = 10;
            waitingRoomFullLimit = 10;
            timeDoctorHasToWashHands = 30;
            timeBetweenHandWashMin = 20;
            timeBetweenHandWashMax = 120;
            warningTime = 10;
            currentMoney = 50;
        }

        else if (gameDifficulty == 1)
        {
            timeBetweenNewPatients = 8;
            waitingRoomFullLimit = 10;
            timeDoctorHasToWashHands = 20;
            timeBetweenHandWashMin = 20;
            timeBetweenHandWashMax = 60;
            warningTime = 8;
            currentMoney = 20;
        }

        else
        {
            timeBetweenNewPatients = 6;
            waitingRoomFullLimit = 10;
            timeDoctorHasToWashHands = 15;
            timeBetweenHandWashMin = 20;
            timeBetweenHandWashMax = 40;
            warningTime = 5;
            currentMoney = 0;
        }
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

    public void ClickDischargePanelDischarge(int location)
    {

        if (location == 1)
        {
            // Check to see if patient is healthy.
            if (currentPatients[doctorsCurrentBed].GetComponent<PatientData>().statusOfPatient == "HEALTHY")
            {
                // Delete the patient and update stat.
                Destroy(currentPatients[doctorsCurrentBed]);
                currentPatients[doctorsCurrentBed] = null;
                patientsHealed += 1;

                // Update Patient Data and reset bed button
                UpdatePatientDataToScreen(doctorsCurrentBed);
                SetNewPatientButtonsOnOff(doctorsCurrentBed, true);

                // Delete Warning Bubble if it is still there.
                if (activeWarningBubbles[doctorsCurrentBed + 1] != null)
                {
                    DestroyWarningBubbleAtBed(doctorsCurrentBed + 1);
                }

                // Destroy the physical patient image.
                RemovePhysicalPatientFromBed(doctorsCurrentBed);

                // Player gets MONEY$$$$
                currentMoney += 5;
                currentMoneyText.text = currentMoney.ToString();

                // Play Click Sound
                clickGood1.Play();

                // Hide Panel
                HideDischargePanel();
            }

            else
            {
                Debug.Log("Patient is not healthy");
                clickBad1.Play();
            }
        }

        else if (location == 2)
        {
            // Check to see if patient is dead.
            if (currentPatients[doctorsCurrentBed].GetComponent<PatientData>().statusOfPatient == "DECEASED")
            {
                // Delete the patient and update stat.
                Destroy(currentPatients[doctorsCurrentBed]);
                currentPatients[doctorsCurrentBed] = null;
                patientsDeceased += 1;

                // Update Patient Data and reset bed button
                UpdatePatientDataToScreen(doctorsCurrentBed);
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

                // Hide Panel
                HideDischargePanel();
            }

            else
            {
                Debug.Log("Patient is not dead");
                // Play Click Sound
                clickBad1.Play();
            }
        }

        


        
    }

    public void UpdateStatsToScreen()
    {
        // Currently only for Day and New Patients.
        numberOfNewPatientsText.text = numberOfNewPatients.ToString();
        dayText.text = day.ToString();
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
            bool providerLoaded = CreatePatientAndPopulateBed(bed);
            UpdatePatientDataToScreen(bed);

            if (providerLoaded) SetNewPatientButtonsOnOff(bed, false);

            numberOfNewPatients -= 1;
            // Update Text object on screen.
            UpdateStatsToScreen();

            // Place the image of the patient at the bed.
            PlacePhysicalPatientOnBed(bed);

            // Play Click Sound
            clickGood1.Play();
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
        if (currentPatients[bed] == null)
        {
            GameObject temp = Instantiate(patientPrefab);
            currentPatients[bed] = temp;
            return true;
        }

        else
        {
            Debug.Log("Bed Is Taken");
            return false;
        }
    }

    public void UpdatePatientDataToScreen(int bed)
    {
        if (currentPatients[bed] != null)
        {
            nameOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().nameOfPatient;
            ageOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().ageOfPatient.ToString();
            sexOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().sexOfPatient;
            statusOfPatientsText[bed].text = currentPatients[bed].GetComponent<PatientData>().statusOfPatient;
            picOfPatientImages[bed].sprite = currentPatients[bed].GetComponent<PatientData>().picOfPatient;
        }

        else
        {
            nameOfPatientsText[bed].text = "";
            ageOfPatientsText[bed].text = "";
            sexOfPatientsText[bed].text = "";
            statusOfPatientsText[bed].text = "";
            picOfPatientImages[bed].sprite = blankImage;
        }
    }

    public void ClearPatientDataFromScreen(int bed)
    {
      
        nameOfPatientsText[bed].text = "";
        ageOfPatientsText[bed].text = "";
        sexOfPatientsText[bed].text = "";
        statusOfPatientsText[bed].text = "";
        picOfPatientImages[bed].sprite = blankImage;


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

    }

    public void DestroyWarningBubbleAtBed(int bed)
    {
        Destroy(activeWarningBubbles[bed]);
        activeWarningBubbles[bed] = null;
    }

    public void PlacePhysicalPatientOnBed(int bed)
    {
        // Pic a physical patient image depending on the age and gender.
        var indexTemp = System.Array.IndexOf(GlobalPatientData.picOfPatient, currentPatients[bed].GetComponent<PatientData>().picOfPatient);
        physicalPatientTemp = Instantiate(physicalPatientPrefabs[indexTemp]);
        activePhysicalPatients[bed] = physicalPatientTemp;

        // Place patient on bed.
        physicalPatientTemp.transform.position = physicalBeds[bed].transform.position + (Vector3.up * .6f);
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
        // Play the animation and switch the bool so the doctor can not walk away.
        movingDisabledForHandWashing = true;
        docAnim.Play("MWash");
        CreateSoapBubbles();

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

    public void CreateSoapBubbles()
    { 
        for (int i = 0; i < soapBubbles.Length; i++)
        {
            var randUp = Random.Range(-.4f, .6f);
            var randRight = Random.Range(-.1f, .1f);
            tempSoapBubble = Instantiate(soapBubblePrefab);
            tempSoapBubble.transform.position = sink.transform.position + (Vector3.up * randUp) + (Vector3.right * randRight);
            soapBubbles[i] = tempSoapBubble;
        }
    }
     
    public void RemoveSoapBubbles()
    {
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

    #endregion
}
