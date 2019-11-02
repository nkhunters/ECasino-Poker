using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
   public List<Cards.Card> hand = new List<Cards.Card>(5);
    public float Chips = 1000;
    public float chipsOnTable = 0;
    public int playerId;
    public int rank;

   public Player(float chips, int playerid)
    {
        Chips = chips;
        playerId = playerid;
    }
        
   

}
