using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening ; 
using UnityEngine;

public class GemAnimation : MonoBehaviour
{
    [SerializeField] private float swapDuration ;
    [SerializeField] private float destroyDuration ; 
    [SerializeField] private float fallingDuration ; 
    [SerializeField] private float delayAfterFalling ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator PlaySwapAnimation(Gem gemA, Gem gemB)
    {
        // Create a container for managing tweens
        Sequence seq = DOTween.Sequence();
        seq.Join(gemA.SwapTo(gemA.BoardPosition, swapDuration));
        seq.Join(gemB.SwapTo(gemB.BoardPosition, swapDuration));
        yield return seq.WaitForCompletion();
    }
    public IEnumerator PlayFallingAnimation(Dictionary<int , List<Gem>> falling)
    {
        foreach(var pair in falling.OrderBy( pair => pair.Key))
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
    public IEnumerator PlayGemDestroyAnimation(HashSet<Gem> gemsToDestroy)
    {
        var seq = DOTween.Sequence() ; 
        if(gemsToDestroy != null && gemsToDestroy.Count > 0)
        {
            foreach(var gem in gemsToDestroy)
            {
               // Debug.Log(gem) ; 
                seq.Join(gem.Destroy(destroyDuration)) ; 
            }
        }
        yield return seq.WaitForCompletion();
    }
}
