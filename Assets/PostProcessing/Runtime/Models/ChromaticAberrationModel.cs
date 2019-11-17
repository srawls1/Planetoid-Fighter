using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class ChromaticAberrationModel : PostProcessingModel
    {
        [Serializable]
        public struct Settings
        {
            [Tooltip("Shift the hue of chromatic aberrations.")]
            public Texture2D spectralTexture;

            [Range(0f, 1f), Tooltip("Amount of tangential distortion.")]
            public float intensity;

            public static Settings Lerp(Settings s1, Settings s2, float t)
            {
                Settings settings = new Settings();
                settings.intensity = Mathf.Lerp(s1.intensity, s2.intensity, t);
                settings.spectralTexture = s1.spectralTexture;
                return settings;
            }

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        spectralTexture = null,
                        intensity = 0.1f
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

                public void Lerp(ChromaticAberrationModel m1, ChromaticAberrationModel m2, float t)
                {
                    settings = Settings.Lerp(m1.settings, m2.settings, t);
                }

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }
    }
}
