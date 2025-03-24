using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurRendererFeature : ScriptableRendererFeature
{
    class BlurRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RTHandle tempTexture1;
        private RTHandle tempTexture2;

        public BlurRenderPass(Material material)
        {
            this.material = material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            tempTexture1 = RTHandles.Alloc(cameraTextureDescriptor.width, cameraTextureDescriptor.height, 
                                           depthBufferBits: DepthBits.None, 
                                           filterMode: FilterMode.Bilinear, 
                                           name: "_TempBlurTex1");

            tempTexture2 = RTHandles.Alloc(cameraTextureDescriptor.width, cameraTextureDescriptor.height, 
                                           depthBufferBits: DepthBits.None, 
                                           filterMode: FilterMode.Bilinear, 
                                           name: "_TempBlurTex2");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Gaussian Blur");

            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // ✅ FIXED: Use RTHandles properly
            Blitter.BlitCameraTexture(cmd, source, tempTexture1, material, 0);
            Blitter.BlitCameraTexture(cmd, tempTexture1, tempTexture2, material, 1);
            Blitter.BlitCameraTexture(cmd, tempTexture2, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (tempTexture1 != null)
            {
                RTHandles.Release(tempTexture1);
                tempTexture1 = null;
            }
            if (tempTexture2 != null)
            {
                RTHandles.Release(tempTexture2);
                tempTexture2 = null;
            }
        }
    }

    public Material blurMaterial;
    private BlurRenderPass blurPass;

    public override void Create()
    {
        blurPass = new BlurRenderPass(blurMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (blurMaterial == null)
            return;

        renderer.EnqueuePass(blurPass);
    }
}
