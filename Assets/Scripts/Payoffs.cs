using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payoffs : MonoBehaviour


{
    //assign the payoffs in the inspector
    public TextAsset payoffsFile;

    public int[,] intPayoffs;


    private void Awake()
    {
        //on awake, load & parse payoffs & cast to integer (raw data is floats)

        //declare a local str for the 'raw' string; split on newlines 
        string[] lines = payoffsFile.ToString().Split('\n');

        //define the array to hold the integer payoffs 
        intPayoffs = new int[lines.Length, 4];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split(',');
            for (int j = 0; j < values.Length - 1; j++)
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
