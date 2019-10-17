using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    List<Player> players;
    Cards cards;
    
    Sprite sprite;
    public int playerCount;
    GameObject go;
    SpawnCards spawnCards;
    Vector3 position;
  public  GameObject CardPrefab;
    public float speed;
    float step;

    private void GiveHand(int round)
    {
        int i = 1;
        foreach(Player player in players)
        {
            player.hand.Add(cards.DeckOfCards[0]);

                GameObject newCard;
              

            newCard = Instantiate(CardPrefab);

            newCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/cards/" + cards.DeckOfCards[0].CardName);
               
                position =  GameObject.Find("Seat" + i).transform.position;
                Debug.Log(position);

            newCard.transform.position = Vector3.MoveTowards(transform.position, position, step);
                
            if(round == 1)
            {
                Vector3 newPosition = new Vector3(position.x + 1f, position.y, position.z);
                newCard.transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            }
           

            SpriteRenderer renderer =  newCard.GetComponent<SpriteRenderer>();
                renderer.sprite = newCard.GetComponent<SpriteRenderer>().sprite;
                renderer.sortingLayerName = "Cards";

            

            cards.DeckOfCards.RemoveAt(0);
            i++;
        }
    }

    private void Start()
    {
        step = speed * Time.deltaTime * 10f;
        spawnCards = FindObjectOfType<SpawnCards>();
        cards = FindObjectOfType<Cards>();
        players = new List<Player>();

        
        sprite = GetComponent<Sprite>();

        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new Player());
        }

       

       
        for (int i = 0; i < 2; i++)
            GiveHand(i);

        
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
