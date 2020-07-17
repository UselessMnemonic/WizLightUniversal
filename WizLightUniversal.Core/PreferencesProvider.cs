namespace WizLightUniversal.Core
{
    public abstract class PreferencesProvider
    {
        public static PreferencesProvider Default { get; set; }

        public abstract int HomeID { get; set; }
    }
}