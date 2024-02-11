using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// the Bandit Enum 
enum Bandits { Red, Green, Blue, Yellow };

public class GameManager : MonoBehaviour

     
{
    // payoff variables 
    public TextAsset payoffsFile; // assigned in inspector
    private int[,] intPayoffs;  // filled by ***

    // 'interaction' vars (displaying either finish this thought) 
    public TextMeshProUGUI progress;
    public TextMeshProUGUI alert;
    public GameObject failedTrial;



    private int nn = 0;
    // gui info public vars 


    
    //public GameObject payoffs;

    private int currentTrial = 1;
    private int currentSession = 1;
    public int trialsPerSession = 3;
    public int sessionsPerTask = 2;

    //assign the bandits as refs I guess 
    //public GameObject[] bandits;


    [ContextMenu("NextTrial")]
    private void NextTrial()
    {
        if (currentTrial == trialsPerSession) //start new session 
        {
            if (currentSession == sessionsPerTask) // end the game 
            {
                Debug.Log("End Game: Offer a Reset");
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

        UpdateProgress(currentTrial, currentSession);

    }

    private void Start()
    {

        UpdateProgress(currentTrial, currentSession);

    }

    public void BanditClicked(string banditName)

    {
        Bandits bandit;

        if (Bandits.TryParse<Bandits>(banditName, out bandit))
        {
            // Successfully parsed the banditName into an enum value
            // Now you can use the 'bandit' variable to determine which bandit was clicked
            Debug.Log("Clicked bandit: " + bandit);
        }
        else
        {
            Debug.LogWarning("Invalid bandit name: " + banditName);
        }
    }
 

    [ContextMenu("Win")]
    public void Trial()
    {
        currentTrial++;

        UpdateProgress(currentTrial, currentSession);


        // if failedtrial = false (or completed; more logically)
        // this probs needs to be in update 
        int prizeVal = Random.Range(1, 600);

        UpdatePrizeAlert(prizeVal);

        //if failed trial = true

    }

    [ContextMenu("Fail")]
    private void FailedTrial()
    {
        bool isFail = true;
        if (isFail)
            {
            Debug.Log(isFail);
            failedTrial.SetActive(isFail);
        }

    }

    [ContextMenu("PayoffsInGS")]
    private void PayoffsCheck()
    {
      

        Debug.Log("Col 0 " + intPayoffs[nn, 0].ToString());
        Debug.Log("Col 1 " + intPayoffs[nn, 1].ToString());
        Debug.Log("Col 2 " + intPayoffs[nn, 2].ToString());
        Debug.Log("Col 3 " + intPayoffs[nn, 3]. ToString());

        nn++;
    }



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