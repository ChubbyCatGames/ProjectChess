using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{

    [SerializeField]GameObject fightUI;
    [SerializeField]Transform attackerCardPos;
    [SerializeField]Transform defensorCardPos;
    private GameObject attackerCard;
    private GameObject defensorCard;
    private Animator cardAnimator;


    [SerializeField] Board board;
    //game object interfaz in game
    [SerializeField] GameObject inGameUi;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI blessingText;
    //cards prefabs
    [SerializeField] private GameObject[] cardsPrefabs;
    Animation animation;
    private GameObject card;
    [SerializeField] GameObject[] icons;

    [Header("Shop")]
    [SerializeField] GameObject shopImg;

    [Header("Names")]
    [SerializeField] TextMeshProUGUI name1;
    [SerializeField] TextMeshProUGUI name2;
    [SerializeField] TextMeshProUGUI name3;

    [Header("Descriptions")]
    [SerializeField] TextMeshProUGUI description1;
    [SerializeField] TextMeshProUGUI description2;
    [SerializeField] TextMeshProUGUI description3;

    [Header("Costs")]
    [SerializeField] TextMeshProUGUI cost1;
    [SerializeField] TextMeshProUGUI cost2;
    [SerializeField] TextMeshProUGUI cost3;

    
    GameObject item1;
    GameObject item2;
    GameObject item3;
    class ShopItem
    {
        public string name;
        public string description;
        public int cost;
        

        public ShopItem(string name, string description, int cost)
        {
            this.name = name;
            this.description = description;
            this.cost = cost;
        }
    }
    



    //Dictionary with game cards and game objects
    private Dictionary<string, GameObject> CardsDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        fightUI.SetActive(false);

        shopImg.SetActive(false);


        foreach (var concreteCard in cardsPrefabs)
        {
            CardsDict.Add(concreteCard.name, concreteCard);
            Debug.Log(concreteCard.name);
        }
    }
    public void ChangePlayerUI(Player player)
    {
        goldText.text= player.gold.ToString();
        blessingText.text= player.blessing.ToString();
    }
    
    public void UpdateUI()
    {
        if (board.getSelectedPiece() != null)
        {
            Piece piece = board.getSelectedPiece();
            //info.text = piece.GetData();
            //attackAndLife.text = piece.GetAttack() + " " + " " + " " + piece.GetLife();

            //Debug.Log(piece.GetName());
            //Debug.Log(CardsDict);
            if (CardsDict.ContainsKey(piece.GetName()))
            {
                card = CardsDict[piece.GetName()];
            }
           
            if(card != null) 
            {
                inGameUi.SetActive(false);
                card.SetActive(true);
                //activateIcons();

                //card.GetComponentInChildren<TextMeshProUGUI>().SetText("xd");
               
                    GameObject.Find("cardAttack").GetComponent<TextMeshProUGUI>().SetText(piece.GetAttack());
                    GameObject.Find("cardLife").GetComponent<TextMeshProUGUI>().SetText(piece.GetLife());
                    GameObject.Find("cardRichness").GetComponent<TextMeshProUGUI>().SetText(piece.GetRichness());
                    GameObject.Find("cardGold").GetComponent<TextMeshProUGUI>().SetText(goldText.text + "/" + piece.GetGoldDevelopCost());
                    GameObject.Find("cardBlessing").GetComponent<TextMeshProUGUI>().SetText(blessingText.text + "/" + piece.GetBlessingDevelopCost());

                /*
                    attack.text = piece.GetAttack();
                    life.text = piece.GetLife();
                    richness.text = piece.GetRichness();
                    fractionGold.text = goldText.text + "/" + piece.GetGoldDevelopCost();
                    fractionBlessing.text = blessingText.text + "/" + piece.GetBlessingDevelopCost();*/

            }

        }
        else
        {

            if (card != null)
            {
                card.SetActive(false);
                inGameUi.SetActive(true);
            }
        }
    }

    public IEnumerator StartFightUI(Piece attacker, Piece defensor,int hitsAtck,int hitsDef)
    {

        // NOT IMPLEMENTED YET ////////////////////////////////////////////////////////////////
        fightUI.SetActive(true);
        attackerCard = Instantiate(CardsDict[attacker.GetName()], attackerCardPos);
        defensorCard = Instantiate(CardsDict[defensor.GetName()], defensorCardPos);
        attackerCard.transform.position = attackerCardPos.position;
        defensorCard.transform.position = defensorCardPos.position;
        attackerCard.transform.localScale = new Vector3(0.25f,0.25f,0.8f);
        defensorCard.transform.localScale = new Vector3(0.25f,0.25f,0.8f);

        attackerCard.SetActive(true);
        defensorCard.SetActive(true);

        Debug.Log("hola" + attackerCard.name);
        //ANIMACION
        /*
        if(attackerCard.GetComponent<Animator>())
        {
            //bucle para numero de golpes
            Debug.Log(hitsAtck);
            int n = hitsAtck;

            
            while (n > 0)
            {
                cardAnimator = attackerCard.GetComponent<Animator>();
                cardAnimator.SetBool("isFighting", true);
                yield return new WaitForSeconds(0.2f);
                n--;
            }
            
            cardAnimator.SetBool("isFighting", false);
        }
        yield return new WaitForSeconds(1f);

        if (defensorCard.GetComponent<Animator>())
        {
            Debug.Log(hitsDef);

            
           int m = hitsDef;
            while (m > 0)
            {
                defensorCard.GetComponent<Animator>().SetBool("isDefending", true);
                yield return new WaitForSeconds(0.2f);
                m--;
            }

            defensorCard.GetComponent<Animator>().SetBool("isDefending", false);
    }*/
        int n = hitsAtck;
        int m = hitsDef;

        while (n + m > 0)
        {
            if (attackerCard.GetComponent<Animator>())
            {
                //bucle para numero de golpes
                Debug.Log(hitsAtck);

                if (n > 0)
                {
                    cardAnimator = attackerCard.GetComponent<Animator>();
                    cardAnimator.SetBool("isFighting", true);
                    yield return new WaitForSeconds(0.2f);
                    n--;
                }

                cardAnimator.SetBool("isFighting", false);
            }
            yield return new WaitForSeconds(1f);

            if (defensorCard.GetComponent<Animator>())
            {
                Debug.Log(hitsDef);

                if (m > 0)
                {
                    defensorCard.GetComponent<Animator>().SetBool("isDefending", true);
                    yield return new WaitForSeconds(0.2f);
                    m--;
                }

                defensorCard.GetComponent<Animator>().SetBool("isDefending", false);
            }
            yield return new WaitForSeconds(1f);
        }



            yield return new WaitForSeconds(3f);
        //yield return new WaitForSeconds(3f);
        //animation.Play();
        //yield return new WaitUntil(()=>!animation.isPlaying);
        //Ejecutar animaciones de pegarse
        //Hacer un diccionario de prefabs de cartas, llamarlas aqui y ejecutar su animacion.        

        fightUI.SetActive(false);
        attackerCard.SetActive(false);
        defensorCard.SetActive(false);
        
        //For{
        /*
         * ejecucion
         * wait
         * 
         */

        

    }

    public IEnumerator AttackAnimation()
    {
        //attackerCard.GetComponent<Animator>().SetBool("isFighting", true);
        yield return new WaitForSeconds(0.2f);
        //attackerCard.GetComponent<Animator>().SetBool("isFighting", false);
    }

    public IEnumerator DefenseAnimation()
    {
        //cardAnimator = defensorCard.GetComponent<Animator>();
        //cardAnimator.SetBool("isDefending", true);
        yield return new WaitForSeconds(0.2f);
        //cardAnimator.SetBool("isDefending", false);
    }

    public void StopFight()
    {
        //cardAnimator.SetBool("isFighting", false);
        //fightUI.SetActive(false);
        //Devolver a la corutina
    }

    public void activateIcons()
    {
        foreach (var i in icons)
        {
            i.SetActive(true);
        }
    }

    internal void OpenShop(List<GameObject> items)
    {
        shopImg.SetActive(true);
        item1 = items[0];
        item2 = items[1];
        item3 = items[2];
        Debug.Log(item1.GetComponent<Object>().objectName.ToString());
        ShopItem i1 = new ShopItem(item1.GetComponent<Object>().objectName, item1.GetComponent<Object>().objectDescription, item1.GetComponent<Object>().cost);
        ShopItem i2 = new ShopItem(item2.GetComponent<Object>().objectName, item2.GetComponent<Object>().objectDescription, item2.GetComponent<Object>().cost);
        ShopItem i3 = new ShopItem(item3.GetComponent<Object>().objectName, item3.GetComponent<Object>().objectDescription, item3.GetComponent<Object>().cost);
        
        name1.text = i1.name;
        name2.text = i2.name;
        name3.text = i3.name;

        /*
        description1.text = i1.description;
        description2.text = i2.description;
        description3.text = i3.description;

        cost1.text = i1.cost.ToString();
        cost2.text = i2.cost.ToString();
        cost3.text = i3.cost.ToString();
        */

    }
}
