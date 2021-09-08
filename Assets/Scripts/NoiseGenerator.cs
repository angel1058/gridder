using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator
{
    private int width;

    private int height;

    private int magnification;

    private int edgeSize;

    private int cornerRadius;

    private int waterFillPercent;

    private int waterFillPercentRows;

    private int smoothIterations;

     int waterPercentage ;
        int soilPercentage ;

    private Color color_land = new Color(30f / 255f, 100f / 255f, 05f / 255f);
    private Color color_sea = Color.blue;
    private Color color_soil = Color.grey;

    private Color[]
        steppedColors =
            new Color[10]
            {
                new Color(30f / 255f, 80f / 255f, 05f / 255f),
                new Color(30f / 255f, 85f / 255f, 05f / 255f),
                new Color(30f / 255f, 90f / 255f, 05f / 255f),
                new Color(30f / 255f, 95f / 255f, 05f / 255f),
                new Color(30f / 255f, 100f / 255f, 05f / 255f),
                new Color(30f / 255f, 105f / 255f, 05f / 255f),
                new Color(30f / 255f, 110f / 255f, 05f / 255f),
                new Color(30f / 255f, 115f / 255f, 05f / 255f),
                new Color(30f / 255f, 120f / 255f, 05f / 255f),
                new Color(30f / 255f, 125f / 255f, 05f / 255f)
            };

    public NoiseGenerator(
        int width_,
        int height_,
        int magnification_,
        int edgeSize_,
        int cornerRadius_,
        int waterPercent,
        int waterRows,
        int smoothIterations_,
        int waterPercentage_,
        int soilPercentage_
    )
    {
        width = width_;
        height = height_;
        magnification = magnification_;
        edgeSize = edgeSize_;
        cornerRadius = cornerRadius_;
        waterFillPercentRows = waterRows;
        waterFillPercent = waterPercent;
        smoothIterations = smoothIterations_;
        waterPercentage =waterPercentage_;
        soilPercentage = soilPercentage_;
    }

    public Color[,] GenerateNoise(int type)
    {
        switch (type)
        {
            case 1:
                return generatePerlinNoise();
            case 2:
                return generateSmoothNoise();
        }
        return null;
    }

    private bool isEdge(int x, int y)
    {
        return (
        x < edgeSize ||
        y < edgeSize ||
        x > width - 1 - edgeSize ||
        y > height - 1 - edgeSize
        );
    }

    private bool isInWaterPercent(int x, int y)
    {
        int distance = waterFillPercentRows + edgeSize;
        return (
        x < distance ||
        y < distance ||
        x > width - 1 - distance ||
        y > height - 1 - distance
        );
    }

    private bool isNearCorner(int x, int y)
    {
        bool nearLeft = x <= cornerRadius;
        bool nearBottom = y <= cornerRadius;
        bool nearRight = x >= (width - 1) - cornerRadius;
        bool nearTop = y >= (height - 1) - cornerRadius;

        bool nearBottomLeft = (nearLeft && nearBottom);
        bool nearBottomRight = (nearRight && nearBottom);
        bool nearTopRight = (nearRight && nearTop);
        bool nearTopLeft = (nearLeft && nearTop);

        return nearBottomLeft || nearBottomRight || nearTopLeft || nearTopRight;
    }

    private bool isInWaterPercentRow(int x, int y)
    {
        return !isEdge(x, y) && !isNearCorner(x, y) && isInWaterPercent(x, y);
    }

    private bool flipTheIsWaterCoin()
    {
        int chance = Random.Range(1, 101);
        return chance <= waterFillPercent;
    }

    private Color[,] Smooth(Color[,] current)
    {
        Color[,] target = new Color[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                target[x, y] = smoothCell(x, y, current);
            }
        }
        return target;
    }

    private int GetSurroundingWaterCount(int x, int y, Color[,] current)
    {
        int waterCount = 0;
        for (int neighbourX = x - 2; neighbourX <= x + 2; neighbourX++)
        {
            for (int neighbourY = y - 2; neighbourY <= y + 2; neighbourY++)
            {
            if (neighbourX == x && neighbourY == y) continue;
            if (
                neighbourX >= 0 &&
                neighbourX < width &&
                neighbourY >= 0 &&
                neighbourY < height
            )
            {
                if (current[neighbourX, neighbourY] == Color.blue) 
                    waterCount++;
            }
            else
            {
                waterCount++;
            }
            }
        }
        return waterCount;
    }

    private Color smoothCell(int x, int y, Color[,] current)
    {
        int neighbourWaterTiles = GetSurroundingWaterCount(x, y, current);
        if (neighbourWaterTiles > 4)
            return Color.blue;
        else if (neighbourWaterTiles < 4)
        {
            float noiseValue = getFloatUsingPerlin(x, y);
                return steppedColors[(int)(noiseValue / 0.2f)];
        }

        return current[x,y];
    }



Color assignLand(int x , int y)
{
        int chance = Random.Range(1, 101);
        if ( chance <= waterPercentage)
            return color_sea;
        if ( chance > waterPercentage && chance <= soilPercentage)
            return color_soil;

             float noiseValue = getFloatUsingPerlin(x, y);
                return steppedColors[(int)(noiseValue / 0.2f)];
  }

#region generateSmoothNoise
    private Color[,] generateSmoothNoise()
    {
        Color[,] noise = new Color[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //give it a value regardless
                noise[x, y] = assignLand(x,y);
                if (isInWaterPercentRow(x, y))
                {
                    if (flipTheIsWaterCoin())
                    {
                        noise[x, y] = Color.blue;
                    }
                }
                else if (isEdge(x, y) || isNearCorner(x, y))
                    noise[x, y] = Color.blue;
            }
        }

        for (int smooth = 0; smooth < smoothIterations; smooth++)
            noise = Smooth(noise);

        return noise;
    }


#endregion



#region Perlin
    private Color[,] generatePerlinNoise()
    {
        Color[,] noise = new Color[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = getFloatUsingPerlin(x, y);
                noise[x, y] = steppedColors[(int)(noiseValue / 0.2f)];
            }
        }
        return noise;
    }

    float getFloatUsingPerlin(int x, int y)
    {
        float seed = 0;
        float rawPerlin =
            Mathf
                .PerlinNoise((x + seed) / magnification,
                (y + seed) / magnification);

        float clamped = Mathf.Clamp(rawPerlin, 0.0001f, 1f);
        return clamped;
    }
#endregion

}
