using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject Player; 
    public GameObject AI;

    public Deck deck;
    public Deck played;
    public int turn;

    private GameObject PlayerInstance;
    private GameObject[] AIInstances = new GameObject[3];
    private Deck DeckInstance;
    private Deck PlayedInstance;
    private Card DeckCardInstance;
    private Card PlayedCardInstance;

    public RawImage PlayerTurn;
    public RawImage AI1Turn;
    public RawImage AI2Turn;
    public RawImage AI3Turn;

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
                GetNextCard(PlayerInstance, -91, -90, -90);
                turn++;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))//Moves the cards up to see which card is currently being viewed to the right
            {
                PlayerInstance.GetComponent<Hand>().MoveRight();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PlayerInstance.GetComponent<Hand>().MoveLeft();
            }
      
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) //Selects or Deselects cards going to be played
            {
                PlayerInstance.GetComponent<Hand>().SelectCard();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayCards(PlayerInstance, PlayerInstance.GetComponent<Hand>().SelectedCards, PlayerInstance.GetComponent<Hand>().SelectedInstances);
            }
        }

        else if (turn > 0)
        {
            if (Input.GetKeyDown(KeyCode.S)){
                if (turn < 3)
                {
                    turn++;
                }
                else
                {
                    turn = 0;
                }
            }
        }

        if (DeckInstance.GetComponent<Deck>().UnoDeck.Count == 0)
        {
            DeckInstance.GetComponent<Deck>().ReShuffle(PlayedInstance);
            PlayedInstance.GetComponent<Deck>().UnoDeck.RemoveRange(0, PlayedInstance.GetComponent<Deck>().UnoDeck.Count -2);
            PlayedInstance.GetComponent<Deck>().cardsLeft = 1;
        }
        print(turn);
        ViewTurn();
    }

    void ViewTurn()
    {
        if (turn == 0)
            PlayerTurn.enabled = true;
        else
            PlayerTurn.enabled = false;
        if (turn == 1)
            AI1Turn.enabled = true;
        else
            AI1Turn.enabled = false;
        if (turn == 2)
            AI2Turn.enabled = true;
        else
            AI2Turn.enabled = false;
        if (turn == 3)
            AI3Turn.enabled = true;
        else
            AI3Turn.enabled = false;
    }

    void FirstDeal() //Hands out first 7 cards to all players
    {
        for (int i = 0; i < 7; i++)
        {
            GetNextCard(PlayerInstance, -91, -90, -90);
            GetNextCard(AIInstances[0], 91, 0, 90);
            GetNextCard(AIInstances[1], 91, -90, 90);
            GetNextCard(AIInstances[2], 91, 180, 90);
        }
    }

    void GetNextCard(GameObject player, int xRotation, int yRotation, int zRotation)
    {
        Card nextDraw = DeckInstance.Draw();
        SetDeckCardStandIn();
        player.GetComponent<Hand>().AddCard(nextDraw);
        if (player.transform.position.z != -4f)
        {
            player.GetComponent<AI>().CheckCardsLeft(nextDraw);
        }
        nextDraw = (Card)Instantiate(nextDraw, new Vector3(0, 0, 0), Quaternion.Euler(xRotation, yRotation, zRotation));
        player.GetComponent<Hand>().AddInstance(nextDraw);
        Hand(player);
    }

    void Hand(GameObject player)
    {
        if (player.transform.position.z == PlayerInstance.transform.position.z || player.transform.position.z == AIInstances[1].transform.position.z)
        {
            player.GetComponent<Hand>().HandFormatLeftRight();
        }
        else
        {
            player.GetComponent<Hand>().HandFormatTopDown();
        }
    }

    void PlayCards(GameObject Player, List<Card> Cards, List<Card> Instances)
    {
        //MUST ADD: CHECK FOR SPECIAL 
        if (CanPlay(Instances))
        {
            Card newPlayedInstance = Cards[Cards.Count - 1];
            for (int i = 0; i < Instances.Count; i++) //Moves the selected cards to the middle 
            {
                Destroy(Instances[i].gameObject);
                Instances.RemoveAt(i); //Removes the array from the arraylist
                PlayedInstance.GetComponent<Deck>().UnoDeck.Add(Cards[i]); //Adds the card to cards played 
                for (int j = 0; j < 3; j++)
                {
                    AIInstances[j].GetComponent<AI>().CheckCardsLeft(Cards[i]);
                }
                Cards.RemoveAt(i); //Removes the card from the selected cards' array
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
            if (turn != 3)
            {
                turn++;
            }
            else
            {
                turn = 0;
            }
            Player.GetComponent<Hand>().cardSelected = 0;
            SetPlayedStandIn(newPlayedInstance);
            Hand(Player);
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
        DeckCardInstance = (Card)Instantiate(DeckCardInstance, new Vector3(-3, 0, 0), Quaternion.Euler(90, 180, -180));
    }

    void BeginGame()
    {
        PlayerInstance = (GameObject)Instantiate(Player, new Vector3(0, 0, -4f), Quaternion.Euler(0, 0, 0));
        AIInstances[0] = (GameObject)Instantiate(AI, new Vector3(6.5f, 0, 0), Quaternion.Euler(0, 0, 0));
        AIInstances[1] = (GameObject)Instantiate(AI, new Vector3(0, 0, 4f), Quaternion.Euler(0, 0, 0));
        AIInstances[2] = (GameObject)Instantiate(AI, new Vector3(-6.5f, 0, 0), Quaternion.Euler(0, 0, 0));

        DeckInstance = (Deck)Instantiate(deck, new Vector3(7, 0, 0), Quaternion.Euler(0, 0, 0));
        PlayedInstance = (Deck)Instantiate(played, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        DeckInstance.CreateDeck();
        DeckInstance.Shuffle(3);
        SetDeckCardStandIn();

        FirstDeal();

        Card firstCard = DeckInstance.Draw();
        PlayedInstance.GetComponent<Deck>().UnoDeck.Add(firstCard);
        SetPlayedStandIn(firstCard);
        PlayedInstance.GetComponent<Deck>().cardsLeft++;

        for (int i = 0; i< 3; i++)
        {
            AIInstances[i].GetComponent<AI>().CheckCardsLeft(firstCard);
        }
    }

    void EndGame()
    {

    }
}
