using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
// ReSharper disable InconsistentNaming
public struct ShdaerSetupTask_Restrict
{
    public ShdaerSetupTask_Restrict(ShaderSetupTask_SampleScalarField setupSampleScalarField,
        CommandBuffer cmd,
        ComputeShader shader,
        ComputeBuffer RestrictedGrid,
        int3 restrictSize)
    {
        int RestrictedGridSizeX = restrictSize.x;
        int RestrictedGridSizeXByY = restrictSize.x * restrictSize.y;

        cmd.SetShaderProperty(shader, nameof(RestrictedGrid), RestrictedGrid);
        cmd.SetShaderProperty(shader, nameof(RestrictedGridSizeX), RestrictedGridSizeX);
        cmd.SetShaderProperty(shader, nameof(RestrictedGridSizeXByY), RestrictedGridSizeXByY);
    }
}