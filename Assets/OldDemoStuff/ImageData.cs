using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ImageData", menuName = "Game/ImageData")]
public class ImageData : ScriptableObject
{
    public Vector2Int coordinates;
    public Sprite image;
    public List<string> yarnNodes; // can contain multiple descriptions
}
