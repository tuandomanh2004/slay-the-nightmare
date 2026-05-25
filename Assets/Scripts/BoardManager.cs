using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject backgroundTile;
    [SerializeField] private Gem[] gems;
    [SerializeField] private Gem[,] board ; 
    [SerializeField] private SwapSystem swapManager; 
    public static Vector2 BoardOffset { get; private set; }
    public static float CellSpace { get; private set; } = 1.1f;
    public Gem[,] Board => board;
    public int Width => width ; 
    public int Height => height ;   

    void OnEnable()
    {
        InputHandler.OnSwapRequested += TrySwapGem;
        Match.OnGemsDestroyed += RemoveGemsFromBoard ; 
    }
    void OnDisable()
    {
        InputHandler.OnSwapRequested -= TrySwapGem;
        Match.OnGemsDestroyed -= RemoveGemsFromBoard ;
    }
    void Start()
    {
        swapManager = GetComponent<SwapSystem>();
        InitBoard();

    }
    private Gem GetRandomGem()
    {
        int gemIndex = Random.Range(0, gems.Length);
        return gems[gemIndex];
    }
    private Gem GetRandomGem(List<Gem> possibleGems)
    {
        int gemIndex = Random.Range(0, possibleGems.Count);
        return possibleGems[gemIndex];
    }
    public Gem SpawnGem(Gem gem, Vector2 worldPos, Vector2Int boardPos)
    {
        Gem gemToSpawn = Instantiate(gem, worldPos, Quaternion.identity);
        gemToSpawn.transform.parent = transform;
        gemToSpawn.transform.localPosition = worldPos;
        gemToSpawn.Init(boardPos);
        return gemToSpawn;
    }
    public void TrySwapGem(Gem gemA, Gem gemB)
    {
        StartCoroutine(swapManager.SwapRoutine(board, gemA, gemB));
    }
    private void InitBoard()
    {
        board = new Gem[height, width];
        BoardOffset = new Vector2((width - 1) * CellSpace / 2, (height - 1) * CellSpace / 2);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                List<Gem> possibleGems = gems.ToList();
                if (x >= 2)
                {
                    if (board[y, x - 2].Type == board[y, x - 1].Type)
                    {
                        possibleGems.RemoveAll(gem => gem.Type == board[y, x - 1].Type);
                    }
                }
                if (y >= 2)
                {
                    if (board[y - 2, x].Type == board[y - 1, x].Type)
                    {
                        possibleGems.RemoveAll(gem => gem.Type == board[y - 1, x].Type);
                    }
                }
                var currentGem = GetRandomGem(possibleGems);
                var gemBoardPos = new Vector2Int(y, x);
                var gemWorldPos = new Vector2(x * CellSpace, y * CellSpace) - BoardOffset;
                Debug.Log(possibleGems.Count);
                Debug.Log($"{currentGem} , {gemBoardPos} , {gemWorldPos}");
                board[y, x] = SpawnGem(currentGem, gemWorldPos, gemBoardPos);
            }
        }
    }
    private void RemoveGemsFromBoard (HashSet<Gem> gemsToDestroy)
    {
        foreach(var gem in gemsToDestroy)
        {
            var pos = gem.BoardPosition ; 
            board[pos.x , pos.y] = null ;
            Debug.Log(board[pos.x , pos.y]) ; 
        }

    }
    public void MoveGemAtCol(int col ,int startRow , int endRow)
    {
        var gemAtStartPos = board[startRow,col] ;
        var newPosition = new Vector2Int(endRow , col) ; 
        gemAtStartPos.SetBoardPosition(newPosition) ; 
        board[endRow ,col] = gemAtStartPos ; 
        board[startRow ,col] = null ;
    }
}
