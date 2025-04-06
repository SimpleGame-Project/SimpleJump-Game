using UnityEngine;

public abstract class Platform : MonoBehaviour
{
    public enum PlatformType {Attack, Blink, Horizon};
    public PlatformType platformType;
    public Vector3 platfromPostiion;

    public abstract void InitPlatform();
    
    public virtual void DestroyPlatfrom()
    {
        Destroy(this.gameObject);
    }

    
}

public class Platform_Attack : Platform
{
    public override void InitPlatform()
    {
        platformType = PlatformType.Attack;
        platfromPostiion = this.transform.position;
    }
}