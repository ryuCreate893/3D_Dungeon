using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Slash_collider : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;
    private Transform user_transform;

    private float angle;
    private float time = 0;
    private float max_time;
    private float atk;

    public void SetValue(float angle, float max_time, int atk, Transform user_transform)
    {
        this.angle = angle;
        this.max_time = max_time;
        this.atk = atk;
        this.user_transform = user_transform;
    }

    private void Update()
    {
        float t = Time.deltaTime;
        time = Mathf.Min(time + t, max_time);

        if (time == max_time)
        {
            t = time + t - max_time;
        }

        _transform.RotateAround(user_transform.position, user_transform.up, angle * t / max_time);

        if (time == max_time)
        {
            Destroy(gameObject);
        }
    }
}