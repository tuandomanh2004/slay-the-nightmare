using UnityEngine;

public class RefillSystem : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    void Start()
    {
        boardManager = GetComponent<BoardManager>();
    }
     
}
