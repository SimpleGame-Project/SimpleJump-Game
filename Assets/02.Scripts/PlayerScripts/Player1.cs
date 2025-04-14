using Jang;
using UnityEngine;

public class Player1 : PlayerController
{
    protected override void InitCharacter()
    {
        MaxHp = 5;
        Hp = MaxHp;
        
        _jumpForce = 15f;
    }
}
