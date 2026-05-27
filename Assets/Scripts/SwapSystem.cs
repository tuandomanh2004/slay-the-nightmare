using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SwapSystem : MonoBehaviour
{
    [SerializeField] private GemAnimation anim ; 
    private Match matchManager;
    private GravitySystem gravity;
    private RefillSystem refill ; 
    
    void Start()
    {
        matchManager = GetComponent<Match>();
        gravity = GetComponent<GravitySystem>();
        refill = GetComponent<RefillSystem>();
    }
    public bool IsAdjacent(Vector2Int currentGem, Vector2Int targetGem)
    {
        int xDiff = Mathf.Abs(currentGem.x - targetGem.x);
        int yDiff = Mathf.Abs(currentGem.y - targetGem.y);
        int isAdjacent = xDiff + yDiff;
        return isAdjacent == 1;
    }
    public void SwapBoardData(Gem[,] board, Gem gemA, Gem gemB)
    {
        Gem temp = gemA;
        Vector2Int prevAPos = gemA.BoardPosition;
        Vector2Int prevBPos = gemB.BoardPosition;
        UpdateGemPosition(gemA, gemB);
        board[prevAPos.x, prevAPos.y] = gemB;
        board[prevBPos.x, prevBPos.y] = temp;

    }
    public IEnumerator SwapRoutine(Gem[,] board, Gem gemA, Gem gemB)
    {
        if (IsAdjacent(gemA.BoardPosition, gemB.BoardPosition))
        {
            // Debug.Log($"gemA : {gemA.name} {gemA.BoardPosition} , gemB : {gemB.name} {gemB.BoardPosition}");

            SwapBoardData(board, gemA, gemB);
            //  Debug.Log($"gemA : {gemA.name} {gemA.BoardPosition} , gemB : {gemB.name} {gemB.BoardPosition}");
            yield return anim.PlaySwapAnimation(gemA, gemB);
            if (matchManager.HasMatch(board, gemA.BoardPosition, gemB.BoardPosition))
            {
                //  Debug.Log("MATCH") ; 
                yield return matchManager.OnMatch();
                yield return gravity.OnDestroyedGems() ; 
                yield return refill.OnRefilled()  ;
            }
            else
            {
                SwapBoardData(board, gemA, gemB);
                yield return anim.PlaySwapAnimation(gemA, gemB);
            }
            // Debug.Log($"{board[0,0].name} {board[0,0].BoardPosition} , {board[0,1].name} {board[0,1].BoardPosition}") ; 
        }
    }
    
    public void UpdateGemPosition(Gem gemA, Gem gemB)
    {
        Vector2Int gemAPrevPos = gemA.BoardPosition;
        //  Debug.Log($"gemAPos :{gemAPrevPos} , gemBPos : {gemB}") ; 
        gemA.SetBoardPosition(gemB.BoardPosition);
        gemB.SetBoardPosition(gemAPrevPos);
    }
}
