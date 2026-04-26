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
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FronkonGames.Retro.VintageFilters
{
  /// <summary> Vintage Filters Volume. </summary>
  [Serializable, VolumeComponentMenu("Fronkon Games/Retro/Vintage Filters"), HelpURL(Constants.Support.Documentation)]
  public sealed class VintageFiltersVolume : VolumeComponent, IPostProcessComponent
  {
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Common settings.

    /// <summary> Controls the intensity of the effect [0, 1]. Default 1. </summary>
    /// <remarks> An effect with Intensity equal to 0 will not be executed. </remarks>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f, "Controls the intensity of the effect [0, 1]. Default 1.")]
    public FloatSliderParameterLinear intensity = new(1.0f, 0.0f, 1.0f);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Vintage Filters settings.

    /// <summary> Current filter. </summary>
    [EnumDropdown((int)Filters.Hefe, "Current filter.")]
    public EnumParameterNoInterpolation<Filters> filter = new(Filters.Hefe);

    /// <summary> Amaro: Overlay strength [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Amaro: Overlay strength [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation amaroOverlay = new(0.5f, 0.0f, 1.0f);

    /// <summary> Hefe: Edge burn strength [0, 1]. Default 1.0. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f, "Hefe: Edge burn strength [0, 1]. Default 1.0.")]
    public FloatSliderParameterNoInterpolation hefeEdgeBurn = new(1.0f, 0.0f, 1.0f);

    /// <summary> Hefe: Gradient strength [0, 1]. Default 1.0. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f, "Hefe: Gradient strength [0, 1]. Default 1.0.")]
    public FloatSliderParameterNoInterpolation hefeGradient = new(1.0f, 0.0f, 1.0f);

    /// <summary> Hefe: Soft light strength [0, 1]. Default 1.0. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 1.0f, "Hefe: Soft light strength [0, 1]. Default 1.0.")]
    public FloatSliderParameterNoInterpolation hefeSoftLight = new(1.0f, 0.0f, 1.0f);

    /// <summary> Hudson: Overlay strength [0, 1]. Default 0.25. </summary>
    [FloatSliderWithReset(0.25f, 0.0f, 1.0f, "Hudson: Overlay strength [0, 1]. Default 0.25.")]
    public FloatSliderParameterNoInterpolation hudsonOverlay = new(0.25f, 0.0f, 1.0f);

    /// <summary> Rise: Overlay strength [0, 1]. Default 0.25. </summary>
    [FloatSliderWithReset(0.25f, 0.0f, 1.0f, "Rise: Overlay strength [0, 1]. Default 0.25.")]
    public FloatSliderParameterNoInterpolation riseOverlay = new(0.25f, 0.0f, 1.0f);

    /// <summary> Sierra: Overlay strength [0, 1]. Default 0.25. </summary>
    [FloatSliderWithReset(0.25f, 0.0f, 1.0f, "Sierra: Overlay strength [0, 1]. Default 0.25.")]
    public FloatSliderParameterNoInterpolation sierraOverlay = new(0.25f, 0.0f, 1.0f);

    /// <summary> Toaster: Soft light strength [0, 1]. Default 0.25. </summary>
    [FloatSliderWithReset(0.25f, 0.0f, 1.0f, "Toaster: Soft light strength [0, 1]. Default 0.25.")]
    public FloatSliderParameterNoInterpolation toasterOverlayWarm = new(0.25f, 0.0f, 1.0f);

    /// <summary> Sepia: Sepia intensity [0, 1]. Default 0.8. </summary>
    [FloatSliderWithReset(0.8f, 0.0f, 1.0f, "Sepia: Sepia intensity [0, 1]. Default 0.8.")]
    public FloatSliderParameterNoInterpolation sepiaIntensity = new(0.8f, 0.0f, 1.0f);

    /// <summary> Kodachrome: Color enhancement [0, 1]. Default 0.6. </summary>
    [FloatSliderWithReset(0.6f, 0.0f, 1.0f, "Kodachrome: Color enhancement [0, 1]. Default 0.6.")]
    public FloatSliderParameterNoInterpolation kodachromeEnhancement = new(0.6f, 0.0f, 1.0f);

    /// <summary> Kodachrome: Warmth [0, 1]. Default 0.3. </summary>
    [FloatSliderWithReset(0.3f, 0.0f, 1.0f, "Kodachrome: Warmth [0, 1]. Default 0.3.")]
    public FloatSliderParameterNoInterpolation kodachromeWarmth = new(0.3f, 0.0f, 1.0f);

    /// <summary> Polaroid: Overexposure [0, 1]. Default 0.2. </summary>
    [FloatSliderWithReset(0.2f, 0.0f, 1.0f, "Polaroid: Overexposure [0, 1]. Default 0.2.")]
    public FloatSliderParameterNoInterpolation polaroidOverexposure = new(0.2f, 0.0f, 1.0f);

    /// <summary> Polaroid: Softness [0, 1]. Default 0.4. </summary>
    [FloatSliderWithReset(0.4f, 0.0f, 1.0f, "Polaroid: Softness [0, 1]. Default 0.4.")]
    public FloatSliderParameterNoInterpolation polaroidSoftness = new(0.4f, 0.0f, 1.0f);

    /// <summary> Cross Process: Color shift intensity [0, 1]. Default 0.7. </summary>
    [FloatSliderWithReset(0.7f, 0.0f, 1.0f, "Cross Process: Color shift intensity [0, 1]. Default 0.7.")]
    public FloatSliderParameterNoInterpolation crossProcessColorShift = new(0.7f, 0.0f, 1.0f);

    /// <summary> Cross Process: Contrast boost [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Cross Process: Contrast boost [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation crossProcessContrastBoost = new(0.5f, 0.0f, 1.0f);

    /// <summary> Bleach Bypass: Desaturation amount [0, 1]. Default 0.6. </summary>
    [FloatSliderWithReset(0.6f, 0.0f, 1.0f, "Bleach Bypass: Desaturation amount [0, 1]. Default 0.6.")]
    public FloatSliderParameterNoInterpolation bleachBypassDesaturation = new(0.6f, 0.0f, 1.0f);

    /// <summary> Bleach Bypass: Contrast increase [0, 1]. Default 0.4. </summary>
    [FloatSliderWithReset(0.4f, 0.0f, 1.0f, "Bleach Bypass: Contrast increase [0, 1]. Default 0.4.")]
    public FloatSliderParameterNoInterpolation bleachBypassContrast = new(0.4f, 0.0f, 1.0f);

    /// <summary> Vintage 80s: Neon intensity [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Vintage 80s: Neon intensity [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation vintage80sNeonIntensity = new(0.5f, 0.0f, 1.0f);

    /// <summary> Vintage 80s: Color pop [0, 1]. Default 0.6. </summary>
    [FloatSliderWithReset(0.6f, 0.0f, 1.0f, "Vintage 80s: Color pop [0, 1]. Default 0.6.")]
    public FloatSliderParameterNoInterpolation vintage80sColorPop = new(0.6f, 0.0f, 1.0f);

    /// <summary> Film Grain: Grain intensity [0, 1]. Default 0.3. </summary>
    [FloatSliderWithReset(0.3f, 0.0f, 1.0f, "Film Grain: Grain intensity [0, 1]. Default 0.3.")]
    public FloatSliderParameterNoInterpolation filmGrainIntensity = new(0.3f, 0.0f, 1.0f);

    /// <summary> Film Grain: Grain size [0.5, 2.0]. Default 1.0. </summary>
    [FloatSliderWithReset(1.0f, 0.5f, 2.0f, "Film Grain: Grain size [0.5, 2.0]. Default 1.0.")]
    public FloatSliderParameterNoInterpolation filmGrainSize = new(1.0f, 0.5f, 2.0f);

    /// <summary> Technicolor: Saturation [0, 2]. Default 1.5. </summary>
    [FloatSliderWithReset(1.5f, 0.0f, 2.0f, "Technicolor: Saturation [0, 2]. Default 1.5.")]
    public FloatSliderParameterNoInterpolation technicolorSaturation = new(1.5f, 0.0f, 2.0f);

    /// <summary> Technicolor: Color balance [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Technicolor: Color balance [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation technicolorColorBalance = new(0.5f, 0.0f, 1.0f);

    /// <summary> Daguerreotype: Contrast [0, 1]. Default 0.8. </summary>
    [FloatSliderWithReset(0.8f, 0.0f, 1.0f, "Daguerreotype: Contrast [0, 1]. Default 0.8.")]
    public FloatSliderParameterNoInterpolation daguerreotypeContrast = new(0.8f, 0.0f, 1.0f);

    /// <summary> Daguerreotype: Silvering/Metallic effect [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Daguerreotype: Silvering/Metallic effect [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation daguerreotypeSilvering = new(0.5f, 0.0f, 1.0f);

    /// <summary> Cyanotype: Blue intensity [0, 1]. Default 0.7. </summary>
    [FloatSliderWithReset(0.7f, 0.0f, 1.0f, "Cyanotype: Blue intensity [0, 1]. Default 0.7.")]
    public FloatSliderParameterNoInterpolation cyanotypeBlueIntensity = new(0.7f, 0.0f, 1.0f);

    /// <summary> Western: Warmth [0, 1]. Default 0.6. </summary>
    [FloatSliderWithReset(0.6f, 0.0f, 1.0f, "Western: Warmth [0, 1]. Default 0.6.")]
    public FloatSliderParameterNoInterpolation westernWarmth = new(0.6f, 0.0f, 1.0f);

    /// <summary> Western: Dust/Exposure [0, 1]. Default 0.3. </summary>
    [FloatSliderWithReset(0.3f, 0.0f, 1.0f, "Western: Dust/Exposure [0, 1]. Default 0.3.")]
    public FloatSliderParameterNoInterpolation westernDust = new(0.3f, 0.0f, 1.0f);

    /// <summary> Night Vision: Luminance gain [0, 2]. Default 1.0. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "Night Vision: Luminance gain [0, 2]. Default 1.0.")]
    public FloatSliderParameterNoInterpolation nightVisionGain = new(1.0f, 0.0f, 2.0f);

    /// <summary> Night Vision: Noise intensity [0, 1]. Default 0.4. </summary>
    [FloatSliderWithReset(0.4f, 0.0f, 1.0f, "Night Vision: Noise intensity [0, 1]. Default 0.4.")]
    public FloatSliderParameterNoInterpolation nightVisionNoise = new(0.4f, 0.0f, 1.0f);

    /// <summary> Infrared: Foliage sensitivity [0, 1]. Default 0.8. </summary>
    [FloatSliderWithReset(0.8f, 0.0f, 1.0f, "Infrared: Foliage sensitivity [0, 1]. Default 0.8.")]
    public FloatSliderParameterNoInterpolation infraredFoliage = new(0.8f, 0.0f, 1.0f);

    /// <summary> Infrared: Bloom intensity [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Infrared: Bloom intensity [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation infraredBloom = new(0.5f, 0.0f, 1.0f);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Color settings.

    /// <summary> Brightness [-1, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, -1.0f, 1.0f, "Brightness [-1, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation brightness = new(0.0f, -1.0f, 1.0f);

    /// <summary> Contrast [0, 10]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 10.0f, "Contrast [0, 10]. Default 1.")]
    public FloatSliderParameterNoInterpolation contrast = new(1.0f, 0.0f, 10.0f);

    /// <summary> Gamma [0.1, 10]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.1f, 10.0f, "Gamma [0.1, 10]. Default 1.")]
    public FloatSliderParameterNoInterpolation gamma = new(1.0f, 0.1f, 10.0f);

    /// <summary> The color wheel [0, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f, "The color wheel [0, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation hue = new(0.0f, 0.0f, 1.0f);

    /// <summary> Intensity of a colors [0, 2]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "Intensity of a colors [0, 2]. Default 1.")]
    public FloatSliderParameterNoInterpolation saturation = new(1.0f, 0.0f, 2.0f);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region Advanced settings.

    /// <summary> Does it affect the Scene View? </summary>
    [ToggleWithReset(false, "Does it affect the Scene View?")]
    public BoolParameterNoInterpolation affectSceneView = new(false);

    /// <summary> Use scaled time. </summary>
    [ToggleWithReset(true, "Use scaled time.")]
    public BoolParameterNoInterpolation useScaledTime = new(true);

    #endregion
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary> Reset to default values. </summary>
    public void Reset()
    {
      intensity.value = 1.0f;
      filter.value = Filters.Hefe;
      amaroOverlay.value = 0.5f;
      hefeEdgeBurn.value = 1.0f;
      hefeGradient.value = 1.0f;
      hefeSoftLight.value = 1.0f;
      hudsonOverlay.value = 0.25f;
      riseOverlay.value = 0.25f;
      sierraOverlay.value = 0.25f;
      toasterOverlayWarm.value = 0.25f;
      sepiaIntensity.value = 0.8f;
      kodachromeEnhancement.value = 0.6f;
      kodachromeWarmth.value = 0.3f;
      polaroidOverexposure.value = 0.2f;
      polaroidSoftness.value = 0.4f;
      crossProcessColorShift.value = 0.7f;
      crossProcessContrastBoost.value = 0.5f;
      bleachBypassDesaturation.value = 0.6f;
      bleachBypassContrast.value = 0.4f;
      vintage80sNeonIntensity.value = 0.5f;
      vintage80sColorPop.value = 0.6f;
      filmGrainIntensity.value = 0.3f;
      filmGrainSize.value = 1.0f;
      technicolorSaturation.value = 1.5f;
      technicolorColorBalance.value = 0.5f;
      daguerreotypeContrast.value = 0.8f;
      daguerreotypeSilvering.value = 0.5f;
      cyanotypeBlueIntensity.value = 0.7f;
      westernWarmth.value = 0.6f;
      westernDust.value = 0.3f;
      nightVisionGain.value = 1.0f;
      nightVisionNoise.value = 0.4f;
      infraredFoliage.value = 0.8f;
      infraredBloom.value = 0.5f;

      brightness.value = 0.0f;
      contrast.value = 1.0f;
      gamma.value = 1.0f;
      hue.value = 0.0f;
      saturation.value = 1.0f;

      affectSceneView.value = false;
      useScaledTime.value = true;
    }

    /// <summary> Is the effect active? </summary>
    public bool IsActive() => intensity.overrideState && intensity.value > 0.0f;

    /// <summary> Is the effect tile compatible? </summary>
    public bool IsTileCompatible() => false;
  }
}
