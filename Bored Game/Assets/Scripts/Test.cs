using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public GameObject Player;
    public GameObject AI;
    public Deck deck;
    public Deck played;

    public int turn; //Regulates order of players playing
    public int AINumber;

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
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        BeginGame();
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

        for (int i = 0; i < 3; i++)
        {
            AIInstances[i].GetComponent<AI>().CheckCardsLeft(firstCard);
            AIInstances[i].GetComponent<AI>().turnNumber = i + 1;
        }
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

    void GetNextCard(GameObject player)
    {
        Card nextDraw = DeckInstance.Draw();
        SetDeckCardStandIn();
        player.GetComponent<Hand>().AddCard(nextDraw);
        if (player.transform.position.z != -4f)
        {
            player.GetComponent<AI>().CheckCardsLeft(nextDraw);
        }
        nextDraw = (Card)Instantiate(nextDraw);
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

        if (DeckInstance.GetComponent<Deck>().cardsLeft == 0)
        {
          //StartCoroutine(ReShuffleWait());
            DeckInstance.GetComponent<Deck>().ReShuffle(PlayedInstance);
            PlayedInstance.GetComponent<Deck>().cardsLeft = PlayedInstance.GetComponent<Deck>().UnoDeck.Count;
        }
        DeckCardInstance = DeckInstance.GetComponent<Deck>().UnoDeck[DeckInstance.GetComponent<Deck>().UnoDeck.Count - 1];
        DeckCardInstance = (Card)Instantiate(DeckCardInstance, new Vector3(-3, 0, 0), Quaternion.Euler(90, 180, -180));
    }
}
