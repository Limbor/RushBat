using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    // 与player位置的偏移
    public Vector3 offset;
    // 相机移动速度
    public float speed;
    // player在一定范围内移动时相机固定
    public float xOffset, yOffset;
    // 相机边界
    public Vector2 maxPos, minPos;

    private Transform target;
    private Vector3 position;

    private void Start()
    {
        target = GameManager.GetInstance().GetPlayer().transform;
        position = target.position + offset;
        if (maxPos == minPos)
        {
            maxPos = Vector2.positiveInfinity;
            minPos = Vector2.negativeInfinity;
        }
    }


    private void LateUpdate()
    {
        if (!(Mathf.Abs((target.position + offset).x - transform.position.x) <= xOffset &&
            Mathf.Abs((target.position + offset).y - transform.position.y) <= yOffset))
        {
            position = target.position + offset;
        }
        position.x = Mathf.Clamp(position.x, minPos.x, maxPos.x);
        position.y = Mathf.Clamp(position.y, minPos.y, maxPos.y);
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }

    public void Shake()
    {
        transform.DOComplete();
        transform.DOShakePosition(0.1f, 0.3f, 14, 90, false,true);
    }
}
