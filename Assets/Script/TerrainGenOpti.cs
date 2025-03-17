using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenOpti : MonoBehaviour
{
    public int width = 16;           // Largeur du terrain
    public int depth = 16;          // Profondeur du terrain
    public int height = 8;          // Hauteur maximale des blocs
    public float scale = 10f;       // Échelle du bruit de Perlin

    public GameObject blockPrefab;  // Préfabriqué pour les blocs (un simple cube)

    private int[,] heightMap;       // Tableau pour stocker la hauteur des blocs

    public Material grassMaterial;   // Material pour l'herbe
    public Material dirtMaterial;   // Material pour la terre
    public Material rockMaterial;  // Material pour la roche

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        // Créer un tableau 2D pour stocker les hauteurs
        heightMap = new int[width, depth];

        // Générer les hauteurs avec le bruit de Perlin
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                int yHeight = Mathf.FloorToInt(PerlinNoise(x, z) * height);
                heightMap[x, z] = yHeight;
            }
        }

        // Générer uniquement les blocs de surface
        for (int x = 1; x < width - 1 ; x++) // Ignorer les bords
        {
            for (int z = 1; z < depth - 1 ; z++) // Ignorer les bords
            {
                int y = heightMap[x, z];
                Vector3 position = new Vector3(x, y, z);
                Instantiate(blockPrefab, position, Quaternion.identity, transform);

                // Attribuer le matériau en fonction de la hauteur
                Renderer renderer = blockPrefab.GetComponent<Renderer>();
                if (y < height * 0.3f)
                {
                    renderer.material = grassMaterial; // Herbe pour les basses altitudes
                }
                else if (y < height * 0.7f)
                {
                    renderer.material = dirtMaterial; // Terre pour les altitudes moyennes
                }
                else
                {
                    renderer.material = rockMaterial; // Roche pour les hautes altitudes
                }
            }
        }
    }

    float PerlinNoise(int x, int z)
    {
        // Générer un bruit de Perlin normalisé
        float xCoord = x / scale;
        float zCoord = z / scale;
        return Mathf.PerlinNoise(xCoord, zCoord);
    }
}
