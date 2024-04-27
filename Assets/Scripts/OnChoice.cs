using UnityEngine;
using TMPro; 

public class OnChoice : MonoBehaviour
{
    //assign the Game Manager in inspector (to each instance)
    public GameManager GameManager;

    // Children of The Bandit 
    private GameObject chosenScreen;
    private GameObject banditScreen;
    private GameObject handle; 
    private TMP_Text rewardNotification;

    // Other flags & vars 
    private bool isChosen = false;
    private bool animate = false;
    private bool showReward = false;

    private int reward = 0; 
    private string banditName;



    private void Awake()
        //get bandit name & objects 
    {
        banditName = gameObject.name;
    
        chosenScreen = gameObject.transform.Find("Chosen").gameObject;
        banditScreen = gameObject.transform.Find("BanditScreen").gameObject;
        handle = gameObject.transform.Find("Handle").gameObject;

        GameObject rewardDisplay = banditScreen.transform.Find("RewardNotification").gameObject;
        rewardNotification = rewardDisplay.GetComponent<TextMeshPro>();
        rewardNotification.text = "";
    }

    void Update()
    {
        // Only run following if bandit has been clicked

        float rotationSpeed = 1.0f;
        float endAngle = 150f;

        if (isChosen)
        {
            animate = GameManager.animate;
            showReward = GameManager.showReward;

            float step = rotationSpeed * Time.deltaTime;

            // Rotate the object
            handle.transform.Rotate(Vector3.back * step);

            // Check if the rotation has reached the end angle
            if (handle.transform.rotation.eulerAngles.z >= endAngle)
            {
                // Stop rotating
                handle.transform.rotation = Quaternion.Euler(0f, 0f, endAngle);
            }


            if (!animate)
            {
                if (showReward)
                {
                    DisplayReward();
                }

                if (!showReward)
                {
                    ResetChoice();
                }
              
            }

        }
    }

    void OnMouseDown()
        // when this object is selected 
    {
        // alert GameManager that this Bandit is clicked
        Debug.Log(banditName);
        GameManager.OnChoice(banditName);

        isChosen = true;

        banditScreen.SetActive(false);
        chosenScreen.SetActive(true);


    }

    public void DisplayReward()
    // method to reset the bandit to unchosen state
    {
        banditScreen.SetActive(true);
        chosenScreen.SetActive(false);
        reward = GameManager.reward;
        rewardNotification.text = "+" + reward.ToString() + "<br> Points";

    }

    public void ResetChoice()
        // method to reset the bandit to unchosen state
    {
        isChosen = false;
        showReward = false;
        reward = 0;
        ResetSprite();
        
    }


    private void ResetSprite()
        // method to reset the sprite to unchosen state 
    {
        banditScreen.SetActive(true);
        chosenScreen.SetActive(false);
        rewardNotification.text = "";
        handle.transform.rotation = Quaternion.Euler(0f, 0f, 230f);
  
  
    }
  

}
