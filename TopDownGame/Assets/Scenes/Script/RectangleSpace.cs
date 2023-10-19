using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���簢�� ������ �����ϴ� Ŭ����
public class RectangleSpace : MonoBehaviour
{
    //���簢�� ���� �Ʒ� ��ǥ
    public Vector2Int leftDown;
    //���簢�� x�� y��
    public int width;
    public int height;

    public RectangleSpace(Vector2Int leftDown, int width, int height)
    {
        this.leftDown = leftDown;
        this.width = width;
        this.height = height;
    }
    //�簢���� �߰� ��ǥ�� ��Ÿ���� �Լ�
    public Vector2Int Center()
    {
        return new Vector2Int(((leftDown.x *2)+ width -1)/2,((leftDown.y * 2) + height - 1) / 2);
    }
}
