using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "ImageDatabase", menuName = "Game/ImageDatabase")]
public class ImageDatabase : ScriptableObject
{
    public List<ImageData> images;

    private Dictionary<Vector2Int, ImageData> imageMap;

    public void Initialize()
    {
        imageMap = new Dictionary<Vector2Int, ImageData>();

        foreach (var imageData in images)
        {
            Vector2Int coord = ExtractCoordinatesFromName(imageData.name);
            if (!imageMap.ContainsKey(coord))
            {
                imageMap[coord] = imageData;
            }
            else
            {
                Debug.LogWarning($"Duplicate coordinate at {coord} for {imageData.name}");
            }
        }
    }

    public ImageData GetImage(Vector2Int position)
    {
        if (imageMap == null)
            Initialize();

        imageMap.TryGetValue(position, out var image);
        return image;
    }

    private Vector2Int ExtractCoordinatesFromName(string name)
    {
        // Expecting format: img_1_4
        var match = Regex.Match(name, @"img_(\-?\d+)\_(\-?\d+)");
        if (match.Success)
        {
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);
            return new Vector2Int(x, y);
        }

        Debug.LogWarning($"Invalid filename format for coordinate extraction: {name}");
        return new Vector2Int(0, 0); // fallback
    }
}
