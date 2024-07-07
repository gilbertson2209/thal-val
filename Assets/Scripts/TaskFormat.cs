using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskFormat : MonoBehaviour
{
    public Toggle walk1;
    public Toggle walk2;
    public Toggle walk3;
    public TMP_InputField firstTrial;
    public TMP_InputField lastTrial;


    public int GetFirstTrial()
    {
        if (!string.IsNullOrEmpty(firstTrial.text) && int.TryParse(firstTrial.text, out int startAtTrial))
        {
            return startAtTrial;
        }
        return 1;
    }

    public int GetLastTrial(int szPayoffs)
    {
        if (!string.IsNullOrEmpty(lastTrial.text) && int.TryParse(lastTrial.text, out int endAfterTrial))
        {
            return endAfterTrial;
        }
        return szPayoffs;
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