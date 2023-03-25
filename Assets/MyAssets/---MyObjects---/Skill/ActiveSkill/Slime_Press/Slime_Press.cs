using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Slime_Press : ActiveSkill
{
    [SerializeField, Tooltip("攻撃の範囲を示すメッシュを設定します。")]
    private GameObject hit_mesh;
    private GameObject _hit_mesh;

    [SerializeField, Tooltip("攻撃の当たり判定、動きを設定します。")]
    private GameObject hit_collider;
    private GameObject _hit_collider;

    [SerializeField, Tooltip("スキル発動中のどこでコライダーを設置するか")]
    [Range(0, 1.0f)]
    private float set_collider_time;

    public override bool TrySkill()
    {
        bool check = base.TrySkill();
        if (!check && user.isCharge)
        { // チャージ開始時、当たる範囲をゲーム上で反映

            _hit_mesh = Instantiate(hit_mesh, user_transform);
            _hit_mesh.GetComponent<Slime_Press_Mesh>().SetMesh(charge_time, range, user_transform, user.target_transform);
        }
        return check;
    }

    public override void SkillContent()
    {
        user_animation.SetTrigger("JumpAttack");
        Invoke("SetCollider", freeze_time * set_collider_time);
    }

    public override void ChargeCancel()
    {
        base.ChargeCancel();
        Destroy(_hit_mesh);
    }

    private void SetCollider()
    {
        _hit_collider = Instantiate(hit_collider, user_transform);
        _hit_collider.transform.position = _hit_mesh.transform.position;
        _hit_collider.GetComponent<Slime_Press_Collider>().SetValue(user_status.Atk);
        Destroy(_hit_mesh);
        Invoke("DestroyCollider", 0.1f);
    }

    private void DestroyCollider()
    {
        Destroy(_hit_collider);
    }
}