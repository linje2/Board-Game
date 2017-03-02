 using UnityEngine;
using System.Collections;
using System;

public abstract class Card {
    public int number;
    public int suit;

    public int ReturnSuit()
    {
        return suit;
    }

    public int GetNumber()
    {
        return number;
    }
} 
