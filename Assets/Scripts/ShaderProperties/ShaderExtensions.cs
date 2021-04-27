using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
// ReSharper disable InconsistentNaming

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