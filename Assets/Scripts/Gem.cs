using System;
using Unity.VisualScripting;
using UnityEngine;

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

    public void SetBoardPosition(Vector2Int boardPosition)
    {
        BoardPosition = boardPosition;
    }
    public Vector2Int WorldPos => new Vector2Int(BoardPosition.y,BoardPosition.x) ; 
    void OnMouseDown()
    {
        OnGemClicked?.Invoke(this) ; 
    }
}
