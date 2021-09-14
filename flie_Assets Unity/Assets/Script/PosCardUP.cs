using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCardUP : MonoBehaviour
{
    public List<Transform> card = new List<Transform>();
    public GameObject GlupCard;
    void Update(){
        card.Clear();

        foreach (Transform item in GlupCard.transform) {
            card.Add(item);
        }
        if(card.Count >0){
            for (int i = 0; i < card.Count; i++){
                if(i == card.Count - 1){
                    card[i].GetComponent<Selectable>().faceUp = true;
                }
                else{
                    card[i].GetComponent<Selectable>().faceUp = false;
                }
            }
        }
    }
}
