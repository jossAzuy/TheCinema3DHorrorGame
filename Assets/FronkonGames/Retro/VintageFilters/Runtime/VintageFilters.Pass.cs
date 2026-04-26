////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

namespace FronkonGames.Retro.VintageFilters
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary> Render Pass. </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  public sealed partial class VintageFilters
  {
    [DisallowMultipleRendererFeature]
    private sealed class RenderPass : ScriptableRenderPass
    {
      // Internal use only.
      internal Material material { get; set; }

      private VintageFiltersVolume volume;

      private Texture2D blowoutTex;
      private Texture2D overlayTex;
      private Texture2D processTex;
      private Texture2D contrastTex;
      private Texture2D lumaTex;
      private Texture2D screenTex;
      private Texture2D curvesTex;
      private Texture2D levelsTex;
      private Texture2D edgeBurnTex;
      private Texture2D gradientTex;
      private Texture2D softyLightTex;
      private Texture2D metalTex;
      private Texture2D overlayWarmTex;
      private Texture2D colorShiftTex;

      private Texture3D lutTex3D;
      private Texture2D lutTex2D;

      private int lastFilter = -1;
      private readonly bool supports3DTextures;
      
      private static class ShaderIDs
      {
        public static readonly int Intensity = Shader.PropertyToID("_Intensity");
        public static readonly int EffectTime = Shader.PropertyToID("_EffectTime");
        
        public static readonly int Filter = Shader.PropertyToID("_Filter");
        public static readonly int LevelsTex = Shader.PropertyToID("_LevelsTex");
        public static readonly int LutParams = Shader.PropertyToID("_LutParams");
        public static readonly int LutTex = Shader.PropertyToID("_LutTex");
        public static readonly int BlowoutTex = Shader.PropertyToID("_BlowoutTex");
        public static readonly int OverlayTex = Shader.PropertyToID("_OverlayTex");
        public static readonly int OverlayStrength = Shader.PropertyToID("_OverlayStrength");
        public static readonly int ProcessTex = Shader.PropertyToID("_ProcessTex");
        public static readonly int ContrastTex = Shader.PropertyToID("_ContrastTex");
        public static readonly int LumaTex = Shader.PropertyToID("_LumaTex");
        public static readonly int ScreenTex = Shader.PropertyToID("_ScreenTex");
        public static readonly int CurvesTex = Shader.PropertyToID("_CurvesTex");
        public static readonly int EdgeBurnTex = Shader.PropertyToID("_EdgeBurnTex");
        public static readonly int GradientTex = Shader.PropertyToID("_GradientTex");
        public static readonly int SoftLightTex = Shader.PropertyToID("_SoftLightTex");
        public static readonly int MetalTex = Shader.PropertyToID("_MetalTex");
        public static readonly int OverlayWarmTex = Shader.PropertyToID("_OverlayWarmTex");
        public static readonly int ColorShiftTex = Shader.PropertyToID("_ColorShiftTex");

        public static readonly int EdgeBurn = Shader.PropertyToID("_EdgeBurn");
        public static readonly int Gradient = Shader.PropertyToID("_Gradient");
        public static readonly int SoftLight = Shader.PropertyToID("_SoftLight");
        public static readonly int OverlayWarm = Shader.PropertyToID("_OverlayWarm");
        
        public static readonly int SepiaIntensity = Shader.PropertyToID("_SepiaIntensity");
        public static readonly int KodachromeEnhancement = Shader.PropertyToID("_KodachromeEnhancement");
        public static readonly int KodachromeWarmth = Shader.PropertyToID("_KodachromeWarmth");
        public static readonly int PolaroidOverexposure = Shader.PropertyToID("_PolaroidOverexposure");
        public static readonly int PolaroidSoftness = Shader.PropertyToID("_PolaroidSoftness");
        public static readonly int CrossProcessColorShift = Shader.PropertyToID("_CrossProcessColorShift");
        public static readonly int CrossProcessContrastBoost = Shader.PropertyToID("_CrossProcessContrastBoost");
        public static readonly int BleachBypassDesaturation = Shader.PropertyToID("_BleachBypassDesaturation");
        public static readonly int BleachBypassContrast = Shader.PropertyToID("_BleachBypassContrast");
        public static readonly int Vintage80sNeonIntensity = Shader.PropertyToID("_Vintage80sNeonIntensity");
        public static readonly int Vintage80sColorPop = Shader.PropertyToID("_Vintage80sColorPop");
        public static readonly int FilmGrainIntensity = Shader.PropertyToID("_FilmGrainIntensity");
        public static readonly int FilmGrainSize = Shader.PropertyToID("_FilmGrainSize");

        public static readonly int TechnicolorSaturation = Shader.PropertyToID("_TechnicolorSaturation");
        public static readonly int TechnicolorColorBalance = Shader.PropertyToID("_TechnicolorColorBalance");
        public static readonly int DaguerreotypeContrast = Shader.PropertyToID("_DaguerreotypeContrast");
        public static readonly int DaguerreotypeSilvering = Shader.PropertyToID("_DaguerreotypeSilvering");
        public static readonly int CyanotypeBlueIntensity = Shader.PropertyToID("_CyanotypeBlueIntensity");
        public static readonly int WesternWarmth = Shader.PropertyToID("_WesternWarmth");
        public static readonly int WesternDust = Shader.PropertyToID("_WesternDust");
        public static readonly int NightVisionGain = Shader.PropertyToID("_NightVisionGain");
        public static readonly int NightVisionNoise = Shader.PropertyToID("_NightVisionNoise");
        public static readonly int InfraredFoliage = Shader.PropertyToID("_InfraredFoliage");
        public static readonly int InfraredBloom = Shader.PropertyToID("_InfraredBloom");
        
        public static readonly int Brightness = Shader.PropertyToID("_Brightness");
        public static readonly int Contrast = Shader.PropertyToID("_Contrast");
        public static readonly int Gamma = Shader.PropertyToID("_Gamma");
        public static readonly int Hue = Shader.PropertyToID("_Hue");
        public static readonly int Saturation = Shader.PropertyToID("_Saturation");      
      }

      private static class Keywords
      {
        public const string ENABLE_LUT_3D = "ENABLE_LUT_3D";
        public const string ENABLE_LUT_2D = "ENABLE_LUT_2D";
      }

      /// <summary> Render pass constructor. </summary>
      public RenderPass() : base()
      {
        supports3DTextures = SystemInfo.supports3DTextures;
        profilingSampler = new ProfilingSampler(Constants.Asset.AssemblyName);
      }

      /// <summary> Destroy the render pass. </summary>
      ~RenderPass() => material = null;

      private void UpdateMaterial()
      {
        material.shaderKeywords = null;
        material.SetFloat(ShaderIDs.Intensity, volume.intensity.value);

        float time = volume.useScaledTime.value == true ? Time.time : Time.unscaledTime;
        material.SetVector(ShaderIDs.EffectTime, new Vector4(time / 20.0f, time, time * 2.0f, time * 3.0f));

        material.SetInt(ShaderIDs.Filter, (int)volume.filter.value);

        switch (volume.filter.value)
        {
          case Filters.Inkwell:
          case Filters.Lomofi:
          case Filters.LordKevin:
          case Filters.Nashville:
          case Filters.Walden:
          case Filters.XProII:
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            break;
          case Filters._70s: break;
          case Filters.Aden:
          case Filters.Crema:
          case Filters.Juno:
          case Filters.Reyes:
          case Filters.Slumber:
            if (supports3DTextures == true)
            {
              material.EnableKeyword(Keywords.ENABLE_LUT_3D);
              material.SetTexture(ShaderIDs.LutTex, lutTex3D);

              int lutSize = lutTex3D.width;
              material.SetVector(ShaderIDs.LutParams, new Vector2((lutSize - 1) / (1.0f * lutSize), 1.0f / (2.0f * lutSize)));
            }
            else
            {
              material.EnableKeyword(Keywords.ENABLE_LUT_2D);
              material.SetTexture(ShaderIDs.LutTex, lutTex2D);

              float lutSize = Mathf.Sqrt(lutTex2D.width * 1.0f);
              material.SetVector(ShaderIDs.LutParams, new Vector3(1.0f / (float)lutTex2D.width, 1.0f / (float)lutTex2D.height, lutSize - 1.0f));
            }
            break;
          case Filters.Amaro:
            material.SetFloat(ShaderIDs.OverlayStrength, volume.amaroOverlay.value);
            material.SetTexture(ShaderIDs.BlowoutTex, blowoutTex);
            material.SetTexture(ShaderIDs.OverlayTex, overlayTex);
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            break;
          case Filters.Hudson:
            material.SetFloat(ShaderIDs.OverlayStrength, volume.hudsonOverlay.value);
            material.SetTexture(ShaderIDs.BlowoutTex, blowoutTex);
            material.SetTexture(ShaderIDs.OverlayTex, overlayTex);
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            break;
          case Filters.Rise:
            material.SetFloat(ShaderIDs.OverlayStrength, volume.riseOverlay.value);
            material.SetTexture(ShaderIDs.BlowoutTex, blowoutTex);
            material.SetTexture(ShaderIDs.OverlayTex, overlayTex);
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            break;
          case Filters.Sierra:
            material.SetFloat(ShaderIDs.OverlayStrength, volume.sierraOverlay.value);
            material.SetTexture(ShaderIDs.BlowoutTex, blowoutTex);
            material.SetTexture(ShaderIDs.OverlayTex, overlayTex);
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            break;
          case Filters.Brannan:
            material.SetTexture(ShaderIDs.ProcessTex, processTex);
            material.SetTexture(ShaderIDs.BlowoutTex, blowoutTex);
            material.SetTexture(ShaderIDs.ContrastTex, contrastTex);
            material.SetTexture(ShaderIDs.LumaTex, lumaTex);
            material.SetTexture(ShaderIDs.ScreenTex, screenTex);
            break;
          case Filters.Earlybird:
            material.SetTexture(ShaderIDs.CurvesTex, curvesTex);
            material.SetTexture(ShaderIDs.OverlayTex, overlayTex);
            material.SetTexture(ShaderIDs.BlowoutTex, blowoutTex);
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            break;
          case Filters.Hefe:
            material.SetFloat(ShaderIDs.EdgeBurn, volume.hefeEdgeBurn.value);
            material.SetFloat(ShaderIDs.Gradient, volume.hefeGradient.value);
            material.SetFloat(ShaderIDs.SoftLight, volume.hefeSoftLight.value);
            material.SetTexture(ShaderIDs.EdgeBurnTex, edgeBurnTex);
            material.SetTexture(ShaderIDs.GradientTex, gradientTex);
            material.SetTexture(ShaderIDs.SoftLightTex, softyLightTex);
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            break;
          case Filters.Sutro:
            material.SetTexture(ShaderIDs.CurvesTex, curvesTex);
            material.SetTexture(ShaderIDs.EdgeBurnTex, edgeBurnTex);
            break;
          case Filters.Toaster:
            material.SetFloat(ShaderIDs.OverlayWarm, volume.toasterOverlayWarm.value);
            material.SetTexture(ShaderIDs.MetalTex, metalTex);
            material.SetTexture(ShaderIDs.SoftLightTex, softyLightTex);
            material.SetTexture(ShaderIDs.CurvesTex, curvesTex);
            material.SetTexture(ShaderIDs.OverlayWarmTex, overlayWarmTex);
            material.SetTexture(ShaderIDs.ColorShiftTex, colorShiftTex);
            break;
          case Filters.Valencia:
            material.SetTexture(ShaderIDs.LevelsTex, levelsTex);
            material.SetTexture(ShaderIDs.GradientTex, gradientTex);
            break;
          case Filters.Sepia:
            material.SetFloat(ShaderIDs.SepiaIntensity, volume.sepiaIntensity.value);
            break;
          case Filters.Kodachrome:
            material.SetFloat(ShaderIDs.KodachromeEnhancement, volume.kodachromeEnhancement.value);
            material.SetFloat(ShaderIDs.KodachromeWarmth, volume.kodachromeWarmth.value);
            break;
          case Filters.Polaroid:
            material.SetFloat(ShaderIDs.PolaroidOverexposure, volume.polaroidOverexposure.value);
            material.SetFloat(ShaderIDs.PolaroidSoftness, volume.polaroidSoftness.value);
            break;
          case Filters.CrossProcess:
            material.SetFloat(ShaderIDs.CrossProcessColorShift, volume.crossProcessColorShift.value);
            material.SetFloat(ShaderIDs.CrossProcessContrastBoost, volume.crossProcessContrastBoost.value);
            break;
          case Filters.BleachBypass:
            material.SetFloat(ShaderIDs.BleachBypassDesaturation, volume.bleachBypassDesaturation.value);
            material.SetFloat(ShaderIDs.BleachBypassContrast, volume.bleachBypassContrast.value);
            break;
          case Filters.Vintage80s:
            material.SetFloat(ShaderIDs.Vintage80sNeonIntensity, volume.vintage80sNeonIntensity.value);
            material.SetFloat(ShaderIDs.Vintage80sColorPop, volume.vintage80sColorPop.value);
            break;
          case Filters.FilmGrain:
            material.SetFloat(ShaderIDs.FilmGrainIntensity, volume.filmGrainIntensity.value);
            material.SetFloat(ShaderIDs.FilmGrainSize, volume.filmGrainSize.value);
            break;
          case Filters.Technicolor:
            material.SetFloat(ShaderIDs.TechnicolorSaturation, volume.technicolorSaturation.value);
            material.SetFloat(ShaderIDs.TechnicolorColorBalance, volume.technicolorColorBalance.value);
            break;
          case Filters.Daguerreotype:
            material.SetFloat(ShaderIDs.DaguerreotypeContrast, volume.daguerreotypeContrast.value);
            material.SetFloat(ShaderIDs.DaguerreotypeSilvering, volume.daguerreotypeSilvering.value);
            break;
          case Filters.Cyanotype:
            material.SetFloat(ShaderIDs.CyanotypeBlueIntensity, volume.cyanotypeBlueIntensity.value);
            break;
          case Filters.Western:
            material.SetFloat(ShaderIDs.WesternWarmth, volume.westernWarmth.value);
            material.SetFloat(ShaderIDs.WesternDust, volume.westernDust.value);
            break;
          case Filters.NightVision:
            material.SetFloat(ShaderIDs.NightVisionGain, volume.nightVisionGain.value);
            material.SetFloat(ShaderIDs.NightVisionNoise, volume.nightVisionNoise.value);
            break;
          case Filters.Infrared:
            material.SetFloat(ShaderIDs.InfraredFoliage, volume.infraredFoliage.value);
            material.SetFloat(ShaderIDs.InfraredBloom, volume.infraredBloom.value);
            break;
        }

        material.SetFloat(ShaderIDs.Brightness, volume.brightness.value);
        material.SetFloat(ShaderIDs.Contrast, volume.contrast.value);
        material.SetFloat(ShaderIDs.Gamma, 1.0f / volume.gamma.value);
        material.SetFloat(ShaderIDs.Hue, volume.hue.value);
        material.SetFloat(ShaderIDs.Saturation, volume.saturation.value);
      }

      /// <inheritdoc/>
      public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
      {
        volume = VolumeManager.instance.stack.GetComponent<VintageFiltersVolume>();
        if (material == null || volume == null || volume.IsActive() == false)
          return;

        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        if (resourceData.isActiveTargetBackBuffer == true)
          return;

        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        if (cameraData.camera.cameraType == CameraType.SceneView && volume.affectSceneView.value == false || cameraData.postProcessEnabled == false)
          return;

        TextureHandle source = resourceData.activeColorTexture;
        TextureHandle destination = renderGraph.CreateTexture(source.GetDescriptor(renderGraph));

        if (lastFilter != (int)volume.filter.value)
          ManageResources();

        UpdateMaterial();

        RenderGraphUtils.BlitMaterialParameters pass = new(source, destination, material, 0);
        renderGraph.AddBlitPass(pass, $"{Constants.Asset.AssemblyName}.Pass");

        resourceData.cameraColor = destination;
      }

      private void ManageResources()
      {
        switch ((Filters)lastFilter)
        {
          case Filters._70s: break;
          case Filters.Inkwell:
          case Filters.Lomofi:
          case Filters.LordKevin:
          case Filters.Nashville:
          case Filters.Walden:
          case Filters.XProII:
            levelsTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Aden:
          case Filters.Crema:
          case Filters.Juno:
          case Filters.Reyes:
          case Filters.Slumber:
            lutTex3D = null;
            lutTex2D = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Amaro:
            blowoutTex = null;
            overlayTex = null;
            levelsTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Brannan:
            processTex = null;
            blowoutTex = null;
            contrastTex = null;
            lumaTex = null;
            screenTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Earlybird:
            curvesTex = null;
            overlayTex = null;
            blowoutTex = null;
            levelsTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Hefe:
            edgeBurnTex = null;
            levelsTex = null;
            gradientTex = null;
            softyLightTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Hudson:
          case Filters.Rise:
          case Filters.Sierra:
            blowoutTex = null;
            overlayTex = null;
            levelsTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Sutro:
            curvesTex = null;
            edgeBurnTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Toaster:
            metalTex = null;
            softyLightTex = null;
            curvesTex = null;
            overlayWarmTex = null;
            colorShiftTex = null;
            Resources.UnloadUnusedAssets();
            break;
          case Filters.Valencia:
            levelsTex = null;
            gradientTex = null;
            Resources.UnloadUnusedAssets();
            break;
        }

        switch (volume.filter.value)
        {
          case Filters._70s: break;
          case Filters.Aden:
            if (SystemInfo.supports3DTextures == true)
              lutTex3D = CreateTexture3DFromResources("Textures/adenLut", 33);
            else
              lutTex2D = LoadTexture<Texture2D>("Textures/adenLut");
            break;
          case Filters.Crema:
            if (SystemInfo.supports3DTextures == true)
              lutTex3D = CreateTexture3DFromResources("Textures/cremaLut", 33);
            else
              lutTex2D = LoadTexture<Texture2D>("Textures/cremaLut");
            break;
          case Filters.Amaro:
            levelsTex = LoadTexture<Texture2D>("Textures/amaroMap");
            blowoutTex = LoadTexture<Texture2D>("Textures/blackboard1024");
            overlayTex = LoadTexture<Texture2D>("Textures/overlayMap");
            break;
          case Filters.Brannan:
            processTex = LoadTexture<Texture2D>("Textures/brannanProcess");
            blowoutTex = LoadTexture<Texture2D>("Textures/brannanBlowout");
            contrastTex = LoadTexture<Texture2D>("Textures/brannanContrast");
            lumaTex = LoadTexture<Texture2D>("Textures/brannanLuma");
            screenTex = LoadTexture<Texture2D>("Textures/brannanScreen");
            break;
          case Filters.Earlybird:
            curvesTex = LoadTexture<Texture2D>("Textures/earlyBirdCurves");
            overlayTex = LoadTexture<Texture2D>("Textures/earlybirdOverlayMap");
            blowoutTex = LoadTexture<Texture2D>("Textures/earlybirdBlowout");
            levelsTex = LoadTexture<Texture2D>("Textures/earlybirdMap");
            break;
          case Filters.Hefe:
            edgeBurnTex = LoadTexture<Texture2D>("Textures/edgeBurn");
            levelsTex = LoadTexture<Texture2D>("Textures/hefeMap");
            gradientTex = LoadTexture<Texture2D>("Textures/hefeGradientMap");
            softyLightTex = LoadTexture<Texture2D>("Textures/hefeSoftLight");
            break;
          case Filters.Hudson:
            blowoutTex = LoadTexture<Texture2D>("Textures/hudsonBackground");
            overlayTex = LoadTexture<Texture2D>("Textures/overlayMap");
            levelsTex = LoadTexture<Texture2D>("Textures/hudsonMap");
            break;
          case Filters.Inkwell:
            levelsTex = LoadTexture<Texture2D>("Textures/inkwellMap");
            break;
          case Filters.Juno:
            if (SystemInfo.supports3DTextures == true)
              lutTex3D = CreateTexture3DFromResources("Textures/junoLut", 33);
            else
              lutTex2D = LoadTexture<Texture2D>("Textures/junoLut");
            break;
          case Filters.Lomofi:
            levelsTex = LoadTexture<Texture2D>("Textures/lomoMap");
            break;
          case Filters.LordKevin:
            levelsTex = LoadTexture<Texture2D>("Textures/kelvinMap");
            break;
          case Filters.Nashville:
            levelsTex = LoadTexture<Texture2D>("Textures/nashvilleMap");
            break;
          case Filters.Reyes:
            if (SystemInfo.supports3DTextures == true)
              lutTex3D = CreateTexture3DFromResources("Textures/reyesLut", 33);
            else
              lutTex2D = LoadTexture<Texture2D>("Textures/reyesLut");
            break;
          case Filters.Rise:
            blowoutTex = LoadTexture<Texture2D>("Textures/blackboard1024");
            overlayTex = LoadTexture<Texture2D>("Textures/overlayMap");
            levelsTex = LoadTexture<Texture2D>("Textures/riseMap");
            break;
          case Filters.Sierra:
            blowoutTex = LoadTexture<Texture2D>("Textures/blackboard1024");
            overlayTex = LoadTexture<Texture2D>("Textures/overlayMap");
            levelsTex = LoadTexture<Texture2D>("Textures/sierraMap");
            break;
          case Filters.Slumber:
            if (SystemInfo.supports3DTextures == true)
              lutTex3D = CreateTexture3DFromResources("Textures/slumberLut", 33);
            else
              lutTex2D = LoadTexture<Texture2D>("Textures/slumberLut");
            break;
          case Filters.Sutro:
            curvesTex = LoadTexture<Texture2D>("Textures/sutroCurves");
            edgeBurnTex = LoadTexture<Texture2D>("Textures/sutroEdgeBurn");
            break;
          case Filters.Toaster:
            metalTex = LoadTexture<Texture2D>("Textures/toasterMetal");
            softyLightTex = LoadTexture<Texture2D>("Textures/toasterSoftLight");
            curvesTex = LoadTexture<Texture2D>("Textures/toasterCurves");
            overlayWarmTex = LoadTexture<Texture2D>("Textures/toasterOverlayMapWarm");
            colorShiftTex = LoadTexture<Texture2D>("Textures/toasterColorShift");
            break;
          case Filters.Valencia:
            levelsTex = LoadTexture<Texture2D>("Textures/valenciaMap");
            gradientTex = LoadTexture<Texture2D>("Textures/valenciaGradientMap");
            break;
          case Filters.Walden:
            levelsTex = LoadTexture<Texture2D>("Textures/waldenMap");
            break;
          case Filters.XProII:
            levelsTex = LoadTexture<Texture2D>("Textures/xproMap");
            break;
        }

        lastFilter = (int)volume.filter.value;
      }
      
      private Texture3D CreateTexture3DFromResources(string texturePathFromResources, int slices)
      {
        Texture3D texture3D = null;

        Texture2D texture2D = Resources.Load<Texture2D>(texturePathFromResources);
        if (texture2D != null)
        {
          int height = texture2D.height;
          int width = texture2D.width / slices;

          Color[] pixels2D = texture2D.GetPixels();
          Color[] pixels3D = new Color[pixels2D.Length];

          for (int z = 0; z < slices; ++z)
            for (int y = 0; y < height; ++y)
              for (int x = 0; x < width; ++x)
                pixels3D[x + (y * width) + (z * (width * height))] = pixels2D[x + (z * width) + (((width - y) - 1) * width * height)];

          texture3D = new Texture3D(width, height, slices, TextureFormat.ARGB32, false);
          texture3D.SetPixels(pixels3D);
          texture3D.Apply();
          texture3D.filterMode = FilterMode.Bilinear;
          texture3D.wrapMode = TextureWrapMode.Clamp;
          texture3D.anisoLevel = 1;
        }

        return texture3D;
      }

      private T LoadTexture<T>(string path) where T : Texture
      {
        T texture = Resources.Load<T>(path);
        texture.wrapMode = TextureWrapMode.Clamp;

        return texture;
      }
    }    
  }
}
