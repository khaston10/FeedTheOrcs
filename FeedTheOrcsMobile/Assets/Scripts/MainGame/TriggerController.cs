using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    // Trigger Number 0- Sink, 1 - Bed1, ...
    public int triggerNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        // Check to see if the doctor hit the correct trigger.
        if(Vector3.Distance(GameObject.Find("MainController").GetComponent<Main>().targetPos, transform.position) < .2)
        {
            // Play The Doctors Idle Animation.
            GameObject.Find("MainController").GetComponent<Main>().docAnim.Play("MIdle");

            // If patient is healthy or dead and doctor enters trigger, display the Discharge Panel.
            if (triggerNum != 0 && GameObject.Find("MainController").GetComponent<Main>().currentPatients[triggerNum - 1] != null &&
                (GameObject.Find("MainController").GetComponent<Main>().currentPatients[triggerNum - 1].GetComponent<PatientData>().statusOfPatient == "HEALTHY" ||
                GameObject.Find("MainController").GetComponent<Main>().currentPatients[triggerNum - 1].GetComponent<PatientData>().statusOfPatient == "DECEASED"))
            {
                GameObject.Find("MainController").GetComponent<Main>().DischargePanel.SetActive(true);
            }

        }

        // Update Current Patient Buttons if Doctor is on trigger.
        if (triggerNum != 0)
        {
            GameObject.Find("MainController").GetComponent<Main>().SetBedButtonsOnOff(triggerNum - 1, true);
        }
        else if (triggerNum == 0) GameObject.Find("MainController").GetComponent<Main>().SetBedButtonsOnOff(6, true);

        // Update the doctors current bed.
        GameObject.Find("MainController").GetComponent<Main>().doctorsCurrentBed = triggerNum - 1;

        
        
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Update Current Patient Buttons if Doctor is leaving trigger.
        if (triggerNum != 0)
        {
            GameObject.Find("MainController").GetComponent<Main>().SetBedButtonsOnOff(triggerNum - 1, false);
        }
        else if (triggerNum == 0) GameObject.Find("MainController").GetComponent<Main>().SetBedButtonsOnOff(6, false);

        // If the Discharge panel is active and the doctor walks away. We want to set it to inactive.
        if (GameObject.Find("MainController").GetComponent<Main>().DischargePanel){
            GameObject.Find("MainController").GetComponent<Main>().DischargePanel.SetActive(false);
        }
    }

 
}
