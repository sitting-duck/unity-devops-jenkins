#if UNITY_STANDALONE_WIN

using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

/* Works with Unity version 2021.16f1 */

namespace Jenkins {

    public class CmdLine : MonoBehaviour 
    {

        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern System.IntPtr FindWindow(System.String className, System.String windowName);        

        static BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();        

        public static void parseCommandLineArgs() {
            string programName = "NothingGame"; // hardcoded here, but can also be passed in with -appname argument

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
            BuildReport report = BuildPipeline.BuildPlayer(EnabledLevels(), programName, BuildTarget.StandaloneWindows, buildPlayerOptions.options);
            //BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if(summary.result == BuildResult.Succeeded) {
                Debug.Log("Build Succeeded: " + summary.totalSize + " bytes");
            }

            if(summary.result == BuildResult.Failed) {
                Debug.Log("Build Failed");
            }
        }

        // Changes the title bar, often used to visually indicate a game is a debug build
        public static void ChangeTitle(string newTitle) {
            var windowPtr = FindWindow(null, newTitle);
            SetWindowText(windowPtr, newTitle);            
        }    

        private static string[] EnabledLevels() {
            List<string> scenes = new List<string>();
            foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
                if(scene.enabled) {
                    scenes.Add(scene.path);
                }
            }
            return scenes.ToArray();
        }
    
    }
}

#endif
