using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Runtime.InteropServices;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private GemAnimation anim;
    [SerializeField] private Dictionary<int, List<Gem>> fallingGems;
    [SerializeField] private Dictionary<int, int> emptySlotsPerColumn;
    [SerializeField] private float delayAfterFalling = 0.5f;
    [SerializeField] private float fallingDuration = 0.2f;

    public Dictionary<int, int> EmptySlotsPerColumn => emptySlotsPerColumn;
    void Start()
    {
        boardManager = GetComponent<BoardManager>();
        fallingGems = new Dictionary<int, List<Gem>>();
        emptySlotsPerColumn = new Dictionary<int, int>();
    }


    void Update()
    {

    }
    public void CollapseBoardData()
    {
        fallingGems.Clear();
        var width = boardManager.Width;
        var height = boardManager.Height;
        for (int col = 0; col < width; col++)
        {
            int posToFall = 0;
            for (int row = 0; row < height; row++)
            {
                Gem currentGem = boardManager.Board[row, col];
                if (currentGem != null)
                {
                    if (row != posToFall)
                    {
                        Debug.Log($" {col} , {row} , {posToFall}");
                        int fallingDistance = row - posToFall;
                        SetFallingGemsByDistance(fallingDistance , currentGem);
                        boardManager.MoveGemAt(col, row, posToFall);
                        Debug.Log($"After Swap -> [{row},{col}] : {boardManager.Board[row, col]}, [{posToFall},{col}] :{boardManager.Board[posToFall, col]}");
                    }

                    posToFall++;
                }
            }
            emptySlotsPerColumn[col] = height - posToFall;
        }
    }
    private void SetFallingGemsByDistance(int distance, Gem gem)
    {
        if (!fallingGems.ContainsKey(distance))
        {
            fallingGems[distance] = new List<Gem>();
        }
        fallingGems[distance].Add(gem);
    }
    public IEnumerator PlayFallingAnimation()
    {
        foreach (var pair in fallingGems.OrderBy(pair => pair.Key))
        {
            var seq = DOTween.Sequence();
            List<Gem> gems = pair.Value;
            foreach (var gem in gems)
            {
                seq.Join(gem.SwapTo(gem.BoardPosition, fallingDuration));
            }
            yield return seq.WaitForCompletion();
            yield return new WaitForSeconds(delayAfterFalling);
        }
    }
    public IEnumerator OnDestroyedGems()
    {
        CollapseBoardData();
        yield return anim.PlayFallingAnimation(fallingGems);
    }
}
