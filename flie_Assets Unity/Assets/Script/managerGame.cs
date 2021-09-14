using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class managerGame : MonoBehaviour
{
    public GameObject popUp;
    public Text txt_Message,txt_yourTime,txt_countTime;
    public bool iSplay = false;
    private bool iSTime;
    float countTime = 0f;

    private void Start() {
        txt_Message.text = "Ready !!!";
        txt_yourTime.text = "your time " + disPlayTime(get_youTime());
        popUp.SetActive(true);

    }
    void Update(){
        GameObject[] card = GameObject.FindGameObjectsWithTag("Card");
        if(card.Length <= 0 && iSplay == true)
            overGame();

        if(iSTime == true){
            countTime += Time.deltaTime;
            txt_countTime.text = disPlayTime(countTime);
        }
    }
    public void bttn_PlayGame(){
        popUp.SetActive(false);
        bttn_newGame();
    }

    public void bttn_newGame(){
        CardSprite[] cards = FindObjectsOfType<CardSprite>();
        foreach (var item in cards) {
            Destroy(item.gameObject);
        }

        FindObjectOfType<ManagerCard>().dealCard();
        iSTime = true;
        countTime = 0;
        iSplay = false;
    }

    void overGame(){
        iSTime = false;
        set_yourTime(countTime);
        txt_Message.text = "You Win!";
        txt_yourTime.text = "your time " + disPlayTime(get_youTime());
        popUp.SetActive(true);
    }

    void set_yourTime(float _time){
        if(_time < get_youTime() || get_youTime() <= 0){
            PlayerPrefs.SetFloat("time",_time);
            get_youTime();
        }
        
    }
    float get_youTime(){
        float t;
        t = PlayerPrefs.GetFloat("time");
        return t;
    }

    string disPlayTime(float _time){
        string t;
        float min = Mathf.FloorToInt(_time / 60);
        float sec = Mathf.FloorToInt(_time % 60);
        
        t = string.Format("{0:00}:{1:00}",min,sec);
        return t;
    }
}