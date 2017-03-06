 using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour {
    public int number;
    /* Regular --> 0-9
     * Reverse --> -1
     * Skip --> -2
     * +2 --> -3
     * +4 --> -4
     * ColorPicker -->-5
     */
    public int suit;
    /* Blue = 0
     * Green = 1
     * Red = 2
     * Yellow = 3
     * None = 4
     * */
    public bool selected;

    public int ReturnSuit()
    {
        return suit;
    }

    public int GetNumber()
    {
        return number;
    }


} 
