using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI info;
    [SerializeField]GameObject fightUI;
    [SerializeField] Board board;

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

    public void StartFightUI()
    {
        fightUI.SetActive(true);
    }

    public void StopFight()
    {
        fightUI.SetActive(false);
    }
}
