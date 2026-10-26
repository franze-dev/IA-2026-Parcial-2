using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public sealed class RotationSystem : ECSSystem
{
    private ParallelOptions parallelOptions;
    private IDictionary<uint, RotationComponent> rotationComponents;
    private IDictionary<uint, SpeedComponent> speedComponents;
    private IDictionary<uint, RotationAxisComponent> rotationAxisComponents;
    private IEnumerable<uint> queryedEntities;

    public override void Initialize()
    {
        parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 32 };
    }

    protected override void Execute(float deltaTime)
    {
        Parallel.ForEach(queryedEntities, parallelOptions, i =>
        {
            Quaternion rotation = rotationComponents[i].rotationQuaternion;
            float speed = speedComponents[i].speed;
            System.Numerics.Vector3 axis = rotationAxisComponents[i].axis;

            float DEG_TO_RAD = (float)(Math.PI / 180.0);
            float halfAngle = speed * DEG_TO_RAD * deltaTime * 0.5f;
            float sin = (float)Math.Sin(halfAngle);
            float dx = axis.X * sin;
            float dy = axis.Y * sin;
            float dz = axis.Z * sin;
            float dw = (float)Math.Cos(halfAngle);

            float qx = rotation.x;
            float qy = rotation.y;
            float qz = rotation.z;
            float qw = rotation.w;

            float nx = qw * dx + qx * dw + qy * dz - qz * dy;
            float ny = qw * dy - qx * dz + qy * dw + qz * dx;
            float nz = qw * dz + qx * dy - qy * dx + qz * dw;
            float nw = qw * dw - qx * dx - qy * dy - qz * dz;

            float length = (float)Math.Sqrt(nx * nx + ny * ny + nz * nz + nw * nw);
            rotation.x = nx / length;
            rotation.y = ny / length;
            rotation.z = nz / length;
            rotation.w = nw / length;

            rotationComponents[i].rotationQuaternion = rotation;
        });
    }

    protected override void PostExecute(float deltaTime)
    {
    }

    protected override void PreExecute(float deltaTime)
    {
        rotationComponents ??= ECSRegistry.GetComponents<RotationComponent>();
        speedComponents ??= ECSRegistry.GetComponents<SpeedComponent>();
        rotationAxisComponents ??= ECSRegistry.GetComponents<RotationAxisComponent>();
        queryedEntities ??= ECSRegistry.GetEntitiesWhitComponentTypes(typeof(RotationComponent), typeof(SpeedComponent), typeof(RotationAxisComponent));
    }
}