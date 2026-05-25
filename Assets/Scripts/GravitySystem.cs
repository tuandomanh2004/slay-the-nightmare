using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private Dictionary<int, List<Gem>> fallingGems;
    [SerializeField] private float delayAfterFalling = 0.5f;
    [SerializeField] private float fallingDuration = 0.2f;

    void Start()
    {
        boardManager = GetComponent<BoardManager>();
        fallingGems = new Dictionary<int, List<Gem>>();
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
                        if (!fallingGems.ContainsKey(posToFall))
                        {
                            fallingGems[posToFall] = new List<Gem>();
                        }
                        fallingGems[posToFall].Add(currentGem);
                    }
                    boardManager.MoveGemAtCol(col, row, posToFall);
                    posToFall++;
                }
            }
        }
    }
    public IEnumerator PlayFallingAnimation()
    {
        int maxRow = boardManager.Height ; 
        for (int currentRow = 0 ; currentRow < maxRow ; currentRow++)
        {
            if (!fallingGems.ContainsKey(currentRow)) continue;
            var seq = DOTween.Sequence();
            List<Gem> gems = fallingGems[currentRow];
            foreach (var gem in gems)
            {
                seq.Join(gem.SwapTo(gem.BoardPosition, fallingDuration));
            }
            yield return seq.WaitForCompletion();
            yield return new WaitForSeconds(delayAfterFalling) ; 
        }
    }
}
