using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGame : MonoBehaviour
{
    /* 
     * 0 - Club
     * 1 - Diamond
     * 2 - Heart
     * 3 - Spade
     */

    private string[] cards = new string[] {
        "0_2", "0_3", "0_4", "0_5", "0_6", "0_7", "0_8", "0_9", "0_10", "0_11", "0_12", "0_13", "0_14",
        "1_2", "1_3", "1_4", "1_5", "1_6", "1_7", "1_8", "1_9", "1_10", "1_11", "1_12", "1_13", "1_14",
        "2_2", "2_3", "2_4", "2_5", "2_6", "2_7", "2_8", "2_9", "2_10", "2_11", "2_12", "2_13", "2_14",
        "3_2", "3_3", "3_4", "3_5", "3_6", "3_7", "3_8", "3_9", "3_10", "3_11", "3_12", "3_13", "3_14" };

    private string[] communityCards = new string[5];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
