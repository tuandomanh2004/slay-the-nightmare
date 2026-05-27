using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening ; 
using UnityEngine;

public class GemAnimation : MonoBehaviour
{
    [SerializeField] private float fallingDuration ; 
    [SerializeField] private float delayAfterFalling ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
