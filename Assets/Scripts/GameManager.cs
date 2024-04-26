using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum Bandits { Red, Green, Blue, Yellow };

public class GameManager : MonoBehaviour
{ 
    // PAYOFFS & INTERTRIALS 
    public GameObject GameData;
    private Payoffs Payoffs;
    private Intertrial Intertrial; 
    private int[,] intPayoffs;
    private int [] intervals;

    // CANVAS OBJECTS (set in inspector) 
    public GameObject startScreen; // has a button to start task
    public GameObject failedTrial;
    public GameObject intertrialScreen;
    public Toggle blockBreaks;
    public TMP_InputField startPosition; 


    // Bandits 
    public GameObject banditsGroup;

    // TASK VARS 
    private int trial = 0;  
    private int block= 1;
    private int trialCount = 0;
    // can change in inspector during tests 
    public int trialsPerBlock = 3;
    public int blocksPerTask = 2;

    // TRIAL TIMES 
    public float animateTime = 3.0f;
    public float rewardDisplayTime = 2.5f; //1.0f too fast
    public float timeLimit = 2.5f; //1.5f too fast 
    public float failDisplayTime = 4.2f;


    // TRIAL FLAGS & TIMERS 
    private bool inTrial = false;
    public bool spinning = false;
    public float taskTimer = 0f;
    
    // idk what to call you vars 
    public bool choiceMade = false;
    public Bandits chosenBandit;
    public bool showReward = false; 
    public int reward = 0; //to pass to 


    
    public void Awake()
    // references to scripts containing the game data 
    {
        Payoffs = GameData.GetComponent<Payoffs>();
        Intertrial = GameData.GetComponent<Intertrial>();
        
    }

    public void Start()
    // called on start TODO ran sort intervals   
    {
        intPayoffs = Payoffs.intPayoffs;
        intervals = Intertrial.intervals;

        banditsGroup.SetActive(false);

        // start screen is active
        startScreen.SetActive(true);
        // other canvas objects not 
        intertrialScreen.SetActive(false);
        failedTrial.SetActive(false);

    }

    public void StartTask()

    {
        banditsGroup.SetActive(true);
        failedTrial.SetActive(false);
        startScreen.SetActive(false);
        intertrialScreen.SetActive(false);
        StartTrial(); 
    }

    public void StartTrial()
    {
        intertrialScreen.SetActive(false);
        NextTrial();
        CursorLock(false);
      
        UpdateProgress(trial, block);   
        taskTimer = 0f;
        inTrial = true;
    }

    public void EndTrial()
    {
        // 
        failedTrial.SetActive(false);
        choiceMade = false;
        inTrial = false;
        StartCoroutine(ShowIntertrial());

    }

    IEnumerator ShowIntertrial()
    { 
        //TODO lerp 
        intertrialScreen.SetActive(true);
        yield return new WaitForSeconds(intervals[trialCount]);
        StartTrial();
    }


    public void Update()
    {
        // only execute following if we are inTrial AND no choicemade
        if (inTrial && !choiceMade)
        {
            taskTimer += Time.deltaTime;
            if (taskTimer >= timeLimit)
            {
                inTrial = false;
                StartCoroutine(FailTrial());
            }
        }
    }


    public void OnChoice(string banditName)
    // called from the 'OnChoice' script attached to each bandits
    // idea here is to parse the ObjectName of the clicked bandit into the corresponding enum value 

    {
        choiceMade = true;

        // parse the chosen bandit (str) to the enum 

        if (Bandits.TryParse<Bandits>(banditName, out chosenBandit))
        {
            // Parse the clicked GameObject name (banditName) into an enum value


            Debug.Log("Clicked bandit: " + chosenBandit);
        }
        else
        {
            Debug.LogWarning("Invalid bandit name: " + banditName);
        }

        //lock and hide the cursor

        CursorLock(true);

        // start 'spinning' the bandit 
        StartCoroutine(StartSpin());

    }

    private void CursorLock(bool flag)
    {
        if (flag)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (!flag)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
 
    IEnumerator StartSpin()

    
    {

        // bandit's OnChoice will catch this flag & change appearance
        spinning = true;

        // Wait for 4.2 seconds
        yield return new WaitForSeconds(animateTime);

        //set spinning to false; the Bandit OnChoice will catch this in Update() & stop the 'animation' 
        spinning = false;

        StartCoroutine(DisplayReward());
    }


    IEnumerator DisplayReward()

    {
     
        int bandit = (int)chosenBandit;
        reward = intPayoffs[trialCount, bandit];
        // bandit's OnChoice will catch this flag & display the reward 
        showReward = true;

        yield return new WaitForSeconds(rewardDisplayTime);
        showReward = false; 

        EndTrial();
    }




    IEnumerator FailTrial()
    {
        // display the fail signal

        failedTrial.SetActive(true);
        CursorLock(true);

        // Wait for 4.2 seconds
        yield return new WaitForSeconds(failDisplayTime);

        // move to a 'reset everything' function 
        EndTrial();
    }


    private void NextTrial()
        // function to control next flow trial 
    {
        trialCount++;

        if (trial == trialsPerBlock) //start new session 
        {
            if (block == blocksPerTask) // end the game 
            {
                Debug.Log("End Game:: DO SOMETHING");

            }
            else
            // next Session (reset current trial to 0)
            {

                block++;
                trial = 1;
            }

        }
        else
        // next trial within current session 
        {
            trial++;
        }

    }


    private void UpdateProgress(int trial, int block)
    {
        string trialString = "Trial " + trial.ToString() + " of " + trialsPerBlock.ToString();
        string sessionString = " in Session " + block.ToString() + " of " + blocksPerTask.ToString();
        Debug.Log(trialString + sessionString);
   
    }

}