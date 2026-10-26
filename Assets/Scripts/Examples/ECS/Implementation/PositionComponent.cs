public class PositionComponent : ECSComponent
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public PositionComponent(float X, float Y, float Z) 
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
        this.W = 0;
    }
}
