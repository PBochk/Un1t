public class BlueSlimeWeaponController : MeleeWeaponController
{

    protected override void Awake()
    {
        base.Awake();
        modelMB = GetComponent<BlueSlimeMeleeWeaponModelMB>();
    }
}