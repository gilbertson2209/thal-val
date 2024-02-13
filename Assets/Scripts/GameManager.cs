using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// the Bandit Enum 
enum Bandits { Red, Green, Blue, Yellow };

public class GameManager : MonoBehaviour


    //TODO delete payoffs object and payoffs script not needed
    //TODO make as much of below local as possible 

     
{
    // payoff variables 
    public TextAsset payoffsFile; // assigned in inspector
    private int[,] intPayoffs;  // filled by *** (check order; make new fn to deal with this)

    // ::: TASK VARS ::: (ideally set in GUI) 
    private int currentTrial = 0;
    private int currentSession = 0;
    public int trialsPerSession = 3;
    public int sessionsPerTask = 2;


    // 'interaction' vars (displaying prizes; failed trials & task progress ) 
    public TextMeshProUGUI progress;
    public TextMeshProUGUI alert;
    public GameObject failedTrial;

    public GameObject placeHolderSquare; // placeholder for the clearscreen 

    // ::: TRIAL VARS :::

    // trial flag 
    private bool inTrial = false;
    private bool fail = false; 

    // timers 
    public float taskTimer = 0f;
    private float animateTime = 3.0f;
    private float prizeDisplayTime = 1.0f;
    private float timeLimit = 1.5f;
    private float failDisplayTime = 4.2f;

    // choice vars 
    public bool choiceMade = false;
    Bandits chosenBandit;


    [ContextMenu("StartTrial")]
    public void StartTrial()
    {
        NextTrial();
        UpdateProgress(currentTrial, currentSession);
        taskTimer = 0f;
        inTrial = true;
    }

    public void Update()
    {
        // only execute following if we are in a trial and choice is not made
        if (inTrial && !choiceMade)
        {
            taskTimer += Time.deltaTime;
            if (taskTimer>= timeLimit)
            {
                inTrial = false; 
                StartCoroutine(FailTrial());
            }
        }
    }

    IEnumerator FailTrial()
    {
        // display the fail signal
        failedTrial.SetActive(true);
        // Wait for 4.2 seconds
        yield return new WaitForSeconds(failDisplayTime);

        // move to a 'reset everything' function 
        failedTrial.SetActive(false);
    }

    public void EndTrial()
    {
        failedTrial.SetActive(false);
        alert.text = " "; 

        // clear prize award
        // clear eerything
        // black screen 
    }

    public void ClearChoice()
    {
        choiceMade = false;
    }

 

    private void NextTrial()
    {
        if (currentTrial == trialsPerSession) //start new session 
        {
            if (currentSession == sessionsPerTask) // end the game 
            {
                Debug.Log("End Game: Offer a Reset?");
            } else
                // next Session (reset current trial to 0)
            {

                currentSession++;
                currentTrial = 1;
            }
            
        } else
            // next trial within current session 
        {
            currentTrial++;
        }

    }

    private void Start()
        // On start, the current trial/ session needs to be displayed 
    {

        UpdateProgress(currentTrial, currentSession);

    }

 

    [ContextMenu("Win")]
    public void Trial()
    {
      
        // if failedtrial = false (or completed; more logically)
        // this probs needs to be in update 
        int prizeVal = Random.Range(1, 600);

        UpdatePrizeAlert(prizeVal);

        //if failed trial = true

    }

    


    public void BanditChoice(string banditName)
    // called from the 'OnChoice' scripts attached to the bandits 

    {
        choiceMade = true;

        // parsing the chosen bandit into an enum value 
        Bandits bandit;

        if (Bandits.TryParse<Bandits>(banditName, out bandit))
        {
            // Parse the clicked GameObject name (banditName) into an enum value


            Debug.Log("Clicked bandit: " + bandit);
        }
        else
        {
            Debug.LogWarning("Invalid bandit name: " + banditName);
        }

        chosenBandit = bandit;

    }


    // GUI update functions 

    private void UpdatePrizeAlert(int prizeVal)
    {
        alert.text = "You Win " + prizeVal.ToString();
    }

    private void UpdateProgress(int currentTrial, int currentSession)
    {
        string trialString = "Trial " + currentTrial.ToString() + " of " + trialsPerSession.ToString();
        string sessionString = " in Session " + currentSession.ToString() + " of " + sessionsPerTask.ToString();

        progress.text = trialString + sessionString;

    }

    private void Awake()
    // on awake; Payoffs need to be loaded from CSV
    {
        LoadFromCSV();
    }


    private void LoadFromCSV()
        // loading from original Daw CSV from Tom
        // data is floats; so loading here casts to int
        // into a 700 x 4 int array 
    {
        string[] lines = payoffsFile.ToString().Split('\n');
        int rows = lines.Length;
        int columns = Bandits.GetNames(typeof(Bandits)).Length;
        Debug.Log(rows);
        Debug.Log(columns);

        intPayoffs = new int[rows, columns];
        
        for (int i = 0; i < rows-1; i++)
        {
            string[] values = lines[i].Trim().Split(',');
            for (int j = 0; j < columns; j++)
            {
               
                if (float.TryParse(values[j], out float floatValue))
                {
                    intPayoffs[i, j] = Mathf.RoundToInt(floatValue);
                   
                }
                else
                {
                    Debug.LogError("Failed to parse value at row " + (i + 1) + ", column " + (j + 1));
                }
            }
        }


    }



}