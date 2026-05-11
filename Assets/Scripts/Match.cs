using System;
using System.Numerics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Match : MonoBehaviour
{
    [SerializeField] private int requiredAdjacentMatches = 2;
    [SerializeField] private bool hasMatch = false ; 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool HasMatch(Gem[,] board, Vector2Int gemAPos, Vector2Int gemBPos)
    {
        //int gemCounter = 1;
        if (IsVerticalSwap(gemAPos, gemBPos))
        {
           return HasVerticalMatch(board , gemAPos , gemBPos) || HasHorizontalMatch(board ,gemAPos, gemBPos);
            
        }
        return false ; 
    }
    public bool HasHorizontalMatch(Gem[,] board, Vector2Int gemAPos ,Vector2Int gemBPos){
       return true ; 
    }
    public bool HasVerticalMatch(Gem[,] board, Vector2Int gemAPos, Vector2Int gemBPos)
    {
        int gemCounter = 1;
        int horizontalIndex = gemAPos.y;
        int verticalStart = Math.Max(Math.Min(gemAPos.x, gemBPos.x) - requiredAdjacentMatches, 0);
        int verticalEnd = Math.Min(Math.Max(gemAPos.x, gemBPos.x) + requiredAdjacentMatches, board.GetLength(0) - 1);
        int startPointer = verticalStart, endPointer = verticalStart + 1;
      //  Debug.Log($"{verticalStart} , {verticalEnd} ");
        while (endPointer <= verticalEnd)
        {
           
            if (board[endPointer, horizontalIndex].Type != board[startPointer, horizontalIndex].Type)
            {
                startPointer = endPointer;
                gemCounter = 1;
            }
            else
            {
                gemCounter++;
                if (gemCounter >= 3)
                {
                    hasMatch = true;
                    Debug.Log("MATCH!!!") ; 
                    break ; 
                }
            }
            endPointer++;
        }
       
        for(int i = 0 ; i < board.GetLength(0) ; i++)
        {
            Debug.Log($"pos[{i},{horizontalIndex}] :  {board[i,horizontalIndex].name} , {board[i, horizontalIndex].BoardPosition}");
        }
      
        return hasMatch;
    }
    public bool IsVerticalSwap(Vector2Int gemAPos, Vector2Int gemBPos)
    {
        return Math.Abs(gemAPos.x - gemBPos.x) == 1 && gemAPos.y == gemBPos.y;
    }
}
