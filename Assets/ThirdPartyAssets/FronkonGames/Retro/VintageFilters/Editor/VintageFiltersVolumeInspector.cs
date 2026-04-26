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
using UnityEditor;
using UnityEditor.Rendering;

namespace FronkonGames.Retro.VintageFilters.Editor
{
  /// <summary> Vintage Filters Volume inspector. </summary>
  [CustomEditor(typeof(VintageFiltersVolume))]
  public class VintageFiltersVolumeInspector : Inspector
  {
    protected override void InspectorGUI()
    {
      VintageFiltersVolume volume = (VintageFiltersVolume)target;

      /////////////////////////////////////////////////
      // Common.
      /////////////////////////////////////////////////
      DrawFloatSliderWithReset("intensity");

      /////////////////////////////////////////////////
      // Vintage Filters.
      /////////////////////////////////////////////////
      Separator();

      DrawEnumDropdownWithReset("filter", null, Filters.Hefe);
      IndentLevel++;
      switch (volume.filter.value)
      {
        case Filters.Amaro:
          DrawFloatSliderWithReset("amaroOverlay", "Overlay");
          break;
        case Filters.Hefe:
          DrawFloatSliderWithReset("hefeEdgeBurn", "Edge burn");
          DrawFloatSliderWithReset("hefeGradient", "Gradient");
          DrawFloatSliderWithReset("hefeSoftLight", "Soft light");
          break;
        case Filters.Hudson:
          DrawFloatSliderWithReset("hudsonOverlay", "Overlay");
          break;
        case Filters.Rise:
          DrawFloatSliderWithReset("riseOverlay", "Overlay");
          break;
        case Filters.Sierra:
          DrawFloatSliderWithReset("sierraOverlay", "Overlay");
          break;
        case Filters.Toaster:
          DrawFloatSliderWithReset("toasterOverlayWarm", "Overlay warm");
          break;
        case Filters.Sepia:
          DrawFloatSliderWithReset("sepiaIntensity", "Intensity");
          break;
        case Filters.Kodachrome:
          DrawFloatSliderWithReset("kodachromeEnhancement", "Enhancement");
          DrawFloatSliderWithReset("kodachromeWarmth", "Warmth");
          break;
        case Filters.Polaroid:
          DrawFloatSliderWithReset("polaroidOverexposure", "Overexposure");
          DrawFloatSliderWithReset("polaroidSoftness", "Softness");
          break;
        case Filters.CrossProcess:
          DrawFloatSliderWithReset("crossProcessColorShift", "Color shift");
          DrawFloatSliderWithReset("crossProcessContrastBoost", "Contrast boost");
          break;
        case Filters.BleachBypass:
          DrawFloatSliderWithReset("bleachBypassDesaturation", "Desaturation");
          DrawFloatSliderWithReset("bleachBypassContrast", "Contrast");
          break;
        case Filters.Vintage80s:
          DrawFloatSliderWithReset("vintage80sNeonIntensity", "Neon intensity");
          DrawFloatSliderWithReset("vintage80sColorPop", "Color pop");
          break;
        case Filters.FilmGrain:
          DrawFloatSliderWithReset("filmGrainIntensity", "Intensity");
          DrawFloatSliderWithReset("filmGrainSize", "Size");
          break;
        case Filters.Technicolor:
          DrawFloatSliderWithReset("technicolorSaturation", "Saturation");
          DrawFloatSliderWithReset("technicolorColorBalance", "Color balance");
          break;
        case Filters.Daguerreotype:
          DrawFloatSliderWithReset("daguerreotypeContrast", "Contrast");
          DrawFloatSliderWithReset("daguerreotypeSilvering", "Silvering");
          break;
        case Filters.Cyanotype:
          DrawFloatSliderWithReset("cyanotypeBlueIntensity", "Blue intensity");
          break;
        case Filters.Western:
          DrawFloatSliderWithReset("westernWarmth", "Warmth");
          DrawFloatSliderWithReset("westernDust", "Dust");
          break;
        case Filters.NightVision:
          DrawFloatSliderWithReset("nightVisionGain", "Gain");
          DrawFloatSliderWithReset("nightVisionNoise", "Noise");
          break;
        case Filters.Infrared:
          DrawFloatSliderWithReset("infraredFoliage", "Foliage");
          DrawFloatSliderWithReset("infraredBloom", "Bloom");
          break;
      }
      IndentLevel--;
    }

    protected override void ResetValues() => ((VintageFiltersVolume)target).Reset();

    protected override void CheckForErrors()
    {
      if (VintageFilters.IsInAnyRenderFeatures() == false)
      {
        Separator();
        EditorGUILayout.HelpBox($"Renderer Feature '{Constants.Asset.Name}' not found. You must add it as a Render Feature.", MessageType.Error);
      }
      else
      {
        VintageFilters[] effects = VintageFilters.Instances;
        bool anyEnabled = false;
        for (int i = 0; i < effects.Length; i++)
        {
          if (effects[i].isActive == true)
          {
            anyEnabled = true;
            break;
          }
        }

        if (anyEnabled == false)
        {
          Separator();
          EditorGUILayout.HelpBox($"No Renderer Feature '{Constants.Asset.Name}' is active. You must activate it in the Render Features.", MessageType.Warning);
        }
      }
    }
  }
}
