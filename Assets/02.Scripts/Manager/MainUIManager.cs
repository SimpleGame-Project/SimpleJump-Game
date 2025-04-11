using UnityEngine;
using UnityEngine.UI;
public class MainUIManager : MonoBehaviour
{
    [SerializeField] private Text goldText;

    private static MainUIManager _instance;
    public static MainUIManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(MainUIManager)) as MainUIManager;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // 인스턴스가 존재하는데 이 오브젝트가 아니라면 파괴
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void UpdateGoldUI(int gold)
    {
        goldText.text = gold.ToString();
    }
}
