using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Slash : ActiveSkill
{
    [SerializeField, Tooltip("çUåÇÇÃìñÇΩÇËîªíËÅAìÆÇ´Çê›íËÇµÇ‹Ç∑ÅB")]
    private GameObject slash;
    private Transform slash_transform;

    private float slash_size = 1.0f;
    private float angle = 90.0f;

    public override void SkillContent()
    {
        Vector3 v3 = user_transform.position;
        Vector3 v3forward = user_transform.forward;
        v3forward = new Vector3(v3forward.x * slash_size * 0.5f, 0.5f, v3forward.z * slash_size * 0.5f);
        v3 += v3forward;
        GameObject _slash = Instantiate(slash, user_transform);

        slash_transform = _slash.GetComponent<Transform>();
        slash_transform.position = v3;
        slash_transform.rotation = user_transform.rotation;
        slash_transform.localScale = new Vector3(0.2f, 0.2f, slash_size);
        slash_transform.RotateAround(user_transform.position, user_transform.up, -angle * 0.5f);

        _slash.GetComponent<Slash_collider>().SetValue(angle, freeze_time, user_status.Atk, user_transform);
    }
}