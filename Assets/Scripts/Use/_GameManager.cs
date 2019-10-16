using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    List<Player> players;
    Cards cards;
    SpriteRenderer renderer;
    Sprite sprite;
    public int playerCount;
   
    

  private void GiveHand()
    {
        foreach(Player player in players)
        {
            player.hand.Add(cards.DeckOfCards[0]);
            cards.DeckOfCards.RemoveAt(0);
            
        }
    }

    private void Start()
    {
        cards = FindObjectOfType<Cards>();
        players = new List<Player>();

        renderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = GetComponent<Sprite>();

        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new Player());
        }

       

        sprite = Resources.Load<Sprite>("Sprites/cards/3_14");
        renderer.sprite = sprite;
      //  renderer.sortingLayerName = "Cards";
        Instantiate(gameObject.GetComponent<SpriteRenderer>().sprite = sprite, transform.position, Quaternion.identity);

        for (int i = 0; i < 2; i++)
            GiveHand();

        //SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        // card =  Instantiate(Resources.Load<Sprite>("Sprites/cards/CardBack"), transform.position, Quaternion.identity );
        // renderer.sprite = card;
        //   gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/cards/CardBack");
       
        //  gameObject.transform.localScale = new Vector3(46.84328f, 46.84328f, 46.84328f);

        GiveCommCards();
        PrintStatus();
    }

    void GiveCommCards()
    {
        for (int i = 0; i < 5; i++)
        {
            cards.CommunityCards.Add(cards.DeckOfCards[0]);
            cards.DeckOfCards.RemoveAt(0);
        }
    }

    void PrintStatus()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log("Player " + i + " Cards: ");
            players[i].hand.ForEach((Cards.Card card) => { Debug.Log(card.CardName); });

        }

        for (int i = 0; i < cards.CommunityCards.Count; i++)
            Debug.Log(cards.CommunityCards[i].CardName);
    }

}
