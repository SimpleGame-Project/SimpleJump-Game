using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public enum UpgradeType { Attack, Shield, Jump } // 업그레이드 타입
    public UpgradeType upgradeType;
    [SerializeField] int cost;
    [SerializeField] GameObject isBought;
    private Button myBtn;

    [Header("PopUp UI")]
    [SerializeField] GameObject popUp;
    [SerializeField] Button okBtn;
    private int upgradeIdx => (int)upgradeType; // Enum 타입 정수로 변환
    void Awake()
    {
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(OpenPopUp);
    }

    void Update()
    {
        if (GameManager.Instance != null) UpdateBuyUI();
    }

    private void UpdateBuyUI()
    {
        // 업그레이드 된 상태와 동기화
        bool isUpgrade = GameManager.Instance.isUpgrade[upgradeIdx];

        isBought.SetActive(isUpgrade);
        myBtn.interactable = !isUpgrade;
    }

    private void OpenPopUp()
    {
        SFXManager.Instance.PlayClickSound();

        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(UpgradeAbility);
        popUp.SetActive(true);
    }

    private void UpgradeAbility()
    {
        if (GameManager.Instance.Gold >= cost)
        {
            GameManager.Instance.Gold -= cost;

            GameManager.Instance.isUpgrade[upgradeIdx] = true;

            isBought.SetActive(true);
            popUp.SetActive(false);

            SFXManager.Instance.PlayBuySound();
        }
        else
            SFXManager.Instance.PlayDenySound();
    }
}
