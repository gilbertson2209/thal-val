using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


// the Bandit Enum 
public enum Bandits { Red, Green, Blue, Yellow };

public class GameManager : MonoBehaviour


    //TODO delete payoffs object and payoffs script not needed
    //TODO make as much of below local as possible/ remove items / private
    // ie check GM in inspector looks ok 
    //TODO put payofffs in a script 

     
{
    // some of the vars and 


    // payoff variables 
    public TextAsset payoffsFile; // assigned in inspector
    private int[,] intPayoffs;  // filled by *** (check order; make new fn to deal with this)

    // ::: TASK VARS ::: (ideally set in GUI) 
    private int currentTrial = 0;  //set these on start 
    private int currentSession = 1;
    public int trialsPerSession = 3;
    public int sessionsPerTask = 2;


    // 'interaction' vars (displaying prizes; failed trials & task progress )
    // tb all contained in canvas 
    public TextMeshProUGUI progress;
    public TextMeshProUGUI alert;
    public GameObject failedTrial;

    public GameObject startScreen; 

    public GameObject placeHolderSquare; // placeholder for the clearscreen 

    // ::: TRIAL VARS :::

    // trial flag 
    private bool inTrial = false;

    // timers 
    public float taskTimer = 0f;
    private float animateTime = 3.0f;
    private float prizeDisplayTime = 2.5f; //1.0f too fast
    private float timeLimit = 2.5f; //1.5f too fast 
    private float failDisplayTime = 4.2f;

    public bool spinning = false; 

    // choice vars 
    public bool choiceMade = false;
    public Bandits chosenBandit;


    // what goes into reset and what goes into start?
    // Start is ok for next, update profgres, fine

    public void Start()
    {
        startScreen.SetActive(true);


    }

    [ContextMenu("StartTrial")]
    public void StartTrial()
    {
        Debug.Log("In Start Trial");
        startScreen.SetActive(false);
        NextTrial();
        CursorLock(false);
        placeHolderSquare.SetActive(false);
        UpdateProgress(currentTrial, currentSession);
        placeHolderSquare.SetActive(false);
        taskTimer = 0f;
        inTrial = true;
    }
    // need to add an intertrial interval (not jittered to start with

    public void EndTrial()
    {
        // set t
        failedTrial.SetActive(false);
        alert.text = " ";
        choiceMade = false;
        inTrial = false;
        StartCoroutine(Intertrial());

    }
    IEnumerator Intertrial()
    {
        // display the fail signal
        placeHolderSquare.SetActive(true);

        // Wait for 4.2 seconds
        yield return new WaitForSeconds(failDisplayTime);

        // move to a 'reset everything' function 
        StartTrial();
    }


    // everything under here I'm quite quite happy with; surprisingly

    public void Update()
    {
        // only execute following if we are in a trial and choice is not made
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
        // bandit's OnChoice will catch this flag; and enter Update() 
        spinning = true; 

        // Wait for 4.2 seconds
        yield return new WaitForSeconds(animateTime);

        //set spinning to false; the Bandit OnChoice will catch this in Update() & stop the 'animation' 
        spinning = false;

        //change to DAW GRW 
        int prizeVal = Random.Range(1, 600);

        UpdatePrizeAlert(prizeVal);

        yield return new WaitForSeconds(prizeDisplayTime);
        
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
        if (currentTrial == trialsPerSession) //start new session 
        {
            if (currentSession == sessionsPerTask) // end the game 
            {
                Debug.Log("End Game: Offer a Reset?");

            }
            else
            // next Session (reset current trial to 0)
            {

                currentSession++;
                currentTrial = 1;
            }

        }
        else
        // next trial within current session 
        {
            currentTrial++;
        }

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