using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Match : MonoBehaviour
{
    [SerializeField] private int requiredAdjacentMatches = 2;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool HasMatch(Gem[,] board, Vector2Int gemAPos, Vector2Int gemBPos)
    {
        //int minGemToMatch = 1;
        if (IsVerticalSwap(gemAPos, gemBPos))
        {
            HasVerticalMatch(board , gemAPos , gemBPos) ; 
        }
        // else
        // {
            
        // }
        return false ; 
    }
    public bool HasVerticalMatch(Gem[,] board, Vector2Int gemAPos, Vector2Int gemBPos)
    {
        bool hasMatch = false ; 
        int minGemToMatch = 1;
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
                minGemToMatch = 1;
            }
            else
            {
                minGemToMatch++;
                if (minGemToMatch >= 3)
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
        return Math.Abs(gemAPos.x - gemBPos.x) == 1;
    }
}
