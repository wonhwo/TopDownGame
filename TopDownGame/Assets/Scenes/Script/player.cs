using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class player : MonoBehaviour
{
    public GameManager gamemanager;
    public Animator animation;
    public float Speed;
    Vector3 dirVec;
    GameObject scanObject;

    Rigidbody2D rigid;
    float h;
    float v;
    bool isHorizonMove;
    bool isJumping = false;
    bool isWalking = false;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
    }
    void Update()
    {
        h = gamemanager.isAction ?  0 :Input.GetAxisRaw("Horizontal");
        v = gamemanager.isAction ? 0 : Input.GetAxisRaw("Vertical");
        Animations();
        if (Input.GetButtonDown("Player1_Jump") && scanObject != null)
        {
            gamemanager.Action(scanObject);
        }



    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            scanObject = collision.gameObject;

            
        }
        else
        {
            scanObject = null;
        }
    }
    private void Animations()
    {

        // �ȱ� �ִϸ��̼� ����
        if (!isJumping)
        {
            isWalking = (h != 0 || v != 0);

        }

        animation.SetBool("Walking", isWalking);

        // �ȴ� ���⿡ ���� ĳ���� ������ ����
        transform.localScale = new Vector3((h < 0) ? -1 : ((h > 0) ? 1 : transform.localScale.x), 1, 1);

        // "Jump" Ű (��: 'C' Ű) ó��
        if (Input.GetKeyDown(KeyCode.C) && !isJumping)
        {
            // "Jump" �ִϸ��̼� ����
            animation.SetTrigger("Jumping");
            isJumping = true;
            isWalking = false;
            // ���⿡ ���� ���� �߰�
        }

        // "Jump" Ű�� ���� ��
        if (Input.GetKeyUp(KeyCode.C))
        {
            // "Jump" �ִϸ��̼� ���ߵ��� ����
            animation.ResetTrigger("Jumping");
            isJumping = false;
        }
    }
    private void FixedUpdate()
    {

        if (rigid != null)
        {

            Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
            rigid.velocity = new Vector2(h, v) * Speed;
        }
    }
}
