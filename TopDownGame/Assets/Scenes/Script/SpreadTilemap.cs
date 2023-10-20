using System;
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
    private TileBase[] floorTiles;
    [SerializeField]
    private TileBase wallTile;
    [SerializeField]
    private Tilemap ObjedctTiles;
    // 각 타일의 배치 확률 가중치
    [SerializeField]
    private int[] tileWeights;
    //HashSet << 집합 관련 함수
    //바닥에 타일을 까는 함수
    public void SpreadFloorTilemap(HashSet<Vector2Int> positions)
    {
        SpreadRandomFloorTile(positions);
    }
    //벽을 까는 함수
    public void SpreadWallTilemap(HashSet<Vector2Int> positions)
    {
        SpreadTile(positions, wall, wallTile);
    }//장애물 함수
    public void AddTileToObjedctTiles(Vector2Int position, TileBase tile)
    {
        ObjedctTiles.SetTile((Vector3Int)position, tile);
    }
    private void SpreadTile(HashSet<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            tilemap.SetTile((Vector3Int)position, tile);
        }
    }
    public void SpreadRandomFloorTile(HashSet<Vector2Int> positions)
    {
        foreach (Vector2Int position in positions)
        {
            TileBase selectedTile = GetRandomTileWithWeight(floorTiles, tileWeights);

            if (selectedTile != null)
            {
                floor.SetTile((Vector3Int)position, selectedTile);

                // 0번 인덱스의 타일은 floor에, 나머지는 ObjectTiles에 저장
                if (Array.IndexOf(floorTiles, selectedTile) != 0)
                {
                    AddTileToObjedctTiles((position), selectedTile);
                }
            }
        }
    }

    // 가중치를 고려한 확률적으로 타일을 선택하는 함수
    private TileBase GetRandomTileWithWeight(TileBase[] tiles, int[] weights)
    {
        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight + 1);
        int cumulativeWeight = 0;

        for (int i = 0; i < tiles.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return tiles[i];
            }
        }

        // 여기까지 오면 예외 처리 또는 기본 타일 반환 가능
        return tiles[0];
    }
    public void ClearAllTiles()
    {
        floor.ClearAllTiles();
        wall.ClearAllTiles();
    }

}