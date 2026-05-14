using System;
using System.Numerics;
using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using Unity.Collections;

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
    [SerializeField] private HashSet<Gem> gemsToDestroy = new HashSet<Gem>() ; 

    void Start()
    {

    }
    void Update()
    {

    }
    public bool HasMatch(Gem[,] board, Vector2Int gemAPos, Vector2Int gemBPos)
    {
        HashSet<Gem> matchedGemsA = GetMatchedGemsAt(board, gemAPos) ;
        HashSet<Gem> matchedGemsB = GetMatchedGemsAt(board, gemBPos) ; 
        matchedGemsA.UnionWith(matchedGemsB);
        gemsToDestroy = matchedGemsA;
        return gemsToDestroy.Count > 0 ;
    }
    public HashSet<Gem> GetMatchedGemsAt(Gem[,] board, Vector2Int pos)
    {
        var horizontalMatches = GetHorizontalMatchesOnBoard(board, pos); 
        var verticalMatches = GetVerticalMatchesOnBoard(board, pos) ;

        var horizontalGemsToDestroy = GetGemsToDestroy(horizontalMatches) ; 
        var verticalGemsToDestroy = GetGemsToDestroy(verticalMatches) ; 

        horizontalGemsToDestroy.UnionWith(verticalGemsToDestroy) ; 
        return horizontalGemsToDestroy ;
    } 
    public List<MatchGroup> GetHorizontalMatchesOnBoard(Gem[,] board, Vector2Int gemPos)
    {
        int verticalIndex = gemPos.x;
        int boardWidth = board.GetLength(1);
        var matches = new List<MatchGroup>();
        
        int start = 0;
        while (start < boardWidth)
        {
            int end = start;
            // Tìm dãy liên tiếp cùng loại
            while (end < boardWidth && board[verticalIndex, end].Type == board[verticalIndex, start].Type)
            {
                end++;
            }
            
            // Nếu dãy >= 3 viên, thêm vào match
            if (end - start >= 3)
            {
                var match = new MatchGroup();
                for (int i = start; i < end; i++)
                {
                    match.matchedGems.Add(board[verticalIndex, i]);
                }
                match.Length = end - start;
                matches.Add(match);
            }
            
            start = end;
        }
        
        return matches;
    }
    public List<MatchGroup> GetVerticalMatchesOnBoard(Gem[,] board, Vector2Int gemPos)
    {
        int horizontalIndex = gemPos.y;
        int boardHeight = board.GetLength(0);
        var matches = new List<MatchGroup>();
        
        int start = 0;
        while (start < boardHeight)
        {
            int end = start;
            // Tìm dãy liên tiếp cùng loại
            while (end < boardHeight && board[end, horizontalIndex].Type == board[start, horizontalIndex].Type)
            {
                end++;
            }
            
            // Nếu dãy >= 3 viên, thêm vào match
            if (end - start >= 3)
            {
                var match = new MatchGroup();
                for (int i = start; i < end; i++)
                {
                    match.matchedGems.Add(board[i, horizontalIndex]);
                }
                match.Length = end - start;
                matches.Add(match);
            }
            start = end;
        }
        
        return matches;
    }
    public IEnumerator OnMatch()
    {
        var seq = DOTween.Sequence() ; 
        Debug.Log(gemsToDestroy) ; 
        if(gemsToDestroy != null)
        {
            
            foreach(var gem in gemsToDestroy)
            {
                Debug.Log(gem) ; 
                seq.Join(gem.Destroy()) ; 
            }
        }
        yield return seq.WaitForCompletion();
    }
    public HashSet<Gem> GetGemsToDestroy(List<MatchGroup> groups)
    {
        HashSet<Gem> gemsToDestroy = new HashSet<Gem>();
        foreach(var group in groups)
        {
            foreach(var gem in group.matchedGems)
            {
                gemsToDestroy.Add(gem);
            }
        }
        return gemsToDestroy ; 
    }
}
