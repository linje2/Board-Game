using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
    public List<Card> Cards = new  List<Card>();
    public List<Card> CardInstances = new List<Card>();
    public int count;
    public int cardSelected = 0;
    public bool isShowing;

    public List<Card> SelectedCards = new List<Card>();
    public List<Card> SelectedInstances = new List<Card>();
    float increaseBy;
    float zCoordinate;

    void Start()
    {
        count = Cards.Count;
        zCoordinate = gameObject.transform.position.z;
        if (gameObject.transform.position.z < 0)
        {
            increaseBy = 0.5f;
        }
        else
        {
            increaseBy = -0.5f;
        }
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
        count++;
    }

    public void AddInstance(Card card)
    {
        CardInstances.Add(card);
    }

    public void MoveRight()
    {
        if (cardSelected < CardInstances.Count - 1)
        {
            cardSelected++;
            if (cardSelected != 0 && CardInstances[cardSelected - 1].selected == false)
            {
                CardInstances[cardSelected - 1].transform.position =
                new Vector3(CardInstances[cardSelected - 1].transform.position.x, 0, zCoordinate);
            }
            CardInstances[cardSelected].transform.position =
                new Vector3(CardInstances[cardSelected].transform.position.x, 0, zCoordinate + increaseBy);
        }
    }

    public void MoveLeft()
    {
        if (cardSelected > 0) //Moves the cards up to see which card is currently being viewed to the left
        {
            cardSelected--;
            if (cardSelected != CardInstances.Count - 1 && CardInstances[cardSelected + 1].selected == false)
            {
                CardInstances[cardSelected + 1].transform.position =
                new Vector3(CardInstances[cardSelected + 1].transform.position.x, 0, zCoordinate);
            }
            CardInstances[cardSelected].transform.position =
                new Vector3(CardInstances[cardSelected].transform.position.x, 0, zCoordinate + increaseBy);
        }
    }

    public void SelectCard()
    {
        CardInstances[cardSelected].selected = !CardInstances[cardSelected].selected;

        if (CardInstances[cardSelected].selected == true) //Adds cards to the selected cards list 
        {
            SelectedCards.Add(Cards[cardSelected]);
            SelectedInstances.Add(CardInstances[cardSelected]);
        }

        else if (CardInstances[cardSelected].selected == false) //Removes cards in the selected cards list 
        {
            for (int i = 0; i < SelectedInstances.Count; i++)
            {
                if (CardInstances[cardSelected].suit == SelectedInstances[i].suit &&
                    CardInstances[cardSelected].number == SelectedInstances[i].number)
                {
                    SelectedCards.RemoveAt(i);
                    SelectedInstances.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void HandFormatTopDown() //Formats the cards from topto bottom
    {
        float xValue = gameObject.transform.position.x;
        float yValue = gameObject.transform.position.y;

        if (count <= 7)
        {
            float zValue = -1.5f * (count / 2);
            for (int i = 0; i < count; i++)
            {
                Vector3 transformValue = new Vector3(xValue, yValue, zValue);
                CardInstances[i].gameObject.transform.position = transformValue;
                zValue += 1.5f;
            }
        }
        else
        {
            float zValue = -4.5f;
            float interval = 8f / count;
            print(interval);
            for (int i = 0; i < count; i++)
            {
                Vector3 transformValue = new Vector3(xValue, yValue, zValue);
                CardInstances[i].gameObject.transform.position = transformValue;
                zValue += interval;
            }
        }
    }

    public void HandFormatLeftRight() //Formats the cards on hand from left to right
    {
        float zValue = gameObject.transform.position.z;
        float yValue = gameObject.transform.position.y;

        if (count <= 7){
            float xValue = -1.5f * (count / 2);
            for (int i = 0; i < count; i++){ 
                Vector3 transformValue = new Vector3(xValue, yValue, zValue);
                CardInstances[i].gameObject.transform.position = transformValue;
                xValue += 1.5f;
            }
        }

        else{
            float xValue = -4.5f;
            float interval = 8f / count;
            print(interval);
            for (int i = 0; i < count; i++){
                Vector3 transformValue = new Vector3(xValue, yValue, zValue);
                CardInstances[i].gameObject.transform.position = transformValue;
                xValue += interval;
            } 
        }
    }
}
