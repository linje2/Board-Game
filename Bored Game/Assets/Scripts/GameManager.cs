using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject Player;
    public GameObject AI;
    public Deck cards;

    void Update()
    {

    }

    void BeginGame() //Instantiate Player, User, and Cards 
    {

        cards.Shuffle();
        for (int i = 0; i < 7; i++) //Gives seven cards to player and user
        {
            Player.GetComponent<Hand>().isShowing = true;
            AI.GetComponent<Hand>().isShowing = false;
            Player.GetComponent<Hand>().Cards.Add(cards.Draw());
            AI.GetComponent<Hand>().Cards.Add(cards.Draw());
        }

    }

    void EndGame()
    {

    }

    void InstantiateDeck()
    {

    }
}
