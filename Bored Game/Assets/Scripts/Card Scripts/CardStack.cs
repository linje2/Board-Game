using UnityEngine;
using System.Collections;

public class CardStack : MonoBehaviour {
    public Card StandIn;
    public float xValue;
    public float yValue;
    public float zValue;

    void Update()
    {
        StandIn.transform.position = new Vector3(xValue, yValue, zValue);
    }


}
