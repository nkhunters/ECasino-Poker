using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class _GameManager : MonoBehaviour
{
    List<Player> players;
    List<string> PlayerLog;
    Cards cards;
   public int currentPlayer;
    Sprite sprite;
    public int playerCount;
    GameObject go;
    SpawnCards spawnCards;
    Vector3 position;
    Vector3 DealerPosition;
  public  GameObject CardPrefab;
    public GameObject DealerPrefab;
    public float speed;
    float step;
    bool turnIsComplete;
    public float turnTime;
    float timeLeft ;
    public int Dealer;
    private int SmallBlindIndex;
    private int BigBlindIndex;
    public int SmallBlindChips;
    public int BigBlindChips;
    private Player current_player;
    public int CallValue;
    public int PotValue;
    public Text Money;
    public Slider Slider;

    private void GiveHand(int round)
    {
        int i = 0;
        foreach(Player player in players)
        {
            player.hand.Add(cards.DeckOfCards[0]);

            GameObject newCard = Instantiate(CardPrefab);

            newCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/cards/" + cards.DeckOfCards[0].CardName);
               
            position =  GameObject.Find("Seat" + i).transform.position;  
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
        PlayerLog = new List<string>();
        sprite = GetComponent<Sprite>();
        Money = FindObjectOfType<Text>();
        turnIsComplete = false;
        timeLeft = turnTime;
        BigBlindChips = 2 * SmallBlindChips;


        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new Player(1000));
        }



        for (int i = 0; i < 2; i++)
            GiveHand(i);

        
        GiveCommCards();
        PrintStatus();

        SetPlayPositions();
    }

    private void Update()
    {
  //   current_player.Chips = Map(current_player.Chips, BigBlindChips, current_player.Chips, 0, 1);
     current_player.Chips = Slider.value;
     Money.text = current_player.Chips.ToString();
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

   

   void SetPlayPositions()
    {
        Dealer = Random.Range(0, players.Count);
        SmallBlindIndex = Dealer + 1;
        BigBlindIndex = SmallBlindIndex + 1;

        if (SmallBlindIndex >= players.Count)
        {
            SmallBlindIndex = 0;
            BigBlindIndex = SmallBlindIndex + 1;
        }

        if(BigBlindIndex >= players.Count)
        {
            BigBlindIndex = 0;
            SmallBlindIndex = players.Count - 1;
        }
       
        Debug.Log("Dealer" + Dealer);
        Debug.Log("SmallBlind" + SmallBlindIndex);
        Debug.Log("BigBlind" + BigBlindIndex);

        currentPlayer = BigBlindIndex;
        Play(currentPlayer);

        GameObject dealer = Instantiate(DealerPrefab);
        dealer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/dealer button");

        SpriteRenderer renderer = dealer.GetComponent<SpriteRenderer>();
        renderer.sprite = dealer.GetComponent<SpriteRenderer>().sprite;
        renderer.sortingLayerName = "Cards";


        DealerPosition = GameObject.Find("Seat" + Dealer).transform.position;
        dealer.transform.position = Vector3.MoveTowards(transform.position, DealerPosition, step);


    }
    void Play(int currPlayer)
    {
        if (currentPlayer >= players.Count)
        {
            currentPlayer = 0;
        }
        current_player = players[currentPlayer];
         CallValue = BigBlindChips;

    }

   public void Call()
    {
        current_player.Chips -= CallValue;
        Debug.Log(current_player.Chips);
        PlayerLog.Add("Player " + currentPlayer + " Called " + CallValue + " Chips");
        Debug.Log("Player " + currentPlayer + " Called " + CallValue + " Chips");
        PotValue += CallValue;
        Debug.Log(PotValue);
        currentPlayer += 1;
        Play(currentPlayer);
       
    }

    public void Fold()
    {   
        players.Remove(players[currentPlayer]);
        Debug.Log("Player " + currentPlayer + " Folded " );
        currentPlayer += 1;
        Play(currentPlayer);
     //   timeLeft = turnTime;

        if(players.Count == 1)
        {
            current_player.Chips += PotValue;
            Debug.Log("Player " + currentPlayer + " WON " + current_player.Chips);
        }

    }

    public void Raise()
    {

    }


   /* public void Map(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

    }
    */
    
}


