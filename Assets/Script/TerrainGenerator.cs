using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 16;        // Largeur du terrain
    public int depth = 16;       // Profondeur du terrain
    public int height = 8;      // Hauteur maximal
    public float scale = 10f;  // Echelle du bruit de perlin

    public GameObject blockPrefab;

    private int[,,] terrainData;    // Tableau pour stocker les données du terrain


    void Start()
    {
        GeneratorTerrain();
    }

    float PerlinNoise(int x, int z)
    {
        // Générer un bruit de Perlin normalisé

        float xCoord = x / scale;
        float zCoord = z / scale;
        return Mathf.PerlinNoise(xCoord, zCoord);
    }



    bool IsBlockVisible(int x, int y, int z)
    {
        // Vérifie si le bloc est à la surface ou a un côté exposé

        // Si le bloc est au bord, il est toujours visible
        if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || z == 0 || z == depth - 1)
            return true;
            

        // Vérifie les blocs voisins
        if (terrainData[x - 1, y, z] == 0) return true; // Bloc à gauche
        if (terrainData[x + 1, y, z] == 0) return true; // Bloc à droite
        if (terrainData[x, y - 1, z] == 0) return true; // Bloc en dessous
        if (terrainData[x, y + 1, z] == 0) return true; // Bloc au-dessus
        if (terrainData[x, y, z - 1] == 0) return true; // Bloc devant
        if (terrainData[x, y, z + 1] == 0) return true; // Bloc derrière

        // Si tous les voisins sont occupés, le bloc n'est pas visible
        return false;
    }


    void GeneratorTerrain()
    {
        // Créer un tableau  pour stocker les informations sur le terrain

        terrainData = new int[width, height, depth];


        // Générer les données du terrain

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Calculer la hauteur à partir du bruit de Perlin

                int yHeight = Mathf.FloorToInt(PerlinNoise(x, z) * height);

                // Crée un bloc à la position correspondante

                for (int y = 0; y <= yHeight; y++)
                {
                    terrainData[x, y, z] = 1; // Marquer la position comme occupée
                }
            }
        }

        // Générer uniquement les blocs visibles

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (terrainData[x, y, z] == 1 && IsBlockVisible(x, y, z))
                    {
                        Vector3 position = new Vector3(x, y, z);
                        Instantiate(blockPrefab, position, Quaternion.identity, transform);
                    }
                }

            }
        }
    }
}