using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSprite : MonoBehaviour
{
    public Sprite faceCard;
    public Sprite backCard;
    SpriteRenderer displayCard;
    ManagerCard c_managerCard;
    Selectable c_Selectable;
    mouseInput c_mouseInput;

    
    void Start(){
        setDeck();
        }
    void Update(){
        if(c_Selectable.faceUp == true){
            displayCard.sprite = faceCard;
        }
        else{
            displayCard.sprite = backCard;
        }

        if(c_mouseInput.slot1){
            if(name == c_mouseInput.slot1.name){
                displayCard.color = Color.green;
            }
            else{
                displayCard.color = Color.white;
            }
        }
    }

    void setDeck(){
        List<string> deck = ManagerCard.generateDeck();
        c_managerCard = FindObjectOfType<ManagerCard>();
         c_mouseInput = FindObjectOfType<mouseInput>();

        int indexs = 0;

        foreach (var card in deck){
            if(this.name == card){
                faceCard = c_managerCard.faceCard[indexs];
                break;
            }
            indexs++;
        }
        displayCard = GetComponent<SpriteRenderer>();
        c_Selectable  = GetComponent<Selectable>();
    }
}
