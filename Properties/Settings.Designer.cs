﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.2034
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SpchListBuilder.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>aaaaaaaaaaaaaa</string>
  <string>bbbbbbbbbbbbbb</string>
  <string>cccccccccccccccc</string>
  <string>dddddddddddddddd</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection VCSUrlStore {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["VCSUrlStore"]));
            }
            set {
                this["VCSUrlStore"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("600")]
        public int WinHeight {
            get {
                return ((int)(this["WinHeight"]));
            }
            set {
                this["WinHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("560")]
        public int WinWidth {
            get {
                return ((int)(this["WinWidth"]));
            }
            set {
                this["WinWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int WinLeft {
            get {
                return ((int)(this["WinLeft"]));
            }
            set {
                this["WinLeft"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int WinTop {
            get {
                return ((int)(this["WinTop"]));
            }
            set {
                this["WinTop"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>C:\TortoiseSVN\bin\svn.exe</string>
  <string>C:\TortoiseGIT\bin\git.exe</string>
  <string />
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection VCSBinPath {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["VCSBinPath"]));
            }
            set {
                this["VCSBinPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int VCSBinSelect {
            get {
                return ((int)(this["VCSBinSelect"]));
            }
            set {
                this["VCSBinSelect"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>SVN</string>
  <string>GIT</string>
  <string>HG</string>
  <string>NO!</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection VCSBinText {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["VCSBinText"]));
            }
            set {
                this["VCSBinText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://github.com/ClnViewer/SpchListBuilder")]
        public string GitSPCHLBHome {
            get {
                return ((string)(this["GitSPCHLBHome"]));
            }
            set {
                this["GitSPCHLBHome"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool TreeViewCollapse {
            get {
                return ((bool)(this["TreeViewCollapse"]));
            }
            set {
                this["TreeViewCollapse"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("spch-default.slist")]
        public string VCSOutListFileName {
            get {
                return ((string)(this["VCSOutListFileName"]));
            }
            set {
                this["VCSOutListFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://github.com/ClnViewer/Split-post-commit-Hook---SVN-GIT-HG")]
        public string GitSPCHHome {
            get {
                return ((string)(this["GitSPCHHome"]));
            }
            set {
                this["GitSPCHHome"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"SPLIT POST COMMIT HOOK allows you to create and maintain a new SVN / GIT / HG repository based on the file list.
SpchListBuilder is an application for creating a list of files for the SPCH based on the repository.
More information is available on the links below:")]
        public string AboutText {
            get {
                return ((string)(this["AboutText"]));
            }
            set {
                this["AboutText"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>svn.exe</string>
  <string>git.exe</string>
  <string>hg.exe</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection VCSBinExe {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["VCSBinExe"]));
            }
        }
    }
}
