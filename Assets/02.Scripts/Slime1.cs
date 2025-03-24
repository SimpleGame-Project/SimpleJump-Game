using Jang;
using UnityEngine;

public class Slime1 : PlayerController
{
    protected override void InitCharacter()
    {
        _hp = 5;
        _jumpForce = 5f;
    }
}
