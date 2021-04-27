using UnityEngine;
// ReSharper disable InconsistentNaming

public struct ShaderSetupTask_VisualizeScalarCube
{
    public ShaderSetupTask_VisualizeScalarCube(ShaderSetupTask_3Dto1D shaderSetupTask3Dto1D,
        Material material,
        ComputeBuffer Field,
        float InverseResolution,
        float HalfSize_m)
    {
        material.SetShaderProperty(nameof(InverseResolution), InverseResolution);
        material.SetShaderProperty(nameof(HalfSize_m), HalfSize_m);
        material.SetShaderProperty(nameof(Field), Field);
    }
}