using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
    public List<Card> Cards = new  List<Card>();
    public int count;
  //  public List<GameObject> CardInstances = new List<GameObject>();
    public bool isShowing;
    void Start()
    {
        count = Cards.Count;
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
        count++;
    }
    void MoveCards()
    {
        int count = Cards.Count;
        if (count < 9)
        {
            for (int i = 0; i < count; i++)
            {

            }
        }
    }
}
