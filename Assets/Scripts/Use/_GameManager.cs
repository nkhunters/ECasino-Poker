using System;
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
    public Text Winner;
    Sprite sprite;
    public int playerCount;
    GameObject go;
    SpawnCards spawnCards;
    Vector3 position;
    Vector3 DealerPosition;
    Vector3 CommPos;
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
    public int RaiseValue;
    public int PotValue;
    public Text Money;
    public Image TimeBar;
    public Slider Slider;
    private bool playerChecked;
    public GameObject checkBtn;
    public GameObject betButton1;
    public GameObject betButton2;
    public GameObject callButton;
    public GameObject raiseButton;
    private int rounds;
    private int numberOfChecks;

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
        TimeBar = FindObjectOfType<Image>();
        turnIsComplete = false;
        timeLeft = turnTime;
        BigBlindChips = 2 * SmallBlindChips;
        playerChecked = false;
        rounds = 0;
        numberOfChecks = 0;


        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new Player(1000, i));
        }



        for (int i = 0; i < 2; i++)
            GiveHand(i);


        GiveCommCards();
        PrintStatus();

        SetPlayPositions();
    }

    private void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            TimeBar.fillAmount = timeLeft / turnTime;
        }

        else
        {
            Fold();
             currentPlayer++;
             Play(currentPlayer);
            timeLeft = turnTime;

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
        Dealer = UnityEngine.Random.Range(0, players.Count);
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

        currentPlayer = SmallBlindIndex;
        Play(currentPlayer);

        GameObject dealer = Instantiate(DealerPrefab);
        dealer.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/dealer button");

        SpriteRenderer renderer = dealer.GetComponent<SpriteRenderer>();
        renderer.sprite = dealer.GetComponent<SpriteRenderer>().sprite;
        renderer.sortingLayerName = "Dealer";

        if(Dealer == 0 )
        DealerPosition = GameObject.Find("Seat" + Dealer).transform.position;
          Vector3 newPos0 = new Vector3(dealer.transform.position.x + 100, DealerPosition.y, DealerPosition.z);
        //Vector3 toAdd = new Vector3(100, 0, 0);
        dealer.transform.position = Vector3.MoveTowards(transform.position,newPos0, step);


        if (Dealer == 1)
            DealerPosition = GameObject.Find("Seat" + Dealer).transform.position;
        Vector3 newPos1 = new Vector3(dealer.transform.position.x - 100, DealerPosition.y, DealerPosition.z);
        dealer.transform.position = Vector3.MoveTowards(DealerPosition, newPos1, step);

        if (Dealer == 2 )
            DealerPosition = GameObject.Find("Seat" + Dealer).transform.position;
          Vector3  newPos2 = new Vector3(DealerPosition.x , dealer.transform.position.y + 100, DealerPosition.z);
            dealer.transform.position = Vector3.MoveTowards(DealerPosition, newPos2, step);

        if (Dealer == 3)
            DealerPosition = GameObject.Find("Seat" + Dealer).transform.position;
       Vector3 newPos3 = new Vector3(DealerPosition.x, dealer.transform.position.y - 100, DealerPosition.z);
        dealer.transform.position = Vector3.MoveTowards(DealerPosition, newPos3, step);

        if (Dealer == 4 || Dealer == 5)
            DealerPosition = GameObject.Find("Seat" + Dealer).transform.position;
           Vector3 newPos4 = new Vector3(DealerPosition.x, DealerPosition.y, DealerPosition.z);
            dealer.transform.position = Vector3.MoveTowards(DealerPosition, newPos4, step);

        if (Dealer == 6 || Dealer == 7)
            DealerPosition = GameObject.Find("Seat" + Dealer).transform.position;
            Vector3 newPos5 = new Vector3(DealerPosition.x, DealerPosition.y, DealerPosition.z);
             dealer.transform.position = Vector3.MoveTowards(DealerPosition, newPos5, step);


    }
    void Play(int currPlayer)
    {
        if (currentPlayer >= players.Count)
        {
            currentPlayer = 0;
        }
        current_player = players[currentPlayer];

        if (currentPlayer == SmallBlindIndex && rounds == 0)
            CallValue = SmallBlindChips;
        else
            CallValue = BigBlindChips;


        if(current_player.Chips > BigBlindChips)
        {
            Money.text = BigBlindChips.ToString();
            Slider.minValue = CallValue;
            Slider.maxValue = current_player.Chips;
        }


    }

   public void Call()
    {
        current_player.Chips -= CallValue;
        current_player.chipsOnTable += CallValue;
        Debug.Log(current_player.Chips);
        PlayerLog.Add("Player " + current_player.playerId + " Called " + CallValue + " Chips");
        Debug.Log("Player " + current_player.playerId + " Called " + CallValue + " Chips");
        PotValue += CallValue;
        Debug.Log(PotValue);
        currentPlayer += 1;
        Play(currentPlayer);

        if (Rounds())
        {
            rounds++;
            StartCommRound();

            Debug.Log("Round is Completed");
        }

    }

    public void Fold()
    {   
        players.Remove(players[currentPlayer]);
        Debug.Log("Player " + current_player.playerId + " Folded " );
        currentPlayer += 1;
        Play(currentPlayer);
     //   timeLeft = turnTime;

        if(players.Count == 1)
        {
            current_player.Chips += PotValue;
            Debug.Log("Player " + current_player.playerId + " WON " + current_player.Chips);
            Winner.text = "Player " + current_player.playerId + " WON " + current_player.Chips;
        }

    }
    
    public void Raise()
    {
        current_player.Chips -= Slider.value;
        current_player.chipsOnTable += Slider.value;
        PotValue += Mathf.RoundToInt(Slider.value);

        PlayerLog.Add("Player " + current_player.playerId + " Raised " + Slider.value + " Chips");
        Debug.Log("Player " + current_player.playerId + " Raised " + Slider.value + " Chips");

        currentPlayer += 1;
        Play(currentPlayer);

        if (Rounds())
        {
            rounds++;
            StartCommRound();
            Debug.Log("Round is Completed");
        }
    }

   
     public void onSliderValueChanged(float value)
    {
        Money.text = value.ToString();
    }

    public void Check()
    {
        PlayerLog.Add("Player " + current_player.playerId + " Checked " );
        Debug.Log("Player " + current_player.playerId + " Checked");

        playerChecked = true;
        numberOfChecks += 1;
        currentPlayer += 1;
        Play(currentPlayer);

        if(numberOfChecks == players.Count)
        {
            rounds++;
            StartCommRound();
            Debug.Log("Round is completed");
        }
    }

    public void Bet()
    {
        current_player.Chips -= Slider.value;
        current_player.chipsOnTable += Slider.value;
        PotValue += Mathf.RoundToInt(Slider.value);

        PlayerLog.Add("Player " + current_player.playerId + "Bet " + Slider.value + " Chips");
        Debug.Log("Player " + current_player.playerId + " Bet " + Slider.value + " Chips");

        checkBtn.SetActive(false);
        raiseButton.SetActive(true);
        betButton1.SetActive(false);
        betButton2.SetActive(true);

        currentPlayer += 1;
        Play(currentPlayer);

        if (Rounds())
        {
            rounds++;
            StartCommRound();

            Debug.Log("Round is Completed");

        }


    }

   public bool Rounds()
    {
        int k = 0;
        int sum = 0;
        bool status = false;

        for (int i = 0; i < players.Count; i++)   
        {
            for(int j = i+1; j < players.Count; j++)
            {
                if(players[i].chipsOnTable == players[j].chipsOnTable)
                {
                   k++;
                }
            
            }
        }

       for(int i = players.Count - 1; i > 0; i--)
        {
            for (int j = i; j > 0; j--)
            {
                sum += 1;
            }
        }

       if( k == sum)
        {
            status = true;
        }

       return status;
    }

    void GiveCommCards()
    {
        for (int i = 0; i < 5; i++)
        {
            cards.CommunityCards.Add(cards.DeckOfCards[0]);
            cards.DeckOfCards.RemoveAt(0);
        }
    }

    public void StartCommRound()
    {
        Debug.Log("Round  no - " + rounds);
        numberOfChecks = 0;

        if (rounds == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject commcard = Instantiate(CardPrefab);

                commcard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/cards/" + cards.CommunityCards[i].CardName);

                SpriteRenderer renderer = commcard.GetComponent<SpriteRenderer>();
                renderer.sprite = commcard.GetComponent<SpriteRenderer>().sprite;
                renderer.sortingLayerName = "Cards";


                CommPos = GameObject.Find("CommCardPos" + i).transform.position;
                commcard.transform.position = Vector3.MoveTowards(transform.position, CommPos, step);
            }
        }
        
            if(rounds == 2)
            {
                GameObject commcard = Instantiate(CardPrefab);

                commcard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/cards/" + cards.CommunityCards[3].CardName);

              SpriteRenderer renderer = commcard.GetComponent<SpriteRenderer>();
             renderer.sprite = commcard.GetComponent<SpriteRenderer>().sprite;
             renderer.sortingLayerName = "Cards";


                CommPos = GameObject.Find("CommCardPos" + 3).transform.position;
                commcard.transform.position = Vector3.MoveTowards(transform.position, CommPos, step);
            }

        if (rounds == 3)
        {
            GameObject commcard = Instantiate(CardPrefab);

            commcard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/cards/" + cards.CommunityCards[4].CardName);

            SpriteRenderer renderer = commcard.GetComponent<SpriteRenderer>();
            renderer.sprite = commcard.GetComponent<SpriteRenderer>().sprite;
            renderer.sortingLayerName = "Cards";


            CommPos = GameObject.Find("CommCardPos" + 4).transform.position;
            commcard.transform.position = Vector3.MoveTowards(transform.position, CommPos, step);  
        }

        if(rounds >= 3)
        {
            SetRank();
        }

        checkBtn.SetActive(true);
        betButton1.SetActive(true);
        callButton.SetActive(false);
        raiseButton.SetActive(false);

    }
    
    
    public bool RoyalFlush(Player player)
    {
        var RF = new List<string> { "10", "11", "12", "13", "14" };

        var newCommCards = new List<string>();

        foreach(Cards.Card card in cards.CommunityCards)
        {
            string cardValue = card.CardName.Split('_')[1];
            newCommCards.Add(cardValue);
        }

        Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue = cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue = cardTwo.CardName.Split('_')[1];

        newCommCards.Add(cardOneValue);
        newCommCards.Add(cardTwoValue);

        if (! cardOneSuite.Equals(cardTwoSuite))
        {
            return false;
        }

       else if(! RF.Contains(cardOneValue) && RF.Contains(cardTwoValue))
        {
            return false;
        }

        else if(newCommCards.Contains("10") && newCommCards.Contains("11") 
            && newCommCards.Contains("12") && newCommCards.Contains("13")
            && newCommCards.Contains("14"))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool StraightFlush(Player player)
    {
        if (Straight(player) && Flush(player))
            return true;
        else return false;
    }

    public bool FourOfAKind(Player player)
    {
        var newCommCards = new List<int>();

        foreach (Cards.Card card in cards.CommunityCards)
        {
            string cardValue = card.CardName.Split('_')[1];
            newCommCards.Add(Int32.Parse(cardValue));
        }

        Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue = cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue = cardTwo.CardName.Split('_')[1];

        newCommCards.Sort();

        var list1 = new List<int>();
        list1.Add(newCommCards[0]);
        list1.Add(newCommCards[1]);
        list1.Add(newCommCards[2]);
        list1.Add(Int32.Parse(cardOneValue));
        list1.Add(Int32.Parse(cardTwoValue));

        var list2 = new List<int>();
        list2.Add(newCommCards[1]);
        list2.Add(newCommCards[2]);
        list2.Add(newCommCards[3]);
        list2.Add(Int32.Parse(cardOneValue));
        list2.Add(Int32.Parse(cardTwoValue));

        var list3 = new List<int>();
        list3.Add(newCommCards[2]);
        list3.Add(newCommCards[3]);
        list3.Add(newCommCards[4]);
        list3.Add(Int32.Parse(cardOneValue));
        list3.Add(Int32.Parse(cardTwoValue));

        list1.Sort();
        list2.Sort();
        list3.Sort();

        if (checkFourOfKind(list1))
            return true;
        else if (checkFourOfKind(list2))
            return true;
        else if (checkFourOfKind(list3))
            return true;
        else
            return false;
    }

    public bool FullHouse(Player player)
    {
        
        Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue = cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue = cardTwo.CardName.Split('_')[1];

        if (!cardOneValue.Equals(cardTwoValue))
        {
            return false;
        }

        else if (ThreeOfAKind(player) && OnePair(player))
                return true;
        else return false;
    }

    public bool Flush(Player player)
    {

        var newCommCards = new List<int> ();

        foreach (Cards.Card card in cards.CommunityCards)
        {
            string cardSuite = card.CardName.Split('_')[0];
            newCommCards.Add(Int32.Parse(cardSuite));
        }

        Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue = cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue = cardTwo.CardName.Split('_')[1];

        if (!cardOneSuite.Equals(cardTwoSuite))
        {
            return false;
        }

        newCommCards.Sort();

        var list1 = new List<int>();
        list1.Add(newCommCards[0]);
        list1.Add(newCommCards[1]);
        list1.Add(newCommCards[2]);
        list1.Add(Int32.Parse(cardOneSuite));
        list1.Add(Int32.Parse(cardTwoSuite));

        var list2 = new List<int>();
        list2.Add(newCommCards[1]);
        list2.Add(newCommCards[2]);
        list2.Add(newCommCards[3]);
        list2.Add(Int32.Parse(cardOneSuite));
        list2.Add(Int32.Parse(cardTwoSuite));

        var list3 = new List<int>();
        list3.Add(newCommCards[2]);
        list3.Add(newCommCards[3]);
        list3.Add(newCommCards[4]);
        list3.Add(Int32.Parse(cardOneSuite));
        list3.Add(Int32.Parse(cardTwoSuite));

        list1.Sort();
        list2.Sort();
        list3.Sort();

        if (list1[0] == list1[1] && list1[0] == list1[2] && list1[0] == list1[3] && list1[0] == list1[4])
            return true;
        else if (list2[0] == list2[1] && list2[0] == list2[2] && list2[0] == list2[3] && list2[0] == list2[4])
            return true;
        else if (list3[0] == list3[1] && list3[0] == list3[2] && list3[0] == list3[3] && list3[0] == list3[4])
            return true;
        else
            return false;
    }

    public bool Straight(Player player)
    {

        var newCommCards = new List<int>();

        foreach (Cards.Card card in cards.CommunityCards)
        {
            string cardValue = card.CardName.Split('_')[1];
            newCommCards.Add(Int32.Parse(cardValue));
        }

        Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue = cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue = cardTwo.CardName.Split('_')[1];

        newCommCards.Sort();

        var list1 = new List<int>();
        list1.Add(newCommCards[0]);
        list1.Add(newCommCards[1]);
        list1.Add(newCommCards[2]);
        list1.Add(Int32.Parse(cardOneValue));
        list1.Add(Int32.Parse(cardTwoValue));

        var list2 = new List<int>();
        list2.Add(newCommCards[1]);
        list2.Add(newCommCards[2]);
        list2.Add(newCommCards[3]);
        list2.Add(Int32.Parse(cardOneValue));
        list2.Add(Int32.Parse(cardTwoValue));

        var list3 = new List<int>();
        list3.Add(newCommCards[2]);
        list3.Add(newCommCards[3]);
        list3.Add(newCommCards[4]);
        list3.Add(Int32.Parse(cardOneValue));
        list3.Add(Int32.Parse(cardTwoValue));

        list1.Sort();
        list2.Sort();
        list3.Sort();

        if (!list1.Select((i, j) => i - j).Distinct().Skip(1).Any())
            return true;
        else if (!list2.Select((i, j) => i - j).Distinct().Skip(1).Any())
            return true;
        else if (!list3.Select((i, j) => i - j).Distinct().Skip(1).Any())
            return true;
        else
            return false;
    }

    public bool ThreeOfAKind(Player player)
    {
        var newCommCards = new List<int> ();
      

        foreach (Cards.Card card in cards.CommunityCards)
        {
            string cardValue = card.CardName.Split('_')[1];
            newCommCards.Add(Int32.Parse(cardValue));
        }

         Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue = cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue = cardTwo.CardName.Split('_')[1];

        newCommCards.Sort();

        var list1 = new List<int>();
        list1.Add(newCommCards[0]);
        list1.Add(newCommCards[1]);
        list1.Add(newCommCards[2]);
        list1.Add(Int32.Parse(cardOneValue));
        list1.Add(Int32.Parse(cardTwoValue));

        var list2 = new List<int>();
        list2.Add(newCommCards[1]);
        list2.Add(newCommCards[2]);
        list2.Add(newCommCards[3]);
        list2.Add(Int32.Parse(cardOneValue));
        list2.Add(Int32.Parse(cardTwoValue));

        var list3 = new List<int>();
        list3.Add(newCommCards[2]);
        list3.Add(newCommCards[3]);
        list3.Add(newCommCards[4]);
        list3.Add(Int32.Parse(cardOneValue));
        list3.Add(Int32.Parse(cardTwoValue));

        list1.Sort();
        list2.Sort();
        list3.Sort();

        if (checkThreeOfKind(list1))
           return true;
        else if (checkThreeOfKind(list2))
            return true;
        else if (checkThreeOfKind(list3))
            return true;
        else
            return false; 
    }

    public bool TwoPair(Player player)
    {
        var newCommCards = new List<int>();

        foreach (Cards.Card card in cards.CommunityCards)
        {
            string cardValue = card.CardName.Split('_')[1];
            newCommCards.Add(Int32.Parse(cardValue));
        }

        Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue =  cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue =  cardTwo.CardName.Split('_')[1];

        newCommCards.Sort();

        var list1 = new List<int>();
        list1.Add(newCommCards[0]);
        list1.Add(newCommCards[1]);
        list1.Add(newCommCards[2]);
        list1.Add(Int32.Parse(cardOneValue));
        list1.Add(Int32.Parse(cardTwoValue));

        var list2 = new List<int>();
        list2.Add(newCommCards[1]);
        list2.Add(newCommCards[2]);
        list2.Add(newCommCards[3]);
        list2.Add(Int32.Parse(cardOneValue));
        list2.Add(Int32.Parse(cardTwoValue));

        var list3 = new List<int>();
        list3.Add(newCommCards[2]);
        list3.Add(newCommCards[3]);
        list3.Add(newCommCards[4]);
        list3.Add(Int32.Parse(cardOneValue));
        list3.Add(Int32.Parse(cardTwoValue));

        list1.Sort();
        list2.Sort();
        list3.Sort();

        if (checkTwoPair(list1))
            return true;
        else if (checkTwoPair(list2))
            return true;
        else if (checkTwoPair(list3))
            return true;
        else
            return false; 
    }

    public bool OnePair(Player player)
    {
        var newCommCards = new List<int>();
        
        foreach (Cards.Card card in cards.CommunityCards)
        {
            string cardValue = card.CardName.Split('_')[1];
            newCommCards.Add(Int32.Parse(cardValue));
        }

        Cards.Card cardOne = player.hand[0];
        Cards.Card cardTwo = player.hand[1];

        string cardOneSuite = cardOne.CardName.Split('_')[0];
        string cardOneValue = cardOne.CardName.Split('_')[1];

        string cardTwoSuite = cardTwo.CardName.Split('_')[0];
        string cardTwoValue = cardTwo.CardName.Split('_')[1];

        newCommCards.Sort();

        var list1 = new List<int>();
        list1.Add(newCommCards[0]);
        list1.Add(newCommCards[1]);
        list1.Add(newCommCards[2]);
        list1.Add(Int32.Parse(cardOneValue));
        list1.Add(Int32.Parse(cardTwoValue));

        var list2 = new List<int>();
        list2.Add(newCommCards[1]);
        list2.Add(newCommCards[2]);
        list2.Add(newCommCards[3]);
        list2.Add(Int32.Parse(cardOneValue));
        list2.Add(Int32.Parse(cardTwoValue));

        var list3 = new List<int>();
        list3.Add(newCommCards[2]);
        list3.Add(newCommCards[3]);
        list3.Add(newCommCards[4]);
        list3.Add(Int32.Parse(cardOneValue));
        list3.Add(Int32.Parse(cardTwoValue));

        list1.Sort();
        list2.Sort();
        list3.Sort();

        if (checkPair(list1))
            return true;
        else if (checkPair(list2))
            return true;
        else if (checkPair(list3))
            return true;
        else
            return false; 
    }

    private bool checkFourOfKind(List<int> list)
    {
        bool result = false;
        for (int i = 0; i < list.Count - 3; i++)
        {
            if ((list[i] == list[i + 1]) && (list[i] == list[i + 2]) && (list[i] == list[i + 3]))
            {
                result = true;
                break;
            }
        }
        return result;
    }

    private bool checkThreeOfKind(List<int> list)
    {
        bool result = false;
         for(int i = 0; i<list.Count - 2; i++ )
        {
            if((list[i] == list[i+1]) && (list[i] == list[i+2]))
            {
                result = true;
                break;
            }
        }
        return result;
    }

    private bool checkPair(List<int> list)
    {
        bool result = false;
        for (int i = 0; i < list.Count - 1 ; i++)
        {
            if (list[i] == list[i + 1])
            {
                result = true;
                break;
            }
        }
        return result;
    }

    private bool checkTwoPair(List<int> list)
    {
        int noOfPairs = 0;
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i] == list[i + 1])
            {
                noOfPairs++ ;
               // i += 2;
            }
        }

        if (noOfPairs >= 2)
            return true;
        else return false;
    }

    public void SetRank()
    {
        Debug.Log("in set rank");
        foreach (Player player in players)
        {
            player.rank = 0;

            if (OnePair(player))
                player.rank = 1;
            if (TwoPair(player))
                player.rank = 2;
            if (ThreeOfAKind(player))
                player.rank = 3;
            if (Straight(player))
                player.rank = 4;
            if (Flush(player))
                player.rank = 5;
            if (FullHouse(player))
                player.rank = 6;
            if (FourOfAKind(player))
                player.rank = 7;
            if (StraightFlush(player))
                player.rank = 8;
            if (RoyalFlush(player))
                player.rank = 9;
        }

        Evaluate();
    }

    public void Evaluate()
    {
        List<Player> roundOneList = new List<Player>(players);

        List<Player> SortedList = players.OrderByDescending(o => o.rank).ToList();

        if(SortedList[0].rank == SortedList[1].rank)
        {
            Player playerOne = SortedList[0];
            Player playerTwo = SortedList[1];
            Player winner = SortedList[0];

            int playerOneHighCard, playerTwoHighCard;

            int playerOneFirstCard = Int32.Parse(playerOne.hand[0].CardName.Split('_')[0]);
            int playerOneSecondCard = Int32.Parse(playerOne.hand[1].CardName.Split('_')[1]);

            int playerTwoFirstCard = Int32.Parse(playerTwo.hand[0].CardName.Split('_')[0]);
            int playerTwoSecondCard = Int32.Parse(playerTwo.hand[1].CardName.Split('_')[1]);

            if (playerOneFirstCard > playerOneSecondCard)
                playerOneHighCard = playerOneFirstCard;
            else
                playerOneHighCard = playerOneSecondCard;

            if (playerTwoFirstCard > playerTwoSecondCard)
                playerTwoHighCard = playerTwoFirstCard;
            else
                playerTwoHighCard = playerTwoSecondCard;

            if(playerTwoHighCard > playerOneHighCard)
            {
                winner = SortedList[1];
            }

            showWinner(winner);
        }
        else
        {
            showWinner(SortedList[0]);
        }

       /* for (int i = 0; i < players.Count; i++)
        {
            for (int j = i+1; j < players.Count; j++)
            {
                if (players[i].rank < players[j].rank)
                {
                    Player player = players[i];
                    players.Remove(player);
                }
                else
                {
                    Player player = players[j];
                    players.Remove(player);
                }
            }
        }*/

        foreach(Player player in SortedList)
        {
            Debug.Log("Player - " + player.playerId + " Rank - " + player.rank);
        }
    }

    private void showWinner(Player winner)
    {
        string strRank = "";
        switch (winner.rank)
        {
            case 9:
                strRank = "Royal Flush";
                break;
            case 8:
                strRank = "Straight Flush";
                break;
            case 7:
                strRank = "Four Of a Kind";
                break;
            case 6:
                strRank = "Full House";
                break;
            case 5:
                strRank = "Flush";
                break;
            case 4:
                strRank = "Straight";
                break;
            case 3:
                strRank = "Three of a kind";
                break;
            case 2:
                strRank = "Two Pair";
                break;
            case 1:
                strRank = "One Pair";
                break;

        }

        
        PlayerLog.Add("Player " + winner.playerId + " won " + PotValue + " chips with " + strRank);
        Debug.Log("Player " + winner.playerId + " won " + PotValue + " chips with " + strRank);
        Winner.text = "Winner " + winner.playerId + " won " + PotValue + " chips with " + strRank;
    }

  
}


