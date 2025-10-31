using UnityEngine;

public class GameManager0 : MonoBehaviour
{
    public DropSlot[] slots;          // slot_0 ~ slot_8 드래그해서 넣기
    public GameObject resultPanel;    // 결과창 UI (처음엔 비활성화 상태로)
    private bool resultShown = false; // 결과창이 여러 번 뜨는 걸 방지

    void Start()
    {
        resultPanel.SetActive(false);
    }

    void Update()
    {
        if (!resultShown && AreAllSlotsCorrect())
        {
            resultShown = true;
            resultPanel.SetActive(true);
        }
    }

    bool AreAllSlotsCorrect()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsCorrect())
            {
                Debug.Log($"❌ 슬롯 {slot.id} 이(가) 비어있거나 틀림");
                return false;
            }
        }
        Debug.Log("✅ 모든 슬롯이 올바름");
        return true;
    }

}
