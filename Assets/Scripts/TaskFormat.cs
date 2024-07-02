using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskFormat : MonoBehaviour
{
    public Toggle blockBreaks;
    public Toggle walk1;
    public Toggle walk2;
    public Toggle walk3;
    public TMP_InputField firstTrial;
    public TMP_InputField lastTrial;

    private Payoffs payoffs;


    public int TrialsPerBlock { get; private set; } = 3;
    public int BlocksPerTask { get; private set; } = 4;


    public void Awake()
    {
        payoffs = GetComponent<Payoffs>();

    }


    public int GetStartPosition()
    {
        if (!string.IsNullOrEmpty(firstTrial.text) && int.TryParse(firstTrial.text, out int startAtTrial))
        {
            return startAtTrial;
        }
        return 0;
    }

    public int GetTrialDuration()
    {
        if (!string.IsNullOrEmpty(lastTrial.text) && int.TryParse(lastTrial.text, out int endAfterTrial))
        {
            return endAfterTrial;
        }
        return TrialsPerBlock * BlocksPerTask;
    }

    public int[,] SelectPayoffs(Payoffs payoffs)
    {
        if (walk1.isOn)
        {
            return payoffs.intPayoffsWalk1;
        }
        if (walk2.isOn)
        {
            return payoffs.intPayoffsWalk2;
        }
        if (walk3.isOn)
        {
            return payoffs.intPayoffsWalk3;
        }
        return payoffs.intPayoffsWalk1;
    }
}