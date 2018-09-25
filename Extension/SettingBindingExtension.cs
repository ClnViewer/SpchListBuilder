using System.Windows.Data;

namespace SpchListBuilder
{
    public class SettingBindingExtension : Binding
    {
        public SettingBindingExtension()
        {
            Initialize();
        }
 
        public SettingBindingExtension(string path)
            :base(path)
        {
            Initialize();
        }
 
        private void Initialize()
        {
            this.Source = SpchListBuilder.Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }
    }
}
