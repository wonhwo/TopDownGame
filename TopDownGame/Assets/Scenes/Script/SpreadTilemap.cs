using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//�ٴ�, ���� ��ġ�ϴ� ��ũ��Ʈ
public class SpreadTilemap : MonoBehaviour
{
    //SerializeField Ÿ�ϸ� �����̺� ������ �ν����� â���� ���� �� ���ְ� ���ִ� �Լ�
    // Ÿ�ϸ� ������Ʈ
    [SerializeField]
    private Tilemap floor;
    [SerializeField]
    private Tilemap wall;
    //����� Ÿ�� ����
    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private TileBase wallTile;
    //HashSet << ���� ���� �Լ�
    //�ٴڿ� Ÿ���� ��� �Լ�
    public void SpreadFloorTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, floor, floorTile);
    }
    //���� ��� �Լ�
    public void SpreadWallTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, wall, wallTile);
    }
    private void SpreadTile(HashSet<Vector2Int> positions, Tilemap tilemap,TileBase tile)
    {
        foreach(var position in positions)
        {
            tilemap.SetTile((Vector3Int)position, tile);
        }
    }
    public void ClearAllTiles()
    {
        floor.ClearAllTiles();
        wall.ClearAllTiles();
    }

}
