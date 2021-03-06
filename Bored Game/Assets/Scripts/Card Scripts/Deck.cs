﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {
    public Card[] cards;
    public List<Card> UnoDeck = new List<Card>();
    public int cardsLeft;

    public void CreateDeck()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            UnoDeck.Add(cards[i]);
            if (cards[i].suit == 4) {
                UnoDeck.Add(cards[i]);
                UnoDeck.Add(cards[i]);
            }
            if(cards[i].number != 0)
            {
                UnoDeck.Add(cards[i]);
            }
        }
        cardsLeft = UnoDeck.Count;
        print(cardsLeft);
    }

    public Card Draw()
    {
        Card nextCard = UnoDeck[UnoDeck.Count - 1];
        UnoDeck.RemoveAt(UnoDeck.Count - 1);
        cardsLeft -= 1;
        return nextCard;
    }

    public List<Card> ReShuffle(Deck Cards)
    {
        for (int i = 0; i< Cards.GetComponent<Deck>().UnoDeck.Count - 1; i++)
        {
            UnoDeck.Add(Cards.GetComponent<Deck>().UnoDeck[i]);
            Cards.GetComponent<Deck>().UnoDeck.RemoveAt(i);
            i--;
        }
        cardsLeft = UnoDeck.Count;
        Shuffle(3);
        return UnoDeck;
    }

    public List<Card> Shuffle(int numShuffle)  
    {
        if (numShuffle == 0)
        {
            return UnoDeck;
        }
        for (int i = 0; i < UnoDeck.Count; i++)
        {
            int randNo = Random.Range(0, UnoDeck.Count);
            Card temp = UnoDeck[i];
            UnoDeck[i] = UnoDeck[randNo];
            UnoDeck[randNo] = temp;
        }
        Shuffle(numShuffle - 1);
        return UnoDeck;
    }
}
