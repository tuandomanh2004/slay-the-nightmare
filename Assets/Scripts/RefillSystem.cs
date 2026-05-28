using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class RefillSystem : MonoBehaviour
{
    [SerializeField] private GemAnimation anim;
    [SerializeField] private float heightScale;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private GravitySystem gravity;
    [SerializeField] private Dictionary<int, List<Gem>> gemsToSpawn = new Dictionary<int, List<Gem>>();
    void Start()
    {
        // boardManager = GetComponent<BoardManager>();
        gravity = GetComponent<GravitySystem>();
    }
    public void RefillBoardData()
    {
        gemsToSpawn.Clear();
        for (int col = 0; col < boardManager.Width; col++)
        {
            if (!gravity.EmptySlotsPerColumn.TryGetValue(col, out int spawnQuantity))
            {
                Debug.LogWarning($"Column {col} not found in EmptySlotsPerColumn");
                continue;
            }
            //Debug.Log(spawnQuantity);
            if (spawnQuantity == 0) continue;

            gemsToSpawn[col] = new List<Gem>();
            for (int index = 0; index < spawnQuantity; index++)
            {
                int row = boardManager.Height - spawnQuantity + index;
                var gemBoardPos = new Vector2Int(row, col);
                Gem gem = SpawnGemOnTop(boardManager.Gems, gemBoardPos);

                boardManager.SetBoardData(gemBoardPos.x, gemBoardPos.y, gem);
                gemsToSpawn[col].Add(gem);
                //  Debug.Log($"[{row},{col}] : {boardManager.Board[row, col]}");
            }
        }
    }
    public Gem SpawnGemOnTop(Gem[] gemPrefabs, Vector2Int boardPos)
    {
        int randomIndex = UnityEngine.Random.Range(0, gemPrefabs.Length);
        Vector2 worldPosOnBoard = boardManager.ConvertToWorldPosition(boardPos);
        Vector2 offset = Vector2.up * heightScale;
        Vector2 onTopPos = worldPosOnBoard + offset;
        return boardManager.SpawnGem(gemPrefabs[randomIndex], onTopPos, boardPos);
    }
    public IEnumerator OnRefilled()
    {
        RefillBoardData();
        yield return anim.PlayFallingAnimation(gemsToSpawn);
    }
}
