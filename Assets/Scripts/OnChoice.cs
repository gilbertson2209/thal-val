//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class OnChoice : MonoBehaviour
{
    // controls flow 
    public GameManager GameManager;

    public bool isChosen = false;
    private bool spinning = false; 
    public Sprite chosenBanditSprite;

    // set following in inspector 
    private string banditName;
    private SpriteRenderer spriteRenderer;
    private Sprite banditSprite;

    private void Awake()
        //get bandit name & original sprite 
    {
        banditName = gameObject.name;
        spriteRenderer = GetComponent<SpriteRenderer>();
        banditSprite = spriteRenderer.sprite;
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
        InvokeRepeating("RotateSprite", 0.2f, 0.4f);
  
     
    }

    public void ResetChoice()
        // method to reset the bandit to unchosen state
    {
        isChosen = false;
        ResetSprite();
        
    }

    private void RotateSprite()
        // method to rotate the sprite (appearance of animation; bit rubbish but easy to implement)
    {
        transform.Rotate(Vector3.back * -90);
    } 

    private void ResetSprite()
        // method to reset the sprite to unchosen state 
    {

        CancelInvoke("RotateSprite");
        transform.up = Vector3.up;
        spriteRenderer.sprite = banditSprite;
  
    }
  

}
