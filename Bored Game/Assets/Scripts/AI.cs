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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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

    public void PlayCard ()
    {

    }
}
