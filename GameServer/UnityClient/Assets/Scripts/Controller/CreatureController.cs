using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using Google.Protobuf.Protocol;
using TMPro;

public class CreatureController : MonoBehaviour
{
    HpBar _hpBar;
    public int Id { get; set; }

    // ����Ȱ�� �����ϵ��� �ֻ��� Ŭ������ ����
    StatInfo _stat = new StatInfo();    
    public StatInfo Stat
    {
        get { return _stat; }
        set
        {   // ���� ��ȭ ������ ��ŵ
            if (_stat.Equals(value))
                return;
            _stat.Level = value.Level;
            _stat.Hp = value.Hp;
            _stat.MaxHp = value.MaxHp;
            _stat.Speed = value.Speed;
            _stat.Atk = value.Atk;
            UpdateHpBar();
        }
    }
    // ���ǵ� �ҷ����� ����...
    public float Speed
    {
        get { return _stat.Speed; }
        set { Stat.Speed = value; }
    }

    public int Hp
    {
        get { return Stat.Hp; }
        set
        {
            Stat.Hp = value;
            UpdateHpBar();
        }
    }
    // HP �ҷ����� ����...

    protected void AddHpBar()
    {
        GameObject go = Managers.Resource.Instantiate("UI/HpBar", transform); // HpBar UI
        go.transform.localPosition = new Vector3(0, 3f, 0);   // �Ӹ� ����
        go.name = "HpBar";
        _hpBar = go.GetComponent<HpBar>();
        if (_hpBar == null)
        {
            return;
        }
        UpdateHpBar();  // hp UI �� ����ȭ
    }
    void UpdateHpBar()
    {
        if (_hpBar == null)
        {
            return;
        }
        float ratio = 0.0f;
        if (Stat.MaxHp > 0)
            ratio = ((float)Stat.Hp) / Stat.MaxHp;   // ������ HP ����
        _hpBar.SetHpBar(ratio);    // hp ���� �Է�
        Debug.Log($"HP ratio : {ratio}");
    }

    protected bool _updated = false;

    PositionInfo _positionInfo = new PositionInfo();
    public PositionInfo PosInfo // PosInfo ���ٿ�
    {
        get { return _positionInfo; }
        set
        {
            if (_positionInfo.Equals(value))
                return;
            CellPos = new Vector3Int(value.PosX, value.PosY, value.PosZ);
            State = value.State;
            Dir = value.MoveDir;
        }
    }

    public Vector3Int CellPos 
    {
        get
        {   // PosInfo�� X,Y,Z ���� ���������� ����.
            return new Vector3Int(PosInfo.PosX, PosInfo.PosY, PosInfo.PosZ);
        }
        set
        {
            // ��ġ�� ������ �ʾ��� �� - ����ó��
            if (PosInfo.PosX == value.x && PosInfo.PosY == value.y && PosInfo.PosZ == value.z)
                return;

            PosInfo.PosX = value.x;
            PosInfo.PosY = value.y;
            PosInfo.PosZ = value.z;
            _updated = true;
        }
    }

    public virtual CreatureState State
    {
        get { return PosInfo.State; }
        set
        {
            if (PosInfo.State == value)
                return;

            PosInfo.State = value;
            UpdateAnimation();
            _updated = true;
        }
    }

    public MoveDir Dir
    {
        get { return PosInfo.MoveDir; }
        set
        {
            if (PosInfo.MoveDir == value)
                return;

            PosInfo.MoveDir = value;

            UpdateAnimation();
            _updated = true;
        }
    }

    public MoveDir GetDirFromVec(Vector3Int dir)
    {
        if (dir.x > 0)
            return MoveDir.Right;
        else if (dir.x < 0)
            return MoveDir.Left;
        else if (dir.z > 0)
            return MoveDir.Up;
        else
            return MoveDir.Down;

    }

    public Vector3Int GetFrontCellPos()
    {
        Vector3Int cellPos = CellPos;

        switch (Dir)
        {
            case MoveDir.Up:
                cellPos += Vector3Int.forward;
                break;
            case MoveDir.Down:
                cellPos += Vector3Int.back;
                break;
            case MoveDir.Left:
                cellPos += Vector3Int.left;
                break;
            case MoveDir.Right:
                cellPos += Vector3Int.right;
                break;
        }

        return cellPos;
    }

    protected virtual void UpdateAnimation()
    {
        if (State == CreatureState.Idle)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    //_animator.Play("IDLE_BACK");
                    //_sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    //_animator.Play("IDLE_FRONT");
                    //_sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    //_animator.Play("IDLE_RIGHT");
                    //_sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    //_animator.Play("IDLE_RIGHT");
                    //_sprite.flipX = false;
                    break;
            }
        }
        else if (State == CreatureState.Moving)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    //_animator.Play("WALK_BACK");
                    //_sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    //_animator.Play("WALK_FRONT");
                    //_sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    //_animator.Play("WALK_RIGHT");
                    //_sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    //_animator.Play("WALK_RIGHT");
                    //_sprite.flipX = false;
                    break;
            }
        }
        else if (State == CreatureState.Skill)
        {
            switch (Dir)
            {
                case MoveDir.Up:
                    //_animator.Play("ATTACK_BACK");
                    //_sprite.flipX = false;
                    break;
                case MoveDir.Down:
                    //_animator.Play("ATTACK_FRONT");
                    //_sprite.flipX = false;
                    break;
                case MoveDir.Left:
                    //_animator.Play("ATTACK_RIGHT");
                    //_sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    //_animator.Play("ATTACK_RIGHT");
                    //_sprite.flipX = false;
                    break;
            }
        }
        else
        {

        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        //_animator = GetComponent<Animator>();
        //_sprite = GetComponent<SpriteRenderer>();
        Vector3 pos = CellPos + new Vector3(0, 0, 0);
        transform.position = pos;

        // ũ������ ���� �ʱ� ���� ����
        State = CreatureState.Idle; 
        Dir = MoveDir.Down;
        UpdateAnimation();
    }

    public void SyncPos(Vector3 vec)
    {
        Vector3 destPos = vec + new Vector3(0.5f,0,0.5f);
        transform.position = destPos;
    }

    protected virtual void UpdateController()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                UpdateMoving();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle()
    {
    }

    // ������ �̵��ϴ� ���� ó��
    protected virtual void UpdateMoving()
    {
        Vector3 destPos = CellPos + new Vector3(0, 0, 0);
        Vector3 moveDir = destPos - transform.position;

        // ���� ���� üũ
        float dist = moveDir.magnitude;
        if (dist < Speed * Time.deltaTime)
        {
            transform.position = destPos;
            MoveToNextPos();
        }
        else
        {
            transform.position += moveDir.normalized * Speed * Time.deltaTime;
            State = CreatureState.Moving;
        }
    }

    protected virtual void MoveToNextPos()
    {
        
    }

    protected virtual void UpdateSkill()
    {

    }

    protected virtual void UpdateDead()
    {

    }

    public virtual void OnDamaged()
    {

    }
}