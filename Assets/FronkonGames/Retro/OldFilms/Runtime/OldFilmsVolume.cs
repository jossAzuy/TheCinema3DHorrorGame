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

namespace FronkonGames.Retro.OldFilms
{
  /// <summary> Old Films Volume. </summary>
  [Serializable, VolumeComponentMenu("Fronkon Games/Retro/Old Films")]
  public sealed class OldFilmsVolume : VolumeComponent, IPostProcessComponent
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
    #region Old Films settings.

    /// <summary> Film manufacturer. Default Black & White. </summary>
    [EnumDropdown((int)Manufacturers.Black_white, "Film manufacturer. Default Black & White.")]
    public EnumParameterNoInterpolation<Manufacturers> manufacturer = new(Manufacturers.Black_white);

    /// <summary> Frame movement. Default (0.2, 0.2). </summary>
    [Vector2WithReset(0.2f, 0.2f, "Frame movement. Default (0.2, 0.2).")]
    public Vector2ParameterNoInterpolation moveFrame = new(Vector2.one * 0.2f);

    /// <summary> Frame jumping [0, 1]. Default 0.1. </summary>
    [FloatSliderWithReset(0.1f, 0.0f, 1.0f, "Frame jumping [0, 1]. Default 0.1.")]
    public FloatSliderParameterNoInterpolation jumpFrame = new(0.1f, 0.0f, 1.0f);

    /// <summary> Natural vignette [0, 2]. Default 0.2. </summary>
    [FloatSliderWithReset(0.2f, 0.0f, 2.0f, "Natural vignette [0, 2]. Default 0.2.")]
    public FloatSliderParameterNoInterpolation vignette = new(0.2f, 0.0f, 2.0f);

    /// <summary> Sepia tones [0, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f, "Sepia tones [0, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation sepia = new(0.0f, 0.0f, 1.0f);

    /// <summary> Grain strength [0, 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Grain strength [0, 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation grain = new(0.5f, 0.0f, 1.0f);

    /// <summary> Blink strength [0, 1]. Default 0. </summary>
    [FloatSliderWithReset(0.0f, 0.0f, 1.0f, "Blink strength [0, 1]. Default 0.")]
    public FloatSliderParameterNoInterpolation blinkStrength = new(0.0f, 0.0f, 1.0f);

    /// <summary> Blink speed. Default 10. </summary>
    [FloatSliderWithReset(10.0f, 0.0f, 30.0f, "Blink speed. Default 10.")]
    public FloatSliderParameterNoInterpolation blinkSpeed = new(10.0f, 0.0f, 30.0f);

    /// <summary> Film blotches [0 - 6]. Default 3. </summary>
    [IntSliderWithReset(3, 0, 6, "Film blotches [0 - 6]. Default 3.")]
    public ClampedIntParameterNoInterpolation blotches = new(3, 0, 6);

    /// <summary> Film blotch size [0 - 10]. Default 1. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 10.0f, "Film blotch size [0 - 10]. Default 1.")]
    public FloatSliderParameterNoInterpolation blotchSize = new(1.0f, 0.0f, 10.0f);

    /// <summary> Film scratches [0.0 - 1]. Default 0.5. </summary>
    [FloatSliderWithReset(0.5f, 0.0f, 1.0f, "Film scratches [0.0 - 1]. Default 0.5.")]
    public FloatSliderParameterNoInterpolation scratches = new(0.5f, 0.0f, 1.0f);

    /// <summary> Film lines [0 - 8]. Default 4. </summary>
    [IntSliderWithReset(4, 0, 8, "Film lines [0 - 8]. Default 4.")]
    public ClampedIntParameterNoInterpolation lines = new(4, 0, 8);

    /// <summary> Lines strength [0.0 - 1]. Default 0.25. </summary>
    [FloatSliderWithReset(0.25f, 0.0f, 1.0f, "Lines strength [0.0 - 1]. Default 0.25.")]
    public FloatSliderParameterNoInterpolation linesStrength = new(0.25f, 0.0f, 1.0f);

    #region Color Decision List (https://en.wikipedia.org/wiki/ASC_CDL).

    /// <summary> 'Color Decision List' slope of the transfer function without shifting the black level. </summary>
    [Vector3WithReset(1.0f, 1.0f, 1.0f, "'Color Decision List' slope of the transfer function without shifting the black level.")]
    public Vector3ParameterNoInterpolation cldSlope = new(Vector3.one);

    /// <summary> 'Color Decision List' raises or lowers overall brightness of a component. </summary>
    [Vector3WithReset(0.0f, 0.0f, 0.0f, "'Color Decision List' raises or lowers overall brightness of a component.")]
    public Vector3ParameterNoInterpolation cldOffset = new(Vector3.zero);

    /// <summary> 'Color Decision List' changes the intermediate shape of the transfer function. </summary>
    [Vector3WithReset(1.0f, 1.0f, 1.0f, "'Color Decision List' changes the intermediate shape of the transfer function.")]
    public Vector3ParameterNoInterpolation cldPower = new(Vector3.one);

    /// <summary> 'Color Decision List' saturation. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "'Color Decision List' saturation.")]
    public FloatSliderParameterNoInterpolation cldSaturation = new(1.0f, 0.0f, 2.0f);

    /// <summary> 'Color Decision List' contrast. </summary>
    [FloatSliderWithReset(1.0f, 0.0f, 2.0f, "'Color Decision List' contrast.")]
    public FloatSliderParameterNoInterpolation cldContrast = new(1.0f, 0.0f, 2.0f);

    /// <summary> 'Color Decision List' gamma. </summary>
    [FloatSliderWithReset(1.0f, 0.1f, 2.0f, "'Color Decision List' gamma.")]
    public FloatSliderParameterNoInterpolation cldGamma = new(1.0f, 0.1f, 2.0f);

    /// <summary> 'Color Decision List' extra color contrast. </summary>
    [ToggleWithReset(false, "'Color Decision List' extra color contrast.")]
    public BoolParameterNoInterpolation cldFilmContrast = new(false);

    #endregion

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

      manufacturer.value = Manufacturers.Black_white;
      moveFrame.value = Vector2.one * 0.2f;
      jumpFrame.value = 0.1f;
      vignette.value = 0.2f;
      sepia.value = 0.0f;
      grain.value = 0.5f;
      blinkStrength.value = 0.0f;
      blinkSpeed.value = 10.0f;
      blotches.value = 3;
      blotchSize.value = 1.0f;
      scratches.value = 0.5f;
      lines.value = 4;
      linesStrength.value = 0.25f;

      cldSlope.value = Vector3.one;
      cldOffset.value = Vector3.zero;
      cldPower.value = Vector3.one;
      cldSaturation.value = 1.0f;
      cldContrast.value = 1.0f;
      cldGamma.value = 1.0f;
      cldFilmContrast.value = false;

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
