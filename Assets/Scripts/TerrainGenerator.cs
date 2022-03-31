using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Sprite tileDirt;
    public Sprite tileWater;
    public int worldSize = 100;
    private float scaleDown = 1f;
    public float noiseFreq = 0.05f;
    public float seed;
    public Texture2D noiseTexture;
    public List<GameObject> worldTiles = new List<GameObject>();
    private List<GameObject> caveTiles = new List<GameObject>();
    private List<GameObject> waterTiles = new List<GameObject>();
    private float posx = 0;
    private float posy = 0;
    private int scaleC = 20;
    private int sizeOfCx = 0;
    private int sizeOfCy = 0;
    public int cavesCount = 5;

    public int caveSize = 10;
    private void Start()
    {
        seed = Random.Range(-10000, 10000);

        GenNoise();
        GenTer();
        CaveGen();
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
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                GameObject newTile = new GameObject();
                newTile.transform.parent = this.transform;
                newTile.AddComponent<SpriteRenderer>();
                newTile.GetComponent<SpriteRenderer>().sprite = tileDirt;
                newTile.name = "Dirt";
                newTile.transform.position = new Vector2(x / scaleDown - 10, y / scaleDown - 10);
                worldTiles.Add(newTile);

            }

        }
    }

    private void CaveGen()
    {
        bool corPosCave = false;
        posx = Random.Range(0, worldSize);
        posy = Random.Range(scaleC - 10, scaleC);

        for (int c = -1; c < cavesCount; c++)
        {
            posx = Random.Range(0, worldSize);
            posy = Random.Range(scaleC - 10, scaleC);
            if (caveTiles.Count == 0)
            {
                GameObject newCave = new GameObject();
                newCave.transform.parent = this.transform;
                newCave.name = "Cave";
                newCave.transform.position = new Vector2(posx, posy);
                caveTiles.Add(newCave);
            }
            else
            {
                int i = 0;
                while (!((caveTiles[c].transform.position.y - worldSize / cavesCount) < posy || (caveTiles[c].transform.position.y + worldSize / cavesCount) > posy) && (posx < worldSize - caveSize && posy < worldSize - caveSize) && i < 100)
                {
                    posx = Random.Range(0, worldSize);
                    posy = Random.Range(scaleC - 10, scaleC);
                    i++;
                }
                {
                    posx = Random.Range(0, worldSize);
                    posy = Random.Range(scaleC - 10, scaleC);
                }
                {
                    corPosCave = true;
                    posx = Random.Range(0, worldSize);
                    posy = Random.Range(scaleC - 10, scaleC);
                }
                if ((caveTiles[c].transform.position.y - worldSize / cavesCount) < posy || (caveTiles[c].transform.position.y + worldSize / cavesCount) > posy)
                {
                    corPosCave = true;
                }
            }

            if (corPosCave == true)
            {
                GameObject newCave = new GameObject();
                newCave.transform.parent = this.transform;
                newCave.name = "Cave";
                newCave.transform.position = new Vector2(posx, posy);
                caveTiles.Add(newCave);

                scaleC += worldSize / cavesCount + caveSize / 2;
                corPosCave = false;
            }
            //find any worldtile that has the same position as a cavetile and destroy it
            for (int i = 0; i < worldTiles.Count; i++)
            {
                for (int j = 0; j < caveTiles.Count; j++)
                {
                    if (worldTiles[i].transform.position == caveTiles[j].transform.position)
                    {
                        sizeOfCx = Random.Range(4, caveSize);
                        sizeOfCy = Random.Range(4, caveSize);
                        for (int x = 0; x < sizeOfCx; x++)
                        {
                            for (int y = 0; y < sizeOfCy; y++)
                            {
                                if (y > (sizeOfCy / 2))
                                {
                                    GameObject newTile = new GameObject();
                                    newTile.transform.parent = this.transform;
                                    newTile.AddComponent<SpriteRenderer>();
                                    newTile.GetComponent<SpriteRenderer>().sprite = tileWater;
                                    newTile.name = "Water";
                                    newTile.transform.position = new Vector2(worldTiles[i - (y + x * worldSize)].transform.position.x, worldTiles[i - (y + x * worldSize)].transform.position.y);
                                    waterTiles.Add(newTile);

                                    Destroy(worldTiles[i - (y + x * worldSize)]);
                                    worldTiles.Remove(worldTiles[i - (y + x * worldSize)]);
                                } else {
                                    Destroy(worldTiles[i - (y + x * worldSize)]);
                                    worldTiles.Remove(worldTiles[i - (y + x * worldSize)]);
                                }


                            }
                        }

                    }
                }
            }
        }

    }

}


