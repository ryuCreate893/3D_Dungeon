using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Slime_Press_Mesh : MonoBehaviour
{
    [SerializeField]
    private Transform my_transform;
    private Transform user_transform;
    private Transform target_transform;
    private float range;
    private float charge_time;


    private void Update()
    {
        if (charge_time > 0)
        {
            charge_time = Mathf.Max(charge_time - Time.deltaTime, 0);
            float distance = (target_transform.position - user_transform.position).magnitude;
            Vector3 v3 = Vector3.Lerp(user_transform.position, target_transform.position, Mathf.Min(1.0f, range / distance));

            // yç¿ïWÇÃåàíË
            Ray ray = new Ray(v3, -Vector3.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                v3.y = hit.point.y + 0.01f;
            }
            my_transform.position = v3;
        }
    }

    public void SetMesh(float charge, float range, Transform user, Transform target)
    {
        this.range = range;
        charge_time = charge;
        user_transform = user;
        target_transform = target;
        my_transform.position = target_transform.position;
    }
}