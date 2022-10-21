using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI info;
    [SerializeField]GameObject fightUI;
    [SerializeField] Board board;
    Animation animation;

    private void Awake()
    {
        fightUI.SetActive(false);
    }
    public void UpdateUI()
    {
        if (board.getSelectedPiece() != null)
        {
            Piece piece = board.getSelectedPiece();
            info.text = piece.GetData();
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
