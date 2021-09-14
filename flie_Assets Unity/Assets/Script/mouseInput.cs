using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class mouseInput : MonoBehaviour
{
    private ManagerCard c_managerCard;
    public GameObject slot1;
    private float times,setTemes;
    private float douleClickTime = 0.5f;
    private int clickCount;
    void Start() {
        c_managerCard = FindObjectOfType<ManagerCard>();
        slot1 = this.gameObject;
        setTemes = times;
    }
    
    void Update(){
        if(clickCount == 1)
            times -= Time.deltaTime;

        if(times <= 0 ){
            times = setTemes;
            clickCount = 0;
        }

        if(clickCount == 3){
            times = 0;
            clickCount = 1;
        }
        GetMouseClick();
    }

    void GetMouseClick(){
        if(Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-10f));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);

            if(hit){
                if(hit.collider.CompareTag("Deck")){
                    c_managerCard.DealFromDeck();
                       slot1 = this.gameObject;
             }
                else if(hit.collider.CompareTag("Card")){
                    GameObject selected = hit.collider.gameObject;

                    if(!selected.GetComponent<Selectable>().faceUp){
                        if(!blocked(selected)){
                            selected.GetComponent<Selectable>().faceUp = true;
                            slot1 = this.gameObject;
                        }
                    }
                    else if(selected.GetComponent<Selectable>().iSDeckPile) {
                        if(!blocked(selected))
                            slot1 = selected;
                    }
                    if(!selected.GetComponent<Selectable>().faceUp){
                        if (!blocked(selected)){
                            if (slot1 == selected){
                                if (DoubleClick()){
                                    AutoStack(selected);
                                }
                            }
                            else{
                                slot1 = selected;
                            }                
                        }
                    }

                    if(slot1 == this.gameObject){
                        slot1 = selected;
                    }
                    else if(slot1 != selected){
                        if(stackable(selected)){
                            Stack_Card(selected);
                        }
                        else{
                            slot1 = selected;
                        }
                    } 
                }
                else if(hit.collider.CompareTag("PosTop")){
                    GameObject selected = hit.collider.gameObject;
                    if(slot1.CompareTag("Card")){
                        if(slot1.GetComponent<Selectable>().faceUp == true){
                            for (int i = 1; i < 14; i++) {
                                if(slot1.GetComponent<Selectable>().values == i){
                                    Stack_Pos(selected);
                                    break;
                                }
                            }
                        }  
                    }
                }
                else if(hit.collider.CompareTag("PosDown")){
                    GameObject selected = hit.collider.gameObject;
                    if(slot1.CompareTag("Card")){ 
                        if(slot1.GetComponent<Selectable>().faceUp == true){
                            for (int i = 1; i < 14; i++) {
                                if(slot1.GetComponent<Selectable>().values == i){
                                    Stack_Pos(selected);
                                    break;
                                }
                            }
                        }
                    }
                }    
            }
        }
    }
    public bool stackable(GameObject _seleced){
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = _seleced.GetComponent<Selectable>();
        if(!s2.iSDeckPile){
            if(s2.top){
                if(s1.suit == s2.suit || (s1.values == 1 && s2.suit == null)){
                    if(s1.values == s2.values + 1){
                        return true;
                    }
                    else{
                        return false;
                    }
                }
            }
            else{
                if(s1.values == s2.values -1){
                    bool card1Red = true;
                    bool card2Red = true;

                    if(s1.suit == "C" || s1.suit == "S")
                        card1Red = false;

                    if(s2.suit == "C" || s2.suit == "S")
                        card2Red = false;

                    if(card1Red == card2Red){
                        return false;
                    }
                    else{
                        return true;
                    }
                }  
            }
        }
        return false;
    }
    bool DoubleClick(){
        if (times < douleClickTime && clickCount == 2) {
            return true;
        }
        else{
            return false;
        }
    }
    void AutoStack(GameObject selected) {
        for (int i = 0; i < c_managerCard.Pos_top.Length; i++){
            Selectable stack = c_managerCard.Pos_top[i].GetComponent<Selectable>();
            if (selected.GetComponent<Selectable>().values == 1) {
                if (c_managerCard.Pos_top[i].GetComponent<Selectable>().values == 0) {
                    slot1 = selected;
                    Stack_Card(stack.gameObject); 
                    break;                  
                }
            }
            else{
                if ((c_managerCard.Pos_top[i].GetComponent<Selectable>().suit == slot1.GetComponent<Selectable>().suit) && (c_managerCard.Pos_top[i].GetComponent<Selectable>().values == slot1.GetComponent<Selectable>().values - 1)){
                    if (HasNoChildren(slot1)){
                        slot1 = selected;
                        string lastCardname = stack.suit + stack.values.ToString();
                        if (stack.values == 1){
                            lastCardname = stack.suit + "A";
                        }
                        if (stack.values == 11){
                            lastCardname = stack.suit + "J";
                        }
                        if (stack.values == 12){
                            lastCardname = stack.suit + "Q";
                        }
                        if (stack.values == 13){
                            lastCardname = stack.suit + "K";
                        }
                        GameObject lastCard = GameObject.Find(lastCardname);
                        Stack_Card(lastCard);
                        break;
                    }
                }
            }
        }
    }    
    bool HasNoChildren(GameObject card){
        int i = 0;
        foreach (Transform child in card.transform){
            i++;
        }
        if (i == 0){
            return true;
        }
        else{
            return false;
        }
    }

    void Stack_Card(GameObject _selected){
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = _selected.GetComponent<Selectable>();

        float yPos =  0.3f;
            if(s2.top || (!s2.top && s1.values == 13)){
                yPos = 0;
        }
        

        slot1.transform.position = new Vector3(_selected.transform.position.x,_selected.transform.position.y - yPos,_selected.transform.position.z -0.01f);
        slot1.transform.parent = _selected.transform;

        if(s1.iSDeckPile){
            c_managerCard.tripsOnDisplay.Remove(slot1.name);

        }
        else if(s1.top && s2.top && s1.values ==1){
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().values = 0;
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().suit = null;
        }
        else if(s1.top){
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().values = s1.values -1;
        }
        else{
            c_managerCard.pos[s1.row].Remove(slot1.name);
        }

        s1.iSDeckPile = false;
        s1.row = s2.row;

        if(s2.top){
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().values = s1.values;
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;
        }
        else{
            s1.top = false;
        }
        slot1 = this.gameObject;
    }
    void Stack_Pos(GameObject _selected){
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = _selected.GetComponent<Selectable>();

        float yPos =  0.3f;
        for (int i = 0; i < 14; i++){
            if(s2.top || (!s2.top && s1.values == i)){
                yPos = 0;
                break;
            }   
        }
        

        slot1.transform.position = new Vector3(_selected.transform.position.x,_selected.transform.position.y - yPos,_selected.transform.position.z -0.01f);
        slot1.transform.parent = _selected.transform;

        if(s1.iSDeckPile){
            c_managerCard.tripsOnDisplay.Remove(slot1.name);

        }
        else if(s1.top && s2.top && s1.values ==1){
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().values = 0;
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().suit = null;
        }
        else if(s1.top){
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().values = s1.values -1;
        }
        else{
            c_managerCard.pos[s1.row].Remove(slot1.name);
        }

        s1.iSDeckPile = false;
        s1.row = s2.row;

        if(s2.top){
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().values = s1.values;
            c_managerCard.Pos_top[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;
        }
        else{
            s1.top = false;
        }
        slot1 = this.gameObject;
    }
    bool blocked(GameObject _selected){

        Selectable s2 = _selected.GetComponent<Selectable>();
        if(s2.iSDeckPile == true){
            if(s2.name == c_managerCard.tripsOnDisplay.Last()){
                return false;
            }
            else{
                return true;
            }
        }
        else{
            if(s2.name == c_managerCard.pos[s2.row].Last()){
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
