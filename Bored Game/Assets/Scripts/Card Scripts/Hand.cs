using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {
    public List<Card> Cards = new  List<Card>();
    public List<Card> CardInstances = new List<Card>();
    public int count;
    public bool isShowing;

    void Start()
    {
        count = Cards.Count;
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
        count++;
    }

    public void AddInstance(Card card)
    {
        CardInstances.Add(card);
    }
    public void HandFormat() //Formats the cards on hand
    {
        float zValue = 0f;

        if (gameObject.transform.position.z > 0){
            zValue = gameObject.transform.position.z - 1.5f;
        }
        else{
            zValue = gameObject.transform.position.z + 1.5f;
        }
        float yValue = gameObject.transform.position.y;
        if (count <= 9){
            float xValue = -1.5f * (count / 2);
            for (int i = 0; i < count; i++){ 
                Vector3 transformValue = new Vector3(xValue, yValue, zValue);
                CardInstances[i].gameObject.transform.position = transformValue;
                xValue += 1.5f;
            }
        }

        else{
            float xValue = -4.5f;
            float interval = 8f / count;
            print(interval);
            for (int i = 0; i < count; i++){
                Vector3 transformValue = new Vector3(xValue, yValue, zValue);
                CardInstances[i].gameObject.transform.position = transformValue;
                xValue += interval;
            } 
        }
    }
}
