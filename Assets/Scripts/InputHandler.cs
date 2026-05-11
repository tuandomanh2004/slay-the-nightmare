using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private int selectionCount = 0;
    [SerializeField] private Gem selectedGem;
    public static event Action<Gem, Gem> OnSwapRequested;
    void OnEnable()
    {
        Gem.OnGemClicked += SelectGem;
    }
    void OnDisable()
    {
        Gem.OnGemClicked -= SelectGem;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SelectGem(Gem clickedGem)
    {
        if (selectedGem == null)
        {
            selectedGem = clickedGem;
            Debug.Log($"Clicked {selectedGem}") ;  
        }
        else if(selectedGem == clickedGem)
        {
            selectedGem = null ; 
            Debug.Log($"Unclicked {selectedGem}") ;  
        }
        else
        {
            Debug.Log($"{selectedGem} ,{clickedGem}") ; 
            OnSwapRequested?.Invoke(selectedGem , clickedGem) ; 
            selectedGem = null ; 
        }
    }
}
