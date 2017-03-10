using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject Player; 
    public GameObject AI;
    public Deck deck;
    public Deck played;

    public int turn; //Regulates order of players playing
    public bool DrawCards = false;
    public int numToDraw;

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
    public Button Uno;
    public Button Draw;
    public Button Red;
    public Button Blue;
    public Button Green;
    public Button Yellow;
    public Text Shout;
    public Text Reshuffle;
    public Text ColorNow;


    void Start()
    {
        BeginGame();
        UpdateColor();
    }

    void UpdateColor()
    {
        int color = PlayedCardInstance.suit;
        if (color == 0)
        {
            ColorNow.text = "Blue";
        }
        else if (color == 1)
        {
            ColorNow.text = "Green";
        }
        else if (color == 2)
        {
            ColorNow.text = "Red";
        }
        else if (color == 3)
        {
            ColorNow.text = "Yellow";
        }
    }
    void Update()
    {
        UpdateColor();

        if (turn == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space)) //Draws Card
            {
                GetNextCard(PlayerInstance);
                NextTurn();
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
        else
        {
            int playing = FindAIPlaying();
            AIInstances[playing].GetComponent<AI>().PlayCard();
              if (AIInstances[playing].GetComponent<Hand>().SelectedCards.Count == 0)
                 {
                     GetNextCard(AIInstances[playing]);
                     NextTurn();
                 }
                 else
                 {
                     PlayCards(AIInstances[playing], AIInstances[playing].GetComponent<Hand>().Cards, AIInstances[playing].GetComponent<Hand>().SelectedCards);
                 } 
        }

        ViewTurn();
    }

    int FindAIPlaying()
    {
        int playing = 0;
        for (int i = 0; i< AIInstances.Length; i++)
        {
            if (turn == AIInstances[i].GetComponent<AI>().turnNumber)
            {
                playing = i;
                break;
            }
        }
        return playing;
    }

    void NextTurn() //goes to the next player
    {  
        turn++;
        RegulateTurn();
    }

    void ViewTurn()
    {
        if (turn == 0)
            PlayerTurn.enabled = true;
        else
            PlayerTurn.enabled = false;
        if (turn == AIInstances[0].GetComponent<AI>().turnNumber)
            AI1Turn.enabled = true;
        else
            AI1Turn.enabled = false;
        if (turn == AIInstances[1].GetComponent<AI>().turnNumber)
            AI2Turn.enabled = true;
        else
            AI2Turn.enabled = false;
        if (turn == AIInstances[2].GetComponent<AI>().turnNumber)
            AI3Turn.enabled = true;
        else
            AI3Turn.enabled = false;
    }

    void FirstDeal() //Hands out first 7 cards to all players
    {
        for (int i = 0; i < 7; i++)
        {
            GetNextCard(PlayerInstance);
            GetNextCard(AIInstances[0]);
            GetNextCard(AIInstances[1]);
            GetNextCard(AIInstances[2]);
        }
    }

    void GetNextCard(GameObject player)  //Gets next card from the deck
    {
        Card nextDraw = DeckInstance.Draw(); 
        SetDeckCardStandIn(); //
        player.GetComponent<Hand>().AddCard(nextDraw);
        if (player.transform.position.z != -4f)
        {
            player.GetComponent<AI>().CheckCardsLeft(nextDraw);
        }
        nextDraw = (Card)Instantiate(nextDraw);
        player.GetComponent<Hand>().AddInstance(nextDraw);
        Hand(player);
    }

    void Hand(GameObject player) //Formats the cards on hand 
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
        if (CanPlay(Instances)) //If the cards selected can be played
        {
            Card newPlayedInstance = Cards[Cards.Count - 1]; //Sets the new instance to the last card selected
            int cardsPlayed = Cards.Count;

            for (int i = 0; i < Instances.Count; i++) //Moves the selected cards to the middle 
            {
                Destroy(Instances[i].gameObject);
                Instances.RemoveAt(i); //Removes the array from the arraylist
                PlayedInstance.GetComponent<Deck>().UnoDeck.Add(Cards[i]); //Adds the card to cards played 

                DoSpecial(Cards[i]); //Checks for special card played

                for (int j = 0; j < 3; j++) //Updates how many of each color cards are left 
                {
                    AIInstances[j].GetComponent<AI>().CheckCardsLeft(Cards[i]);
                }

                Cards.RemoveAt(i); //Removes the card from the selected cards' array
                i--;
                PlayedInstance.GetComponent<Deck>().cardsLeft++; 
            }

            for (int j = 0; j < Player.GetComponent<Hand>().CardInstances.Count; j++) //Removes the cards from the player's hand
            {
                if (Player.GetComponent<Hand>().CardInstances[j].selected) //Checks to see if the card is selected
                {
                    Player.GetComponent<Hand>().CardInstances.RemoveAt(j); //Removes the instance
                    Player.GetComponent<Hand>().Cards.RemoveAt(j); //Removes the card
                    Player.GetComponent<Hand>().count--;
                    j--;
                }
            }
            Player.GetComponent<Hand>().cardSelected = 0; //Reverts the selected card to 0
            SetPlayedStandIn(newPlayedInstance); //Sets the new "Top" card played 

            if (PlayedCardInstance.suit == 4)
            {
                print("Choose new color" + " TURN: " + turn);
                if (turn == 0)
                {
                    ColorNow.text = "Choose New";
                    Red.enabled = true;
                    Blue.enabled = true;
                    Green.enabled = true;
                    Yellow.enabled = true;
                }
                else
                {
                    PlayedCardInstance.suit = AIInstances[FindAIPlaying()].GetComponent<AI>().ChooseColor();
                }
            }

            NextTurn();

            if (numToDraw > 0)
            {
                if (turn == 0)
                {
                    for (int i = 0; i < PlayerInstance.GetComponent<Hand>().Cards.Count; i++)
                    {
                        if (PlayerInstance.GetComponent<Hand>().Cards[i].suit == newPlayedInstance.suit)
                        {
                            Draw.enabled = true;
                            break;
                        }
                    }
                    if (Draw.enabled == false)
                    {
                        for (int j = 0; j < numToDraw; j++)
                        {
                            GetNextCard(PlayerInstance);
                        }
                        NextTurn();
                    }
                }
                else
                {
                    AIInstances[FindAIPlaying()].GetComponent<AI>().DrawTwoOrFour(newPlayedInstance);
                    if (AIInstances[FindAIPlaying()].GetComponent<Hand>().SelectedCards.Count > 0)
                    {
                        PlayCards(AIInstances[FindAIPlaying()], AIInstances[FindAIPlaying()].GetComponent<Hand>().SelectedCards, AIInstances[FindAIPlaying()].GetComponent<Hand>().SelectedInstances);
                    }
                    else
                    {
                        for (int j = 0; j < numToDraw; j++)
                        {
                            GetNextCard(AIInstances[FindAIPlaying()]);
                        }
                        NextTurn();
                    }
                }
                numToDraw = 0;
            }
            Hand(Player); //Updates the hand format
        }
    }
    
    void RegulateTurn()
    {
        if (turn > 3 || turn < 0)
            turn = 0;
    }

    void DoSpecial(Card card)
    {
        int number = card.number;
        if (number == -1) //Reverses the order of the players
        {
            print("Reverse");
            for (int i = 0; i < 3; i++)
            {
                AIInstances[i].GetComponent<AI>().turnNumber = 3 - i;
            }
        }
        else if (number == -2) //Skips the next player
        {
            print("Skip");
            NextTurn();
        }

        else if (number == -3) //Adds two cards to the number of cards that must be drawn
        {
            numToDraw += 2;
            print("Draw Two");
        }
        else if (number == -4) //Adds four to the number of cards that must be drawn
        {
            print("Draw Four");
            numToDraw += 4;
        }
        else if (number == -5) //Enables the player to choose a different color
        {
           print("Color Picker");
        } 
    } 


    public void ClickBlue()
    {
        PlayedCardInstance.suit = 0;
        print(PlayedCardInstance.suit);
        Red.enabled = false;
        Blue.enabled = false;
        Green.enabled = false;
        Yellow.enabled = false;
    }

    public void ClickGreen()
    {
        PlayedCardInstance.suit = 1;
        print(PlayedCardInstance.suit);
        Red.enabled = false;
        Blue.enabled = false;
        Green.enabled = false;
        Yellow.enabled = false;
    }
    public void ClickRed()
    {
        PlayedCardInstance.suit = 2;
        print(PlayedCardInstance.suit);
        Red.enabled = false;
        Blue.enabled = false;
        Green.enabled = false;
        Yellow.enabled = false;
    }

    public void ClickYellow()
    {
        PlayedCardInstance.suit = 3;
        print(PlayedCardInstance.suit);
        Red.enabled = false;
        Blue.enabled = false;
        Green.enabled = false;
        Yellow.enabled = false;
    }

    public void MustDraw() //Player presses draw button
    {
        DrawCards = true;
    }

    bool CanPlay(List<Card> playedCard) //Returns TRUE if the cards selected can be played, FALSE if the cards can't
    {
        if (playedCard.Count > 0)
        {
            if (playedCard[0].GetComponent<Card>().suit == PlayedCardInstance.GetComponent<Card>().suit //Checks if the first card has the same number or suit as the card in the middle
                || playedCard[0].GetComponent<Card>().number == PlayedCardInstance.GetComponent<Card>().number || playedCard[0].GetComponent<Card>().suit == 4)
            {
                for (int i = 1; i < playedCard.Count; i++) //Checks to see if the rest of the cards played has the same number as the first 
                {
                    if (playedCard[i].GetComponent<Card>().number != playedCard[i - 1].GetComponent<Card>().number)
                    {
                        print("Cards not the same number");
                        return false;
                    }
                }
                return true;
            }
        }
        return false;
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

    void SetDeckCardStandIn() //Visual of next card in deck
    {
        if (DeckCardInstance != null) //If there is currently a card 
        {
            Destroy(DeckCardInstance.gameObject); //Destroys the instance
            DeckCardInstance = null; 
        }

        if (DeckInstance.GetComponent<Deck>().cardsLeft == 0) //Checks to see if there are any cards left to draw
        {
            StartCoroutine(ReShuffleWait()); 
            DeckInstance.GetComponent<Deck>().ReShuffle(PlayedInstance); //Reshuffles
            PlayedInstance.GetComponent<Deck>().cardsLeft = PlayedInstance.GetComponent<Deck>().UnoDeck.Count; //Resets the cards that were already played
        }
        DeckCardInstance = DeckInstance.GetComponent<Deck>().UnoDeck[DeckInstance.GetComponent<Deck>().UnoDeck.Count - 1]; //Gets the next Instance
        DeckCardInstance = (Card)Instantiate(DeckCardInstance, new Vector3(-3, 0, 0), Quaternion.Euler(90, 180, -180)); 
    }

    IEnumerator ReShuffleWait()
    {
        Reshuffle.enabled = true;
        yield return new WaitForSeconds(4);
        Reshuffle.enabled = false; 
    }

    void BeginGame()
    {
        PlayerInstance = (GameObject)Instantiate(Player, new Vector3(0, 0, -4f), Quaternion.Euler(0, 0, 0));
        AIInstances[0] = (GameObject)Instantiate(AI, new Vector3(6.5f, 0, 0), Quaternion.Euler(0, 0, 0));
        AIInstances[1] = (GameObject)Instantiate(AI, new Vector3(0, 0, 4f), Quaternion.Euler(0, 0, 0));
        AIInstances[2] = (GameObject)Instantiate(AI, new Vector3(-6.5f, 0, 0), Quaternion.Euler(0, 0, 0));

        PlayerInstance.GetComponent<Hand>().SetRotation(-91, -90, -90);
        AIInstances[0].GetComponent<Hand>().SetRotation(91, 0, 90);
        AIInstances[1].GetComponent<Hand>().SetRotation(91, -90, 90);
        AIInstances[2].GetComponent<Hand>().SetRotation(91, 180, 90);

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

        if (PlayedCardInstance.suit == 4)
        {
            print("Changing color");
            int randSuit = Random.Range(0, 3);
            PlayedCardInstance.suit = randSuit;
            print("Color = " + firstCard.suit);
        }

        for (int i = 0; i< 3; i++)
        {
            AIInstances[i].GetComponent<AI>().CheckCardsLeft(firstCard);
            AIInstances[i].GetComponent<AI>().turnNumber = i + 1;
        }
    }

    void EndGame()
    {
        if (deck.GetComponent<Deck>().UnoDeck.Count == 0 && played.GetComponent<Deck>().UnoDeck.Count == 1)
        {
            print("END GAME");
        }
    }
}
