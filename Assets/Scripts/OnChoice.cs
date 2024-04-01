using UnityEngine;

public class OnChoice : MonoBehaviour
{
    //assign the following in the inspector::
    //... Game Manager
    public GameManager GameManager;
    //... Sprite for 'chosen' state (same as sprite) 
    public Sprite chosenBanditSprite;


    private bool isChosen = false;
    private bool spinning = false;
    private string banditName;
    private SpriteRenderer spriteRenderer;
    private Sprite banditSprite;
    private Color banditColor; 



    private void Awake()
        //get bandit name & original sprite 
    {
        banditName = gameObject.name;
        spriteRenderer = GetComponent<SpriteRenderer>();
        banditSprite = spriteRenderer.sprite;
        banditColor = spriteRenderer.color;
    }

    void Update()
    {     
        // Only run following if bandit has been clicked
        // 'Spinning' means 'Animated' in the Daw paper 
        if (isChosen)
        {
            spinning = GameManager.spinning; 
           
           
            if (!spinning) {

                ResetChoice();

            }
        }
    }

    void OnMouseDown()
        // when bandit is selected... 
    {
        // alert GameManager that this Bandit is clicked 
        GameManager.OnChoice(banditName);
        isChosen = true;

        // ...change the sprite to 'chosen' version & 'animate'
        spriteRenderer.sprite = chosenBanditSprite;
        spriteRenderer.color = Color.gray;
        InvokeRepeating("AnimateSprite", 0.2f, 0.4f);
  
     
    }

    public void ResetChoice()
        // method to reset the bandit to unchosen state
    {
        isChosen = false;
        ResetSprite();
        
    }

    private void AnimateSprite()
        // method to rotate the sprite (appearance of animation; bit rubbish but easy to implement)
    {
        transform.Rotate(Vector3.back * -45);
    } 

    private void ResetSprite()
        // method to reset the sprite to unchosen state 
    {

        CancelInvoke("AnimateSprite");
        transform.up = Vector3.up;
        spriteRenderer.sprite = banditSprite;
        spriteRenderer.color = banditColor;
  
    }
  

}
