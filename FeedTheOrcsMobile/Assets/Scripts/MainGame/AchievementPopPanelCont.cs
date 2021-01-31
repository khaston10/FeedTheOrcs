using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPopPanelCont : MonoBehaviour
{
    #region Variables

    private float timeUntilPanelDisapears = 4f;
    private float timer;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the timer so it can make it self disapear.
        timer += Time.deltaTime;

        if (timer >= timeUntilPanelDisapears)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }
    }
}
