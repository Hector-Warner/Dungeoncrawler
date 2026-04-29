using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class TileGenerationScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int width = 16;
    public int height = 16;
    public Tile[] tiles;
    public Tile[] walls;
    public int chunkSize = 16;
    public Grid tileMapPrefab;
    public PlayerController playerController;
    private Dictionary<Vector2, Tilemap> loadedChunks = new Dictionary<Vector2, Tilemap>();


    public class Chunk : MonoBehaviour
    {
        public Vector2Int chunkCoords;
        public Tilemap chunkTileMap;

        public void Initialisation(int width, int height, Tile[] tiles, Tile[] walls)
        {
            chunkTileMap = GetComponentInChildren<Tilemap>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == width - 1 || y == height - 1 || x == 0 || y == 0)
                    {
                        int tileNum = Random.Range(0, walls.Length);
                        chunkTileMap.SetTile(new Vector3Int(chunkCoords.x * width, chunkCoords.y * height), walls[tileNum]);
                    }
                    else
                    {
                        int tileNum = Random.Range(0, tiles.Length);

                        chunkTileMap.SetTile(new Vector3Int(chunkCoords.x * width, chunkCoords.y * height), tiles[tileNum]);
                    }
                }
            }
        }
    }
    void Start()
    {
    }

    public void createChunk(Vector2 chunkStart)
    {
        Grid newChunk = Instantiate(tileMapPrefab);
        Tilemap newTilemap = newChunk.GetComponentInChildren<Tilemap>();
        newChunk.transform.position = new Vector2(chunkStart.x*chunkSize, chunkStart.y*chunkSize);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                if (x == chunkSize - 1 || y == chunkSize - 1 || x == 0 || y == 0)
                {
                    int tileNum = Random.Range(0, walls.Length);
                    Debug.Log("x: " + x + " y: " + y);
                    newTilemap.SetTile(new Vector3Int(x, y), walls[tileNum]);
                }
                else
                {
                    int tileNum = Random.Range(0, tiles.Length);

                    newTilemap.SetTile(new Vector3Int(x, y), tiles[tileNum]);
                }
            }
        }
        loadedChunks.Add(chunkStart, newTilemap);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentChunk = new Vector2(Mathf.FloorToInt(playerController.transform.position.x / chunkSize), Mathf.FloorToInt(playerController.transform.position.y / chunkSize));
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++) { 
                Vector2 targetCoord = new Vector2(currentChunk.x + x, currentChunk.y + y);
                if (!loadedChunks.ContainsKey(targetCoord))
                {
                    createChunk(targetCoord);
                }
            } 
        }
    }
}
