using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGenerationScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int width;
    public int height;
    public Tile[] tiles;
    private Tilemap f_tileMap;
    public Tile[] walls;
    public bool[] passable;
    void Start()
    {
        f_tileMap = GetComponentInChildren<Tilemap>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == width - 1 ||  y == height - 1 || x == 0 || y == 0)
                {
                    int tileNum = Random.Range(0, walls.Length);
                    f_tileMap.SetTile(new Vector3Int(x, y), walls[tileNum]);
                } else
                {
                    int tileNum = Random.Range(0, tiles.Length);

                    f_tileMap.SetTile(new Vector3Int(x, y), tiles[tileNum]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
