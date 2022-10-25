using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI info;
    [SerializeField] TextMeshProUGUI attackAndLife;
    [SerializeField]GameObject fightUI;
    [SerializeField] Board board;
    //game object interfaz in game
    [SerializeField] GameObject inGameUi;
    //one Card
    [SerializeField] GameObject churchCard;
    //cards prefabs
    [SerializeField] private GameObject[] cardsPrefabs;
    Animation animation;


    //Dictionary with game cards and game objects
    private Dictionary<string, GameObject> CardsDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        fightUI.SetActive(false);
    }
    public void UpdateUI()
    {
        if (board.getSelectedPiece() != null)
        {
            Piece piece = board.getSelectedPiece();
            //info.text = piece.GetData();
            attackAndLife.text = piece.GetAttack() + " " + " " + " " + piece.GetLife();
            inGameUi.SetActive(false); 
            churchCard.SetActive(true);
        }
        else
        {
            info.text = "";
        }
    }

    public IEnumerator StartFightUI(int hitsAtck,int hitsDef)
    {
        fightUI.SetActive(true);
        yield return new WaitForSeconds(.4f);
        animation.Play();
        yield return new WaitUntil(()=>!animation.isPlaying);
        //Ejecutar animaciones de pegarse
        //Hacer un diccionario de prefabs de cartas, llamarlas aqui y ejecutar su animacion.

        fightUI.SetActive(false);
    
    }

    public void StopFight()
    {
        //fightUI.SetActive(false);
        //Devolver a la corutina
    }
}
