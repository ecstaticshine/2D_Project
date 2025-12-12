using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{

    private float height; //멤버변수
    [SerializeField]
    private float speed = 10f;

    void Start()
    {
        //높이는 BackGround 이미지의 박스 콜라이더 2D.size.y를 가지고 와야 한다.
        height = transform.GetComponent<BoxCollider2D>().size.y * 7;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y > height)
        {
            Vector2 offset = new Vector2(0, - height * 2f);
            transform.position = (Vector2)transform.position + offset;

        }
    }
}
