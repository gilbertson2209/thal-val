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

    private EmailData emailData;

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
        emailData = GetComponent<EmailData>();
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
        emailData.StoreRecipients();

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
            // Start the intertrial 
            yield return StartCoroutine(Intertrial());

            int[] trialPayoffs = GetTrialPayoffs(intPayoffs, currentTrial);
            trial.NewTrial(trialPayoffs);
         
            while (!trial.trialComplete)
            {
                yield return null;
            }
  
            int chosen = 0;
            if (trial.choiceMade)
            {
                chosen = (int)trial.chosenBandit;
                chosen++; // leaving '0' as 'no choice' and mapping 0->1,1->2 etc for YBRG 
            }
            
            string[] trialData = {currentTrial.ToString(), trial.reward.ToString(), trial.timeElapsed.ToString(), chosen.ToString() };
            SaveData(trialData);

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
        emailData.SendEmails();
        endScreen.SetActive(true);
    }


    IEnumerator Intertrial()
    {
        ShowIntertrial();
        float interval;
        //interval = (float)intervals[currentTrial];
        interval = 1.0f;
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

    private void SaveData(string [] trialResults)
    {
        string dataToWrite = string.Join(",", trialResults);
        Debug.Log(dataToWrite);
        using StreamWriter dataOut = File.AppendText(pathToLogs);
        dataOut.WriteLine(dataToWrite);
    }

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