using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum Bandits { Yellow, Blue, Red, Green };

public class GameManager : MonoBehaviour
{
    // References to ...
    // ... Canvas objects (assigned in inspector) 
    public GameObject startScreen;
    public GameObject failedTrial;
    public GameObject endScreen;
    public GameObject interBlockBreakScreen;
    public GameObject customSettings; 
    public GameObject fixationCross;

    // Game Play Formats 
    public Toggle standardToggle;
    public Toggle customToggle;

    // ... GameObjects & Scripts
    private TaskFormat taskFormat;
    private TaskSettings taskSettings;
    public Trial trial;
    public GameObject allBandits;
    private AllBanditsManager allBanditsManager;
    public GameObject gameData;
    private Payoffs payoffs;
    private Intervals poissIntervals;

    // Task Vars (most in TaskSettings)
    public int[,] intPayoffs;
    private int [] intervals;
    private int currentTrial;
    private int lastTrial;

    private bool onBlockBreak = false;

    //Result Vars
    public string pathToLogs;
    public Bandits chosenBandit; // use int(chosenBandit)

    public void Awake()
    // references to scripts containing the game data 
    {
        payoffs = gameData.GetComponent<Payoffs>();
        poissIntervals = gameData.GetComponent<Intervals>();
        taskFormat = GetComponent<TaskFormat>();
        taskSettings = GetComponent<TaskSettings>();
        trial = GetComponent<Trial>(); 
        allBanditsManager = allBandits.GetComponent<AllBanditsManager>();
    }

    private void Start()
    {
        intervals = poissIntervals.intervals; //TODO Ran sort this 
        SetupInitialUIState();
    }


    private void InitLogFile()
    {
        DateTime currentTime = DateTime.Now;
        string fileName = currentTime.ToString("yyyy.MM.dd.HH.mm.ss");

        pathToLogs = Application.persistentDataPath + "/" + fileName + ".txt";

        using StreamWriter dataOut = File.CreateText(pathToLogs);
        dataOut.WriteLine("Task Initialised at: " + DateTime.Now.ToString());
        dataOut.WriteLine("Trial Number, Reward, Response Time, Response Chosen");
    }

   
    public void StartTask() //Sets up 
    {
        InitLogFile();

        // read in the chosen Daw walk from GUI 
        intPayoffs = taskFormat.SelectPayoffs(payoffs);
        int maxLen = (intPayoffs.Length) / 4;

        // and set up accourding to the 'play mode'
        if (customToggle.isOn)
        {
            currentTrial = taskFormat.GetFirstTrial();
            lastTrial = taskFormat.GetLastTrial(maxLen);
            taskSettings.blocksPerTask = 1;
            taskSettings.trialsPerBlock = lastTrial - currentTrial; 
        } else
        {
            currentTrial = 1;
            lastTrial = taskSettings.trialsPerBlock * taskSettings.blocksPerTask; 
        }

        startScreen.SetActive(false);
        StartCoroutine(RunTask());
    }

    private IEnumerator RunTask()
    {
        int trialCount = 0;

        while (currentTrial < (lastTrial + 1))
        {
            // Execute the current trial
             // Or however you instantiate your Trial
            // start the trial
            Debug.Log("Current Trial " + currentTrial.ToString() + "Trial Count" + trialCount.ToString());
            Debug.Log("Standard Toggle " + standardToggle.isOn);
            yield return StartCoroutine(Intertrial());

            int[] trialPayoffs = GetTrialPayoffs(intPayoffs, currentTrial);
            trial.inTrial = true;
            trial.NewTrial(trialPayoffs);

            Debug.Log(trial.inTrial.ToString() + "  " + Time.time.ToString());
         
            while (trial.inTrial) //hold execution until trial is complete
            {
                yield return null;
            }
            Debug.Log("after trial" + currentTrial.ToString() + "  " + Time.time.ToString());
            int chosen = 0;
            if (!trial.choiceMade)
            {
                chosen = (int)trial.chosenBandit;
                chosen++;
            }
                
            string[] trialData = { currentTrial.ToString(), trial.reward.ToString(), trial.timeElapsed.ToString(), chosen.ToString() };
            string dataToWrite = string.Join(",", trialData);
            Debug.Log(dataToWrite);

            currentTrial++;
            trialCount++;
            // Check to see if in interblock break; if so break. 
            if (trialCount >= taskSettings.trialsPerBlock)
            { 
                if (standardToggle.isOn && currentTrial < lastTrial)
                {
                    onBlockBreak = true;
                    interBlockBreakScreen.SetActive(true);
                    yield return WaitForContinue(); // Wait until the interblock break screen is deactivated
                    interBlockBreakScreen.SetActive(false);
                    trialCount = 0; 
                }   
            }
        }

        endScreen.SetActive(true);
    }


    IEnumerator Intertrial()
    {
        ShowIntertrial();
        float interval;
        interval = (float)intervals[currentTrial];
        yield return new WaitForSeconds(interval);

    }

    public void ShowIntertrial()
    {
        allBanditsManager.BlockInput(true);
        allBanditsManager.ActivateBandits(false);
        failedTrial.SetActive(false);
        fixationCross.SetActive(true);
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

    public void EndTask()
    {
        endScreen.SetActive(true);
    }

    private int[] GetTrialPayoffs(int[,] intPayoffs, int currentTrial)
    {
        int rowIndex = currentTrial-1;
        int numColumns = intPayoffs.GetLength(1); // Get the number of columns
        int[] payoffs = new int[numColumns];

        for (int i = 0; i < numColumns; i++)
        {
            payoffs[i] = intPayoffs[rowIndex, i];
        }

        return payoffs;
    }

    //private void SaveData()
    //{
    //    int bandit = 0; 
    //    //if (choiceMade) {
    //    //    bandit = (int)chosenBandit;
    //    //    bandit ++; // c# starts at 0
    //    //}

    //    string[] trialData = { currentTrial.ToString(), reward.ToString(), responseTime.ToString(), bandit.ToString() };
    //    string dataToWrite = string.Join(",", trialData);

    //    Debug.Log(dataToWrite);

    //    JsonUtility.ToJson(dataToWrite);
    //    Debug.Log(JsonUtility.ToJson(dataToWrite));
    //    using StreamWriter dataOut = File.AppendText(pathToLogs);
    //    dataOut.WriteLine(dataToWrite);

    //}

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetupInitialUIState()
    {
        startScreen.SetActive(true);
        allBanditsManager.ActivateBandits(false);
        fixationCross.SetActive(false);
        failedTrial.SetActive(false);
        interBlockBreakScreen.SetActive(false);
    }
}