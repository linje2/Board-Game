using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public GameObject Player;
    public GameObject AI;
    public Deck deck;
    public Deck played;
    public int turn;
    public bool canClick = true;

    void Start()
    {
        BeginGame();
    }

    void Update()
    {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Card nextDraw = deck.Draw();
                Player.GetComponent<Hand>().AddCard(nextDraw);
                Player.GetComponent<Hand>().HandFormat();
            }

    }

    void FirstDeal() //Hands out first 7 cards to all players
    {
        for (int i = 0; i < 7; i++)
        {
            Card nextDraw = deck.Draw();
            nextDraw = (Card)Instantiate(nextDraw, new Vector3(0, 0, 0), Quaternion.Euler(90, 0, 0));
            Player.GetComponent<Hand>().AddCard(nextDraw);
            Player.GetComponent<Hand>().HandFormat();
            nextDraw = deck.Draw();
            nextDraw = (Card)Instantiate(nextDraw, new Vector3(0, 0, 0), Quaternion.Euler(90, 0, 0));
            AI.GetComponent<Hand>().AddCard(nextDraw);
            AI.GetComponent<Hand>().HandFormat();
        }
    }

    void BeginGame() 
    {
        deck.CreateDeck();
        deck.Shuffle(3);
        FirstDeal();
    }

    void EndGame()
    {

    }
}
