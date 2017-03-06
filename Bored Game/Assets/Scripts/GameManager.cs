using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public GameObject Player;
    public GameObject AI;
    public Deck deck;
    public Deck played;
    public int turn;
    public int cardSelected = 0;
    public List<Card> SelectedCards = new List<Card>();
    public List<Card> SelectedInstances = new List<Card>();
    private Deck DeckInstance;
    private Deck PlayedInstance;
    public Card DeckCardInstance;
    public Card PlayedCardInstance;

    void Start()
    {
        BeginGame(); 
    }

    void Update()
    { 
        if (turn == 0)
        { 
            if (Input.GetKeyDown(KeyCode.Space)) //Draws Card
            {
                GetNextCard(Player,-90, 180);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))//Moves the cards up to see which card is currently being viewed to the right
            {
                MoveRight();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
      
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) //Selects or Deselects cards going to be played
            {
                SelectCard();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayCards();
            }
        }

        else if (turn == 1)
        {
            if (Input.GetKeyDown(KeyCode.S)){
                turn = 0;
            }
        }

        if (DeckInstance.GetComponent<Deck>().UnoDeck.Count == 0)
        {
            DeckInstance.GetComponent<Deck>().ReShuffle(PlayedInstance);
            PlayedInstance.GetComponent<Deck>().UnoDeck.RemoveRange(0, PlayedInstance.GetComponent<Deck>().UnoDeck.Count -2);
            PlayedInstance.GetComponent<Deck>().cardsLeft = 1;
        }
    }

    void FirstDeal() //Hands out first 7 cards to all players
    {
        for (int i = 0; i < 7; i++)
        {
            GetNextCard(Player, -90, 180);
            GetNextCard(AI, 90, 0);
        }
    }

    void GetNextCard(GameObject player, int xRotation, int yRotation)
    {
        Card nextDraw = DeckInstance.Draw();
        SetDeckCardStandIn();
        player.GetComponent<Hand>().AddCard(nextDraw);
        nextDraw = (Card)Instantiate(nextDraw, new Vector3(0, 0, 0), Quaternion.Euler(xRotation, yRotation, 0));
        player.GetComponent<Hand>().AddInstance(nextDraw);
        player.GetComponent<Hand>().HandFormat();
    }

    void MoveRight()
    {
        if (cardSelected < Player.GetComponent<Hand>().CardInstances.Count - 1)
        {
            cardSelected++;
            if (cardSelected != 0 && Player.GetComponent<Hand>().CardInstances[cardSelected - 1].selected == false)
            {
                Player.GetComponent<Hand>().CardInstances[cardSelected - 1].transform.position =
                new Vector3(Player.GetComponent<Hand>().CardInstances[cardSelected - 1].transform.position.x, 0, -3f);
            }
            Player.GetComponent<Hand>().CardInstances[cardSelected].transform.position =
                new Vector3(Player.GetComponent<Hand>().CardInstances[cardSelected].transform.position.x, 0, -2f);
        }
    }

    void MoveLeft()
    {
        if (cardSelected > 0) //Moves the cards up to see which card is currently being viewed to the left
        {
            cardSelected--;
            if (cardSelected != Player.GetComponent<Hand>().CardInstances.Count - 1 && Player.GetComponent<Hand>().CardInstances[cardSelected + 1].selected == false)
            {
                Player.GetComponent<Hand>().CardInstances[cardSelected + 1].transform.position =
                new Vector3(Player.GetComponent<Hand>().CardInstances[cardSelected + 1].transform.position.x, 0, -3f);
            }
            Player.GetComponent<Hand>().CardInstances[cardSelected].transform.position =
                new Vector3(Player.GetComponent<Hand>().CardInstances[cardSelected].transform.position.x, 0, -2f);
        }
    }

    void SelectCard()
    {
        Player.GetComponent<Hand>().CardInstances[cardSelected].selected = !Player.GetComponent<Hand>().CardInstances[cardSelected].selected;

        if (Player.GetComponent<Hand>().CardInstances[cardSelected].selected == true) //Adds cards to the selected cards list 
        {
            SelectedCards.Add(Player.GetComponent<Hand>().Cards[cardSelected]);
            SelectedInstances.Add(Player.GetComponent<Hand>().CardInstances[cardSelected]);
        }

        else if (Player.GetComponent<Hand>().CardInstances[cardSelected].selected == false) //Removes cards in the selected cards list 
        {
            for (int i = 0; i < SelectedInstances.Count; i++)
            {
                if (Player.GetComponent<Hand>().CardInstances[cardSelected].suit == SelectedInstances[i].suit &&
                    Player.GetComponent<Hand>().CardInstances[cardSelected].number == SelectedInstances[i].number)
                {
                    SelectedCards.RemoveAt(i);
                    SelectedInstances.RemoveAt(i);
                    break;
                }
            }
        }
    }

    void PlayCards()
    {
        if (CanPlay(SelectedInstances))
        {
            Card newPlayedInstance = SelectedCards[SelectedCards.Count - 1];
            for (int i = 0; i < SelectedInstances.Count; i++) //Moves the selected cards to the middle 
            {
                Destroy(SelectedInstances[i].gameObject);
                SelectedInstances.RemoveAt(i); //Removes the array from the arraylist
                PlayedInstance.GetComponent<Deck>().UnoDeck.Add(SelectedCards[i]); //Adds the card to cards played 
                SelectedCards.RemoveAt(i); //Removes the card from the selected cards' array
                i--;
                PlayedInstance.GetComponent<Deck>().cardsLeft++;
            }
            for (int j = 0; j < Player.GetComponent<Hand>().CardInstances.Count; j++) //Removes the cards from the player's hand
            {
                if (Player.GetComponent<Hand>().CardInstances[j].selected)
                {
                    Player.GetComponent<Hand>().CardInstances.RemoveAt(j); //Removes the instance
                    Player.GetComponent<Hand>().Cards.RemoveAt(j); //Removes the card
                    Player.GetComponent<Hand>().count--;
                    j--;
                }
            }
            turn = 1;
            cardSelected = 0;
            SetPlayedStandIn(newPlayedInstance);
            Player.GetComponent<Hand>().HandFormat();
        }
        else
        {
            print("Can't Play Try Again");
        }
    }

    bool CanPlay(List<Card> playedCard) //Returns TRUE if the cards selected can be played, FALSE if the cards can't
    {
        if (playedCard[0].GetComponent<Card>().suit != PlayedCardInstance.GetComponent<Card>().suit 
            && playedCard[0].GetComponent<Card>().number != PlayedCardInstance.GetComponent<Card>().number) 
        {
          /*  print("COLOR: " + playedCard[0].GetComponent<Card>().suit + " NUMBER: " + playedCard[0].GetComponent<Card>().number);
            print("COLOR: " + PlayedCardInstance.GetComponent<Card>().suit + " NUMBER: " + PlayedCardInstance.GetComponent<Card>().number); */
                return false;
        }
        if (playedCard.Count > 0)
        {
            for (int i = 1; i < playedCard.Count; i++)
            {
                if (playedCard[i].GetComponent<Card>().number != playedCard[i - 1].GetComponent<Card>().number)
                {
                    print("Cards not the same number");
                    return false;
                }
            }
        }
        return true;
    }

    void SetPlayedStandIn(Card set)
    {
        if (PlayedCardInstance != null)
        {
            Destroy(PlayedCardInstance.gameObject);
            PlayedCardInstance = null;
        }
        PlayedCardInstance = set;
        PlayedCardInstance = (Card)Instantiate(PlayedCardInstance, new Vector3(0, 0, 0), Quaternion.Euler(-90, 180, 0));
    }

    void SetDeckCardStandIn()
    {
        if (DeckCardInstance != null)
        {
            Destroy(DeckCardInstance.gameObject);
            DeckCardInstance = null;
        }
        DeckCardInstance = DeckInstance.GetComponent<Deck>().UnoDeck[DeckInstance.GetComponent<Deck>().UnoDeck.Count - 1];
        DeckCardInstance = (Card)Instantiate(DeckCardInstance, new Vector3(-3, 0, 0), Quaternion.Euler(90, 180, 0));
    }

    void BeginGame() 
    {
        DeckInstance = (Deck)Instantiate(deck, new Vector3(-3, 0, 0), Quaternion.Euler(0, 0, 0));
        PlayedInstance = (Deck)Instantiate(played, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        DeckInstance.CreateDeck();
        DeckInstance.Shuffle(3);
        SetDeckCardStandIn();

        FirstDeal();

        Card firstCard = DeckInstance.Draw();
        PlayedInstance.GetComponent<Deck>().UnoDeck.Add(firstCard);
        SetPlayedStandIn(firstCard);
        PlayedInstance.GetComponent<Deck>().cardsLeft++;
    }

    void EndGame()
    {

    }
}
