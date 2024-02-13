//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class OnChoice : MonoBehaviour
{

    public GameManager GameManager;

    public bool isChosen = false;
    private bool ifChosenFromGM = false; 
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
        // May not actually use 
        if (ifChosenFromGM)
        {
            // not real; to test sprite change & animation 
            elapsed_time += Time.deltaTime;
            if (elapsed_time > 4) {

                ResetChoice();

            }
        }
    }

    void OnMouseDown()
        // when bandit is selected... 
    {
        // alert GameManager that this Bandit is clicked 
        GameManager.BanditChoice(banditName);



        // locally ....
        ifChosenFromGM = GameManager.choiceMade;
        // ...set bool ifChosen to true
        isChosen = true;
        // ...change the sprite to 'chosen' version
        spriteRenderer.sprite = chosenBanditSprite;
        // ...rotate it to give appearance of animation
        InvokeRepeating("RotateSprite", 0.2f, 0.4f);
        // ...begin counter for testing - REMOVE ME 
        elapsed_time = 0f;
        Debug.Log(ifChosenFromGM);
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
