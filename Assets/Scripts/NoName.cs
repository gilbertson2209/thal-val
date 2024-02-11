using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class OnChoice : MonoBehaviour
//{
//    // Awake is called before script is enabled

//    public TextMeshProUGUI alert;

//    private string banditName;
//    private int[] listylist;
//    private int indexer;

//    private int prizeVal;
//    //if(index - 1 > -1)
//    //    prev = list[index - 1];
//    //if(index + 1 < list.Length)
//    //   next = list[index + 1];
//    //SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
//    //spriteRenderer.sprite = Resources.Load<Sprite>("nameSprite");

//    private void Awake()
//    {
//        Debug.Log(gameObject.name);
//        listylist = new int[] { 1, 2, 3, 4, 5, 6 };
//        indexer = 0;

//        prizeVal = 0;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        Debug.Log("On Start");
//        alert.text = "ON START";

//        banditName = gameObject.name;
//    }

//    // Update is called once per frame
//    void OnMouseDown()

//    {
//        Debug.Log(banditName);
//        alert.text = "You chose " + banditName + " you win " + listylist[indexer].ToString();
//        indexer++;
//    }
//}

