using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "WallTile", menuName = "Tiles/Wall")]
public class WallTile : TileBase
{
    public Sprite tileSprite;
    public Tile.ColliderType colliderType;
    public GameObject tilePrefab;
    
    
    public override void GetTileData(Vector3Int position, ITilemap tileMap, ref TileData tileData){
        tileData.sprite = tileSprite;
        tileData.colliderType = colliderType;
        tileData.flags = TileFlags.LockTransform;
        tileData.transform = Matrix4x4.identity;
        tileData.gameObject = tilePrefab;
    }
}
