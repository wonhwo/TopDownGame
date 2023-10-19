using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//바닥, 벽을 설치하는 스크립트
public class SpreadTilemap : MonoBehaviour
{
    //SerializeField 타일맵 프라이빗 변수를 인스펙터 창에서 관리 할 수있게 해주는 함수
    // 타일맵 오브젝트
    [SerializeField]
    private Tilemap floor;
    [SerializeField]
    private Tilemap wall;
    //사용할 타일 에셋
    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private TileBase wallTile;
    //HashSet << 집합 관련 함수
    //바닥에 타일을 까는 함수
    public void SpreadFloorTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, floor, floorTile);
    }
    //벽을 까는 함수
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
