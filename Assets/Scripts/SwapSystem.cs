using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SwapSystem : MonoBehaviour
{
    [SerializeField] private BoardManager board ; 
    [SerializeField] private GemAnimation anim ;   
    
    void Start()
    {
        board = GetComponent<BoardManager>();
    }
    public bool IsAdjacent(Vector2Int currentGem, Vector2Int targetGem)
    {
        int xDiff = Mathf.Abs(currentGem.x - targetGem.x);
        int yDiff = Mathf.Abs(currentGem.y - targetGem.y);
        int isAdjacent = xDiff + yDiff;
        return isAdjacent == 1;
    }
    public void SwapBoardData(Gem gemA, Gem gemB)
    {
        Gem temp = gemA;
        Vector2Int prevAPos = gemA.BoardPosition;
        Vector2Int prevBPos = gemB.BoardPosition;
        UpdateGemPosition(gemA, gemB);
        board.SetBoardData(prevAPos.x , prevAPos.y , gemB) ; 
        board.SetBoardData(prevBPos.x , prevBPos.y, temp) ; 
    }
    public IEnumerator SwapRoutine(Gem gemA, Gem gemB)
    {
        if (IsAdjacent(gemA.BoardPosition, gemB.BoardPosition))
        {
            // Debug.Log($"gemA : {gemA.name} {gemA.BoardPosition} , gemB : {gemB.name} {gemB.BoardPosition}");
            SwapBoardData(gemA, gemB);
            //  Debug.Log($"gemA : {gemA.name} {gemA.BoardPosition} , gemB : {gemB.name} {gemB.BoardPosition}");
            yield return anim.PlaySwapAnimation(gemA, gemB);
            // Debug.Log($"{board[0,0].name} {board[0,0].BoardPosition} , {board[0,1].name} {board[0,1].BoardPosition}") ; 
        }
    }
    
    public void UpdateGemPosition(Gem gemA, Gem gemB)
    {
        Vector2Int gemAPrevPos = gemA.BoardPosition; 
        gemA.SetBoardPosition(gemB.BoardPosition);
        gemB.SetBoardPosition(gemAPrevPos);
    }
}
