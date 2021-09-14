using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countCard : MonoBehaviour
{
    // void Start(){}
    void Update() {
        int number = countCards();
        if(number == 13)
            Destroy(gameObject);
    }

    int countCards(){
        int n = 0;
        foreach (var item in transform.GetComponentsInChildren<Transform>()){
            n++;
        }
        return n;
    }
}
