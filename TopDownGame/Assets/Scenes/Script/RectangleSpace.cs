using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//직사각형 공간을 정의하는 클레스
public class RectangleSpace : MonoBehaviour
{
    //직사각형 왼쪽 아래 좌표
    public Vector2Int leftDown;
    //직사각형 x값 y값
    public int width;
    public int height;

    public RectangleSpace(Vector2Int leftDown, int width, int height)
    {
        this.leftDown = leftDown;
        this.width = width;
        this.height = height;
    }
    //사각형의 중간 좌표를 나타내는 함수
    public Vector2Int Center()
    {
        return new Vector2Int(((leftDown.x *2)+ width -1)/2,((leftDown.y * 2) + height - 1) / 2);
    }
}
