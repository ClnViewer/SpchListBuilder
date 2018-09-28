/*
    MIT License

    Copyright (c) 2018 PS
    GitHub SPCH: https://github.com/ClnViewer/Split-post-commit-Hook---SVN-GIT-HG
    GitHub SpchListBuilder: https://github.com/ClnViewer/SpchListBuilder

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sub license, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 */

#if(SVNCHECK)
#   if(!DEBUG)    
     
    #error SVN - Working copy has multiple revisions, please update to the latest revision before creating a release build. 
#   endif
#endif

using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using SpchListBuilder.Properties;

[assembly: AssemblyTitle("SpchListBuilder")]
[assembly: AssemblyDescription("SPLIT POST COMMIT HOOK allows you to create and maintain a new SVN/GIT/HG repository based on the file list.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("PS")]
[assembly: AssemblyProduct("SpchListBuilder")]
[assembly: AssemblyCopyright("Copyright © PS 2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]
[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly
)]
[assembly: AssemblyVersion("1.0.117.*")]
[assembly: AssemblyFileVersion("1.0.117.0")]
[assembly: AssemblyInformationalVersion("Build date: 2018-09-28 17:34:58; Revision date: 2018-09-28 17:18:25; Revision(s) in working copy: 106:117.")]
