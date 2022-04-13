using System.Collections.Generic;
using UnityEngine; 

public class TerrainGenv2 : MonoBehaviour
{
    //GENERATE NOISE AND WORLD GENERATION
    public static int worldSize = 100;
    public float seed;
    public float noiseFreq = 0.05f;
    public static float[,] Map = new float[worldSize, worldSize];
    public static int[,] watah = new int[worldSize, worldSize];
    [SerializeField]
    private List<GameObject> MapTiles = new List<GameObject>();

    //MAP RENDERING
    public Sprite tileRock;
    public Sprite tileBorder;
    public GameObject tileWater;


    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        GenNoise();
        GenBedrock();
        RenderMap();
        WatahUpdate();
    }

    private void GenNoise()
    {
        for (int x = 0; x < worldSize - 1; x++)
        {

            for (int y = 0; y < worldSize - 1; y++)
            {
                // Debug.Log(x + " " + y);
                // Debug.Log(Map.Count);
                Map[x,y] = Mathf.PerlinNoise((x + seed) * noiseFreq, (y + seed) * noiseFreq);
            }
        }
        // noiseTexture.Apply();
    }
    private void GenBedrock()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Map[x,y] = 105;
            }
            for (int y = worldSize - 3; y < worldSize; y++)
            {
                Map[x,y] = 105;
            }
        }
    }
    private async void RenderMap()
    {
        float max = 0;
        int w = 0;
        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
            if (Map[i,j] > max && Map[i,j] < 100)
            {
                max = Map[i,j];
            }
            }
        }
        for (int x = 0; x < worldSize; x++)
            {
                for (int y = 0; y < worldSize; y++)
                {
                    GameObject newTile = new GameObject();
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    if(Map[x,y] == 105)
                    {
                        newTile.GetComponent<SpriteRenderer>().sprite = tileBorder;
                        newTile.name = "Border";
                    }else if(Map[x,y] < max && Map[x,y] > max - 0.25)
                    {
                        newTile.GetComponent<SpriteRenderer>().sprite = null;
                        newTile.name = "Cave";
                        w++;
                    }
                    else
                    {
                        newTile.GetComponent<SpriteRenderer>().sprite = tileRock;
                        newTile.name = "Rock";
                    }
                    newTile.transform.position = new Vector2(x / 1 - 10, y / 1 - 10);
                    newTile.tag = "Tile";
                    BoxCollider2D bc = newTile.AddComponent<BoxCollider2D>();
                    MapTiles.Add(newTile);
                    // Debug.Log(x + y * worldSize);
                    if (w==2)
                    {
                        Debug.Log(x + y * worldSize);
                        watah[x,y] = 10;
                        w = 0;
                    }
                       
                }
            
            }
    }
    private void WatahUpdate()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                if (watah[x,y] == 10)
                {
                    Instantiate(tileWater);
                    tileWater.transform.position = new Vector2(x - 10, y - 10);
                }
            }
        }
    }

}