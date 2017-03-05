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
        Instantiate(deck, new Vector3(0, 0, 0), Quaternion.Euler(90, 0, 0));
    //    print("nextDraw: " + nextDraw.number + " " + nextDraw.suit);
      //  print(cards.cardsLeft);
      //  BeginGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            Card nextDraw = deck.Draw();
            Player.GetComponent<Hand>().AddCard(nextDraw);
            Instantiate(nextDraw, new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 0));
        }
       /* if (turn == 0)
        {
            canClick = true;
            print("Player turn");
        }
        if (turn == 1)
        {
            canClick = false;
            print("AI turn");
        } */
        
    }

    void FirstDeal() //Hands out first 7 cards to all players
    {
        for (int i = 0; i < 7; i++)
        {
            Player.GetComponent<Hand>().AddCard(deck.Draw());
           
        }
    }

    void HandFormat(GameObject player)
    {
        int count = player.GetComponent<Hand>().count;
        if (count < 9)
        {
            for (int i = 0; i < count; i++)
            {
                 
            }
        }
    }

    void BeginGame() 
    {
        deck.CreateDeck();
        deck.Shuffle(3);
       // Instantiate(cards, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
       /* cards.Shuffle();
        for (int i = 0; i < 7; i++) //Gives seven cards to player and user
        {
            Player.GetComponent<Hand>().isShowing = true;
            AI.GetComponent<Hand>().isShowing = false;
            Player.GetComponent<Hand>().Cards.Add(cards.Draw());
            AI.GetComponent<Hand>().Cards.Add(cards.Draw());
        } */

    }

    void EndGame()
    {

    }
}
