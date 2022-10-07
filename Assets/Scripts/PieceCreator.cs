using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private Material blackRookMaterial;
    [SerializeField] private Material whiteRookMaterial;
    [SerializeField] private Material blackQueenMaterial;
    [SerializeField] private Material whiteQueenMaterial;
    [SerializeField] private Material blackPawnMaterial;
    [SerializeField] private Material whitePawnMaterial;
    [SerializeField] private Material blackKnightMaterial;
    [SerializeField] private Material whiteKnightMaterial;
    [SerializeField] private Material blackKingMaterial;
    [SerializeField] private Material whiteKingMaterial;
    [SerializeField] private Material blackBishopMaterial;
    [SerializeField] private Material whiteBishopMaterial;
    [SerializeField] private Material blackChurchMaterial;
    [SerializeField] private Material whiteChurchMaterial;
    [SerializeField] private Material blackPopeMaterial;
    [SerializeField] private Material whitePopeMaterial;

    private Dictionary<string, GameObject> nameToPieceDict = new Dictionary<string, GameObject>();


    private void Awake()
    {
        foreach (var piece in piecesPrefabs)
        {
            nameToPieceDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
        }
    }

    public GameObject CreatePiece(Type type)
    {
        GameObject prefab = nameToPieceDict[type.ToString()];
        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab);
            return newPiece;
        }
        return null;
    }

    public Material GetPieceMaterial(PieceColor color, Type type)
    {
        switch (color)
        {
            case PieceColor.White:
                switch (type.ToString())
                {
                    case "Rook":
                        return whiteRookMaterial;
                    case "Queen":
                        return whiteQueenMaterial;
                    case "Pawn":
                        return whitePawnMaterial;
                    case "Knight":
                        return whiteKnightMaterial;
                    case "King":
                        return whiteKingMaterial;
                    case "Bishop":
                        return whiteBishopMaterial;
                    case "Church":
                        return whiteChurchMaterial;
                    case "Pope":
                        return whitePopeMaterial;

                    default:
                        return null;
                }
            case PieceColor.Black:
                switch (type.ToString())
                {
                    case "Rook":
                        return blackRookMaterial;
                    case "Queen":
                        return blackQueenMaterial;
                    case "Pawn":
                        return blackPawnMaterial;
                    case "Knight":
                        return blackKnightMaterial;
                    case "King":
                        return blackKingMaterial;
                    case "Bishop":
                        return blackBishopMaterial;
                    case "Church":
                        return blackChurchMaterial;
                    case "Pope":
                        return blackPopeMaterial;

                    default:
                        return null;
                }
            default:
                return null;
            

        }
    }
}
