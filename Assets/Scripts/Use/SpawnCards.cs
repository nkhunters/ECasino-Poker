using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCards : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject newCard;

    private void Start()
    {
        

        newCard = Instantiate(CardPrefab);

        newCard.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/cards/CardBack");


        
        
    }
}
