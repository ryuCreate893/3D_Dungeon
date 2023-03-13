using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Slash : ActiveSkill
{
    [SerializeField, Tooltip("çUåÇÇÃìñÇΩÇËîªíËÅAìÆÇ´Çê›íËÇµÇ‹Ç∑ÅB")]
    private GameObject slash;
    private GameObject _slash; // Instantiateóp
    private Transform slash_transform;
    private float slash_size = 1.0f;
    private float angle = 90.0f;
    private float time = 0;
    private float max_time = 1.0f;

    public override void SkillContent()
    {
        float center = slash_size * 0.5f;
        Vector3 v3 = user_transform.forward;
        v3 = new Vector3(v3.x * center, v3.y, v3.z * center);
        _slash = Instantiate(slash, user_transform);
        slash_transform = _slash.GetComponent<Transform>();
        slash_transform.position = v3;
        slash_transform.rotation = user_transform.rotation;
        slash_transform.RotateAround(user_transform.position, user_transform.up, -angle * center);
        StartCoroutine(Swing());
    }

    private IEnumerator Swing()
    {

        while (time != max_time)
        {
            float t = Time.deltaTime;
            time = Mathf.Min(time + t, max_time);
            if (time == max_time)
            {
                yield return null;
                t = time + t - max_time;
            }
            slash_transform.RotateAround(user_transform.position, user_transform.up, t);
        }
        slash_transform = null;
        Destroy(_slash);
    }
}