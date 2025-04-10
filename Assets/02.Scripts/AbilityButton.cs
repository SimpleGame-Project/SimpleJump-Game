using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public enum UpgradeType { Attack, Shield, Jump }
    public UpgradeType upgradeType;
    public int cost;
    public GameObject isBought;
    private Button myBtn;

    [Header("PopUp UI")]
    public GameObject popUp;
    public Button okBtn;

    void Awake()
    {
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(OpenPopUp);
    }

    void Update()
    {
        if (GameManager.Instance != null) SetIsBought(upgradeType);
    }

    private void SetIsBought(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Attack:
                if (GameManager.Instance.isAttackUpgrade) { isBought.SetActive(true); myBtn.interactable = false; }
                else { isBought.SetActive(false); myBtn.interactable = true; }
                break;

            case UpgradeType.Shield:
                if (GameManager.Instance.isShieldUpgrade) { isBought.SetActive(true); myBtn.interactable = false; }
                else { isBought.SetActive(false); myBtn.interactable = true; }
                break;

            case UpgradeType.Jump:
                if (GameManager.Instance.isJumpUpgrade) { isBought.SetActive(true); myBtn.interactable = false; }
                else { isBought.SetActive(false); myBtn.interactable = true; }
                break;
        }

    }

    private void OpenPopUp()
    {
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
                    GameManager.Instance.isAttackUpgrade = true;
                    break;

                case UpgradeType.Shield:
                    GameManager.Instance.isShieldUpgrade = true;
                    break;

                case UpgradeType.Jump:
                    GameManager.Instance.isJumpUpgrade = true;
                    break;
            }

            isBought.SetActive(true);
            popUp.SetActive(false);
        }
    }
}
