using System;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening ;
using System.Collections;
using Unity.Collections;

public class Gem : MonoBehaviour
{
    public enum GemType
    {
        Attack,
        Defense,
        Rage
    }

    [SerializeField] private GemType type;

    public GemType Type => type;
    public Vector2Int BoardPosition { get; private set; }
    public static event Action<Gem> OnGemClicked ; 

    public void Init(Vector2Int boardPosition)
    {
        BoardPosition = boardPosition;
        name = $"{type}";
    }
    public Vector3 GetWorldPosition(Vector2Int boardPos)
    {
        Vector3 offset = new Vector3(BoardManager.BoardOffset.x , BoardManager.BoardOffset.y , 0f) ;
        Vector3 GemWorldPos = new Vector3(boardPos.y * BoardManager.CellSpace , boardPos.x *BoardManager.CellSpace,0f) - offset ;
        return GemWorldPos ;  
    }
    public void SetBoardPosition(Vector2Int boardPosition)
    {
        BoardPosition = boardPosition;
    }
    public Vector2Int WorldPos => new Vector2Int(BoardPosition.y,BoardPosition.x) ; 
    void OnMouseDown()
    {
        OnGemClicked?.Invoke(this) ; 
    }
    public Tween SwapTo(Vector2Int swapPos)
    {
        Vector3 worldPos = GetWorldPosition(swapPos);
        return transform.DOLocalMove(worldPos,SwapSystem.swapDuration) ; 
    }
    public Tween Destroy()
    {
        Vector3 targetScale = Vector3.zero; 
        return transform.DOScale(targetScale,SwapSystem.destroyDuration) ;
    }
}
