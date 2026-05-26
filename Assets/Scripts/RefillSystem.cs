using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class RefillSystem : MonoBehaviour
{
    [SerializeField] private float heightScale ; 
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private GravitySystem gravity;
    [SerializeField] private Dictionary<int, List<Gem>> gemsToSpawn = new Dictionary<int, List<Gem>>();
    void Start()
    {
        boardManager = GetComponent<BoardManager>();
        gravity = GetComponent<GravitySystem>();
    }
    public void RefillBoardData()
    {
        gemsToSpawn.Clear();
        for (int col = 0; col < boardManager.Width; col++)
        {
            int spawnQuantity = gravity.EmptySlotsPerColumn[col];
            //Debug.Log(spawnQuantity);
            if (spawnQuantity == 0) continue;

            gemsToSpawn[col] = SetGemDataToSpawnEachColumn(spawnQuantity, col);
            for (int index = 0; index < spawnQuantity; index++)
            {
                int row = boardManager.Height - spawnQuantity + index;
                Gem currentGemData = gemsToSpawn[col][index] ; 
                Vector2Int gemPos=  new Vector2Int(row , col) ; 
                Gem gemToSpawn = SpawnGemOnTop(currentGemData, gemPos);
                boardManager.SetBoardData(row, col, gemToSpawn);
            //  Debug.Log($"[{row},{col}] : {boardManager.Board[row, col]}");
            }
        }
    }
    private List<Gem> SetGemDataToSpawnEachColumn(int quantity, int col)
    {
        List<Gem> gems = new List<Gem>();
        for (int i = quantity; i > 0; i--)
        {
            Gem randomGem = boardManager.GetRandomGem(boardManager.Gems.ToList());
            gems.Add(randomGem);
        }
        return gems;
    }
    public Gem SpawnGemOnTop(Gem gem , Vector2Int boardPos)
    {
        Vector2 worldPosOnBoard = boardManager.ConvertToWorldPosition(boardPos);
        Vector2 offset = Vector2.up * heightScale;
        Vector2 onTopPos = worldPosOnBoard + offset ; 
        return boardManager.SpawnGem(gem , onTopPos , boardPos) ;   
    }
}
