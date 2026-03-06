using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneBackground : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float tileWidth = 10f;

    private Transform[] tiles;
    private int leftTileIndex = 0;
    private int rightTileIndex = 1;

    void Start()
    {
        // 获取所有背景块
        tiles = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            tiles[i] = transform.GetChild(i);
        }

        leftTileIndex = 0;
        rightTileIndex = tiles.Length - 1;
    }

    void Update()
    {
        // 移动背景
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }

        // 检查是否需要重置位置
        if (tiles[leftTileIndex].position.x < -tileWidth)
        {
            // 将最左边的块移到最右边
            Vector3 newPos = tiles[leftTileIndex].position;
            newPos.x = tiles[rightTileIndex].position.x + tileWidth;
            tiles[leftTileIndex].position = newPos;

            // 更新索引
            leftTileIndex = (leftTileIndex + 1) % tiles.Length;
            rightTileIndex = (rightTileIndex + 1) % tiles.Length;
        }
    }
}