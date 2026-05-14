using System;
using System.Numerics;
using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;

public class Match : MonoBehaviour
{
    public class MatchGroup
    {
        public int Length;
        public List<Gem> matchedGems;
        public MatchGroup()
        {
            matchedGems = new List<Gem>();
        }
        public MatchGroup(int length, List<Gem> gems)
        {
            Length = length;
            matchedGems = gems;
        }
    }

    [SerializeField] private int requiredAdjacentMatches = 2;
    [SerializeField] private List<MatchGroup> matchGroups;

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
        return GetHorizontalMatchesOnBoard(board, pos).Count > 0 | GetVerticalMatchesOnBoard(board, pos).Count > 0;
    } 
    public List<MatchGroup> GetHorizontalMatchesOnBoard(Gem[,] board, Vector2Int gemPos)
    {
        int gemCounter = 1;
        int verticalIndex = gemPos.x;
        int boardWidth = board.GetLength(1) - 1;
        int start = 0, end = start + 1;
        var matches = new List<MatchGroup>();
        var match = new MatchGroup();
        while (end <= boardWidth)
        {
            Gem currentGem = board[verticalIndex, end];
            Gem gemToMatch = board[verticalIndex, start];

            match.matchedGems.Add(gemToMatch);
            if (currentGem.Type != gemToMatch.Type)
            {
                if (gemCounter >= 3)
                {
                    match.Length = gemCounter;
                    matches.Add(match);
                }
                match = new MatchGroup();
                start = end;
                gemCounter = 1;
            }
            else
            {
                gemCounter++;
                if (gemCounter >= 3)
                {
                    Debug.Log("MATCH!!!");
                    Debug.Log($"board[{verticalIndex},{start}] -> board[{verticalIndex},{end}]");
                    match.matchedGems.Add(currentGem);
                }
            }
            end++;
        }
        return matches != null ? matches : null;
    }
    public List<MatchGroup> GetVerticalMatchesOnBoard(Gem[,] board, Vector2Int gemPos)
    {
        int gemCounter = 1;
        int horizontalIndex = gemPos.y;
        int boardHeight = board.GetLength(0) - 1;
        int start = 0, end = start + 1;
        var matches = new List<MatchGroup>();
        var match = new MatchGroup();
        while (end <= boardHeight)
        {
            Gem currentGem = board[end, horizontalIndex];
            Gem gemToMatch = board[start, horizontalIndex];

            match.matchedGems.Add(gemToMatch);
            if (currentGem.Type != gemToMatch.Type)
            {
                if (gemCounter >= 3)
                {
                    match.Length = gemCounter;
                    matches.Add(match);
                }
                match = new MatchGroup();
                start = end;
                gemCounter = 1;

            }
            else
            {
                gemCounter++;
                if (gemCounter >= 3)
                {
                    Debug.Log("MATCH!!!");
                    Debug.Log($"board[{start},{horizontalIndex}] -> board[{end},{horizontalIndex}]");
                    match.matchedGems.Add(currentGem);
                }
            }
            end++;
        }
        return matches != null ? matches : null;
    }
    public IEnumerator OnMatch()
    {
        var seq = DOTween.Sequence();

        yield return seq.WaitForCompletion();
    }
}
