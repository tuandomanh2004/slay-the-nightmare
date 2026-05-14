using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SwapSystem : MonoBehaviour
{
    private Match matchManager;
    public static float swapDuration = 0.5f ; 
    public static float destroyDuration = 0.3f;
    void Start()
    {
        matchManager = GetComponent<Match>();
    }

    // Update is called once per frame
    void Update()
    {

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
            Debug.Log($"gemA : {gemA.name} {gemA.BoardPosition} , gemB : {gemB.name} {gemB.BoardPosition}");

            SwapBoardData(board, gemA, gemB);
            Debug.Log($"gemA : {gemA.name} {gemA.BoardPosition} , gemB : {gemB.name} {gemB.BoardPosition}");
            yield return SwapAnimation(gemA, gemB);
            if (matchManager.HasMatch(board, gemA.BoardPosition, gemB.BoardPosition))
            {
                Debug.Log("MATCH");
            }
            else
            {
                SwapBoardData(board, gemA, gemB);
                yield return SwapAnimation(gemA, gemB);
            }
            // Debug.Log($"{board[0,0].name} {board[0,0].BoardPosition} , {board[0,1].name} {board[0,1].BoardPosition}") ; 
        }
    }
    public IEnumerator SwapAnimation(Gem gemA, Gem gemB)
    {
        // Create a container for managing tweens
        Sequence seq = DOTween.Sequence();
        seq.Join(gemA.SwapTo(gemA.BoardPosition));
        seq.Join(gemB.SwapTo(gemB.BoardPosition));
        yield return seq.WaitForCompletion();
    }
    public void UpdateGemPosition(Gem gemA, Gem gemB)
    {
        Vector2Int gemAPrevPos = gemA.BoardPosition;
        //  Debug.Log($"gemAPos :{gemAPrevPos} , gemBPos : {gemB}") ; 
        gemA.SetBoardPosition(gemB.BoardPosition);
        gemB.SetBoardPosition(gemAPrevPos);
    }
}
