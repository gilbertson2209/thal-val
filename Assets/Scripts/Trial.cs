using System.Collections;
using UnityEngine;
using System;

public class Trial : MonoBehaviour
{
    public GameManager gameManager;
    public TaskSettings taskSettings;
    public GameObject allBandits;
    public AllBanditsManager allBanditsManager;

    public GameObject failedTrial;
    public GameObject fixationCross;

    public bool inTrial = false;
    public bool choiceMade = false;
    public bool animate = false;
    public bool showReward = false;

    private int[] payoffs; 

    public Bandits chosenBandit;
    public float timeElapsed = 0f;
    public int reward; 




    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        taskSettings = GetComponent<TaskSettings>();
        allBanditsManager = allBandits.GetComponent<AllBanditsManager>();
    }

    public void NewTrial(int[] trialPayoffs)
    {
        Debug.Log("In New Trial");
        this.payoffs = trialPayoffs;
        timeElapsed = 0f;
        reward = 0;
        choiceMade = false;
        inTrial = true;
        ShowBandits();

    }

    private void ShowBandits()
    {
        allBanditsManager.BlockInput(false);
        allBanditsManager.ActivateBandits(true);
    }

    private void ShowFailTrial()
    {
        fixationCross.SetActive(false);
        failedTrial.SetActive(true);
        allBanditsManager.BlockInput(true);
    }

    IEnumerator FailTrial()
    {
        inTrial = false;
        showReward = false;
        ShowFailTrial();
        yield return new WaitForSeconds(taskSettings.failTrialDisplayTime);
        
    }

    public void EndTrial()
    {
        choiceMade = false;
        //write Data 
        // back to GM to show Intertrail 
    }

    public void Update()
    {
        // only keep count of elapsed time if inTrial AND no choicemade
        if (inTrial && !choiceMade)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= taskSettings.trialTimeLimit)
            {
                StartCoroutine(FailTrial());
            }
        }
    }

    public void OnChoice(string banditName)
    // called from the 'OnChoice' script attached to each bandits
    {
        inTrial = false;
        choiceMade = true;
        fixationCross.SetActive(false);
        Enum.TryParse(banditName, out chosenBandit);
        allBanditsManager.BlockInput(true);
        StartCoroutine(AnimateAndDisplay());
    }

    IEnumerator AnimateAndDisplay()
    {
        int bandit = (int)chosenBandit;
        reward = payoffs[bandit];

        animate = true;  // bandit's OnChoice will catch this flag & change appearance
        yield return new WaitForSeconds(taskSettings.animateTime);
        animate = false; //set spinning to false; the Bandit OnChoice will catch this in Update() & stop the 'animation' 
        showReward = true; // bandit's OnChoice will catch this flag & display the reward 
        yield return new WaitForSeconds(taskSettings.rewardDisplayTime);

        showReward = false;
        EndTrial();

    }

}

