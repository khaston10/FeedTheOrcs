using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientData : MonoBehaviour
{
    #region Variables - BasicInformation
    public string nameOfPatient;
    public int ageOfPatient;
    public string sexOfPatient;
    public string statusOfPatient;
    public Sprite picOfPatient;

    public float patientTimer;
    public bool needsTreatment;
    public float treatmentTimer;
    private float timeBetweenNeedForTreatment;
    private int bedAssigned;
    public int currentWarningMessageIndex;
    private int randomIndex;

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        // Populate all the basic information for the patient
        LoadPatientData();

        // Set time until patient needs treatment, currently set at 10 - 30 seconds.
        timeBetweenNeedForTreatment = Random.Range(10, 30);

        
    }

    private void Start()
    {
        bedAssigned = System.Array.IndexOf(GameObject.Find("MainController").GetComponent<Main>().currentPatients, gameObject);

        // Create New Warning Bubble at start.
        currentWarningMessageIndex = System.Array.IndexOf(GlobalPatientData.statusOfPatients, statusOfPatient); // 0 - Healthy, 1 - Infected, ... 5 - Deceased

        if (currentWarningMessageIndex != 0 && currentWarningMessageIndex != 5)
        {
            GameObject.Find("MainController").GetComponent<Main>().CreateWarningBubbleAtBed(bedAssigned + 1, currentWarningMessageIndex);
            needsTreatment = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        patientTimer += Time.deltaTime;

        
        // Check to see if the patient needs to be updated.
        if (patientTimer > timeBetweenNeedForTreatment)
        {

            // Deal with Case: 1 Patient is getting Healthier, Case 2: Patient is getting worse.
            if (needsTreatment == false)
            {
                // Update Patient Status.
                int tempIndex = System.Array.IndexOf(GlobalPatientData.statusOfPatients, statusOfPatient);

                if (tempIndex != 0)
                {
                    statusOfPatient = GlobalPatientData.statusOfPatients[tempIndex - 1];
                }

                // Update patient data on screen.
                GameObject.Find("MainController").GetComponent<Main>().UpdatePatientDataToScreen(System.Array.IndexOf(GameObject.Find("MainController").GetComponent<Main>().currentPatients, gameObject));
            }

            else
            {

                // Update Patient Status.
                int tempIndex = System.Array.IndexOf(GlobalPatientData.statusOfPatients, statusOfPatient);

                if (tempIndex != GlobalPatientData.statusOfPatients.Length - 1)
                {
                    statusOfPatient = GlobalPatientData.statusOfPatients[tempIndex + 1];
                }

                // Update patient data on screen.
                GameObject.Find("MainController").GetComponent<Main>().UpdatePatientDataToScreen(bedAssigned);
            }

            // Create New Warning Bubble if needed and set needsTreatment if needed.
            currentWarningMessageIndex = System.Array.IndexOf(GlobalPatientData.statusOfPatients, statusOfPatient); // 0 - Healthy, 1 - Infected, ... 5 - Deceased

            if (currentWarningMessageIndex != 0 && currentWarningMessageIndex != 5)
            {
                //GameObject.Find("MainController").GetComponent<Main>().CreateWarningBubbleAtBed(bedAssigned + 1, currentWarningMessageIndex);
                needsTreatment = true;
            }

            else if (currentWarningMessageIndex == 0)
            {
                
            }

            else if (currentWarningMessageIndex == 5)
            {
                
            }


            // Delete Current Warning Bubble, if it exists.
            if (GameObject.Find("MainController").GetComponent<Main>().activeWarningBubbles[bedAssigned + 1] != null){
                GameObject.Find("MainController").GetComponent<Main>().DestroyWarningBubbleAtBed(bedAssigned + 1);
            }

            // Create next Warning Bubble
            GameObject.Find("MainController").GetComponent<Main>().CreateWarningBubbleAtBed(bedAssigned + 1, currentWarningMessageIndex);

            // Check to see if doctor is at the bed and the status us Healthy or Deceased.
            // If this is the case then we need to set the discharge panel to active.
            if (GameObject.Find("MainController").GetComponent<Main>().doctorsCurrentBed == bedAssigned && (statusOfPatient == "HEALTHY"  || statusOfPatient == "DECEASED"))
            {
                GameObject.Find("MainController").GetComponent<Main>().DischargePanel.SetActive(true);
            }

            // Reset Patient Timer and need for treatment.
            patientTimer = 0;
            
        }
        
    }

    #region Functions
    public void LoadPatientData()
    {
        randomIndex = Random.Range(0, 12);
        nameOfPatient = GlobalPatientData.namesOfPatients[randomIndex];
        ageOfPatient = GlobalPatientData.agesOfPatients[randomIndex];
        sexOfPatient = GlobalPatientData.sexesOfPatients[randomIndex];
        statusOfPatient = GlobalPatientData.statusOfPatients[Random.Range(1, 4)];

        if(sexOfPatient == "F")
        {
            if (ageOfPatient < 22) picOfPatient = GlobalPatientData.picOfPatient[Random.Range(0, 2)];
            else if (ageOfPatient < 45) picOfPatient = GlobalPatientData.picOfPatient[Random.Range(2, 4)];
            else picOfPatient = GlobalPatientData.picOfPatient[Random.Range(4, 6)];
        }
        else
        {
            if (ageOfPatient < 22) picOfPatient = GlobalPatientData.picOfPatient[Random.Range(6, 8)];
            else if (ageOfPatient < 45) picOfPatient = GlobalPatientData.picOfPatient[Random.Range(8, 10)];
            else picOfPatient = GlobalPatientData.picOfPatient[Random.Range(10, 12)];
        }
    }
    #endregion
}
