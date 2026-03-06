using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 5f;
    public float despawnX = -10f;  // 物体消失的X坐标

    void Update()
    {
        // 向左移动
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 超出左边界时销毁
        if (transform.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }

    // 可选：当物体离开相机视野时销毁（另一种方法）
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}