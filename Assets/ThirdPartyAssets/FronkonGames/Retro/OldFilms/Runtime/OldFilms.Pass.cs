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

namespace FronkonGames.Retro.OldFilms
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary> Render Pass. </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  public sealed partial class OldFilms
  {
    [DisallowMultipleRendererFeature]
    private sealed class RenderPass : ScriptableRenderPass
    {
      // Internal use only.
      internal Material material { get; set; }

      private OldFilmsVolume volume;

      private static class ShaderIDs
      {
        internal static readonly int Intensity = Shader.PropertyToID("_Intensity");
        internal static readonly int EffectTime = Shader.PropertyToID("_EffectTime");

        internal static readonly int Slope = Shader.PropertyToID("_Slope");
        internal static readonly int Offset = Shader.PropertyToID("_Offset");
        internal static readonly int Power = Shader.PropertyToID("_Power");
        internal static readonly int FilmSaturation = Shader.PropertyToID("_FilmSaturation");
        internal static readonly int FilmContrast = Shader.PropertyToID("_FilmContrast");
        internal static readonly int FilmGamma = Shader.PropertyToID("_FilmGamma");
        internal static readonly int SuperContrast = Shader.PropertyToID("_SuperContrast");
        internal static readonly int RandomValue = Shader.PropertyToID("_RandomValue");
        internal static readonly int MoveFrame = Shader.PropertyToID("_MoveFrame");
        internal static readonly int JumpFrame = Shader.PropertyToID("_JumpFrame");
        internal static readonly int Vignette = Shader.PropertyToID("_Vignette");
        internal static readonly int Sepia = Shader.PropertyToID("_Sepia");
        internal static readonly int Grain = Shader.PropertyToID("_Grain");
        internal static readonly int BlinkStrength = Shader.PropertyToID("_BlinkStrength");
        internal static readonly int BlinkSpeed = Shader.PropertyToID("_BlinkSpeed");
        internal static readonly int Blotches = Shader.PropertyToID("_Blotches");
        internal static readonly int BlotchSize = Shader.PropertyToID("_BlotchSize");
        internal static readonly int Scratches = Shader.PropertyToID("_Scratches");
        internal static readonly int Lines = Shader.PropertyToID("_Lines");
        internal static readonly int LinesStrength = Shader.PropertyToID("_LinesStrength");

        internal static readonly int Brightness = Shader.PropertyToID("_Brightness");
        internal static readonly int Contrast = Shader.PropertyToID("_Contrast");
        internal static readonly int Gamma = Shader.PropertyToID("_Gamma");
        internal static readonly int Hue = Shader.PropertyToID("_Hue");
        internal static readonly int Saturation = Shader.PropertyToID("_Saturation");
      }

      /// <summary> Render pass constructor. </summary>
      public RenderPass() : base()
      {
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

        if (volume.manufacturer.value != Manufacturers.Custom)
        {
          switch (volume.manufacturer.value)
          {
            case Manufacturers.Kodak_2383:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.01f, 1.0f, 1.0f));
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, new Vector3(0.95f, 1.0f, 1.0f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.2f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Kodak_2393:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.08f, 1.19f, 1.07f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.04f, -0.06f, 0.02f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.07f, 1.11f, 1.20f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Kodak_2395:
              material.SetVector(ShaderIDs.Slope, new Vector3(0.98f, 1.0f, 1.03f));
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, new Vector3(0.84f, 0.97f, 1.10f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Agfa_1978:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.12f, 1.42f, 1.19f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.04f, -0.06f, 0.02f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.94f, 0.81f, 0.83f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.7f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.06f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Agfa_1905:
              material.SetVector(ShaderIDs.Slope, Vector3.one);
              material.SetVector(ShaderIDs.Offset, new Vector3(-0.05f, -0.04f, -0.03f));
              material.SetVector(ShaderIDs.Power, Vector3.one);
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.33f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.6f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Agfa_1935:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.33f, 1.01f, 0.63f));
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, new Vector3(1.21f, 0.96f, 0.74f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.6f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.83f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Beer_1973:
              material.SetVector(ShaderIDs.Slope, new Vector3(0.88f, 0.96f, 1.24f));
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, new Vector3(1.45f, 1.29f, 1.27f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.93f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.9f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Beer_1933:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.2f, 1.2f, 1.2f));
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, new Vector3(1.3f, 1.3f, 1.3f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.8f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.2f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Beer_2001:
              material.SetVector(ShaderIDs.Slope, new Vector3(0.93f, 0.94f, 0.96f));
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, new Vector3(1.6f, 1.1f, 0.95f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.4f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.1f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.7f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Beer_2006:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.616452f, 1.331932f, 0.842867f));
              material.SetVector(ShaderIDs.Offset, new Vector3(-0.152205f, 0.079621f, 0.197558f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.650251f, 1.536614f, 1.553357f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.7f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.1f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Polaroid:
              material.SetVector(ShaderIDs.Slope, new Vector3(0.65f, 1.0f, 0.8f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.07f, 0.0f, 0.08f));
              material.SetVector(ShaderIDs.Power, Vector3.one);
              material.SetFloat(ShaderIDs.FilmSaturation, 1.4f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Cuba_libre:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.19f, 1.1f, 0.77f));
              material.SetVector(ShaderIDs.Offset, new Vector3(-0.04f, -0.08f, -0.07f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.8f, 0.8f, 0.8f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.9f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.9f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Fuji_4711:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.1f, 1.0f, 0.8f));
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, new Vector3(1.5f, 1.0f, 1.0f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.6f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.9f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.ORWO_0815:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.15f, 1.11f, 0.86f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.0f, 0.01f, -0.02f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.41f, 1.0f, 0.74f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.45f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.98f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.86f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Black_white:
              material.SetVector(ShaderIDs.Slope, Vector3.one);
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, Vector3.one);
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.1f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.7f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Spearmint:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.02f, 1.32f, 1.09f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.04f, -0.06f, 0.02f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.70f, 0.44f, 0.51f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.8f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.30f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Tea_time:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.297496f, 0.943091f, 0.501793f));
              material.SetVector(ShaderIDs.Offset, new Vector3(-0.132450f, 0.036699f, 0.147457f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.180667f, 1.032265f, 1.215274f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Purple_rain:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.671897f, 1.274243f, 0.994490f));
              material.SetVector(ShaderIDs.Offset, new Vector3(-0.052148f, -0.026815f, 0.483182f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.650251f, 1.536614f, 1.553357f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.282609f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Kodak_Ektar_100:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.15f, 1.05f, 0.95f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.02f, 0.01f, -0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.95f, 1.0f, 1.05f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.4f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.1f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.95f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Kodak_Portra_160:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.05f, 1.02f, 0.98f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.01f, 0.0f, 0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.0f, 1.0f, 1.02f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.9f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.95f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Kodak_Portra_400:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.08f, 1.03f, 0.97f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.02f, 0.01f, 0.02f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.98f, 1.0f, 1.03f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.85f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.9f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.05f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Kodak_Portra_800:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.12f, 1.05f, 0.95f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.03f, 0.02f, 0.03f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.95f, 0.98f, 1.05f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.8f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.85f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.1f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Kodak_Ektachrome_E100:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.0f, 1.05f, 1.1f));
              material.SetVector(ShaderIDs.Offset, new Vector3(-0.01f, 0.0f, 0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.05f, 1.0f, 0.95f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.2f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.05f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.9f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Fuji_Pro_400H:
              material.SetVector(ShaderIDs.Slope, new Vector3(0.95f, 1.0f, 1.05f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.01f, 0.0f, -0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.02f, 1.0f, 0.98f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.75f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.9f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.0f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Fuji_Velvia_50:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.2f, 1.15f, 1.0f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.0f, -0.02f, 0.0f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.9f, 0.95f, 1.05f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.6f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.2f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.85f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Fuji_Velvia_100:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.18f, 1.12f, 0.98f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.01f, -0.01f, 0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.92f, 0.97f, 1.03f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.5f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.15f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.88f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Fuji_Provia_100F:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.02f, 1.05f, 1.08f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.0f, 0.0f, -0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.0f, 0.98f, 0.95f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.1f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.95f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Ilford_HP5_Plus:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.1f, 1.1f, 1.1f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.02f, 0.02f, 0.02f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.05f, 1.05f, 1.05f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.15f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.8f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Ilford_FP4_Plus:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.05f, 1.05f, 1.05f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.01f, 0.01f, 0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.02f, 1.02f, 1.02f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.1f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.85f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Ilford_Delta_100:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.02f, 1.02f, 1.02f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.0f, 0.0f, 0.0f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.0f, 1.0f, 1.0f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.05f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.9f);
              material.SetInt(ShaderIDs.SuperContrast, 0);
              break;
            case Manufacturers.Ilford_Delta_400:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.08f, 1.08f, 1.08f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.02f, 0.02f, 0.02f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.03f, 1.03f, 1.03f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.12f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.85f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Cinestill_50D:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.05f, 1.0f, 0.92f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.01f, 0.0f, 0.03f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.98f, 1.0f, 1.08f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.1f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.05f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.95f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Cinestill_800T:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.1f, 0.95f, 0.85f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.03f, 0.02f, 0.08f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.92f, 1.05f, 1.15f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.9f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.95f);
              material.SetFloat(ShaderIDs.FilmGamma, 1.1f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Lomography_Color_Negative_100:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.25f, 1.1f, 0.9f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.05f, 0.02f, 0.05f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.85f, 0.9f, 1.1f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.3f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.0f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.9f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Lomography_Color_Negative_400:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.3f, 1.15f, 0.85f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.08f, 0.03f, 0.08f));
              material.SetVector(ShaderIDs.Power, new Vector3(0.8f, 0.85f, 1.15f));
              material.SetFloat(ShaderIDs.FilmSaturation, 1.4f);
              material.SetFloat(ShaderIDs.FilmContrast, 0.95f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.95f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            case Manufacturers.Ferrania_P30:
              material.SetVector(ShaderIDs.Slope, new Vector3(1.0f, 1.0f, 1.0f));
              material.SetVector(ShaderIDs.Offset, new Vector3(0.01f, 0.01f, 0.01f));
              material.SetVector(ShaderIDs.Power, new Vector3(1.1f, 1.1f, 1.1f));
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.2f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.75f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
            default:
              material.SetVector(ShaderIDs.Slope, Vector3.one);
              material.SetVector(ShaderIDs.Offset, Vector3.zero);
              material.SetVector(ShaderIDs.Power, Vector3.one);
              material.SetFloat(ShaderIDs.FilmSaturation, 0.0f);
              material.SetFloat(ShaderIDs.FilmContrast, 1.1f);
              material.SetFloat(ShaderIDs.FilmGamma, 0.7f);
              material.SetInt(ShaderIDs.SuperContrast, 1);
              break;
          }
        }
        else
        {
          material.SetVector(ShaderIDs.Slope, volume.cldSlope.value);
          material.SetVector(ShaderIDs.Offset, volume.cldOffset.value);
          material.SetVector(ShaderIDs.Power, volume.cldPower.value);
          material.SetFloat(ShaderIDs.FilmSaturation, volume.cldSaturation.value);
          material.SetFloat(ShaderIDs.FilmContrast, volume.cldContrast.value);
          material.SetFloat(ShaderIDs.FilmGamma, volume.cldGamma.value);
          material.SetInt(ShaderIDs.SuperContrast, volume.cldFilmContrast.value ? 1 : 0);
        }

        material.SetVector(ShaderIDs.RandomValue, new Vector4(Random.value, Random.value, Random.value, Random.value));
        material.SetVector(ShaderIDs.MoveFrame, volume.moveFrame.value * 0.01f);
        material.SetFloat(ShaderIDs.JumpFrame, volume.jumpFrame.value * 0.1f);
        material.SetFloat(ShaderIDs.Grain, volume.grain.value);
        material.SetFloat(ShaderIDs.Sepia, volume.sepia.value);
        material.SetFloat(ShaderIDs.Vignette, volume.vignette.value);
        material.SetFloat(ShaderIDs.BlinkStrength, volume.blinkStrength.value);
        material.SetFloat(ShaderIDs.BlinkSpeed, volume.blinkSpeed.value);
        material.SetInt(ShaderIDs.Blotches, volume.blotches.value);
        material.SetFloat(ShaderIDs.BlotchSize, volume.blotchSize.value);
        material.SetFloat(ShaderIDs.Scratches, volume.scratches.value);
        material.SetInt(ShaderIDs.Lines, volume.lines.value);
        material.SetFloat(ShaderIDs.LinesStrength, volume.linesStrength.value / 8.0f);

        material.SetFloat(ShaderIDs.Brightness, volume.brightness.value);
        material.SetFloat(ShaderIDs.Contrast, volume.contrast.value);
        material.SetFloat(ShaderIDs.Gamma, 1.0f / volume.gamma.value);
        material.SetFloat(ShaderIDs.Hue, volume.hue.value);
        material.SetFloat(ShaderIDs.Saturation, volume.saturation.value);
      }

      /// <inheritdoc/>
      public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
      {
        volume = VolumeManager.instance.stack.GetComponent<OldFilmsVolume>();
        if (material == null || volume == null || volume.IsActive() == false)
          return;

        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        if (resourceData.isActiveTargetBackBuffer == true)
          return;

        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        if (cameraData.camera.cameraType == CameraType.SceneView && volume.affectSceneView.value == false || cameraData.postProcessEnabled == false)
          return;

        TextureHandle source = resourceData.activeColorTexture;
        
        // Ensure we have a valid source texture
        if (source.IsValid() == false)
          return;
          
        TextureDesc sourceDesc = source.GetDescriptor(renderGraph);
        TextureHandle destination = renderGraph.CreateTexture(sourceDesc);

        UpdateMaterial();

        RenderGraphUtils.BlitMaterialParameters pass = new(source, destination, material, 0);
        renderGraph.AddBlitPass(pass, $"{Constants.Asset.AssemblyName}.Pass");

        resourceData.cameraColor = destination;
      }
    }
  }
}
