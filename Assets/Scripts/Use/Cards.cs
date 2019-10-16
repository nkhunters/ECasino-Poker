using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{
    public List<Cards.Card> DeckOfCards;
    private Cards.Card[] cardsToAdd;
    public List<Cards.Card> CommunityCards;

    void Start()
    {
        DeckOfCards = new List<Cards.Card>(52);
        CommunityCards = new List<Cards.Card>(5);

        //for (int i = 0; i < 52; i++)
        // DeckOfCards.Add(cardsToAdd[i]);
        /* for (int j = 0; j < 4; j++)
             for (int k = 1; k <= 13; k++)
                 DeckOfCards.Add(j + "_" + k);*/

        for (int i = 0; i <= 3; i++)  //Loop for Suits

        {
            for (int j = 2; j <= 14; j++)  //Loop for Number.
            {
                string temp = "Sprites/cards/" + i + "_" + j;

                DeckOfCards.Add(new Cards.Card(i + "_" + j, Resources.Load<Sprite>(temp)));
            }
        }

        Shuffle();
    }

    private void Shuffle() {
        Cards.Card temp;

        for (int shuffleTimes = 0; shuffleTimes < 1000; shuffleTimes++)
        {
            for (int i = 0; i < 52; i++)
            {
                //swap the cards
                int secondCardIndex = Random.Range(1, 13);
                temp = DeckOfCards[i];
                DeckOfCards[i] = DeckOfCards[secondCardIndex];
                DeckOfCards[secondCardIndex] = temp;
            }
        }
    }
    [System.Serializable]
    public class Card   //A Card class we are going to use in Lists. A datatype.
    {
        public string CardName;
        public Sprite CardSprite;
        public Card( string name, Sprite newSprite)
        {
            CardName = name;
            CardSprite = newSprite;
        }
    }
}
