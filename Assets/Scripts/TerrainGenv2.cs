using System.Collections.Generic;
using UnityEngine;

public class TerrainGenv2 : MonoBehaviour
{
    //GENERATE NOISE AND WORLD GENERATION
    public int worldSize = 100;
    public float seed;
    public float noiseFreq = 0.05f;
    public List<float> Map;
    public List<GameObject> MapTiles = new List<GameObject>();

    //MAP RENDERING
    public Sprite tileRock;
    public Sprite tileBorder;



    private void Start()
    {
        Map = new List<float>(worldSize*worldSize);
        seed = Random.Range(-10000, 10000);
        GenNoise();
        GenBedrock();
        RenderMap();
    }

    private void GenNoise()
    {

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * noiseFreq, (y + seed) * noiseFreq);
                Map.Add(v);
            }
        }

        // noiseTexture.Apply();
    }
    private void GenBedrock()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = (worldSize-3); y < worldSize; y++)
            {
                Map[x + y * worldSize] = 5;
            }
        }
    }
    private void RenderMap()
    {
        float max = 0;
        for (int i = 0; i < Map.Count; i++)
        {
            if (Map[i] > max && Map[i] < 1)
            {
                max = Map[i];
            }
        }
        for (int x = 0; x < worldSize; x++)
            {
                for (int y = 0; y < worldSize; y++)
                {
                    GameObject newTile = new GameObject();
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    if(Map[x + y * worldSize] == 5)
                    {
                        newTile.GetComponent<SpriteRenderer>().sprite = tileBorder;
                        newTile.name = "Border";
                    }else if(Map[x + y * worldSize] < max && Map[x + y * worldSize] > max - 0.25)
                    {
                        newTile.GetComponent<SpriteRenderer>().sprite = null;
                        newTile.name = "Null";
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

                }

            }
    }
}