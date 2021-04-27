using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
// ReSharper disable InconsistentNaming
public struct ShdaerSetupTask_Prolongate
{
    public ShdaerSetupTask_Prolongate(ShaderSetupTask_SampleScalarField setupSampleScalarField,
        CommandBuffer cmd,
        ComputeShader shader,
        ComputeBuffer ProlongatedGrid,
        int3 prolongateSize)
    {
        int ProlongatedGridSizeX = prolongateSize.x;
        int ProlongatedGridSizeXByY = prolongateSize.x * prolongateSize.y;
        cmd.SetShaderProperty(shader, nameof(ProlongatedGrid), ProlongatedGrid);
        cmd.SetShaderProperty(shader, nameof(ProlongatedGridSizeX), ProlongatedGridSizeX);
        cmd.SetShaderProperty(shader, nameof(ProlongatedGridSizeXByY), ProlongatedGridSizeXByY);
    }
}