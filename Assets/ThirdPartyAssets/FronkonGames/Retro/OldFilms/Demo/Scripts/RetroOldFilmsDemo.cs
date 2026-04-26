using System;
using UnityEngine;
using UnityEngine.Rendering;
using FronkonGames.Retro.OldFilms;

/// <summary> Retro: Old Films demo. </summary>
/// <remarks>
/// This code is designed for a simple demo, not for production environments.
/// </remarks>
public class RetroOldFilmsDemo : MonoBehaviour
{
  [Header("This code is only for the demo, not for production environments."), Space(20.0f)]

  [SerializeField]
  private VolumeProfile volumeProfile;

  [Space]

  [SerializeField]
  private Transform floor;

  [SerializeField, Range(0.0f, 10.0f)]
  private float angularVelocity;

  private OldFilmsVolume volume;

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

    if (OldFilms.IsInRenderFeatures() == false)
    {
      Debug.LogWarning($"Effect '{Constants.Asset.Name}' not found. You must add it as a Render Feature.");
#if UNITY_EDITOR
      if (UnityEditor.EditorUtility.DisplayDialog($"Effect '{Constants.Asset.Name}' not found", $"You must add '{Constants.Asset.Name}' as a Render Feature.", "Quit") == true)
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    volume = volumeProfile != null && volumeProfile.TryGet(out OldFilmsVolume vol) ? vol : null;
    this.enabled = OldFilms.IsInRenderFeatures() && volume != null;
  }

  private void Start() => ResetEffect();

  private void Update()
  {
    if (floor != null && angularVelocity > 0.0f)
      floor.rotation = Quaternion.Euler(0.0f, floor.rotation.eulerAngles.y + Time.deltaTime * angularVelocity * 10.0f, 0.0f);
  }

  private void OnGUI()
  {
    if (volume == null)
      return;

    styleTitle ??= new GUIStyle(GUI.skin.label)
    {
      alignment = TextAnchor.LowerCenter,
      fontSize = 32,
      fontStyle = FontStyle.Bold
    };

    styleLabel ??= new GUIStyle(GUI.skin.label)
    {
      alignment = TextAnchor.UpperLeft,
      fontSize = 24
    };

    styleButton ??= new GUIStyle(GUI.skin.button)
    {
      fontSize = 24
    };

    GUILayout.BeginHorizontal();
    {
      GUILayout.BeginVertical("box", GUILayout.Width(500.0f), GUILayout.Height(Screen.height));
      {
        const float space = 10.0f;

        GUILayout.Space(space);
        GUILayout.Label(Constants.Asset.Name.ToUpper(), styleTitle);
        GUILayout.Space(space);

        volume.intensity.value = SliderField("Intensity", volume.intensity.value);

        volume.manufacturer.value = EnumField("Manufacturer", volume.manufacturer.value);
        volume.vignette.value = SliderField("Vignette", volume.vignette.value, 0.0f, 2.0f);
        volume.sepia.value = SliderField("Sepia", volume.sepia.value, 0.0f, 1.0f);
        volume.grain.value = SliderField("Grain", volume.grain.value, 0.0f, 1.0f);
        volume.moveFrame.value = Vector2.one * SliderField("Move frame", volume.moveFrame.value.x, 0.0f, 1.0f);
        volume.jumpFrame.value = SliderField("Jump frame", volume.jumpFrame.value, 0.0f, 1.0f);
        volume.blinkStrength.value = SliderField("Blink", volume.blinkStrength.value, 0.0f, 1.0f);
        volume.blotches.value = SliderField("Blotches", volume.blotches.value, 0, 6);
        volume.scratches.value = SliderField("Scratches", volume.scratches.value, 0.0f, 1.0f);
        volume.lines.value = SliderField("Lines", volume.lines.value, 0, 8);
        
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("RESET", styleButton) == true)
          ResetEffect();

        GUILayout.Space(4.0f);

        if (GUILayout.Button("ONLINE DOCUMENTATION", styleButton) == true)
          Application.OpenURL(Constants.Support.Documentation);

        GUILayout.Space(4.0f);

        if (GUILayout.Button("❤️ WRITE A REVIEW ❤️", styleButton) == true)
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

  private int SliderField(string label, int value, int min, int max)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      value = (int)GUILayout.HorizontalSlider(value, min, max);
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private Color ColorField(string label, Color value, bool alpha = true)
  {
    GUILayout.BeginHorizontal();
    {
      GUILayout.Label(label, styleLabel);

      float originalAlpha = value.a;

      Color.RGBToHSV(value, out float h, out float s, out float v);
      h = GUILayout.HorizontalSlider(h, 0.0f, 1.0f);
      value = Color.HSVToRGB(h, s, v);

      if (alpha == false)
        value.a = originalAlpha;
    }
    GUILayout.EndHorizontal();

    return value;
  }

  private Vector2 Vector2Field(string label, Vector2 value, float min, float max) => Vector2Field(label, value, "X", "Y", min, max);

  private Vector2 Vector2Field(string label, Vector2 value, string x = "X", string y = "Y", float min = 0.0f, float max = 1.0f)
  {
    GUILayout.Label(label, styleLabel);

    value.x = SliderField($"   {x}", value.x, min, max);
    value.y = SliderField($"   {y}", value.y, min, max);

    return value;
  }

  private Vector3 Vector3Field(string label, Vector3 value, float min, float max) => Vector3Field(label, value, "X", "Y", "Z", min, max);

  private Vector3 Vector3Field(string label, Vector3 value, string x = "X", string y = "Y", string z = "Z", float min = 0.0f, float max = 1.0f)
  {
    GUILayout.Label(label, styleLabel);

    value.x = SliderField($"   {x}", value.x, min, max);
    value.y = SliderField($"   {y}", value.y, min, max);
    value.z = SliderField($"   {z}", value.z, min, max);

    return value;
  }

  private Vector4 Vector4Field(string label, Vector4 value, float min, float max) => Vector4Field(label, value, "X", "Y", "Z", "W", min, max);

  private Vector4 Vector4Field(string label, Vector4 value, string x = "X", string y = "Y", string z = "Z", string w = "W", float min = 0.0f, float max = 1.0f)
  {
    GUILayout.Label(label, styleLabel);

    value.x = SliderField($"   {x}", value.x, min, max);
    value.y = SliderField($"   {y}", value.y, min, max);
    value.z = SliderField($"   {z}", value.z, min, max);
    value.w = SliderField($"   {w}", value.w, min, max);

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

      GUILayout.Label(names[index], styleLabel);

      if (GUILayout.Button(">", styleButton) == true)
        index = index < values.Length - 1 ? index + 1 : 0;
    }
    GUILayout.EndHorizontal();

    return (T)(object)index;
  }
}
