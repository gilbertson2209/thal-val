using UnityEngine;

public class Payoffs : MonoBehaviour


{
    // payoff variables 
    public TextAsset payoffsFile1; // assigned in inspector
    public TextAsset payoffsFile2; // assigned in inspector
    public TextAsset payoffsFile3; // assigned in inspector

    public int[,] intPayoffs1;
    public int[,] intPayoffs2;
    public int[,] intPayoffs3;

    private void Awake()
    // on awake; Payoffs need to be loaded from CSV
    {
        intPayoffs1 = LoadFromCSV(payoffsFile1);
        intPayoffs2 = LoadFromCSV(payoffsFile2);
        intPayoffs3 = LoadFromCSV(payoffsFile3);
    }


    private int[,] LoadFromCSV(TextAsset payoffsFile)
    // loading from original Daw CSV from Tom
    // data is floats; so loading here casts to int
    // into a 700 x 4 int array 
    {
        string[] lines = payoffsFile.ToString().Split('\n');
        int rows = lines.Length;
        int columns = Bandits.GetNames(typeof(Bandits)).Length;

        int[,] intPayoffs = new int[rows, columns];

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


        return intPayoffs;
    }
}
