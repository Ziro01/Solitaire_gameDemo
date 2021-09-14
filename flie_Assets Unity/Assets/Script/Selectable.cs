using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool faceUp = false;
    public bool top = false;
    public bool iSDeckPile = false;
    public int values;
    public int row;
    public string suit;
    public string valueString;
    
    void Start() {
        checkValue();
    }

    // void Update(){}

    void checkValue(){
        if(CompareTag("Card")){
            suit = transform.name[0].ToString();

            for (int i = 1; i < transform.name.Length; i++){
                char c = transform.name[i];
                valueString = valueString + c.ToString();
            }

            if(valueString == "A"){
                values = 1;
            }
            if(valueString == "2"){
                values = 2;
            }
            if(valueString == "3"){
                values = 3;
            }
            if(valueString == "4"){
                values = 4;
            }
            if(valueString == "5"){
                values = 5;
            }
            if(valueString == "6"){
                values = 6;
            }
            if(valueString == "7"){
                values = 7;
            }
            if(valueString == "8"){
                values = 8;
            }
            if(valueString == "9"){
                values = 9;
            }
            if(valueString == "10"){
                values = 10;
            }
            if(valueString == "J"){
                values = 11;
            }
            if(valueString == "Q"){
                values = 12;
            }
            if(valueString == "K"){
                values = 13;
            }    
        }
    }
}
