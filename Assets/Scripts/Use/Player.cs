using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
   public List<Cards.Card> hand = new List<Cards.Card>(5);
    public float Chips = 1000;

   public Player(float chips)
    {
        Chips = chips;
    }
}
