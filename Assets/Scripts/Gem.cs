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

    public void Init(Vector2Int boardPosition)
    {
        BoardPosition = boardPosition;
        name = $"{type} Gem ({boardPosition.y}, {boardPosition.x})";
    }

    public void SetBoardPosition(Vector2Int boardPosition)
    {
        BoardPosition = boardPosition;
    }
}
