using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MakeMap : MonoBehaviour
{
    public class Node
    {
        public Node leftNode;
        public Node rightNode;
        public Node parNode;
        public RectInt nodeRect; //�и��� ������ rect����
        public Node(RectInt rect)
        {
            this.nodeRect = rect;
        }
    }

    [SerializeField] Vector2Int mapSize; //����� ���� ���� ũ��
    [SerializeField] private GameObject map; //lineRenderer�� ����ؼ� ���� ū ���� ǥ���ϱ� ����
    void Start()
    {
        DrawMap(0, 0);
    }
    private void DrawMap(int x, int y) //x y�� ȭ���� �߾���ġ�� ����
    {
        //�⺻������ mapSize/2��� ���� ����ؼ� ���� �ɰǵ�, ȭ���� �߾ӿ��� ȭ���� ũ���� ���� ����� ���� �ϴ���ǥ�� ���� �� �ֱ� �����̴�.
        LineRenderer lineRenderer = Instantiate(map).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(1, new Vector2(x + mapSize.x, y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(2, new Vector2(x + mapSize.x, y + mapSize.y) - mapSize / 2);//���� ���
        lineRenderer.SetPosition(3, new Vector2(x, y + mapSize.y) - mapSize / 2); //���� ���

    }

}