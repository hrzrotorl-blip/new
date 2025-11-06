using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door;                 // 문 오브젝트
    public float openAngle = 90f;          // 열리는 각도
    public float openSpeed = 2f;           // 열리는 속도
    public float closeSpeed = 2f;          // 닫히는 속도

    [Header("Sound Settings")]
    public AudioSource bellSound;          // 벨소리
    public AudioSource doorSound;          // 문 여는 소리
    public AudioSource doorCloseSound;     // 문 닫는 소리 (선택)

    [Header("Delay Settings")]
    public float bellDelay = 0.5f;         // 벨소리 후 문소리까지의 지연
    public float doorOpenDelay = 1.0f;     // 문 열리기 전 대기 시간 (뚫림 방지)
    public float autoCloseDelay = 2.0f;    // 플레이어가 나간 후 자동 닫힘 시간

    private bool isTriggered = false;
    private bool isOpen = false;
    private bool isClosing = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Collider doorCollider;

    private void Start()
    {
        closedRotation = door.rotation;
        openRotation = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0));

        doorCollider = door.GetComponent<Collider>();
        if (doorCollider == null)
            Debug.LogWarning("🚪 문 오브젝트에 Collider가 없습니다. Collider를 추가하세요.");
    }

    private void Update()
    {
        if (isOpen && !isClosing)
        {
            door.rotation = Quaternion.Slerp(door.rotation, openRotation, Time.deltaTime * openSpeed);
        }
        else if (isClosing)
        {
            door.rotation = Quaternion.Slerp(door.rotation, closedRotation, Time.deltaTime * closeSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(OpenDoorSequence());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 나가면 일정 시간 뒤 자동으로 닫기
            StartCoroutine(AutoCloseSequence());
        }
    }

    private IEnumerator OpenDoorSequence()
    {
        // 1️⃣ 벨소리 재생
        if (bellSound) bellSound.Play();

        // 2️⃣ 일정 시간 대기 (벨소리 후 문 열리기 전까지 Collider 유지)
        yield return new WaitForSeconds(doorOpenDelay);

        // 3️⃣ 문소리 재생
        if (doorSound) doorSound.Play();

        // 4️⃣ Collider 비활성화 → 문 통과 가능
        if (doorCollider) doorCollider.enabled = false;

        // 5️⃣ 문 열림 애니메이션 시작
        isOpen = true;
    }

    private IEnumerator AutoCloseSequence()
    {
        // 1️⃣ 2초 대기
        yield return new WaitForSeconds(autoCloseDelay);

        // 2️⃣ 닫힘 소리 재생
        if (doorCloseSound) doorCloseSound.Play();

        // 3️⃣ 문 닫힘 시작
        isClosing = true;
        isOpen = false;

        // 4️⃣ 닫히는 동안 대기 (충분히 닫힌 뒤 Collider 복구)
        yield return new WaitForSeconds(1.0f / closeSpeed + 0.5f);

        // 5️⃣ Collider 다시 활성화 (다시 막힘)
        if (doorCollider) doorCollider.enabled = true;

        // 6️⃣ 상태 초기화
        isClosing = false;
        isTriggered = false;
    }
}
