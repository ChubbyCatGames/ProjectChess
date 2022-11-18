using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    //Dictionary with game cards and game objects
    private Dictionary<string, GameObject> CardsDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        fightUI.SetActive(false);


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

}
