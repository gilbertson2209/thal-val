using UnityEngine;

public class Payoffs : MonoBehaviour


{
    // payoff variables 
    public TextAsset payoffsFile; // assigned in inspector
    public int[,] intPayoffs; 


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

        intPayoffs = new int[rows, columns];

        for (int i = 0; i < rows - 1; i++)
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
