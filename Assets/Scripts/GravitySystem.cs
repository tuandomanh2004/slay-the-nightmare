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
    [SerializeField] private Dictionary<int,int> emptySlotsPerColumn ; 
    [SerializeField] private float delayAfterFalling = 0.5f;
    [SerializeField] private float fallingDuration = 0.2f;

    public Dictionary<int , int> EmptySlotsPerColumn => emptySlotsPerColumn ; 
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
                        int fallingDistance = row - posToFall;
                        if (!fallingGems.ContainsKey(fallingDistance))
                        {
                            fallingGems[fallingDistance] = new List<Gem>();
                        }
                        fallingGems[fallingDistance].Add(currentGem);
                    }
                    boardManager.MoveGemAtCol(col, row, posToFall);
                    posToFall++;
                }
            }
            emptySlotsPerColumn[col] = height - posToFall ; 
        //    Debug.Log($"col: {col} , {emptySlotsPerColumn[col]}") ; 
        }
    }
    public IEnumerator PlayFallingAnimation()
    {
        foreach (var pair in fallingGems.OrderBy(pair => pair.Key))
        {
            var seq =  DOTween.Sequence() ; 
            List<Gem> gems = pair.Value;
            foreach(var gem in gems)
            {
                seq.Join(gem.SwapTo(gem.BoardPosition, fallingDuration));
            }
            yield return seq.WaitForCompletion();
            yield return new WaitForSeconds(delayAfterFalling) ; 
        }
    }
    public IEnumerator OnDestroyedGems()
    {
        CollapseBoardData() ; 
        yield return anim.PlayFallingAnimation(fallingGems) ; 
    }
}
