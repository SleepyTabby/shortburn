using UnityEngine;

public class SnappingGrid
{
    private ImageSorting imageSorting;
    

    public SnappingGrid(ImageSorting imageSorting)
    {
        this.imageSorting = imageSorting;
    }

    /// <summary>
    /// Builds a grid for puzzle piece snapping.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="xLength"></param>
    /// <param name="zLength"></param>
    /// <returns></returns>
    public Vector3[] BuildGrid(float size, int xLength, int zLength)
    {
        Vector3 pos = imageSorting.boardTransd.position;
        pos.x -= imageSorting.transform.lossyScale.x / 2;
        pos.z -= imageSorting.transform.lossyScale.z / 2;
        
        int xCount = xLength;

        int length = xLength * zLength;
        
        Vector3[] _grid = new Vector3[length];
        float x = 0;
        float z = 0;
        for (int i = 0; i < length; i++)
        {
            _grid[i] = pos + new Vector3(x * size, 0f, z * size);
            x++;
            if (x == xCount)
            {
                x = 0;
                z++;
            }
        }

        
        return _grid;
    }
}