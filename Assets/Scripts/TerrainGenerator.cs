using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Sprite tileDirt;
    public Sprite tileGrass;
    public int worldSize = 100;
    private float scaleDown = 7.3f;
    public float noiseFreq = 0.05f;
    public float seed;
    public Texture2D noiseTexture;
    private WorldCenter = 

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        GenNoise();
        GenTer();
    }

    private void GenNoise()
    {
        noiseTexture = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise((x + seed) * noiseFreq, (y + seed) * noiseFreq);
                noiseTexture.SetPixel(x, y, new Color(v, v, v));
            }
        }

        noiseTexture.Apply();
    }

    private void GenTer()
    {
        for (int x = 0;x < worldSize; x++)
        {
            for (int y = 0;y < worldSize; y++)
            {
                if (noiseTexture.GetPixel(x, y).r < 0.7f)
                {
                    GameObject newTile = new GameObject();
                    newTile.transform.parent = this.transform;
                    newTile.AddComponent<SpriteRenderer>();
                    newTile.GetComponent<SpriteRenderer>().sprite = tileDirt;
                    newTile.transform.position = new Vector2(x/scaleDown - 10, y/scaleDown - 10);
                }
            }
        }
    }
}

