using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class VignetteModel : PostProcessingModel
    {
        public enum Mode
        {
            Classic,
            Masked
        }

        [Serializable]
        public struct Settings
        {
            [Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
            public Mode mode;

            [ColorUsage(false)]
            [Tooltip("Vignette color. Use the alpha channel for transparency.")]
            public Color color;

            [Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
            public Vector2 center;

            [Range(0f, 1f), Tooltip("Amount of vignetting on screen.")]
            public float intensity;

            [Range(0.01f, 1f), Tooltip("Smoothness of the vignette borders.")]
            public float smoothness;

            [Range(0f, 1f), Tooltip("Lower values will make a square-ish vignette.")]
            public float roundness;

            [Tooltip("A black and white mask to use as a vignette.")]
            public Texture mask;

            [Range(0f, 1f), Tooltip("Mask opacity.")]
            public float opacity;

            [Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
            public bool rounded;

            public static Settings Lerp(Settings s1, Settings s2, float t)
            {
                Settings settings = new Settings();
                settings.color = Color.Lerp(s1.color, s2.color, t);
                settings.center = Vector2.Lerp(s1.center, s2.center, t);
                settings.intensity = Mathf.Lerp(s1.intensity, s2.intensity, t);
                settings.smoothness = Mathf.Lerp(s1.smoothness, s2.smoothness, t);
                settings.roundness = Mathf.Lerp(s1.roundness, s2.roundness, t);
                settings.opacity = Mathf.Lerp(s1.opacity, s2.opacity, t);
                settings.rounded = s1.rounded || s2.rounded;
                return settings;
            }

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        mode = Mode.Classic,
                        color = new Color(0f, 0f, 0f, 1f),
                        center = new Vector2(0.5f, 0.5f),
                        intensity = 0.45f,
                        smoothness = 0.2f,
                        roundness = 1f,
                        mask = null,
                        opacity = 1f,
                        rounded = false
                    };
                }
            }
        }

        [SerializeField]
        Settings m_Settings = Settings.defaultSettings;
        public Settings settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        public void Lerp(VignetteModel m1, VignetteModel m2, float t)
        {
            settings = Settings.Lerp(m1.settings, m2.settings, t);
        }

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }
    }
}
