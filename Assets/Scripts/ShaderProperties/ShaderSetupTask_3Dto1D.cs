using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
// ReSharper disable InconsistentNaming
public struct ShaderSetupTask_3Dto1D
{
    public ShaderSetupTask_3Dto1D(CommandBuffer cmd, ComputeShader shader, int3 size)
    {
        int[] GridSize = new []{size.x, size.y, size.z};
        int GridSizeXByY = size.x * size.y;
        int GridSizeXByYByZ = GridSizeXByY * size.z;
        
        cmd.SetShaderProperty(shader, nameof(GridSize), GridSize);
        cmd.SetShaderProperty(shader, nameof(GridSizeXByY), GridSizeXByY);
        cmd.SetShaderProperty(shader, nameof(GridSizeXByYByZ), GridSizeXByYByZ);
    }

    public ShaderSetupTask_3Dto1D(Material material, int3 size)
    {
        Vector4 GridSize = new Vector4(size.x, size.y, size.z);
        int GridSizeXByY = size.x * size.y;
        int GridSizeXByYByZ = GridSizeXByY * size.z;

        material.SetShaderProperty(nameof(GridSize), GridSize);
        material.SetShaderProperty(nameof(GridSizeXByY), GridSizeXByY);
        material.SetShaderProperty(nameof(GridSizeXByYByZ), GridSizeXByYByZ);
    }
}