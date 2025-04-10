using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public enum UpgradeType { Attack, Shield, Jump }
    public UpgradeType upgradeType;
    public int cost;
    private Button myBtn;

    [Header("PopUp UI")]
    public GameObject popUp;
    public Button okBtn;

    void Awake()
    {
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(OpenPopUp);
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

            popUp.SetActive(false);
        }
    }
}
