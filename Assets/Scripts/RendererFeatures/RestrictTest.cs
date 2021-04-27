using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = System.Random;

public class RestrictTest : ScriptableRendererFeature
{
    public ComputeShader restrictCompute;
    
    class RestrictPass : ScriptableRenderPass
    {
        string profilerTag;

        private ComputeBuffer aBuffer;
        private ComputeBuffer bBuffer;

        private ComputeShader restrictCompute;
        bool initialized = false;

        private Material material;

        private int width = 2;
        private int height = 2;
        private int depth = 2;

        private int maxDimension = 512;
        
        private int3 sampleSize;
        private int3 prolongatedSize;
        
        private float timer = 0f;
        private float interval = 1.0f;

        private bool aIsProlongated = false;
        
        // Constructor
        public RestrictPass(string profilerTag, ComputeShader restrictCompute)
        {
            this.restrictCompute = restrictCompute;
            this.profilerTag = profilerTag;
            
            aBuffer = new ComputeBuffer(maxDimension * maxDimension * maxDimension, sizeof(float), ComputeBufferType.Default);
            bBuffer = new ComputeBuffer(maxDimension * maxDimension * maxDimension, sizeof(float), ComputeBufferType.Default);

            Shader visualizeCubeShader = Shader.Find("Unlit/VisualizeCube");
            
            material = new Material(visualizeCubeShader);
            
            sampleSize = new int3(width, height, depth);
            prolongatedSize = sampleSize;
        }

        private static void InitializeBuffer(CommandBuffer cmd, ComputeBuffer buffer, int3 size)
        {
            List<float> vals = new List<float>();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        float val = ((float)x + (float)y + (float)z) / (size.x + size.y + size.z);
                        //float val = UnityEngine.Random.value;
                        vals.Add(val);
                    }
                }
            }
            cmd.SetComputeBufferData(buffer, vals);
        }

        private int3 Prolongate(CommandBuffer cmd,
            ComputeBuffer bufferToSample,
            ComputeBuffer bufferToUpdate,
            int3 startingSize)
        {
            int3 endingSize = startingSize * 2;
            ShaderSetupTask_3Dto1D setup3Dto1D = new ShaderSetupTask_3Dto1D(cmd, restrictCompute, startingSize);
            ShaderSetupTask_SampleScalarField setupSample = new ShaderSetupTask_SampleScalarField(setup3Dto1D, cmd, restrictCompute, bufferToSample, 1.0f);
            ShdaerSetupTask_Prolongate proSetup = new ShdaerSetupTask_Prolongate(setupSample, cmd, restrictCompute, bufferToUpdate, endingSize);
            
            cmd.DispatchCompute(restrictCompute, 0, startingSize.x, startingSize.y, startingSize.z);
            
            return endingSize;
        }

        private static void Visualize(CommandBuffer cmd, Material material, ComputeBuffer buffer, int3 size, Vector3 offset)
        {
            Matrix4x4 trs = Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);
            new ShaderSetupTask_VisualizeScalarCube(new ShaderSetupTask_3Dto1D(material, size),
                material, buffer, size.x, 0.5f);
            cmd.DrawMesh(MeshHelpers.Cube, trs, material);
        }

        private bool SizeCheck()
        {
            return sampleSize.x < maxDimension && sampleSize.y < maxDimension && sampleSize.z < maxDimension;
        }

        private void VizA(CommandBuffer cmd)
        {
            Visualize(cmd, material, aBuffer, prolongatedSize, Vector3.zero);
        }

        private void VizB(CommandBuffer cmd)
        {
            Visualize(cmd, material, bBuffer, prolongatedSize, Vector3.zero);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            cmd.Clear();
            
            if (!initialized)
            {
                InitializeBuffer(cmd, aBuffer, sampleSize);
                initialized = true;
                aIsProlongated = true;
            }
            
            if (aIsProlongated)
            {
                VizA(cmd);
            }
            else
            {
                VizB(cmd);
            }

            if (SizeCheck() && !EditorApplication.isPaused)
            {
                timer += Time.deltaTime;
                if (timer >= interval)
                {
                    timer = 0f;
                    sampleSize = prolongatedSize;
                    if (aIsProlongated)
                    {
                        prolongatedSize = Prolongate(cmd, aBuffer, bBuffer, sampleSize);
                    }
                    else
                    {
                        prolongatedSize = Prolongate(cmd, bBuffer, aBuffer, sampleSize);
                    }
                    aIsProlongated = !aIsProlongated;
                }
            }

            context.ExecuteCommandBuffer(cmd);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    RestrictPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new RestrictPass("Restrict Pass", restrictCompute);

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


