using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// 방을 만들고 방을 기준으로 복도를 만들고 벽을 만드는 클래스
public class MakeRandomMap : MonoBehaviour
{
    //방을만드는 변수들
    [SerializeField]
    private int distance;
    [SerializeField]
    private int minRoomWidth;
    [SerializeField]
    private int minRoomHeight;
    //나눠진 공간들의 리스트에 스페이스 리스트를 가져오는 변수
    [SerializeField]
    private DivideSpace divideSpace;
    //바닥파일과 벽타일 깔기
    [SerializeField]
    private SpreadTilemap spreadTilemap;
    //플레이어
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject entrance;
    
    private HashSet<Vector2Int> floor;
    private HashSet<Vector2Int> wall;
    private void Start()
    {
        StartRandomMap();
    }
    //렌덤 맵 실행 함수
    public void StartRandomMap()
    {
        //모든 타일 제거
        spreadTilemap.ClearAllTiles();
        divideSpace.totalSpace = new RectangleSpace(new Vector2Int(0, 0), divideSpace.totalWidth, divideSpace.totalHeight);
        divideSpace.spaceList = new List<RectangleSpace>();
        floor = new HashSet<Vector2Int>();
        wall = new HashSet<Vector2Int>();
        //스페이스 리스트 생성
        divideSpace.DivideRoom(divideSpace.totalSpace);
        //방, 복도, 벽 좌표 저장
        MakeRandomRooms();

        //MakeCorridors();

        MakeWall();
        //타일 깔기
        spreadTilemap.SpreadFloorTilemap(floor);
        spreadTilemap.SpreadWallTilemap(wall);

        player.transform.position = (Vector2)divideSpace.spaceList[0].Center();
        //entrance.transform.position = (Vector2)divideSpace.spaceList[divideSpace.spaceList.Count - 1].Center();


    }
    //방을 만드는 함수
    private void MakeRandomRooms()
    {
        //스페이서에서 모든 스페이스 리스트 가져오기
        foreach (var space in divideSpace.spaceList){
            HashSet<Vector2Int> positions = MakeRandomRectangleRoom(space);
            floor.UnionWith(positions);
            //플로어에 좌표 추가 UnionWith 합집합
        }
        
        
    }
    //방의 넓이 정하는 함수(취소 길이 와 최대 길이를 기준으로)
    //공간의 중심 찾기
    private HashSet<Vector2Int> MakeRandomRectangleRoom(RectangleSpace space)
    {
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();
        int width = Random.Range(minRoomWidth, space.width + 1 - distance * 2);
        int height = Random.Range(minRoomHeight, space.height + 1 - distance * 2);
        for(int i =space.Center().x - width / 2; i <= space.Center().x + width / 2; i++)
        {
            for(int j=space.Center().y - height / 2; j < space.Center().y + height / 2; j++)
            {
                positions.Add(new Vector2Int(i, j));
            }
        }
        return positions;
    }
    //복도를 만드는 함수
    private void MakeCorridors()
    {
        List<Vector2Int> tempCenters = new List<Vector2Int>();
        foreach(var space in divideSpace.spaceList)
        {
            tempCenters.Add(new Vector2Int(space.Center().x, space.Center().y));
        }
        Vector2Int nextCenter;
        Vector2Int currentCenter = tempCenters[0];
        tempCenters.Remove(currentCenter);
        while(tempCenters.Count != 0)
        {
            nextCenter = ChooseShortestNextCorridor(tempCenters, currentCenter);
            MakeOneCorridor(currentCenter, nextCenter);
            currentCenter = nextCenter;
            tempCenters.Remove(currentCenter);
        }
    }

    private Vector2Int ChooseShortestNextCorridor(List<Vector2Int> tempCenters,Vector2Int previousCenter)
    {
        int n = 0;
        float minLength = float.MaxValue;
        for(int i = 0; i < tempCenters.Count; i++)
        {
            if (Vector2.Distance(previousCenter, tempCenters[i]) < minLength){
                minLength = Vector2.Distance(previousCenter, tempCenters[i]);
                n = i;
            }
        }
        return tempCenters[n];
    }
    private void MakeOneCorridor(Vector2Int currentCenter, Vector2Int nextCenter)
    {
        Vector2Int current = new Vector2Int(currentCenter.x, currentCenter.y);
        Vector2Int next = new Vector2Int(nextCenter.x, nextCenter.y);
        floor.Add(current);
        while(current.x != next.x)
        {
            if(current.x < next.x)
            {
                current.x += 1;
                floor.Add(current);
            }
            else
            {
                current.x -= 1;
                floor.Add(current);
            }
        }
        while (current.y != next.y)
        {
            if (current.y < next.y)
            {
                current.y += 1;
                floor.Add(current);
            }
            else
            {
                current.y -= 1;
                floor.Add(current);
            }
        }
    }
    //벽을 만드는 함수
    private void MakeWall()
    {
        foreach(Vector2Int tile in floor)
        {
            HashSet<Vector2Int> boundary = Make3X3Square(tile);
            boundary.ExceptWith(floor);
            if(boundary.Count != 0)
            {
                wall.UnionWith(boundary);
            }
        }
    }
    private HashSet<Vector2Int> Make3X3Square(Vector2Int tile)
    {
        HashSet<Vector2Int> boundary = new HashSet<Vector2Int>();
        for(int i=tile.x -1; i<=tile.x + 1; i++)
        {
            for (int j = tile.y -1; j<= tile.y + 1; j++)
            {
                boundary.Add(new Vector2Int(i, j));
            }
        }
        return boundary;
    }
}
