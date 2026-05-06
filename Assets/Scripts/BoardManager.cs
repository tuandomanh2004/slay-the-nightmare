using System.Net.Mail;
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
    private Vector2 boardOffset ;
    void Start()
    {
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
                var gemBoardPos = new Vector2Int (x, y) ;
                var gemWorldPos = new Vector2(x*cellSpace, y*cellSpace) - boardOffset ; 
                board[y,x] = SpawnGem(currentGem , gemWorldPos , gemBoardPos) ; 
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
        gemToSpawn.transform.parent = this.transform ;
        gemToSpawn.transform.localPosition = worldPos ; 
        gemToSpawn.Init(boardPos) ;  
        return gemToSpawn ; 
    }
    void Update()
    {
        
    }
}
