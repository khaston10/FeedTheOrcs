using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCont : MonoBehaviour
{

    public static GlobalCont Instance;

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
    public int[] highScores;
    public bool newHighScore;
    public int wealth;
    public int waitingRoomFullLimit;
    public int numberOfNewPatients;
    public bool playerHasRunOutOfSuppliesAndMoney;
    public bool isMuted;
    public bool[] achievementsUnlocked;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
