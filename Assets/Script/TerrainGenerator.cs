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

    private int[,,] terrainData;    // Tableau pour stocker les donn�es du terrain


    void Start()
    {
        GeneratorTerrain();
    }

    float PerlinNoise(int x, int z)
    {
        // G�n�rer un bruit de Perlin normalis�

        float xCoord = x / scale;
        float zCoord = z / scale;
        return Mathf.PerlinNoise(xCoord, zCoord);
    }



    bool IsBlockVisible(int x, int y, int z)
    {
        // V�rifie si le bloc est � la surface ou a un c�t� expos�

        // Si le bloc est au bord, il est toujours visible
        if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || z == 0 || z == depth - 1)
            return true;
            

        // V�rifie les blocs voisins
        if (terrainData[x - 1, y, z] == 0) return true; // Bloc � gauche
        if (terrainData[x + 1, y, z] == 0) return true; // Bloc � droite
        if (terrainData[x, y - 1, z] == 0) return true; // Bloc en dessous
        if (terrainData[x, y + 1, z] == 0) return true; // Bloc au-dessus
        if (terrainData[x, y, z - 1] == 0) return true; // Bloc devant
        if (terrainData[x, y, z + 1] == 0) return true; // Bloc derri�re

        // Si tous les voisins sont occup�s, le bloc n'est pas visible
        return false;
    }


    void GeneratorTerrain()
    {
        // Cr�er un tableau  pour stocker les informations sur le terrain

        terrainData = new int[width, height, depth];


        // G�n�rer les donn�es du terrain

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                // Calculer la hauteur � partir du bruit de Perlin

                int yHeight = Mathf.FloorToInt(PerlinNoise(x, z) * height);

                // Cr�e un bloc � la position correspondante

                for (int y = 0; y <= yHeight; y++)
                {
                    terrainData[x, y, z] = 1; // Marquer la position comme occup�e
                }
            }
        }

        // G�n�rer uniquement les blocs visibles

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