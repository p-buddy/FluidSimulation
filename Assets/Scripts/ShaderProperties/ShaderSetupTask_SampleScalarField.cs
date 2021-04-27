using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
// ReSharper disable InconsistentNaming
public struct ShaderSetupTask_SampleScalarField
{
    public ShaderSetupTask_SampleScalarField(ShaderSetupTask_3Dto1D setup3Dto1D,
        CommandBuffer cmd,
        ComputeShader shader,
        ComputeBuffer ScalarFieldToSample,
        float BoundaryScale)
    {
        cmd.SetShaderProperty(shader, nameof(ScalarFieldToSample), ScalarFieldToSample);
        cmd.SetShaderProperty(shader, nameof(BoundaryScale), BoundaryScale);
    }
}