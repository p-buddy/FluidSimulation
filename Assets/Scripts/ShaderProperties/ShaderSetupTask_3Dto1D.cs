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

public struct VisualizeCubeMaterial
{
    public VisualizeCubeMaterial(ShaderSetupTask_3Dto1D shaderSetupTask3Dto1D,
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

public static class Extensions
{
    private static Dictionary<string, int> nameToPropertyId = new Dictionary<string, int>();

    private static int GetId(string name)
    {
        if (nameToPropertyId.TryGetValue(name, out int id))
        {
            return id;
        }

        id = Shader.PropertyToID(name);
        nameToPropertyId[name] = id;
        return id;
    }
    
    public static void SetShaderProperty(this CommandBuffer cmd, ComputeShader shader, string name, int value)
    {
        cmd.SetComputeIntParam(shader, GetId(name), value);
    }
    
    public static void SetShaderProperty(this Material material, string name, int value)
    {
        material.SetInt(GetId(name), value);
    }
    
    public static void SetShaderProperty(this CommandBuffer cmd, ComputeShader shader, string name, int[] value)
    {
        cmd.SetComputeIntParams(shader, GetId(name), value);
    }
    
    public static void SetShaderProperty(this Material material, string name, Vector4 value)
    {
        material.SetVector(GetId(name), value);
    }
    
    public static void SetShaderProperty(this CommandBuffer cmd, ComputeShader shader, string name, ComputeBuffer value, int kernelIndex = 0)
    {
        cmd.SetComputeBufferParam(shader, kernelIndex, GetId(name), value);
    }
    
    public static void SetShaderProperty(this Material material, string name, ComputeBuffer value, int kernelIndex = 0)
    {
        material.SetBuffer(GetId(name), value);
    }
    
    public static void SetShaderProperty(this CommandBuffer cmd, ComputeShader shader, string name, float value)
    {
        cmd.SetComputeFloatParam(shader, GetId(name), value);
    }
    
    public static void SetShaderProperty(this Material material, string name, float value)
    {
        material.SetFloat(GetId(name), value);
    }
}