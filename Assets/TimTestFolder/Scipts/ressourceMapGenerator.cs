using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ressourceMapGenerator : MonoBehaviour
{

    public float ForestRep = 25;
    public float RockRep = 25;
    public float MetalRep = 25;
    public float CristalRep = 25;

    public Transform mapMaterialObject;
    public Texture2D forestRockNoise;
    public MapGenerator mapGen;
    public Transform StonesParent;
    public Transform TreesParent;

    public float fallof;
    public float fallofPos;

    public float[,] noiseMap;

    public List<GameObject> Rocks;
    public List<GameObject> Trees;

    public float scaleMap = 1;

    public void SetRandomRessourceRepartition()
    {

    }

    public void GenerateOpacityNoise()
    {
        noiseMap = Noise.GenerateNoiseMap(256, 256, mapGen.seed, 50, 5, 0.6f, mapGen.lacunarity, mapGen.offset, mapGen.normalizeMode);
        forestRockNoise = new Texture2D(256, 256);
      
        for (int i = 0; i < 256; i++)
        {
            for (int j= 0; j < 256; j++)
            {
                float pixelColor = 0;
                if(noiseMap[i, j] <fallofPos)
                {
                    float enl = fallofPos - fallof;
                    pixelColor = (noiseMap[i, j]-(enl))*(100*fallof/4);
                        if(pixelColor<0)
                    {
                        pixelColor = 0;
                    }
                }
                else 
                {
                    pixelColor = 1;
                }

                forestRockNoise.SetPixel(i, j, new Color(pixelColor, pixelColor, pixelColor));
               
            }
        }

        forestRockNoise.Apply();
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        mapMaterialObject.GetComponent<MeshRenderer>().material.SetTexture("_rockForestAlpha", forestRockNoise);
    }

    public void DestroyObjects()
    {
        while (StonesParent.childCount > 0)
        {
            foreach (Transform child in StonesParent.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        while (TreesParent.childCount > 0)
        {
            foreach (Transform child in TreesParent.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    public int LayerStone;

    public void SpawnStones(int layerStone)
    {
        //Parcourir les pixels de stone possibles, puis spawn les stones
       
        List<Vector2> pixelsPossible = new List<Vector2>();
        Vector2 vec = new Vector2(0,0);
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256; j++)
            {
                if (layerStone == 0)
                {
                    if (noiseMap[i, j] < fallofPos - (fallof) && noiseMap[i, j] > fallofPos / 1.6f && mapGen.meshHeightCurve.Evaluate(mapGen.noiseMap[i, j]) * mapGen.meshHeightMultiplier > -3)
                    {
                        vec.Set(i, 255 - j);
                        pixelsPossible.Add(vec);
                    }
                }
                else
                {
                    if (noiseMap[i, j] < fallofPos  && noiseMap[i, j] > fallofPos / 2f && mapGen.meshHeightCurve.Evaluate(mapGen.noiseMap[i, j]) * mapGen.meshHeightMultiplier > -6)
                    {
                        vec.Set(i, 255 - j);
                        pixelsPossible.Add(vec);
                    }
                }
            }
        }

        Vector3 pos = new Vector3(0, 0, 0);
        int addRand = layerStone;
        while(pixelsPossible.Count >0)
        {
            int rand = mapGen.seed*addRand*1000 % pixelsPossible.Count;
            pos.Set(pixelsPossible[rand].x * scaleMap ,mapGen.meshHeightCurve.Evaluate(mapGen.noiseMap[Mathf.RoundToInt(pixelsPossible[rand].x) , 255 -Mathf.RoundToInt(pixelsPossible[rand].y)]) * mapGen.meshHeightMultiplier , pixelsPossible[rand].y*scaleMap);

            //ChooseScale
            float stoneScale = mapGen.seed * addRand % 250;
            if (layerStone == 0)
            {
                stoneScale = stoneScale / 150;
            }
            else
            {
                stoneScale = stoneScale / 10000;
            }
            if (stoneScale <0.5f)
            {
                stoneScale = 0.5f;
            }

            Collider[] hitColliders = Physics.OverlapSphere(StonesParent.position + pos, stoneScale);

            bool isAlreadyRock = false;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                
                if (hitColliders[i].transform.tag == "Item")
                {
                   //print("pouet");
                    isAlreadyRock = true;
                    
                }

            }

            if (isAlreadyRock == false)
            {


                GameObject go = Instantiate(Rocks[mapGen.seed * addRand % Rocks.Count], StonesParent);
                go.transform.Rotate(pos * mapGen.seed);
                go.transform.localScale = go.transform.localScale * stoneScale;
                go.transform.localPosition = pos;
                
                
                Physics.SyncTransforms();
            }
            pixelsPossible.Remove(pixelsPossible[rand]);
            addRand++;
        }

    }


    public void SpawnTrees()
    {
        //Parcourir les pixels de stone possibles, puis spawn les stones

        List<Vector2> pixelsPossible = new List<Vector2>();
        Vector2 vec = new Vector2(0, 0);
        for (int i = 0; i < 256; i++)
        {
            for (int j = 0; j < 256; j++)
            {
               // if (layerStone == 0)
                {
                    if (noiseMap[i, j] > fallofPos - (fallof) && mapGen.meshHeightCurve.Evaluate(mapGen.noiseMap[i, j]) * mapGen.meshHeightMultiplier > 2)
                    {
                        vec.Set(i, 255 - j);
                        pixelsPossible.Add(vec);
                    }
                }
                /*else
                {
                    if (noiseMap[i, j] < fallofPos && noiseMap[i, j] > fallofPos / 2f && mapGen.meshHeightCurve.Evaluate(mapGen.noiseMap[i, j]) * mapGen.meshHeightMultiplier > -6)
                    {
                        vec.Set(i, 255 - j);
                        pixelsPossible.Add(vec);
                    }
                }*/
            }
        }

        Vector3 rot = new Vector3(0,11, 0);
        Vector3 pos = new Vector3(0, 0, 0);
        int addRand = 0;
        while (pixelsPossible.Count > 0)
        {
            int rand = mapGen.seed * addRand * 1000 % pixelsPossible.Count;
            pos.Set(pixelsPossible[rand].x * scaleMap, mapGen.meshHeightCurve.Evaluate(mapGen.noiseMap[Mathf.RoundToInt(pixelsPossible[rand].x), 255 - Mathf.RoundToInt(pixelsPossible[rand].y)]) * mapGen.meshHeightMultiplier, pixelsPossible[rand].y * scaleMap);

            //ChooseScale
            float TreeScale = mapGen.seed * addRand % 2500;
            //if (layerStone == 0)
            {
                TreeScale = TreeScale / 2500;
            }
           /* else
            {
                stoneScale = stoneScale / 10000;
            }*/
            if (TreeScale < 0.5f)
            {
                TreeScale = 0.5f;
            }

            Collider[] hitColliders = Physics.OverlapSphere(TreesParent.position + pos , TreeScale*50);

            bool isAlreadyRock = false;
            for (int i = 0; i < hitColliders.Length; i++)
            {

                if (hitColliders[i].transform.tag == "Item")
                {
                    print("pouet");
                    isAlreadyRock = true;

                }

            }

            if (isAlreadyRock == false)
            {

                rot.Set(-90, mapGen.seed * addRand, 0);
                GameObject go = Instantiate(Trees[addRand % Trees.Count], TreesParent);
                go.transform.localEulerAngles = rot; 
                go.transform.localScale = go.transform.localScale * TreeScale;
                go.transform.localPosition = pos;


                Physics.SyncTransforms();
            }
            pixelsPossible.Remove(pixelsPossible[rand]);
            addRand++;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
