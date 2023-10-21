using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideSpace : MonoBehaviour
{
    //전체 공간의 넓이와 높이
    public int totalWidth;
    public int totalHeight;
    //최소 넓이, 높이
    [SerializeField]
    private int minWidth;
    [SerializeField]
    private int minHeight;
    //전체 공간 0,0,totalWidth,totalHeight를 가지는 RectangleSpace 변수
    public RectangleSpace totalSpace;
    //나누어진 공간을 저장하는 리스트
    public List<RectangleSpace> spaceList;
    //파라미터로 받은 사각형을 나누지 못할 때 까지 나누고 리스트에 저장하는 함수
    public void DivideRoom(RectangleSpace space)
    {
        if (space.height >= minHeight * 2 && space.width >= minWidth * 2)
        {
            if (Random.Range(0, 2) < 1)
            {
                RectangleSpace[] spaces = DivideHorizontal(space);
                DivideRoom(spaces[0]);
                DivideRoom(spaces[1]);
            }
            else
            {
                RectangleSpace[] spaces = DivideVertical(space);
                DivideRoom(spaces[0]);
                DivideRoom(spaces[1]);
            }
        }
        else if (space.height < minHeight * 2 && space.width >= minWidth * 2)
        {
            RectangleSpace[] spaces = DivideVertical(space);
            DivideRoom(spaces[0]);
            DivideRoom(spaces[1]);
        }
        else if (space.height >= minHeight * 2 && space.width < minWidth * 2)
        {
            RectangleSpace[] spaces = DivideHorizontal(space);
            DivideRoom(spaces[0]);
            DivideRoom(spaces[1]);
        }
        else
        {
            spaceList.Add(space);
            
        }
    }
    //공간을 가로로 자르는 함수
    private RectangleSpace[] DivideHorizontal(RectangleSpace space)
    {
        int newSpace1Height = minHeight + Random.Range(0, space.height - minHeight * 2 + 1);
        RectangleSpace newSpace1 = new RectangleSpace(space.leftDown, space.width, newSpace1Height);

        int newSpace2Height = space.height - newSpace1Height;
        Vector2Int newSpace2LeftDown = new Vector2Int(space.leftDown.x, space.leftDown.y + newSpace1Height);
        RectangleSpace newSpace2 = new RectangleSpace(newSpace2LeftDown, space.width, newSpace2Height);

        RectangleSpace[] spaces = new RectangleSpace[2];
        spaces[0] = newSpace1;
        spaces[1] = newSpace2;
        return spaces;
    }
    //공간을 세로로 나누는 함수
    private RectangleSpace[] DivideVertical(RectangleSpace space)
    {
        int newSpace1Width = minWidth + Random.Range(0, space.width - minWidth * 2 + 1);
        RectangleSpace newSpace1 = new RectangleSpace(space.leftDown, newSpace1Width, space.height);

        int newSpace2Width = space.width - newSpace1Width;
        Vector2Int newSpace2LeftDown = new Vector2Int(space.leftDown.x + newSpace1Width, space.leftDown.y);
        RectangleSpace newSpace2 = new RectangleSpace(newSpace2LeftDown, newSpace2Width, space.height);

        RectangleSpace[] spaces = new RectangleSpace[2];
        spaces[0] = newSpace1;
        spaces[1] = newSpace2;
        return spaces;
    }
}
