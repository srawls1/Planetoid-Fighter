using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class BloomModel : PostProcessingModel
    {
        [Serializable]
        public struct BloomSettings
        {
            [Min(0f), Tooltip("Strength of the bloom filter.")]
            public float intensity;

            [Min(0f), Tooltip("Filters out pixels under this level of brightness.")]
            public float threshold;

            public float thresholdLinear
            {
                set { threshold = Mathf.LinearToGammaSpace(value); }
                get { return Mathf.GammaToLinearSpace(threshold); }
            }

            [Range(0f, 1f), Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
            public float softKnee;

            [Range(1f, 7f), Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
            public float radius;

            [Tooltip("Reduces flashing noise with an additional filter.")]
            public bool antiFlicker;

            public static BloomSettings Lerp(BloomSettings s1, BloomSettings s2, float t)
            {
                BloomSettings settings = new BloomSettings();
                settings.intensity = Mathf.Lerp(s1.intensity, s2.intensity, t);
                settings.threshold = Mathf.Lerp(s1.threshold, s2.threshold, t);
                settings.softKnee = Mathf.Lerp(s1.softKnee, s2.softKnee, t);
                settings.radius = Mathf.Lerp(s1.radius, s2.radius, t);
                settings.antiFlicker = s1.antiFlicker || s2.antiFlicker;
                return settings;
            }

            public static BloomSettings defaultSettings
            {
                get
                {
                    return new BloomSettings
                    {
                        intensity = 0.5f,
                        threshold = 1.1f,
                        softKnee = 0.5f,
                        radius = 4f,
                        antiFlicker = false,
                    };
                }
            }
        }

        [Serializable]
        public struct LensDirtSettings
        {
            [Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
            public Texture texture;

            [Min(0f), Tooltip("Amount of lens dirtiness.")]
            public float intensity;

            public static LensDirtSettings Lerp(LensDirtSettings s1, LensDirtSettings s2, float t)
            {
                LensDirtSettings settings = new LensDirtSettings();
                settings.intensity = Mathf.Lerp(s1.intensity, s2.intensity, t);
                settings.texture = s1.texture;
                return settings;
            }

            public static LensDirtSettings defaultSettings
            {
                get
                {
                    return new LensDirtSettings
                    {
                        texture = null,
                        intensity = 3f
                    };
                }
            }
        }

        [Serializable]
        public struct Settings
        {
            public BloomSettings bloom;
            public LensDirtSettings lensDirt;

            public static Settings Lerp(Settings s1, Settings s2, float t)
            {
                Settings settings = new Settings();
                settings.bloom = BloomSettings.Lerp(s1.bloom, s2.bloom, t);
                settings.lensDirt = LensDirtSettings.Lerp(s1.lensDirt, s2.lensDirt, t);
                return settings;
            }

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        bloom = BloomSettings.defaultSettings,
                        lensDirt = LensDirtSettings.defaultSettings
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

        public void Lerp(BloomModel m1, BloomModel m2, float t)
        {
            settings = Settings.Lerp(m1.settings, m2.settings, t);
        }

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }
    }
}
