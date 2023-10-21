using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideSpace : MonoBehaviour
{
    //��ü ������ ���̿� ����
    public int totalWidth;
    public int totalHeight;
    //�ּ� ����, ����
    [SerializeField]
    private int minWidth;
    [SerializeField]
    private int minHeight;
    //��ü ���� 0,0,totalWidth,totalHeight�� ������ RectangleSpace ����
    public RectangleSpace totalSpace;
    //�������� ������ �����ϴ� ����Ʈ
    public List<RectangleSpace> spaceList;
    //�Ķ���ͷ� ���� �簢���� ������ ���� �� ���� ������ ����Ʈ�� �����ϴ� �Լ�
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
    //������ ���η� �ڸ��� �Լ�
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
    //������ ���η� ������ �Լ�
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
