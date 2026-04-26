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

namespace FronkonGames.Retro.OldFilms.Editor
{
  /// <summary> Old Films Volume inspector. </summary>
  [CustomEditor(typeof(OldFilmsVolume))]
  public class OldFilmsVolumeInspector : Inspector
  {
    protected override void InspectorGUI()
    {
      OldFilmsVolume volume = (OldFilmsVolume)target;

      /////////////////////////////////////////////////
      // Common.
      /////////////////////////////////////////////////
      DrawFloatSliderWithReset("intensity");

      /////////////////////////////////////////////////
      // Old Films.
      /////////////////////////////////////////////////
      Separator();

      DrawEnumDropdownWithReset<Manufacturers>("manufacturer");
      if (volume.manufacturer.value == Manufacturers.Custom)
      {
        IndentLevel++;
        DrawVector3WithReset("cldSlope", "Slope");
        DrawVector3WithReset("cldOffset", "Offset");
        DrawVector3WithReset("cldPower", "Power");
        DrawFloatSliderWithReset("cldSaturation", "Saturation");
        DrawFloatSliderWithReset("cldContrast", "Contrast");
        DrawFloatSliderWithReset("cldGamma", "Gamma");
        DrawToggleWithReset("cldFilmContrast", "Film contrast");
        IndentLevel--;
      }

      DrawVector2WithReset("moveFrame", "Frame movement");
      DrawFloatSliderWithReset("jumpFrame", "Frame jumping");
      DrawFloatSliderWithReset("vignette", "Vignette");
      DrawFloatSliderWithReset("sepia", "Sepia");
      DrawFloatSliderWithReset("grain", "Grain");
      DrawFloatSliderWithReset("blinkStrength", "Blink strength");
      if (volume.blinkStrength.value > 0.0f)
      {
        IndentLevel++;
        DrawFloatSliderWithReset("blinkSpeed", "Blink speed");
        IndentLevel--;
      }
      DrawIntSliderWithReset("blotches", "Blotches");
      if (volume.blotches.value > 0)
      {
        IndentLevel++;
        DrawFloatSliderWithReset("blotchSize", "Blotch size");
        IndentLevel--;
      }
      DrawFloatSliderWithReset("scratches", "Scratches");
      DrawIntSliderWithReset("lines", "Lines");
      if (volume.lines.value > 0)
      {
        IndentLevel++;
        DrawFloatSliderWithReset("linesStrength", "Lines strength");
        IndentLevel--;
      }
    }

    protected override void ResetValues() => ((OldFilmsVolume)target).Reset();

    protected override void CheckForErrors()
    {
      if (OldFilms.IsInAnyRenderFeatures() == false)
      {
        Separator();
        EditorGUILayout.HelpBox($"Renderer Feature '{Constants.Asset.Name}' not found. You must add it as a Render Feature.", MessageType.Error);
      }
      else
      {
        OldFilms[] effects = OldFilms.Instances;
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
