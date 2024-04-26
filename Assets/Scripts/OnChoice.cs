using UnityEngine;
using TMPro; 

public class OnChoice : MonoBehaviour
{
    //assign the following in the inspector::
    //... Game Manager
    public GameManager GameManager;

    // Children of The Bandit 
    private GameObject chosenScreen;
    private GameObject banditScreen;
    private GameObject handle; 
    private TMP_Text rewardNotification;

    private bool isChosen = false;
    private bool spinning = false;

    private bool showReward = false;
    private int reward = 0; 

    private string banditName;

    private void Awake()
        //get bandit name & original sprite 
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
        // 'Spinning' means 'Animated' in the Daw paper
        float rotationSpeed = 1.0f;
        float endAngle = 30f;

        if (isChosen)
        {
            spinning = GameManager.spinning;
            showReward = GameManager.showReward;

            float step = rotationSpeed * Time.deltaTime;

            // Rotate the object
            handle.transform.Rotate(Vector3.forward * step);

            // Check if the rotation has reached the end angle
            if (handle.transform.rotation.eulerAngles.z >= endAngle)
            {
                // Stop rotating
                handle.transform.rotation = Quaternion.Euler(0f, 0f, endAngle);
            }


            if (!spinning)
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
        // when bandit is selected... 
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
        Debug.Log("In Display");
        banditScreen.SetActive(true);
        chosenScreen.SetActive(false);
        reward = GameManager.reward;
        rewardNotification.text = "Obtained <br>" + reward.ToString() + "<br> Points";

    }

    public void ResetChoice()
        // method to reset the bandit to unchosen state
    {
        Debug.Log("reset choice");
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
        handle.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
  
  
    }
  

}
