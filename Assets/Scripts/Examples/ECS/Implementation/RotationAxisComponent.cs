using System.Numerics;

public class RotationAxisComponent : ECSComponent
{
    public Vector3 axis;
    public float W;

    public RotationAxisComponent(Vector3 axis)
    {
        this.axis = axis;
        this.W = 0;
    }
}