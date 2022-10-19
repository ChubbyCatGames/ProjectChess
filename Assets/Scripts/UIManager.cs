using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI info;
    [SerializeField] Board board;


    public void UpdateUI()
    {
        if (board.getSelectedPiece() != null)
        {
            Piece piece = board.getSelectedPiece();
            info.text = "Name: " + piece.GetType().ToString() + "<br>Vida: "+ piece.life.ToString() + "<br>Atack: " + piece.attackDmg.ToString();
        }
        else
        {
            info.text = "";
        }
    }
}
