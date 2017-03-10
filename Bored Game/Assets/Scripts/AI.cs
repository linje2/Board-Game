using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {
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
        for (int j = 0; j< suits.Length; j++)
        {
            if (suits[max] < suits[j])
                max = j;
        }
        return max;
    }

    void ShoutUno()
    {
        randSec = Random.Range(0f, 2f);
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
    public void PlayCard ()
    {
      StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(4);
    }
}
