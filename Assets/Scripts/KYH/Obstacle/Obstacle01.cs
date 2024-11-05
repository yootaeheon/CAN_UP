using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle01 : MonoBehaviour, IObjectPosition
{
    // Item의 데이터
    [SerializeField] Item _itemData;

    // 강화될 점프 수치
    [SerializeField] float _jumpForce;

    // 점프 수치가 증가했는지 체크용
    [SerializeField] private bool _isIncrease;

    // 아이템이 튕기는 수치
    [SerializeField] private float _itemJumpPower;

    // 이름 설정
    [SerializeField] string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    // 캐릭터의 점프 수치 증가 함수
    public void ForcedJump(PlayerController player)
    {
        // 이미 점프 수치가 증가했을 경우를 예외 처리
        if (_isIncrease == true)
            return;

        // 플레이어가 BaseCharacter인 경우
        if (player.gameObject.tag == "Base")
        {
            PlayerData playerData = player.gameObject.GetComponent<PlayerData>();
            playerData.MaxJumpPower += _jumpForce;           // BaseCharacter의 최대 점프치를 _jumpForce만큼 증가
            _isIncrease = true;                             // 점프 수치가 증가됐다고 체크
        }
        // 플레이어가 나머지 캐릭터 중 하나인 경우
        else
        {
            PlayerData playerData = player.gameObject.GetComponent<PlayerData>();
            playerData.JumpPower += _jumpForce;                // 플레이어의 점프력을 _jumpForce만큼 증가
            _isIncrease = true;                                 // 점프 수치가 증가됐다고 체크
        }
    }

    // 아이템이 공처럼 튕겨지는 함수
    public void BounceItem(Item item)
    {
        Rigidbody itemRigid = item.gameObject.GetComponent<Rigidbody>();        // 아이템의 Rigidbody 컴포넌트 참조 
        itemRigid.AddForce(Vector3.up * _itemJumpPower, ForceMode.Impulse);     // 아이템에게 위로 힘을 가하여 공이 튕기는 것처럼 구현
    }

    // 캐릭터의 점프 수치를 원상태로 되돌리는 함수
    public void CancelJump(PlayerController player)
    {
        // 이미 캐릭터의 점프 수치를 원상태로 되돌렸다면 예외 처리
        if (_isIncrease == false)
            return;

        // 플레이어가 BaseCharacter인 경우
        if (player.gameObject.tag == "Base")
        {
            PlayerData playerData = player.gameObject.GetComponent<PlayerData>();
            playerData.MaxJumpPower -= _jumpForce;           // BaseCharacter의 최대 점프치를 _jumpForce만큼 감소
            _isIncrease = false;                            // 점프 수치가 원상태로 되돌아왔다고 체크
        }
        // 플레이어가 나머지 캐릭터 중 하나인 경우
        else
        {
            PlayerData playerData = player.gameObject.GetComponent<PlayerData>();
            playerData.JumpPower -= _jumpForce;                // 플레이어의 최대 점프치를 _jumpForce만큼 감소
            _isIncrease = false;                                // 점프 수치가 원상태로 되돌아왔다고 체크
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rigid = collision.collider.GetComponent<Rigidbody>();
        if (rigid.velocity.y <= 0 && rigid != null)
        {
            rigid.velocity = Vector3.down;
        }
    }
}
