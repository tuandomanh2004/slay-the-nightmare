using UnityEngine;

public class SwapSystem : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsAdjacent(Vector2Int currentGem , Vector2Int targetGem)
    {
        int xDiff = Mathf.Abs(currentGem.x - targetGem.x) ; 
        int yDiff = Mathf.Abs(currentGem.y - targetGem.y) ;
        int isAdjacent = xDiff + yDiff  ; 
        return isAdjacent == 1 ; 
    }
    public void SwapBoardData(Gem[,] board ,Gem gemA , Gem gemB)
    {
        Gem temp = gemA ;
        board[gemA.BoardPosition.x , gemA.BoardPosition.y] = gemB ; 
        board[gemB.BoardPosition.x , gemB.BoardPosition.y] = temp ; 
    }
    public void UpdateGemPosition( Gem gemA , Gem gemB)
    {
        Vector2Int gemAPrevPos = gemA.BoardPosition ; 
      //  Debug.Log($"gemAPos :{gemAPrevPos} , gemBPos : {gemB}") ; 
        gemA.SetBoardPosition(gemB.BoardPosition) ;
        gemB.SetBoardPosition(gemAPrevPos) ; 
    }
}
