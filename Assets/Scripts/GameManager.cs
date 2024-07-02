using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public enum Bandits { Yellow, Blue, Red, Green };

public class GameManager : MonoBehaviour
{
    // References to ...
    // ... Canvas objects (set these in inspector) 
    public GameObject startScreen;
    public GameObject failedTrial;
    public GameObject endScreen;
    public GameObject interBlockBreakScreen;
    public GameObject customSettings; //only need this for set show/ unshow reallu can remove


    // Game Play Formats 
    public Toggle blockBreaks;
    public Toggle customFormat;

    // move these into settings 
    public Toggle walk1;
    public Toggle walk2;
    public Toggle walk3; 

    public TMP_InputField startPosition;
    public TMP_InputField nTrialsToRunAfterStart;

    // ... GameObjects & Scripts
    public GameObject banditsGroup;
    private Transform banditsTransform;
    public GameObject fixationCross;

    // ...Scripts & Game Data
    public GameObject gameData;
    private Payoffs payoffs;
    private Intervals poissIntervals;
    public int[,] intPayoffs;
    private int [] intervals;

    //...Task vars (set public to alter in inspector)
    public int trialsPerBlock = 3;
    public int blocksPerTask = 4; // as above 
    private int trial = 0;  // these are trackers   
    private int block= 1;
  
    public int trialStart = 0;
    public int trialCount = 0; // tom can change via startScreen
    public int currentTrial;
    public int trialDuration;

    //......& task times // can alter all in inspector 
    public float ANIMATE_TIME = 3.0f; //from Daw 
    public float REWARD_DISP_TIME = 1.0f; //from Daw
    public float TIME_LIMIT = 1.5f; //from Daw
    public float FAIL_DISP_TIME = 4.2f; //from Daw
    public float taskTimer = 0f;
    // .....& flags 
    public bool inTrial = false;
    public bool choiceMade = false;
    public bool animate = false;
    public bool showReward = false;
    private bool taskComplete = false;
    private bool onBlockBreak = false;
    //... & data write vars 
    public Bandits chosenBandit; // use int(chosenBandit)
    public int reward = 0;

    private float tStart;
    private float responseTime;
    private string pathToLogs;



    public TaskFormat taskFormat; 

    public void Awake()
    // references to scripts containing the game data 
    {
        DateTime currentTime = DateTime.Now;
        string fileName = currentTime.ToString("yyyy.MM.dd.HH.mm.ss");
    
        pathToLogs = Application.persistentDataPath + "/" + fileName + ".txt";

        using (StreamWriter dataOut = File.CreateText(pathToLogs))
        {
            dataOut.WriteLine("Task Initialised at: " + DateTime.Now.ToString());
            dataOut.WriteLine("Trial Number, Reward, Response Time, Response Chosen");
        }
        payoffs = gameData.GetComponent<Payoffs>();
        poissIntervals = gameData.GetComponent<Intervals>();


        taskFormat = GetComponent<TaskFormat>();
  
    }

    public void Start()
    {
        
        intervals = poissIntervals.intervals; //TODO ran sort this 
        banditsTransform = banditsGroup.transform;

        startScreen.SetActive(true);
        banditsGroup.SetActive(false);
        fixationCross.SetActive(false);  
        failedTrial.SetActive(false);
        interBlockBreakScreen.SetActive(false);
    }

    public void StartTask()
    {

        intPayoffs = taskFormat.SelectPayoffs(payoffs);


        Debug.Log(intPayoffs[0, 0]);
        Debug.Log(intPayoffs[0, 1]);
        Debug.Log(intPayoffs[0, 2]);
        Debug.Log(intPayoffs[0, 3]);

        Debug.Log(blockBreaks.isOn);

        //if (walk1.isOn)
        //{
        //    intPayoffs = payoffs.intPayoffsWalk1;
        //    Debug.Log("Walk1");
        //}

        //if (walk2.isOn)
        //{
        //    intPayoffs = payoffs.intPayoffsWalk2;
        //    Debug.Log("Walk2");
        //}

        //if (walk3.isOn)
        //{
        //    intPayoffs = payoffs.intPayoffsWalk3;
        //    Debug.Log("Walk3");
        //}


        if (blockBreaks.isOn) // if the 'standard' toggle is on don't change trial start or duratuon 
        {
            currentTrial = trialStart;
            trialDuration = trialsPerBlock * blocksPerTask;
            Debug.Log(blockBreaks.isOn);
        }
        else
        {
            Debug.Log(blockBreaks.isOn);
            // Check if startPosition is filled has a valid integer value; if so use it. 
            if (!string.IsNullOrEmpty(startPosition.text) && int.TryParse(startPosition.text, out int startNumber))
            {
                Debug.Log(startNumber.ToString());
                Debug.Log(blockBreaks.isOn);
                trialStart = startNumber - 1;
            }
            else
            {
                Debug.Log(blockBreaks.isOn);
                trialStart = 0;
            }
            if (!string.IsNullOrEmpty(nTrialsToRunAfterStart.text) && int.TryParse(nTrialsToRunAfterStart.text, out int nTrials))
            {
                Debug.Log(nTrials.ToString());
                Debug.Log(blockBreaks.isOn);
                trialDuration = nTrials;
            }
            else
            {
                Debug.Log(blockBreaks.isOn);
                trialDuration = trialsPerBlock * blocksPerTask;
            }
        }

        currentTrial = trialStart;

        Debug.Log(trialStart.ToString());
        Debug.Log(trialDuration.ToString());
    
        startScreen.SetActive(false);
        StartCoroutine(ShowIntertrial());

    }

   

    IEnumerator ShowIntertrial()
    {
        BlockInput(true);
        failedTrial.SetActive(false);
        ActivateBandits(false);
        banditsGroup.SetActive(false);
        fixationCross.SetActive(true);
   
        if (trialCount == trialDuration)
        {
            taskComplete = true;
            EndTask();
        }
        if (trial == trialsPerBlock && blockBreaks.isOn && !taskComplete)
            {
                onBlockBreak = true;
                interBlockBreakScreen.SetActive(true);
                yield return WaitForContinue();
                interBlockBreakScreen.SetActive(false);
            
        }
        if (!taskComplete)
        {
            float interval;
            interval = 1.0f; //TODO fix back to poiss 
            yield return new WaitForSeconds(interval);
            NextTrial();
        }
    }

    private void NextTrial()
    {
        if (trial == trialsPerBlock)
        {
            block++;
            trial = 1;
        }
        else
        {
            trial++;
        }

        trialCount++;
        currentTrial++;

        BlockInput(false);
        banditsGroup.SetActive(true);
        ActivateBandits(true);
        tStart = Time.time;
        taskTimer = 0f;
        inTrial = true;

    }

    IEnumerator WaitForContinue()
    {
        while (onBlockBreak)
        {
            yield return null;
        }
  
    }
    
    public void ContinueTask() // Call when the UI button on the interblock screen is pressed
    {
        onBlockBreak = false;
    }

    public void ActivateBandits(bool flag)
    {
        for (int i = 0; i < banditsTransform.childCount; i++)
        {
            GameObject child = banditsTransform.GetChild(i).gameObject;
            child.SetActive(flag);
        }
    }

    public void EndTrial()
    {
        responseTime = Time.time - tStart;
        SaveData();
        choiceMade = false;
        failedTrial.SetActive(false);
        StartCoroutine(ShowIntertrial());
    }

    IEnumerator FailTrial()
    {
        inTrial = false;
        reward = 0;
        fixationCross.SetActive(false);
        failedTrial.SetActive(true);
        BlockInput(true);
        yield return new WaitForSeconds(FAIL_DISP_TIME);
        EndTrial();
    }

    public void EndTask()
    {
        endScreen.SetActive(true);
    }

    public void Update()
    {
        // only execute following if we are inTrial AND no choicemade
        if (inTrial && !choiceMade)
        {
            taskTimer += Time.deltaTime;
            if (taskTimer >= TIME_LIMIT)
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
        BlockInput(true);
        StartCoroutine(AnimateAndDisplay());
      
    }

    IEnumerator AnimateAndDisplay()
    {
        int bandit = (int)chosenBandit;
        reward = intPayoffs[currentTrial-1, bandit];

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
        AllCollidersEnabled(!flag);
    }


    private void AllCollidersEnabled(bool enabled) 
    {
        for (int i = 0; i < banditsTransform.childCount; i++)
        {
            GameObject child = banditsTransform.GetChild(i).gameObject;
            PolygonCollider2D polyCollider = child.GetComponent<PolygonCollider2D>();
            if (polyCollider != null)
            {
                polyCollider.enabled = enabled;
            }
        }
    }

    private void SaveData()
    {
        int bandit = 0; 
        if (choiceMade) {
            bandit = (int)chosenBandit;
            bandit ++; // c# starts at 0
        }

        string[] trialData = { currentTrial.ToString(), reward.ToString(), responseTime.ToString(), bandit.ToString() };
        string dataToWrite = string.Join(",", trialData);

        Debug.Log(dataToWrite);

        JsonUtility.ToJson(dataToWrite);
        Debug.Log(JsonUtility.ToJson(dataToWrite));
        using StreamWriter dataOut = File.AppendText(pathToLogs);
        dataOut.WriteLine(dataToWrite);

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}