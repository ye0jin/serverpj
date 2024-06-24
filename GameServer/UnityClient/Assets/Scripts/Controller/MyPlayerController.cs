using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf.Protocol;

public class MyPlayerController : PlayerController
{
    bool _moveKeyPressed = false;

    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                GetDirInput();
                break;
            case CreatureState.Moving:
                GetDirInput();
                break;
        }

        base.UpdateController();
    }

    protected override void UpdateIdle()
    {
        // �̵� ���·� ���� Ȯ��
        if (_moveKeyPressed)
        {
            State = CreatureState.Moving;
            return;
        }

        // ��Ÿ��, Ű�Է� üũ
        if (_coSkillCooltime == null && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Punch");

            C_Skill skill = new C_Skill() { Info = new SkillInfo() };
            skill.Info.SkillId = 1;
            Managers.Network.Send(skill);   // ��ų ��� ������ ��û

            _coSkillCooltime = StartCoroutine("CoInputCooltime", 0.2f);   // ��Ÿ�� 0.2s
        }
        else if (_coSkillCooltime == null && Input.GetKey(KeyCode.R))
        {
            C_Skill skill = new C_Skill() { Info = new SkillInfo() };
            skill.Info.SkillId = 2;
            Managers.Network.Send(skill);   // ��ų ��� ������ ��û

            _coSkillCooltime = StartCoroutine("CoInputCooltime", 0.2f);   // ��Ÿ�� 0.2s
        }
    }

    Coroutine _coSkillCooltime; // ��Ÿ�� üũ �ڷ�ƾ
    IEnumerator CoInputCooltime(float time)
    {
        yield return new WaitForSeconds(time);
        _coSkillCooltime = null;
    }

    void LateUpdate()   // �� ��ġ�� ī�޶� ���󰡵���
    {
        //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    // Ű���� �Է�
    void GetDirInput()
    {
        _moveKeyPressed = true;
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            _moveKeyPressed = false;
        }
    }

    protected override void MoveToNextPos()
    {
        if (_moveKeyPressed == false)
        {
            State = CreatureState.Idle;
            CheckUpdatedFlag(); // �ƹ����� ��� �⺻������ ���� üũ
            return;
        }

        Vector3Int destPos = CellPos;

        switch (Dir)
        {
            case MoveDir.Up:
                destPos += Vector3Int.forward;
                break;
            case MoveDir.Down:
                destPos += Vector3Int.back;
                break;
            case MoveDir.Left:
                destPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                destPos += Vector3Int.right;
                break;
        }

        if (Managers.Object.Find(destPos) == null)
        {
            CellPos = destPos;
            State = CreatureState.Moving;
        }

        CheckUpdatedFlag();  // �̵� �Ŀ��� ���� üũ

    }

    protected override void CheckUpdatedFlag()
    {  
        if(_updated)
        {
            C_Move movePacket = new C_Move();
            movePacket.PosInfo = PosInfo;
            Managers.Network.Send(movePacket);
            _updated = false;   // �������� �������� �ٽ� false�� �ʱ�ȭ
        }
    }
}
