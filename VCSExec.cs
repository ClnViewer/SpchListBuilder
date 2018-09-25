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

using SpchListBuilder.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using SpchListBuilder.Pages;
using SpchListBuilder.Data;

namespace SpchListBuilder
{
    public class VCSExec
    {
        private class VCSExecData
        {
            public string CmdStat;
            public string CmdList;
            public List<String> CmdExport;
        }

        private string __exepath = String.Empty;
        private string __impfile = String.Empty;
        private VCSDataRepo __repo { get; set; }

        private List<VCSExecData> __cmdLine = new List<VCSExecData>
        {
            new VCSExecData
            {
                CmdStat = "stat -v --xml",                                 //MLHIDE
                CmdList = "list {0} -R --xml",                             //MLHIDE
                CmdExport = new List<String>
                {
                    {"add {0}"},                                           //MLHIDE
                    {"import -m \"add spch list: {0}\" \"{1}\" {2}{3}{0}"} //MLHIDE
                }

            },
            new VCSExecData
            {
                CmdStat = "",                                              //MLHIDE
                CmdList = "",                                              //MLHIDE
                CmdExport = new List<String>
                {
                    {"add {0}"},                                           //MLHIDE
                    {"import -m \"add spch list: {0}\" \"{1}\" {2}{3}{0}"} //MLHIDE
                }

            },
            new VCSExecData
            {
                CmdStat = "",                                              //MLHIDE
                CmdList = "",                                              //MLHIDE
                CmdExport = new List<String>
                {
                    {"add {0}"},                                           //MLHIDE
                    {"import -m \"add spch list: {0}\" \"{1}\" {2}{3}{0}"} //MLHIDE
                }

            }
        };

        public string ExecPath
        {
            get { return __exepath; }
            set { __SetExecPath(value); }
        }

        private void __SetExecPath(string exepath)
        {
            __exepath = ((String.IsNullOrWhiteSpace(exepath)) ? __exepath : exepath);

            if (String.IsNullOrWhiteSpace(__exepath))
                throw new VCSDataException(Resources.input_exec_path_is_empty_abort);
            if (!File.Exists(__exepath))
                throw new VCSDataException(Resources.exec_file_not_found + exepath);
        }

        public VCSExec() { }

        public VCSExec(string exepath, VCSDataRepo repo, string importfile = null)
        {
            __SetExecPath(exepath);
            __impfile = importfile;
            __repo = repo;
        }

        public string Exec(VCSDataRepo.EnumTypeRequest treq)
        {
            try
            {
                if ((__repo == null) || (!__repo.DataOK))
                    throw new VCSDataException(Properties.Resources.Exec_Setup_data_is_not_OK);

                return __Exec(treq);
            }
            catch (Exception e)
            {
                throw new VCSDataException(e.Message);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Ликвидировать объекты перед потерей области"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        private Process __PreExec(VCSDataRepo.EnumTypeRequest treq)
        {
            string __args;

            switch (treq)
            {
                case VCSDataRepo.EnumTypeRequest.__VCS_STAT:
                    {
                        __args = __cmdLine[(int)__repo.DataProvider].CmdStat;
                        break;
                    }
                case VCSDataRepo.EnumTypeRequest.__VCS_LIST:
                    {
                        __args = String.Format(
                            __cmdLine[(int)__repo.DataProvider].CmdList,
                            __repo.UriOrigin
                        );
                        break;
                    }
                case VCSDataRepo.EnumTypeRequest.__VCS_IMPORT:
                    {
                        switch (__repo.TypeRequest)
                        {
                            case VCSDataRepo.EnumTypeRequest.__VCS_LIST:
                                {
                                    if (
                                        (!String.IsNullOrWhiteSpace(__repo.UriDirectory)) ||
                                        (String.IsNullOrWhiteSpace(__impfile)) ||
                                        (!File.Exists(__impfile))
                                       )
                                        throw new VCSDataException(
                                            String.Format(
                                                Properties.Resources.Exec_0_EXPORT_Setup_data_not_v,
                                                __repo.TypeRequest.ToString()
                                            )
                                        );

                                    __args = String.Format(
                                        __cmdLine[(int)__repo.DataProvider].CmdExport[(int)__repo.TypeRequest],
                                        Properties.Settings.Default.VCSOutListFileName,
                                        __impfile,
                                        __repo.UriOrigin,
                                        __repo.UriSeparator
                                    );
                                    break;
                                }
                            case VCSDataRepo.EnumTypeRequest.__VCS_STAT:
                                {
                                    if (
                                        (String.IsNullOrWhiteSpace(__repo.UriDirectory)) ||
                                        (String.IsNullOrWhiteSpace(__impfile)) ||
                                        (!File.Exists(__impfile))
                                       )
                                        throw new VCSDataException(
                                            String.Format(
                                                Properties.Resources.Exec_0_EXPORT_Setup_data_not_v,
                                                __repo.TypeRequest.ToString()
                                            )
                                        );

                                    __args = String.Format(
                                        __cmdLine[(int)__repo.DataProvider].CmdExport[(int)__repo.TypeRequest],
                                        Properties.Settings.Default.VCSOutListFileName
                                    );
                                    break;
                                }
                            default:
                                {
                                    throw new VCSDataException(
                                        String.Format(
                                            Properties.Resources.Exec_0_Setup_data_not_valid_la,
                                            treq.ToString(),
                                            1
                                        )
                                    );
                                }
                        }
                        break;
                    }
                default:
                    {
                        throw new VCSDataException(
                            String.Format(
                                Properties.Resources.Exec_0_Setup_data_not_valid_la,
                                treq.ToString(),
                                2
                            )
                        );
                    }
            }
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = __exepath,
                WorkingDirectory = ((String.IsNullOrWhiteSpace(__repo.UriDirectory)) ?
                    String.Empty : __repo.UriDirectory
                ),
                Arguments = __args,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                //StandardOutputEncoding = Settings.Encoding.StandardOutput,
                //StandardErrorEncoding = Settings.Encoding.StandardError,
                UseShellExecute = false
            };
            //startInfo.SetEnvironmentVariables(input.EnvironmentVariables);
            return new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };
        }

        private string __Exec(VCSDataRepo.EnumTypeRequest treq)
        {
            try
            {
                Process exec = null;

                using (exec = __PreExec(treq))
                {
                    if (exec == null)
                        throw new ArgumentNullException();

                    exec.Start();

                    using (StreamReader sr = exec.StandardOutput)
                    {
                        string __out = sr.ReadToEnd();
                        exec.WaitForExit();
                        return __out;
                    }
                }
            }
            catch (Exception e)
            {
                throw new VCSDataException(e.Message);
            }
        }
    }
}
