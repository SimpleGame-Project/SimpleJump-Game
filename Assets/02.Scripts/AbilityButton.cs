using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public enum UpgradeType { Attack, Shield, Jump }
    public UpgradeType upgradeType;
    [SerializeField] int cost;
    [SerializeField] GameObject isBought;
    private Button myBtn;

    [Header("PopUp UI")]
    [SerializeField] GameObject popUp;
    [SerializeField] Button okBtn;
    private int upgradeIdx => (int)upgradeType;
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

            switch (upgradeType)
            {
                case UpgradeType.Attack:
                    GameManager.Instance.isUpgrade[0] = true;
                    break;

                case UpgradeType.Shield:
                    GameManager.Instance.isUpgrade[1] = true;
                    break;

                case UpgradeType.Jump:
                    GameManager.Instance.isUpgrade[2] = true;
                    break;
            }

            isBought.SetActive(true);
            popUp.SetActive(false);

            SFXManager.Instance.PlayBuySound();
        }
        else
            SFXManager.Instance.PlayDenySound();
    }
}
