using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (ressourceMapGenerator))]
public class TerrainEditor : Editor {

	public override void OnInspectorGUI() {
        ressourceMapGenerator mapGen = (ressourceMapGenerator)target;

        if (DrawDefaultInspector())
        {
        }



        if (GUILayout.Button ("Generate Ressource Map")) {
            mapGen.GenerateOpacityNoise();
		}

        if (GUILayout.Button("Spawn Rocks"))
        {
            mapGen.SpawnStones(mapGen.LayerStone);
            mapGen.SpawnStones(mapGen.LayerStone +1);
        }
        if (GUILayout.Button("Spawn Trees"))
        {
            mapGen.SpawnTrees();
        }

            if (GUILayout.Button("Destroy objects"))
        {
            mapGen.DestroyObjects();
        }


    }
}
