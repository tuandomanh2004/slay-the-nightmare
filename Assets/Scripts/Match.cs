using System;
using System.Numerics;
using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using System.Collections.Generic ; 

public class Match : MonoBehaviour
{
    [SerializeField] private int requiredAdjacentMatches = 2;
    [SerializeField] private bool hasMatch = false;
    void Start()
    {

    }
    void Update()
    {

    }
    public bool HasMatch(Gem[,] board, Vector2Int gemAPos, Vector2Int gemBPos)
    {
        return HasMatchAt(board, gemAPos) | HasMatchAt(board, gemBPos);
    }
    public bool HasMatchAt(Gem[,] board, Vector2Int pos)
    {
        return HasHorizontalMatch(board, pos) | HasVerticalMatch(board, pos);
    }
    public bool HasHorizontalMatch(Gem[,] board, Vector2Int gemPos)
    {
        int gemCounter = 1 ; 
        int verticalIndex = gemPos.x ;
        int boardWidth = board.GetLength(1) - 1 ; 
        int start = 0 , end = start + 1 ; 
        while(end <= boardWidth)
        {
            Gem currentGem = board[verticalIndex, end];
            Gem gemToMatch = board[verticalIndex,start];
            if (currentGem.Type != gemToMatch.Type)
            {
                start = end;
                gemCounter = 1;
            }
            else
            {
                gemCounter++;
                if (gemCounter >= 3)
                {
                    Debug.Log("MATCH!!!");
                    Debug.Log($"board[{verticalIndex},{start}] -> board[{verticalIndex},{end}]") ; 
                    return true ; 
                }
            }
            end++ ; 
        }  
        return false ; 
    }
    public bool HasVerticalMatch(Gem[,] board, Vector2Int gemPos)
    {
        int gemCounter = 1;
        int horizontalIndex = gemPos.y;
        int boardHeight = board.GetLength(0) - 1;
        int start = 0, end = start + 1;
        while (end <= boardHeight)
        {
            Gem currentGem = board[end, horizontalIndex];
            Gem gemToMatch = board[start, horizontalIndex];
            if (currentGem.Type != gemToMatch.Type)
            {
                start = end;
                gemCounter = 1;
            }
            else
            {
                gemCounter++;
                if (gemCounter >= 3)
                {
                    Debug.Log("MATCH!!!");
                    Debug.Log($"board[{start},{horizontalIndex}] -> board[{end},{horizontalIndex}]") ; 
                    return true ; 
                }
            }
            end++ ; 
        }
        return false ;  
    }
}
