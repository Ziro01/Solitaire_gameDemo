using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ManagerCard : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject deckButton;
    public Sprite[] faceCard;
    public GameObject[] Pos_down;
    public GameObject[] Pos_top;

    public static string[] setCard = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string>[] pos;
    public List<string>[] tops;
    public List<string> tripsOnDisplay = new List<string>();
    public List<List<string>> deckTrips = new List<List<string>>();

    private List<string> pos_0 = new List<string>();
    private List<string> pos_1 = new List<string>();
    private List<string> pos_2 = new List<string>();
    private List<string> pos_3 = new List<string>();
    private List<string> pos_4 = new List<string>();
    private List<string> pos_5 = new List<string>();
    private List<string> pos_6 = new List<string>();


    public List<string> deckCard;
    public List<string> disCardPile = new List<string>();
    private int deckLocation;
    private int trips;
    private int tripsRemainder;

    // Start is called before the first frame update


    void Start(){
        pos = new List<string>[]{pos_0,pos_1,pos_2,pos_3,pos_4,pos_5,pos_6};
        // dealCard();
    }
    void Update(){
    }

    public void dealCard(){
        foreach (List<string> item in pos)
        {
            item.Clear();
        }

        deckCard = generateDeck();
        shuffleCard(deckCard);

        foreach (string card in deckCard){
            // print(card);
        }

        
        SolitaireSort();
        StartCoroutine(createDecks());
        SortDeckIntoTrips();
    }
    public static List<string> generateDeck(){
        List<string> newDeck = new List<string>();

        foreach (string s in setCard){
            foreach (string v in values){
                newDeck.Add(s+v);
            }
        }

        return newDeck;
    }

    void shuffleCard<T>(List<T> lish){
        System.Random random = new System.Random();
        int n = lish.Count;
        while( n > 1){
            int k = random.Next(n);
            n--;
            T temp = lish[k];
            lish[k] = lish[n];
            lish[n] = temp;
        }
    }

    // public void createDeck(){
    IEnumerator createDecks(){
        for (int i = 0; i < 7; i++){
            float ySet = 0f;
            float zSet = 0.3f;

            foreach (string cardName in pos[i]){
                // GameObject newCard = Instantiate(cardPrefab,transform.position,Quaternion.identity);
                yield return new WaitForSeconds(0.01f);
                GameObject newCard = Instantiate(cardPrefab,new Vector3(Pos_down[i].transform.position.x,Pos_down[i].transform.position.y - ySet,transform.position.z - zSet) ,Quaternion.identity,Pos_down[i].transform);
                newCard.name = cardName;
                newCard.GetComponent<Selectable>().row = i;
                
                if(cardName == pos[i][pos[i].Count - 1]){
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
                    

                ySet = ySet + 0.3f;
                zSet = zSet + 0.05f;
                disCardPile.Add(cardName);
            }

            foreach (string item in disCardPile){
                if(deckCard.Contains(item)){
                    deckCard.Remove(item);
                }
            }
            disCardPile.Clear();
            FindObjectOfType<managerGame>().iSplay = true;
        }
    }

    void SolitaireSort(){  //เลียงไพ่ posDown (เริ่ม)
        for (int i = 0; i < 7; i++) {
            for (int j = i; j < 7; j++) {
                pos[j].Add(deckCard.Last<string>());
                deckCard.RemoveAt(deckCard.Count - 1);
               
            }
        }     
    }

    void SortDeckIntoTrips(){

        trips = deckCard.Count / 3;
        tripsRemainder = deckCard.Count % 3;
        // print("tripsRemainder : "+tripsRemainder);
        // print("trips : "+ trips);
        deckTrips.Clear();

        int index = 0;
        for (int i = 0; i < trips; i++) {
            List<string> myTrips = new List<string>();

            for (int j = 0; j < 3; j++) {
                myTrips.Add(deckCard[j + index]);
            }
            deckTrips.Add(myTrips);
            index = index + 3;
        }

        if(tripsRemainder != 0){
            List<string> myRemainders = new List<string>();
            index = 0;
            for (int k = 0; k < tripsRemainder; k++) {
                myRemainders.Add(deckCard[deckCard.Count - tripsRemainder + index]);
                index ++;
            }

            deckTrips.Add(myRemainders);
            trips ++;
        }
        deckLocation = 0;
    }

    public  void DealFromDeck(){
        foreach (Transform nameCard in deckButton.transform){
           if(nameCard.CompareTag("Card")){ 
                deckCard.Remove(nameCard.name);
                disCardPile.Add(nameCard.name);
                Destroy(nameCard.gameObject);
            }
        }

        if(deckLocation < trips){
            tripsOnDisplay.Clear();
            float xPos = 2.5f;
            float zPos = 0.3f;

            foreach (string nameCard in deckTrips[deckLocation]) {
                GameObject newTopCard = Instantiate(cardPrefab,new Vector3(deckButton.transform.position.x + xPos,deckButton.transform.position.y,deckButton.transform.position.z + zPos),Quaternion.identity,deckButton.transform);
                xPos = xPos + 0.5f;
                zPos = zPos - 0.3f;
                newTopCard.name = nameCard;
                tripsOnDisplay.Add(nameCard);
                newTopCard.GetComponent<Selectable>().faceUp = true;
                newTopCard.GetComponent<Selectable>().iSDeckPile = true;
            }
            deckLocation++;         
        }
        else{
            RestackTopDeck();
        }
    }  

  void RestackTopDeck(){
      deckCard.Clear();
    foreach (string nameCard in disCardPile){
        deckCard.Add(nameCard);
    }
    disCardPile.Clear();
    SortDeckIntoTrips();
    
  }


}