using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AI : MonoBehaviour
{

    public int Plus4CardsLeft = 4;
    public int ColorPickerLeft = 4;
    public int Plus2CardsLeft = 8;
    public int BlueCardsLeft = 25;
    public int GreenCardsLeft = 25;
    public int RedCardsLeft = 25;
    public int YellowCardsLeft = 25;

    public int turnNumber;

    private float randSec;

    public int ChooseColor()
    {
        int[] suits = new int[4];
        for (int i = 0; i < GetComponent<Hand>().Cards.Count; i++)
        {
            if (GetComponent<Hand>().Cards[i].suit != 4)
            {
                suits[GetComponent<Hand>().Cards[i].suit]++;
            }
        }
        int max = 0;
        for (int j = 0; j < suits.Length; j++)
        {
            if (suits[max] < suits[j])
                max = j;
        }
        return max;
    }


    public void ResetCardsLeft(Card MiddleCard)
    {
        int number;
        int suit;
        for (int i = 0; i <= this.GetComponent<Hand>().Cards.Count; i++)
        {
            if (i == this.GetComponent<Hand>().Cards.Count)
            {
                number = MiddleCard.number;
                suit = MiddleCard.suit;
            }
            else
            {
                number = this.GetComponent<Hand>().Cards[i].number;
                suit = this.GetComponent<Hand>().Cards[i].suit;
            }
            if (number == -4)
            {
                Plus4CardsLeft++;
            }
            if (number == -3)
            {
                Plus2CardsLeft++;
            }
            if (number == -5)
            {
                ColorPickerLeft++;
            }
            if (suit == 0)
            {
                BlueCardsLeft--;
            }
            else if (suit == 1)
            {
                GreenCardsLeft--;
            }
            else if (suit == 2)
            {
                RedCardsLeft--;
            }
            else if (suit == 3)
            {
                YellowCardsLeft--;
            }
        }
    }

    public void CheckCardsLeft(Card CardPlayed)
    {
        int number = CardPlayed.number;
        int suit = CardPlayed.suit;
        if (number == -4)
        {
            Plus4CardsLeft--;
        }
        if (number == -3)
        {
            Plus2CardsLeft--;
        }
        if (number == -5)
        {
            ColorPickerLeft--;
        }
        if (suit == 0)
        {
            BlueCardsLeft--;
        }
        else if (suit == 1)
        {
            GreenCardsLeft--;
        }
        else if (suit == 2)
        {
            RedCardsLeft--;
        }
        else if (suit == 3)
        {
            YellowCardsLeft--;
        }
    }

    public bool DrawTwoOrFour(Card card)
    {
        for (int i = 0; i < this.GetComponent<Hand>().Cards.Count; i++)
        {
            if (card.number == GetComponent<Hand>().Cards[i].number)
            {
                this.GetComponent<Hand>().cardSelected = i;
                this.GetComponent<Hand>().SelectCard();
                return true;
            }
        }
        return false;
    }

    public List<List<Card>> results = new List<List<Card>>();
    public void PlayCard(Card MiddleCard, GameObject NextPlayer, GameObject PlayerBefore)
    {
        List<Card> Hand = new List<Card>(this.GetComponent<Hand>().Cards);
        List<Card> result = ChooseCard(Hand, MiddleCard);
        if (NextPlayer.GetComponent<Hand>().Cards.Count < 3 && result.Count < Hand.Count)
        {
            List<Card> PlusTwo = new List<Card>();
            List<Card> PlusFour = new List<Card>();
            List<Card> Skip = new List<Card>();
            List<Card> Reverse = new List<Card>();
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].number == -3)
                {
                    PlusTwo.Add(Hand[i]);
                    AddToSelect(PlusTwo);
                    break;
                }
                else if (Hand[i].number == -4)
                {
                    PlusFour.Add(Hand[i]);
                    AddToSelect(PlusFour);
                    break;
                }
                else if (Hand[i].number == -2)
                {
                    Skip.Add(Hand[i]);
                    AddToSelect(Skip);
                    break;
                }
                else if (Hand[i].number == -1)
                {
                    Reverse.Add(Hand[i]);
                    AddToSelect(Reverse);
                    break;
                }
            }
            if (PlusTwo.Count == 0 && PlusFour.Count == 0 && Skip.Count == 0 && Reverse.Count == 0)
            {
                AddToSelect(result);
            }
        }
        if (PlayerBefore.GetComponent<Hand>().Cards.Count < 3 && result.Count < Hand.Count)
        {
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].number == -1)
                {
                    Hand.RemoveAt(i);
                    i--;
                }
            }
            result = ChooseCard(Hand, MiddleCard);
        }
        AddToSelect(result);
    }

    public List<Card> ChooseCard(List<Card> Hand, Card MiddleCard)
    {
        for (int a = 0; a < Hand.Count; a++)
        {
            if (Hand[a].suit == MiddleCard.suit || Hand[a].number == MiddleCard.number)
            {
                List<Card> chain = new List<Card>();
                chain.Add(Hand[a]);
                List<Card> remaining = new List<Card>(Hand);
                remaining.Remove(Hand[a]);
                subclass(chain, remaining);
            }
        }

        if (results.Count > 0)
        {
            int max = 0;
            for (int i = 1; i < results.Count; i++)
            {
                if (results[max].Count < results[i].Count)
                {
                    results.RemoveAt(max);
                    max = i;
                }
                else
                {
                    results.RemoveAt(i);
                }
            }
        }

        for (int i = 0; i < results.Count; i++)
        {
            print("New Chain");
            for (int j = 0; j < results[i].Count; j++)
            {
                List<Card> card = results[i];
                print(card[j].suit + ", " + card[j].number);
            }
        }
        List<Card> result = new List<Card>();
        if (results.Count > 0)
        {
            result = results[0];
            int bestColor = ChooseColor();
            for (int j = 0; j < result.Count; j++)
            {
                if (result[j].suit == bestColor)
                {
                    Card temp = result[j];
                    result[j] = result[result.Count - 1];
                    result[result.Count - 1] = temp;
                }
            }
        }
        results.Clear();
        return result;
    }

    public void subclass(List<Card> chain, List<Card> remaining)
    {
        if (remaining.Count == 0)
        {
            results.Add(chain);
        }
        else
        {
            for (int b = 0; b < chain.Count; b++)
            {
                if (chain[0].number == remaining[b].number)
                {
                    List<Card> temp = new List<Card>(chain);
                    temp.Add(remaining[b]);
                    List<Card> tempRemaining = new List<Card>(remaining);
                    tempRemaining.Remove(remaining[b]);
                    subclass(temp, tempRemaining);
                }
                else
                {
                    results.Add(chain);
                    break;
                }
            }
        }
    }

    void AddToSelect(List<Card> ToAdd)
    {
        for (int i = 0; i < ToAdd.Count; i++)
        {
            this.GetComponent<Hand>().cardSelected = this.GetComponent<Hand>().Cards.IndexOf(ToAdd[i]);
            this.GetComponent<Hand>().SelectCard();
        }
    }
}
