using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FronkonGames.Retro.VintageFilters
{
  /// <summary> Retro: Vintage Filters demo. </summary>
  /// <remarks>
  /// This code is designed for a simple demo, not for production environments.
  /// </remarks>
  public class RetroVintageFiltersDemo : MonoBehaviour
  {
    [Header("This code is only for the demo, not for production environments."), Space(20.0f)]

    [SerializeField]
    private VolumeProfile volumeProfile;

    private VintageFiltersVolume volume;

    private GUIStyle styleTitle;
    private GUIStyle styleLabel;
    private GUIStyle styleButton;

    private void ResetEffect()
    {
      volume?.Reset();
    }

    private void Awake()
    {
      styleTitle = styleLabel = styleButton = null;

      if (VintageFilters.IsInRenderFeatures() == false)
      {
        Debug.LogWarning($"Effect '{Constants.Asset.Name}' not found. You must add it as a Render Feature.");
#if UNITY_EDITOR
        if (UnityEditor.EditorUtility.DisplayDialog($"Effect '{Constants.Asset.Name}' not found", $"You must add '{Constants.Asset.Name}' as a Render Feature.", "Quit") == true)
          UnityEditor.EditorApplication.isPlaying = false;
#endif
      }

      volume = volumeProfile != null && volumeProfile.TryGet(out VintageFiltersVolume vol) ? vol : null;
      this.enabled = VintageFilters.IsInRenderFeatures() && volume != null;
    }

    private void Start() => ResetEffect();

    private void OnGUI()
    {
      if (volume == null)
        return;

      styleTitle = new GUIStyle(GUI.skin.label)
      {
        alignment = TextAnchor.LowerCenter,
        fontSize = 32,
        fontStyle = FontStyle.Bold
      };

      styleLabel = new GUIStyle(GUI.skin.label)
      {
        alignment = TextAnchor.UpperLeft,
        fontSize = 24
      };

      styleButton = new GUIStyle(GUI.skin.button)
      {
        fontSize = 24
      };

      GUILayout.BeginHorizontal();
      {
        GUILayout.BeginVertical("box", GUILayout.Width(600.0f), GUILayout.Height(Screen.height));
        {
          const float space = 10.0f;

          GUILayout.Space(space);

          GUILayout.Label(Constants.Asset.Name.ToUpper(), styleTitle);

          volume.intensity.value = SliderField("Intensity", volume.intensity.value, 0.0f, 1.0f);

          GUILayout.Space(space);

          volume.filter.value = EnumField("Filter", volume.filter.value);

          switch (volume.filter.value)
          {
            case Filters.Amaro:
              volume.amaroOverlay.value = SliderField("Overlay", volume.amaroOverlay.value);
              break;
            case Filters.Hefe:
              volume.hefeEdgeBurn.value = SliderField("Edge burn", volume.hefeEdgeBurn.value);
              volume.hefeGradient.value = SliderField("Gradient", volume.hefeGradient.value);
              volume.hefeSoftLight.value = SliderField("Soft light", volume.hefeSoftLight.value);
              break;
            case Filters.Hudson:
              volume.hudsonOverlay.value = SliderField("Overlay", volume.hudsonOverlay.value);
              break;
            case Filters.Rise:
              volume.riseOverlay.value = SliderField("Overlay", volume.riseOverlay.value);
              break;
            case Filters.Sierra:
              volume.sierraOverlay.value = SliderField("Overlay", volume.sierraOverlay.value);
              break;
            case Filters.Toaster:
              volume.toasterOverlayWarm.value = SliderField("Overlay warm", volume.toasterOverlayWarm.value);
              break;
            case Filters.Sepia:
              volume.sepiaIntensity.value = SliderField("Intensity", volume.sepiaIntensity.value);
              break;
            case Filters.Kodachrome:
              volume.kodachromeEnhancement.value = SliderField("Enhancement", volume.kodachromeEnhancement.value);
              volume.kodachromeWarmth.value = SliderField("Warmth", volume.kodachromeWarmth.value);
              break;
            case Filters.Polaroid:
              volume.polaroidOverexposure.value = SliderField("Overexposure", volume.polaroidOverexposure.value);
              volume.polaroidSoftness.value = SliderField("Softness", volume.polaroidSoftness.value);
              break;
            case Filters.CrossProcess:
              volume.crossProcessColorShift.value = SliderField("Color shift", volume.crossProcessColorShift.value);
              volume.crossProcessContrastBoost.value = SliderField("Contrast boost", volume.crossProcessContrastBoost.value);
              break;
            case Filters.BleachBypass:
              volume.bleachBypassDesaturation.value = SliderField("Desaturation", volume.bleachBypassDesaturation.value);
              volume.bleachBypassContrast.value = SliderField("Contrast", volume.bleachBypassContrast.value);
              break;
            case Filters.Vintage80s:
              volume.vintage80sNeonIntensity.value = SliderField("Neon intensity", volume.vintage80sNeonIntensity.value);
              volume.vintage80sColorPop.value = SliderField("Color pop", volume.vintage80sColorPop.value);
              break;
            case Filters.FilmGrain:
              volume.filmGrainIntensity.value = SliderField("Intensity", volume.filmGrainIntensity.value);
              volume.filmGrainSize.value = SliderField("Size", volume.filmGrainSize.value, 0.5f, 2.0f);
              break;
            case Filters.Technicolor:
              volume.technicolorSaturation.value = SliderField("Saturation", volume.technicolorSaturation.value, 0.0f, 2.0f);
              volume.technicolorColorBalance.value = SliderField("Color balance", volume.technicolorColorBalance.value);
              break;
            case Filters.Daguerreotype:
              volume.daguerreotypeContrast.value = SliderField("Contrast", volume.daguerreotypeContrast.value);
              volume.daguerreotypeSilvering.value = SliderField("Silvering", volume.daguerreotypeSilvering.value);
              break;
            case Filters.Cyanotype:
              volume.cyanotypeBlueIntensity.value = SliderField("Blue intensity", volume.cyanotypeBlueIntensity.value);
              break;
            case Filters.Western:
              volume.westernWarmth.value = SliderField("Warmth", volume.westernWarmth.value);
              volume.westernDust.value = SliderField("Dust", volume.westernDust.value);
              break;
            case Filters.NightVision:
              volume.nightVisionGain.value = SliderField("Gain", volume.nightVisionGain.value, 0.0f, 2.0f);
              volume.nightVisionNoise.value = SliderField("Noise", volume.nightVisionNoise.value);
              break;
            case Filters.Infrared:
              volume.infraredFoliage.value = SliderField("Foliage", volume.infraredFoliage.value);
              volume.infraredBloom.value = SliderField("Bloom", volume.infraredBloom.value);
              break;
          }

          GUILayout.FlexibleSpace();

          if (GUILayout.Button("RESET", styleButton) == true)
            ResetEffect();

          GUILayout.Space(4.0f);

          if (GUILayout.Button("ONLINE DOCUMENTATION", styleButton) == true)
            Application.OpenURL(Constants.Support.Documentation);

          GUILayout.Space(4.0f);

          if (GUILayout.Button("❤️ LEAVE A REVIEW ❤️", styleButton) == true)
            Application.OpenURL(Constants.Support.Store);

          GUILayout.Space(space * 2.0f);
        }
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
      }
      GUILayout.EndHorizontal();
    }

    private void OnDestroy()
    {
      volume?.Reset();
    }

    private bool ToggleField(string label, bool value)
    {
      GUILayout.BeginHorizontal();
      {
        GUILayout.Label(label, styleLabel);

        value = GUILayout.Toggle(value, string.Empty);
      }
      GUILayout.EndHorizontal();

      return value;
    }

    private float SliderField(string label, float value, float min = 0.0f, float max = 1.0f)
    {
      GUILayout.BeginHorizontal();
      {
        GUILayout.Label(label, styleLabel);

        value = GUILayout.HorizontalSlider(value, min, max);
      }
      GUILayout.EndHorizontal();

      return value;
    }

    private int SliderField(string label, int value, int min, int max = 1)
    {
      GUILayout.BeginHorizontal();
      {
        GUILayout.Label(label, styleLabel);

        value = (int)GUILayout.HorizontalSlider(value, min, max);
      }
      GUILayout.EndHorizontal();

      return value;
    }

    private T EnumField<T>(string label, T value) where T : Enum
    {
      string[] names = Enum.GetNames(typeof(T));
      Array values = Enum.GetValues(typeof(T));
      int index = Array.IndexOf(values, value);

      GUILayout.BeginHorizontal();
      {
        GUILayout.Label(label, styleLabel);

        if (GUILayout.Button("<", styleButton) == true)
          index = index > 0 ? index - 1 : values.Length - 1;

        GUILayout.Label(names[index].Replace("_", ""), styleLabel);

        if (GUILayout.Button(">", styleButton) == true)
          index = index < values.Length - 1 ? index + 1 : 0;
      }
      GUILayout.EndHorizontal();

      return (T)values.GetValue(index);
    }
  }
}
