using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
// ���� ����� ���� �������� ������ ����� ���� ����� Ŭ����
public class MakeRandomMap : MonoBehaviour
{
    //��������� ������
    [SerializeField]
    private int distance;
    [SerializeField]
    private int minRoomWidth;
    [SerializeField]
    private int minRoomHeight;
    //������ �������� ����Ʈ�� �����̽� ����Ʈ�� �������� ����
    [SerializeField]
    private DivideSpace divideSpace;
    //�ٴ����ϰ� ��Ÿ�� ���
    [SerializeField]
    private SpreadTilemap spreadTilemap;
    //�÷��̾�
    [SerializeField]
    private GameObject player;
    //[SerializeField]
    //private GameObject entrance;
    [SerializeField]
    private GameObject portalPrefab; // ��Ż �������� ������ ����
    [SerializeField]
    private GameObject outPortalPrefab; // ��Ż �������� ������ ����
    [SerializeField]
    private GameObject Portal; //�θ� ��Ż
    [SerializeField]
    private GameObject OutPortal; //�θ� ��Ż

    private HashSet<Vector2Int> floor;
    private HashSet<Vector2Int> wall;
    private void Start()
    {
        StartRandomMap();
    }
    //���� �� ���� �Լ�
    public void StartRandomMap()
    {
        //��� Ÿ�� ����
        spreadTilemap.ClearAllTiles();
        divideSpace.totalSpace = new RectangleSpace(new Vector2Int(0, 0), divideSpace.totalWidth, divideSpace.totalHeight);
        divideSpace.spaceList = new List<RectangleSpace>();
        floor = new HashSet<Vector2Int>();
        wall = new HashSet<Vector2Int>();
        //�����̽� ����Ʈ ����
        divideSpace.DivideRoom(divideSpace.totalSpace);
        //��, ����, �� ��ǥ ����
        MakeRandomRooms();

        //MakeCorridors();

        MakeWall();
        //Ÿ�� ���
        spreadTilemap.SpreadFloorTilemap(floor);
        spreadTilemap.SpreadWallTilemap(wall);

        player.transform.position = (Vector2)divideSpace.spaceList[0].Center();
        //entrance.transform.position = (Vector2)divideSpace.spaceList[divideSpace.spaceList.Count - 1].Center();


    }
    //���� ����� �Լ�
    private void MakeRandomRooms()
    {
        //�����̼����� ��� �����̽� ����Ʈ ��������
        foreach (var space in divideSpace.spaceList){
            HashSet<Vector2Int> positions = MakeRandomRectangleRoom(space);
            floor.UnionWith(positions);
            //�÷ξ ��ǥ �߰� UnionWith ������
            MakePortal(space);

        }
        

    }
    int portalNumber = 1; // ù ��° ��Ż
    private void MakePortal(RectangleSpace space)
    {
        Vector3 portalPosition = new Vector3(space.Center().x+5, space.Center().y, 0);
        Vector3 outPortalPosition = new Vector3(space.Center().x-5, space.Center().y, 0);

        GameObject portal = Instantiate(portalPrefab, portalPosition, Quaternion.identity);
        GameObject outPortal = Instantiate(outPortalPrefab, outPortalPosition, Quaternion.identity);
        portal.name = "Portal" + portalNumber;
        outPortal.name = "OutPortal" + portalNumber;
        portal.transform.SetParent(Portal.transform, true);
        outPortal.transform.SetParent(OutPortal.transform, true);
        portal.SetActive(true);
        outPortal.SetActive(true);
        portalNumber++;

    }
    public GameObject[] FindPortalObjects()
    {
        GameObject portal = GameObject.Find("Portal"); // "Portal" ������Ʈ�� ã��
        if (portal == null)
        {
            Debug.LogError("Portal ������Ʈ�� ã�� �� �����ϴ�.");
            return new GameObject[0]; // �� �迭 ��ȯ
        }

        Transform[] portalTransforms = portal.GetComponentsInChildren<Transform>();
        List<GameObject> portalObjectsList = new List<GameObject>();

        foreach (Transform portalTransform in portalTransforms)
        {
            if (portalTransform != portal.transform) // "Portal" ������Ʈ ��ü�� �ƴ� ���
            {
                portalObjectsList.Add(portalTransform.gameObject);
            }
        }
        Debug.Log(portalObjectsList);
        return portalObjectsList.ToArray(); // List�� �迭�� ��ȯ�Ͽ� ��ȯ
    }

    //���� ���� ���ϴ� �Լ�(��� ���� �� �ִ� ���̸� ��������)
    //������ �߽� ã��
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

    //���� ����� �Լ�
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
