using System.Net.Mail;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int width ; 
    [SerializeField] private int height ; 
    [SerializeField] private GameObject backgroundTile ;
    [SerializeField] private Gem[] gems ;
    [SerializeField] private Gem[,] board ;
    [SerializeField] private float cellSpace ; 
    [SerializeField] private SwapSystem swapManager ;
    [SerializeField] private Match matchManager ;
    private Vector2 boardOffset ;
    
    void OnEnable()
    {
        InputHandler.OnSwapRequested += TrySwapGem ; 
    }
    void OnDisable()
    {
        InputHandler.OnSwapRequested -= TrySwapGem ;
    }
    void Start()
    {
        swapManager = GetComponent<SwapSystem>();
        matchManager = GetComponent<Match>() ; 
        Init() ; 
    }

    void Init()
    {
        board = new Gem[height, width] ; 
        boardOffset = new Vector2 ((width -1) * cellSpace /2  ,  (height -1) *cellSpace /2) ;  
        for(int y = 0 ; y < height ; y++)
        {
            for(int x = 0 ; x < width ; x++)
            {
                var currentGem = GetRandomGem() ; 
                var gemBoardPos = new Vector2Int (y, x) ;
                var gemWorldPos = new Vector2(x*cellSpace, y*cellSpace) - boardOffset ; 
                board[y,x] = SpawnGem(currentGem , gemWorldPos , gemBoardPos) ; 
                Debug.Log($"[{y},{x}] : {board[y,x].name} {board[y,x].BoardPosition} , {board[y,x].name} {board[y,x].BoardPosition}") ;  
            }
        }
    }
    private Gem GetRandomGem()
    {
        int gemIndex = Random.Range(0 , gems.Length) ; 
        return gems[gemIndex] ; 
    }
    public Gem SpawnGem(Gem gem , Vector2 worldPos , Vector2Int boardPos)
    {
        Gem gemToSpawn = Instantiate(gem ,worldPos , Quaternion.identity) ;
        gemToSpawn.transform.parent = transform ;
        gemToSpawn.transform.localPosition = worldPos ; 
        gemToSpawn.Init(boardPos) ;  
        return gemToSpawn ; 
    }
    public void TrySwapGem(Gem gemA , Gem gemB)
    {
        if(swapManager.IsAdjacent(gemA.BoardPosition , gemB.BoardPosition))
        {
           // Debug.Log($"{gemA.name} {gemA.BoardPosition} , {gemB.name} {gemB.BoardPosition}");
            swapManager.SwapBoardData(board , gemA , gemB) ; 
          //  swapManager.UpdateGemPosition(gemA , gemB) ;
            if(matchManager.HasMatch(board,gemA.BoardPosition , gemB.BoardPosition))
            {
                Debug.Log("MATCH") ; 
            }
           // Debug.Log($"{board[0,0].name} {board[0,0].BoardPosition} , {board[0,1].name} {board[0,1].BoardPosition}") ; 
        }    
    }
    void Update()
    {
        
    }
}
