using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intervals : MonoBehaviour
{

    public TextAsset interTrial; // Assign your .txt file to this field in the Inspector
    public int[] intervals; 

    private void Awake()
    // on awake; 
    {
        LoadFromTxt();
    }


    void LoadFromTxt()
    {
        // Check if the data file is not null
        if (interTrial != null)
        {
            // Split the text into lines
            string[] lines = interTrial.text.Split('\n');

            // Create an int array to store the parsed integers
            intervals = new int[lines.Length];

            // Parse each line and store the integer value in the array
            for (int i = 0; i < lines.Length; i++)
            {
                int value;
                if (int.TryParse(lines[i].Trim(), out value))
                {
                    intervals[i] = value;
                }
                else
                {
                    Debug.LogError("Failed to parse line " + i + ": " + lines[i]);
                }
            }

        }
        else
        {
            Debug.LogError("Data file is null! Make sure to assign the data file in the Inspector.");
        }
    }
  
}
