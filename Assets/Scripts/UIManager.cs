using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI info;
    [SerializeField] TextMeshProUGUI attack;
    [SerializeField] TextMeshProUGUI life;
    [SerializeField] TextMeshProUGUI richness;
    [SerializeField] TextMeshProUGUI fractionBlessing;
    [SerializeField] TextMeshProUGUI fractionGold;

    [SerializeField]GameObject fightUI;
    [SerializeField]Transform attackerCardPos;
    [SerializeField]Transform defensorCardPos;
    private GameObject attackerCard;
    private GameObject defensorCard;


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
            //churchCard.SetActive(true);
            if(card != null) 
            {
                inGameUi.SetActive(false);
                card.SetActive(true);
                //activateIcons();

                attack.text = piece.GetAttack();
                life.text = piece.GetLife();
                richness.text = piece.GetRichness();
                fractionGold.text = goldText.text + "/" + piece.GetGoldDevelopCost();
                fractionBlessing.text = blessingText.text + "/" + piece.GetBlessingDevelopCost();
            }

        }
        else
        {
            info.text = "";

            if (card != null)
            {
                card.SetActive(false);
                attack.text = "";
                life.text = "";
                richness.text = "";
                fractionGold.text = "";
                fractionBlessing.text = "";
                inGameUi.SetActive(true);
            }
        }
    }

    public IEnumerator StartFightUI(Piece attacker, Piece defensor,int hitsAtck,int hitsDef)
    {

        // NOT IMPLEMENTED YET ////////////////////////////////////////////////////////////////
        fightUI.SetActive(true);
        attackerCard = Instantiate(CardsDict[attacker.GetName()], attackerCardPos );
        defensorCard = Instantiate( CardsDict[defensor.GetName()], defensorCardPos);
        attackerCard.transform.position = attackerCardPos.position;
        defensorCard.transform.position = defensorCardPos.position;
        attackerCard.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        defensorCard.transform.localScale = new Vector3(0.8f,0.8f,0.8f);

        attackerCard.SetActive(true);
        defensorCard.SetActive(true);

        yield return new WaitForSeconds(3f);
        //animation.Play();
        //yield return new WaitUntil(()=>!animation.isPlaying);
        //Ejecutar animaciones de pegarse
        //Hacer un diccionario de prefabs de cartas, llamarlas aqui y ejecutar su animacion.

        fightUI.SetActive(false);
        attackerCard.SetActive(false);
        defensorCard.SetActive(false);
    
    }

    public void StopFight()
    {
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
