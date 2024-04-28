using UnityEngine;
using TMPro; 

public class OnChoice : MonoBehaviour
{
    //assign the Game Manager in inspector (to each instance)
    public GameManager gameManager;

    // Children of The Bandit 
    private GameObject chosenScreen;
    private GameObject banditScreen;
    private GameObject handle; 
    private TMP_Text rewardNotification;

    // handle 'animation'
    private float rotationSpeed = 1.0f;
    private float endAngle = 150f;
    private float startAngle = 230f;
    // Other flags & vars 
    private bool isChosen = false;
    private string banditName;
  

    private void Awake()
    {
        banditName = gameObject.name;
    
        chosenScreen = gameObject.transform.Find("Chosen").gameObject;
        banditScreen = gameObject.transform.Find("BanditScreen").gameObject;
        handle = gameObject.transform.Find("Handle").gameObject;

        rewardNotification = banditScreen.transform.Find("RewardNotification").GetComponent<TextMeshPro>();
    }

    private void Start() //couldn't quite get around fudging this! 
    {
        ResetBandit();
    }
    private void OnEnable()
    {
        ResetBandit();
    }

    public void ResetBandit()
    // method to reset the bandit to unchosen state
    {
        isChosen = false;
        rewardNotification.text = " ";
        banditScreen.SetActive(true);
        chosenScreen.SetActive(false);
        handle.transform.rotation = Quaternion.Euler(0f, 0f, startAngle);

    }

    void Update()
    {
        if (isChosen)
        {
            bool animate = gameManager.animate;
            bool showReward = gameManager.showReward;

            float step = rotationSpeed * Time.deltaTime;  // Code to move the handle. 
            handle.transform.Rotate(Vector3.back * step);
            if (handle.transform.rotation.eulerAngles.z >= endAngle)
                handle.transform.rotation = Quaternion.Euler(0f, 0f, endAngle);

            if (!animate)
            {
                if (showReward) DisplayReward();

            }

        }
    }

    void OnMouseDown()
    {
        
        gameManager.OnChoice(banditName); // alert gameManager that this Bandit is clicked
        isChosen = true;
        banditScreen.SetActive(false);
        chosenScreen.SetActive(true);
    }

    public void DisplayReward()
    // method to reset the bandit to unchosen state
    {
        banditScreen.SetActive(true);
        chosenScreen.SetActive(false);
        int reward = gameManager.reward;
        rewardNotification.text = "+" + reward.ToString() + "<br> Points";
    }
}
