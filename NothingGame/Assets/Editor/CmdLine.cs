#if UNITY_STANDALONE_WIN

using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;
using System.Runtime.InteropServices;

namespace Jenkins {

    public class CmdLine : MonoBehaviour 
    {

        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern System.IntPtr FindWindow(System.String className, System.String windowName);

        static string cmdInfo = "";                     // holds all command line args passed in
        private string programName = "NothingGame.exe"; // hardcoded here, but can also be passed in with -appname argument

        static BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
     
        void Start () 
        {
            string[] arguments = Environment.GetCommandLineArgs();
            foreach(string arg in arguments)
            {
                cmdInfo += arg.ToString() + "\n ";
            }
        }

        public static void ChangeTitle(string newTitle) {
            var windowPtr = FindWindow(null, this.programName);
            SetWindowText(windowPtr, newTitle);
        }

        public static void parseCommandLineArgs() {
            string[] args = System.Environment.GetCommandLineArgs();
            int i = 0;
            
            foreach (string arg in args)
            {
                if (i > 0 && arg == "Release") {
                    buildPlayerOptions.options = BuildOptions.None;
                }
                if (i > 0 && arg == "Debug") {
                    buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.EnableCodeCoverage | BuildOptions.EnableDeepProfilingSupport;
                    PlayerSettings.productName = PlayerSettings.productName + "_Debug";
                    programName = "_Debug.exe";
                    ChangeTitle(programName);

                    EditorUserBuildSettings.development = true;
                    EditorUserBuildSettings.allowDebugging = true;
                    EditorUserBuildSettings.connectProfiler = true;
                }
                if (i > 0 && arg == "-appname") {
                    programName = args[i+1];
                }
                i++;            
            }
            BuildReport report = BuildPipeline.BuildPlayer(EnabledLevels(), programName, BuildTarget.StandaloneWindows, BuildOptions.Development);
            BuildSummary summary = report.summary;

            if(summary.result == BuildResult.Succeeded) {
                Debug.Log("Build Succeeded: " + summary.totalSize + " bytes");
            }

            if(summary.result == BuildResult.Failed) {
                Debug.Log("Build Failed");
            }
        }
    }

    private static string[] EnabledLevels()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }
}

#endif
