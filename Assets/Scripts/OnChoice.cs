//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class OnChoice : MonoBehaviour
{

    public GameManager GameManager;

    public bool isChosen = false;
    public Sprite chosenBanditSprite;

    // set following in inspector 
    private string banditName;
    private SpriteRenderer spriteRenderer;
    private Sprite banditSprite;

    //just for checking animation, delete & within update & onMouseDown 
    private float elapsed_time;


    private void Awake()
        //get bandit name & original sprite 
    {
        banditName = gameObject.name;
        spriteRenderer = GetComponent<SpriteRenderer>();
        banditSprite = spriteRenderer.sprite;
    }

    void Update()
    {     
        // Only run following IF bandit has been clicked
        // May not actually use but may use idk
        // actually may well use but may well not 
        if (isChosen)
        {
            

            // not real; to test sprite change & animation 
            elapsed_time += Time.deltaTime;
            if (elapsed_time > 4) {

                ResetChoice();

            }
        }
    }

    // On click, we set 'isChosen' to true
    // //(& should/ coul gamecontroller listens for this)
    // May do this, IDK; or may try pattern as in FlappyBird Tut 
    //GameManager.Instance.ObjectClicked(gameObject); 
    void OnMouseDown()
        // when bandit is selected... 

    {
        // ...set bool ifChosen to true
        isChosen = true;

        GameManager.BanditClicked(banditName);
       
        // ...change the sprite to 'chosen' version
        spriteRenderer.sprite = chosenBanditSprite;
        // ...rotate it to give appearance of animation
        InvokeRepeating("RotateSprite", 0.2f, 0.4f);
        // ...begin counter for testing 
        elapsed_time = 0f;
    }

    public void ResetChoice()
        // method to reset the bandit to unchosen state
    {
        // ...reset isChosen 
        isChosen = false;
        // ...and reset the sprite
        ResetSprite();
        
    }

    private void RotateSprite()
        // method to rotate the sprite (appearance of animation)
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
