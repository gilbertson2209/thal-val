using UnityEngine;

public class OnChoiceA : MonoBehaviour
{
    //assign the following in the inspector::
    //... Game Manager
    public GameManager GameManager;

    // Children of The Bandit 
    private GameObject chosenScreen;
    private GameObject banditScreen;
    private GameObject handle; 

    private bool isChosen = false;
    private bool spinning = false;



    private string banditName;


    



    private void Awake()
        //get bandit name & original sprite 
    {
        banditName = gameObject.name;
    
        chosenScreen = gameObject.transform.Find("Chosen").gameObject;
        banditScreen = gameObject.transform.Find("BanditScreen").gameObject;
        handle = gameObject.transform.Find("Handle").gameObject;

    }

    void Update()
    {
        // Only run following if bandit has been clicked
        // 'Spinning' means 'Animated' in the Daw paper
        float rotationSpeed = 10f;
        float endAngle = 30f;

        if (isChosen)
        {
            spinning = GameManager.spinning;

            float step = rotationSpeed * Time.deltaTime;

            // Rotate the object
            handle.transform.Rotate(Vector3.forward * step);

            // Check if the rotation has reached the end angle
            if (handle.transform.rotation.eulerAngles.z >= endAngle)
            {
                // Stop rotating
                handle.transform.rotation = Quaternion.Euler(0f, 0f, endAngle);
            }


            if (!spinning) {

                ResetChoice();

            }
        }
    }

    void OnMouseDown()
        // when bandit is selected... 
    {
        // alert GameManager that this Bandit is clicked
        Debug.Log(banditName);
        GameManager.OnChoice(banditName);
        isChosen = true;

        banditScreen.SetActive(false);
        chosenScreen.SetActive(true);


    }

    public void ResetChoice()
        // method to reset the bandit to unchosen state
    {
        isChosen = false;
        ResetSprite();
        
    }


    private void ResetSprite()
        // method to reset the sprite to unchosen state 
    {
        banditScreen.SetActive(true);
        chosenScreen.SetActive(false);

        handle.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
  
  
    }
  

}
