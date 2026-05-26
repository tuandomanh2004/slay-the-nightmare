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
    public Vector2 GetWorldPosition(Vector2Int boardPos)
    {
        Vector2 offset = new Vector2(BoardManager.BoardOffset.x , BoardManager.BoardOffset.y ) ;
        Vector2 GemWorldPos = new Vector2(boardPos.y * BoardManager.CellSpace , boardPos.x *BoardManager.CellSpace) - offset ;
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
    public Tween SwapTo(Vector2Int swapPos , float duration)
    {
        Vector2 worldPos = GetWorldPosition(swapPos);
        return transform.DOLocalMove(worldPos,duration) ; 
    }
    public Tween Destroy(float duration)
    {
        Vector2 targetScale = Vector3.zero; 
        return transform.DOScale(targetScale,duration) ;
    }
}
