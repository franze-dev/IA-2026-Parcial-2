using UnityEngine;

public class RotationComponent : ECSComponent
{
    public Quaternion rotationQuaternion;

    public RotationComponent(Quaternion rotation)
    {
        this.rotationQuaternion = rotation;
        rotationQuaternion = Quaternion.identity;
    }
}
