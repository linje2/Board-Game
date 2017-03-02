using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {
    public ArrayList UnoDeck = new ArrayList();
    public Card[] cards;
   // public ArrayList CardsPlayed = new ArrayList();
    public int cardsLeft;

    void Start()
    {

    }

    public void CreateDeck()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            UnoDeck.Add(cards[i]);
        }
        Shuffle();
    }

    public int CardsLeft()
    {
        return cardsLeft;
    } 

    public Card Draw()
    {
        Card nextCard = (Card)UnoDeck[UnoDeck.Count - 1];
        UnoDeck.RemoveAt(UnoDeck.Count - 1);
        return nextCard;
    }

    public void Shuffle() //using recursion 
    {
        
    }
}
