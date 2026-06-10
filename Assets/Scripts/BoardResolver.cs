using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class BoardResolver : MonoBehaviour
{
    [SerializeField] private Match match;
    [SerializeField] private SwapSystem swap;
    [SerializeField] private GravitySystem gravity;
    [SerializeField] private RefillSystem refill;
    void OnEnable()
    {
        InputHandler.OnSwapRequested += ResolveBoard;
    }
    void OnDisable()
    {
        InputHandler.OnSwapRequested -= ResolveBoard;
    }
    void Start()
    {
        match = GetComponent<Match>();
        swap = GetComponent<SwapSystem>();
        gravity = GetComponent<GravitySystem>();
        refill = GetComponent<RefillSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ResolveBoard(Gem gemA, Gem gemB)
    {
        StartCoroutine(Resolve(gemA, gemB));
    }
    IEnumerator Resolve(Gem gemA, Gem gemB)
    {
        yield return swap.SwapRoutine(gemA, gemB);
        bool matched = match.HasMatch(gemA.BoardPosition, gemB.BoardPosition);
        if (!matched)
        {
            yield return swap.SwapRoutine(gemA, gemB);
            yield break;
        }
        while (true)
        {
            yield return match.OnMatch();

            yield return gravity.OnDestroyedGems();

            yield return refill.OnRefilled();
            if(!match.HasMatchOnBoard()) break ; 
        }
    }
}
