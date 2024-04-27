using System;
using System.IO;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum Bandits { Red, Green, Blue, Yellow };

public class GameManager : MonoBehaviour
{
    // References to ...
    // ... Canvas objects (set these in inspector) 
    public GameObject startScreen;
    public GameObject failedTrial;
    // placehold for interBlockBreakScreen and endTaskScreen
    public Toggle blockBreaks;
    public TMP_InputField startPosition;

    // ... GameObjects & Scripts
    public GameObject banditsGroup;
    private Transform banditsTransform;
    public GameObject fixationCross;

    // ...Scripts & Game Data
    public GameObject gameData;
    private Payoffs payoffs;
    private Intervals poissIntervals;
    private int[,] intPayoffs;
    private int [] intervals;

    //...Task vars (set public to alter in inspector)
    public int trialsPerBlock = 150;
    public int blocksPerTask = 2; // as above 
    private int trial = 0;  // these are trackers   
    private int block= 0;
    private int trialCount = 0; // tom can change via startScreen 
    //......& task times // can alter all in inspector 
    public float ANIMATE_TIME = 3.0f; //from Daw 
    public float REWARD_DISP_TIME = 1.0f; //from Daw
    public float TIME_LIMIT = 1.5f; //from Daw
    public float FAIL_DISP_TIME = 4.2f; //from Daw
    public float taskTimer = 0f;
    // .....& flags 
    private bool inTrial = false;
    public bool choiceMade = false;
    public bool animate = false;
    public bool showReward = false;
    //... & data write vars 
    public Bandits chosenBandit; // use int(chosenBandit)
    public int reward = 0; //to pass to
    private float tStart;
    private float responseTime;
    private string pathToLogs;

    public void Awake()
    // references to scripts containing the game data 
    {
        DateTime currentTime = DateTime.UtcNow;
        string fileName = currentTime.ToString("yyyy.MM.dd.HH.mm.ss");
    
        pathToLogs = Application.persistentDataPath + "/" + fileName + ".txt";

        using (StreamWriter dataOut = File.CreateText(pathToLogs))
        {
            dataOut.WriteLine("Task Initialised at: " + DateTime.Now.ToString());
            dataOut.WriteLine("Trial Number", "Reward", "Response Time", "Response Chosen");
        }
        payoffs = gameData.GetComponent<Payoffs>();
        poissIntervals = gameData.GetComponent<Intervals>();
    }

    public void Start()
    // called on start TODO ran sort the intervals   
    {
        intPayoffs = payoffs.intPayoffs;
        intervals = poissIntervals.intervals;
        banditsTransform = banditsGroup.transform;

        startScreen.SetActive(true);

        banditsGroup.SetActive(false);
        fixationCross.SetActive(false);  
        failedTrial.SetActive(false);
    }

    public void StartTask()
    {
        // Check if startPosition has a valid integer value; if so use it. 
        if (!string.IsNullOrEmpty(startPosition.text) && int.TryParse(startPosition.text, out int startNumber))
        {
            trialCount = startNumber;
        }
        startScreen.SetActive(false);
        StartCoroutine(ShowIntertrial());
        block = 1;
    }

    IEnumerator ShowIntertrial()
    {
        BlockInput(true);
        failedTrial.SetActive(false);
        banditsGroup.SetActive(false);
        fixationCross.SetActive(true);
        float interval;
        interval = (float)intervals[trialCount];
        yield return new WaitForSeconds(interval);
        StartTrial();
    }

    public void StartTrial()
    {
        NextTrial();
        BlockInput(false);
        banditsGroup.SetActive(true);

        tStart = Time.time;
        taskTimer = 0f;
        choiceMade = false;
        inTrial = true;
    }

    public void EndTrial()
    {
        SaveData();
        failedTrial.SetActive(false);
        inTrial = false;
        StartCoroutine(ShowIntertrial());
    }

    IEnumerator FailTrial()
    {
        reward = 0;
        fixationCross.SetActive(false);
        failedTrial.SetActive(true);
        BlockInput(true);
        yield return new WaitForSeconds(FAIL_DISP_TIME);
        EndTrial();
    }

    private void NextTrial()
    {
        trialCount++;

        if (trial == trialsPerBlock)
        {
            if (block == blocksPerTask)
            {
                Debug.Log("End Game:: DO SOMETHING");
            }
            else
            {
                block++;
                trial = 1;
            }
        }
        else
        {
            trial++;
        }
    }


    public void Update()
    {
        // only execute following if we are inTrial AND no choicemade
        if (inTrial && !choiceMade)
        {
            taskTimer += Time.deltaTime;
            if (taskTimer >= TIME_LIMIT)
            {
                inTrial = false;
                StartCoroutine(FailTrial());
            }
        }
    }

    public void OnChoice(string banditName)
    // called from the 'OnChoice' script attached to each bandits
    {
        choiceMade = true;
        fixationCross.SetActive(false);

        // parse the chosen bandit (str) to the enum 
        if (Bandits.TryParse<Bandits>(banditName, out chosenBandit))
        {
            // Parse the clicked GameObject name (banditName) into an enum value
            Debug.Log("Clicked bandit: " + (int)chosenBandit);
        }
        else
        {
            Debug.LogWarning("Invalid bandit name: " + banditName);
        }

        BlockInput(true);
        StartCoroutine(AnimateAndDisplay());

    }

    IEnumerator AnimateAndDisplay()
    {
        int bandit = (int)chosenBandit;
        reward = intPayoffs[trialCount, bandit];

        animate = true;  // bandit's OnChoice will catch this flag & change appearance
        yield return new WaitForSeconds(ANIMATE_TIME);
        animate = false; //set spinning to false; the Bandit OnChoice will catch this in Update() & stop the 'animation' 

        showReward = true; // bandit's OnChoice will catch this flag & display the reward 
        yield return new WaitForSeconds(REWARD_DISP_TIME);
        showReward = false;

        EndTrial();

    }

    private void BlockInput(bool flag)
        // this should work for laptop, browsers and touchscreens 
    {
        Cursor.visible = !flag;
        Cursor.lockState = flag ? CursorLockMode.Locked : CursorLockMode.None;
        AllCollidersEnabled(!flag);
    }


    private void AllCollidersEnabled(bool enabled)
    {
        for (int i = 0; i < banditsTransform.childCount; i++)
        {
            GameObject child = banditsTransform.GetChild(i).gameObject;
            BoxCollider boxCollider = child.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = enabled;
            }
        }
    }

    private void SaveData()
    {
        responseTime = Time.time - tStart;
        int bandit = 0; 
        if (choiceMade) {
            bandit = (int)chosenBandit;
            bandit ++; // c# starts at 0
        }
        string[] trialData = { trialCount.ToString(), reward.ToString(), responseTime.ToString(), bandit.ToString() };
        string dataToWrite = string.Join(",", trialData);

        using StreamWriter dataOut = File.AppendText(pathToLogs);
        dataOut.WriteLine(dataToWrite);

    }
}