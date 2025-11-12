using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [Header("전환할 오브젝트들")]
    public GameObject tvOff;   // 꺼진 TV 오브젝트
    public GameObject tvOn;    // 켜진 TV 오브젝트

    [Header("플레이어 태그 이름")]
    public string playerTag = "Player";

    void Start()
    {
        // 시작 상태: 꺼진 TV 보이기
        if (tvOff != null) tvOff.SetActive(true);
        if (tvOn != null) tvOn.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // 오브젝트 전환
            if (tvOff != null) tvOff.SetActive(false);
            if (tvOn != null) tvOn.SetActive(true);

            // TV 켜짐 알리기 (TVPower 스크립트 호출)
            TVPower tvPower = tvOn.GetComponent<TVPower>();
            if (tvPower != null) tvPower.TurnOn();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // 오브젝트 원상복귀
            if (tvOff != null) tvOff.SetActive(true);
            if (tvOn != null) tvOn.SetActive(false);

            // TV 꺼짐 알리기
            TVPower tvPower = tvOn.GetComponent<TVPower>();
            if (tvPower != null) tvPower.TurnOff();
        }
    }
}
