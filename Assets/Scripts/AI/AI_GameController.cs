using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AI_GameController : GameController
{

    internal override bool IsPlayerTurn()
    {
        if (IsTeamTurnActive(PieceColor.White))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ChangeActiveTeam()
    {
        base.ChangeActiveTeam();
        if (!IsPlayerTurn())
        {
            StartCoroutine(AI_Turn());

        }
    }

    public IEnumerator AI_Turn()
    {

        LastMove moveToDo;
        VirtualBoard virtualBoard = new VirtualBoard();
        int blessing = board.controller.activePlayer.blessing;
        int gold = board.controller.activePlayer.gold;
        List<Piece> piecesToEvolve = new List<Piece>();
        bool evolve=false;

        foreach(var piece in board.controller.activePlayer.activePieces)
        {
            if(piece.goldDevelopCost<=gold && piece.blessingDevelopCost <= blessing)
            {
                piecesToEvolve.Add(piece);
                evolve = true;
            }
        }
        if (evolve)
        {

            int rand = Random.Range(0, piecesToEvolve.Count);
            Piece piece = piecesToEvolve[rand];
            if (piece.GetType() == typeof(Pawn))
            {
                if (rand % 2 == 0)
                {
                    board.SelectPiece(piece);
                    board.OnSelectedPiecePromoteFaith();
                }
                else
                {
                    board.SelectPiece(piece);
                    board.OnSelectedPiecePromoteWar();
                }

            }
            else if(piece.GetType() == typeof(Knight)|| piece.GetType() == typeof(Rook)) 
            {
                board.SelectPiece(piece);
                board.OnSelectedPiecePromoteWar();
            }else if(piece.GetType() == typeof(Bishop) || piece.GetType() == typeof(Church))
            {
                board.SelectPiece(piece);
                board.OnSelectedPiecePromoteFaith();
            }
        }

        virtualBoard.grid = board.grid;

        Minimax(virtualBoard, 1, true, out moveToDo);
        Piece p = board.GetPieceOnSquare(moveToDo.oldCoords);
       

        yield return new WaitForSeconds(1f);
        board.OnAIMove(moveToDo.newCoords, p);

        yield return new WaitForSeconds(0.2f);
        if (p.GetType() != typeof(King))
        {
            board.CheckGridEventsAI(moveToDo.newCoords, p);
        }
        board.EndTurn();
    }

    private float Minimax(VirtualBoard virtualBoard,int depth, bool maximizingPlayer, out LastMove lastMove)
    {
        lastMove = board.moves;
        if(depth==0 || CheckIfGameIsFinished())
        {
            Debug.Log("evalua");
            return EvaluateBoard(virtualBoard.grid);
        }
        if(maximizingPlayer)
        {
            float maxEval = Mathf.NegativeInfinity;
            //Hacer simulacion de todos los movimientos posibles
            List<VirtualBoard> virtualBoards= new List<VirtualBoard>();
            //virtualBoard = eachMove;
            foreach(Piece p in activePlayer.activePieces)
            {
                List<Vector2Int> possibleMoves = p.avaliableMoves;
                foreach (var move in possibleMoves)
                {
                    virtualBoards.Add(virtualBoard.GenerateVirtualBoard(p, move));

                }
                possibleMoves.Clear();
            }
            foreach(var child in virtualBoards)
            {
                float eval = Minimax(child, depth - 1, false, out board.moves);
                if(eval > maxEval)
                {
                    lastMove = child.moves;
                }
                maxEval = Mathf.Max(maxEval, eval);
            }
            virtualBoards.Clear();
            return maxEval;
            
        }
        else
        {
            float minEval = Mathf.Infinity;
            //Hacer simulacion de todos los movimientos posibles
            List<VirtualBoard> virtualBoards = new List<VirtualBoard>();
            //virtualBoard = eachMove;
            foreach (Piece p in board.controller.GetOpponentToPlayer(activePlayer).activePieces)
            {
                List<Vector2Int> possibleMoves = p.avaliableMoves;
                foreach (var move in possibleMoves)
                {
                    virtualBoards.Add(virtualBoard.GenerateVirtualBoard(p,move));

                }
                possibleMoves.Clear();
            }
            foreach (var child in virtualBoards)
            {
                float eval = Minimax(child, depth - 1, true,out virtualBoard.moves);
                minEval = Mathf.Min(minEval, eval);

            }
            virtualBoards.Clear();
            return minEval;
        }
    }

    public float EvaluateBoard(Piece[,]grid)
    {
        //Scores
        float whiteScore = 0;
        float blackScore = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (grid[i, j] != null)
                {
                    Piece piece = grid[i, j];
                    if (piece.color == PieceColor.Black)
                    {
                        blackScore += piece.value + piece.gridValues[7 - i, 7 - j];

                    }
                    else
                    {
                        whiteScore += piece.value + piece.gridValues[i, j];
                    }
                }
            }
        }
        float value = blackScore - whiteScore;
        return value;
    }
}
