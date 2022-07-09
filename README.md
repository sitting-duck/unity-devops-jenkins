# unity-devops-jenkins
Jenkins Devops Pipelines for Unity Games <br>

Unity Version: 2021.1.16f1 <br>
Jenkins Version: 2.289.3 <br>

### NothingGame
NothingGame is a Unity Game with nothing in it. The game that does nothing! I just use it as a placeholder in my devops pipeline for Unity Games. 

# Steps
## Step 0: Install DotNet SDK 
Install the latest version of DotNet SDK, as of now, that version is 6.0. <br>

Go to the download page [2] and click the “Download .NET SDK x64” button. <br>
<img width="349" alt="image" src="https://user-images.githubusercontent.com/1289702/178121062-7f922cba-4169-4bf5-b676-8bb01acd8407.png">

Double click the installer in your Downloads folder and run it. <br>
<img width="162" alt="image" src="https://user-images.githubusercontent.com/1289702/178121070-078c3f84-89ad-448d-b89f-ad4bd282530f.png">

<img width="325" alt="image" src="https://user-images.githubusercontent.com/1289702/178121074-f8e4428d-5f35-4fc8-82dd-a85e2cf43caf.png">

<img width="324" alt="image" src="https://user-images.githubusercontent.com/1289702/178121079-ca51f5e4-67c9-45ef-8f4f-ed1cbf47c850.png">

<img width="324" alt="image" src="https://user-images.githubusercontent.com/1289702/178121082-222c3780-94e6-4937-ad51-748a4c09bb07.png">

If your install was successful, you should see this helpful text on the final page: <br>
``` bash
The installation was successful.
The following products were installed at: 'C:\Program Files\dotnet'
• .NET SDK 6.0.201
• .NET Runtime 6.0.3
• ASP.NET Core Runtime 6.0.3
• .NET Windows Desktop Runtime 6.0.3
This product collects usage data
• More information and opt-out https://aka.ms/dotnet-cli-telemetry
Resources
• .NET Documentation https://aka.ms/dotnet-docs
• SDK Documentation https://aka.ms/dotnet-sdk-docs
• Release Notes https://aka.ms/dotnet6-release-notes
• Tutorials https://aka.ms/dotnet-tutorials
```
Check your command line and make sure you can call dotnet. <br>

Enter <br>
```batch
% dotnet --version
```
and you should get some output. Mine says: <br>
```batch
6.0.201
```
If you can’t access dotnet from the command line add: <br>
``` batch
C:\Program Files\dotnet
```
to your System Path. <br>

## Step 1: Add DotNet bin dir to Your System Path
Search for “Environment Variables” in Windows search and select “Edit the system environment variables” <br>
<img width="347" alt="image" src="https://user-images.githubusercontent.com/1289702/178121153-0d42082b-653f-4908-9996-0b6ecfc59e6c.png">

Click the Environment Variables button in the System Properties panel <br>
<img width="207" alt="image" src="https://user-images.githubusercontent.com/1289702/178121162-e6aaa1a9-8b77-44d5-b4e4-2f42cac4f1e1.png">

In the System Variables table (the bottom one), select the “Path” variable and click the “Edit …” button <br>
<img width="293" alt="image" src="https://user-images.githubusercontent.com/1289702/178121185-c887696b-3644-4850-b619-735635ac3c5a.png">

Click the New button to add C:\Program Files\dotnet to the end of the System Path <br>
<img width="252" alt="image" src="https://user-images.githubusercontent.com/1289702/178121194-30d46632-e952-4820-81d1-11180b45ff30.png">

Refresh your terminal after adding the path to dotnet.exe to the system path and see if you can call <br>
```batch
dotnet --version
```

You will need the dotnet command line interface in the next step. <br>

## Step 2: Create System Environment Variables
Create two system environment variables. <br>

Variable 1: the path to Unity. <br>

<img width="314" alt="image" src="https://user-images.githubusercontent.com/1289702/178121268-6d09eee7-f009-4cb8-a0d8-0e19ff1e4857.png">

Search for Environment in the Windows search bar <br>

<img width="277" alt="image" src="https://user-images.githubusercontent.com/1289702/178121272-8dd8642b-9846-4489-94fe-1f555fcf15cb.png">

Click Environment Variables button <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178121276-23a8ccbb-b960-485d-8b6e-5377877009f9.png">

Select the New … button under System Variables <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178121283-456eee3a-63ea-49c9-b3df-1e96ad47dabd.png">

Give it a meaningful name and paste in the path to the Unity version you are using to build your game for the value. The reason why you are doing is so each build server can set it's own environment variable for the path to Unity, making this Jenkins file reusable.
If your new system variable isn't getting picked up by Jenkins you may just need to restart Jenkins. You can do so by going to http://localhost:8080/restart in your browser. <br>

<img width="291" alt="image" src="https://user-images.githubusercontent.com/1289702/178121293-5be685b9-0313-4d5b-9cef-483a25694eaf.png">

Variable 2: The Path to DotNet <br>
<img width="312" alt="image" src="https://user-images.githubusercontent.com/1289702/178121304-28e60278-f0eb-4ba8-be15-b0cc38524519.png">

Click the New… button again to create another System Variable. <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178121309-3a005a33-b959-46c5-a164-bc5d7f2e120e.png">

Give it a meaningful name and paste the path to dotnet.exe for the version of dotnet that you are using for your game in Unity. <br>

You can see what dotnet version you are using in your Unity Game by opening it in Unity and selecting File-> Build Settings … <br>

<img width="286" alt="image" src="https://user-images.githubusercontent.com/1289702/178121314-887ccb9c-13ff-4b98-a8fc-44af8d6be085.png">

and in the Build Settings window select the Player Settings button. <br>

<img width="290" alt="image" src="https://user-images.githubusercontent.com/1289702/178121319-53fab379-d44e-4998-951f-800484765b62.png">

Scroll down until you see Api Compatibility Level* and there is your dotnet version. <br>

## Step 3: Unity Setup
For my tutorial I've made a small game that does nothing called the NothingGame. <br>

You can open it in Unity Hub to detect what version of Unity it was built with, or else build your own and use whatever version of Unity you want. <br>

The very first line of your Jenkins file is going to look like this: <br>
```batch
UNITY_PATH = '%unity_nothing_game%' // create system environment var pointing to unity install path
```
and basically that's creating a Groovy variable from '%unity_nothing_game%' <br>

Those % characters (pronounced 'modulo') are basically dereferencing a Windows system variable named 'unity_nothing_game', so we need to create that variable. <br>

I tend to use system variables instead of hard-coding a path into the Jenkins file so that this Jenkins file is re-usable, and in theory another build server could have Unity installed wherever and still use this Jenkins file. <br>

Open the NothingGame in Unity Hub: <br>

<img width="315" alt="image" src="https://user-images.githubusercontent.com/1289702/178121367-a7955825-e57d-4c06-b3b6-9317d8511ce8.png">

You may see a warning like this: <br>

<img width="311" alt="image" src="https://user-images.githubusercontent.com/1289702/178121373-1587d608-dfbe-46a0-ad0b-4350a186ab92.png">

If you do, go to this page: Unity Download Archive [4] <br>
Click the 2021.x tab <br>

<img width="316" alt="image" src="https://user-images.githubusercontent.com/1289702/178121390-ace1d1c1-0b5e-42f2-badb-7f156b5d753c.png">

You can search for 16 to find this one: <br>

<img width="302" alt="image" src="https://user-images.githubusercontent.com/1289702/178121408-25f8b6dc-6b0e-4bff-8004-4d348fd45008.png">

Don't worry about finding one with f1 at the end. <br>

Click the green Unity Hub button. It should open up the correct install page in Unity Hub itself. <br>

Allow your browser to open Unity Hub if you need to: <br>

<img width="230" alt="image" src="https://user-images.githubusercontent.com/1289702/178121426-9e39a9c5-1126-49b7-8e7b-15905b228548.png">

Note: you will need Visual Studio 2019 installed, you can see on my screen here it indicates I already have it installed. <br>

<img width="314" alt="image" src="https://user-images.githubusercontent.com/1289702/178121436-4b0e6115-5fbb-4562-979f-fb25ffdf183b.png">

Make sure you have checked to install "Windows Build Support" <br>

<img width="314" alt="image" src="https://user-images.githubusercontent.com/1289702/178121441-36730687-bf5a-4aee-8017-08e26a556dcd.png">

Click the blue install button and watch the progress bar: <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178121449-544d3a33-9a96-4f43-8eb5-2d14be8b5339.png">

<img width="312" alt="image" src="https://user-images.githubusercontent.com/1289702/178121452-f0b16f92-bfbb-4d66-ae78-a1d33b726d77.png">

## Step 3: Creating the System Variable with the Unity Path

<img width="311" alt="image" src="https://user-images.githubusercontent.com/1289702/178121483-46b07fe1-e021-46d2-80e8-af124a0b3513.png">

Click the Environment Variables button <br>

<img width="277" alt="image" src="https://user-images.githubusercontent.com/1289702/178121497-b5b9a620-8960-4c3a-913c-61302be422cd.png">

Click the "New" button iunder the System Variables table. We're about to create a new system variable. <br>

<img width="310" alt="image" src="https://user-images.githubusercontent.com/1289702/178121507-794b08a8-fcfb-4c65-9bb9-5086f5c87f67.png">

My Unity is installed to this path: <br>

```batch
C:\Program Files\Unity\Hub\Editor\2021.1.16f1\Editor
```

You are going to enter the path to the correct Unity.exe basically in this system variable "unity_nothing_game" if your folder doesn't have Unity.exe in it, it is not the correct folder: <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178121844-cac60b39-ed8a-40b9-bd1f-5b2fb9fc8f71.png">

Enter that path in the text field: <br>

<img width="308" alt="image" src="https://user-images.githubusercontent.com/1289702/178121851-37168451-a6cf-4613-ae73-457bfba1d58f.png">

Append Unity.exe to the end of the path to make a complete path to the executable. This is convenient for calling Unity from the command line in the Jenkins file later. [not optional] <br>

<img width="309" alt="image" src="https://user-images.githubusercontent.com/1289702/178121865-abb744fb-19d4-4d2f-8edd-957201744bf4.png">

Make sure Unity.exe is at the end of the environment variable. We are going to call Unity.exe from the command line in our Jenkins file. <br>

Step 4: Create a Jenkins file

Create a Jenkins file like this: 

```batch
UNITY_PATH = '%unity_nothing_game%' // create system environment var pointing to unity install path

pipeline {
    parameters {
        choice(name: 'build', choices: ['Release', 'Debug'], description: "Release or Debug. Debug Builds take longer")
        choice(name: 'release', choices: ['alpha', 'beta', 'release'], description: "alpha - risky builds that may crash, beta - more mature builds testing before release release - builds ready for deployment to user")    
    }

    agent any 
    
    environment {
        appname = "NothingGame" // Set to your own game. "NothingGame: The game that does nothing!"
        release_name = "${ "${release}" == "alpha" || "${release}" == "beta" ? "${release}" : "" }" 
        target = "${ "${build}" == "Release" ? "${appname}${release_name}.exe" : " ${appname}_Debug_${release_name}.exe" }" // append debug for debug builds, nothing for release builds
    }

    stages {
        stage ('Build') {
        steps { script {
            bat """
            \"${UNITY_PATH}\" -nographics -buildTarget Win64 -quit -batchmode -projectPath . -appname ${appname} -executeMethod Jenkins.CmdLine.parseCommandLineArgs ${build} -buildWindows64Player "${target}"
            """
        }}}
        
    } // end stages
}
```

## Step 5: Create a Command Line Parser inside the Unity with a C# Script
Inside Assets/Editor Create a C# script inside the Unity Editor. 

<img width="266" alt="image" src="https://user-images.githubusercontent.com/1289702/178121998-e0b23d98-dc43-4629-9905-c450f2ea6d92.png">

Use this text: <br>

``` csharp
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
```

Replace "NothingGame" with the name of your Unity game. <br>

This will append "_debug" to the program name in the title bar, making it really obvious if you have shipped the debug build on accident and so on. <br>

## Step 6: Create Jenkins Environment Variables

In the main dashboard (go to http://localhost:8080 if you get lost to go back to the main dashboard) and select "Manage Jenkins" <br>

<img width="153" alt="image" src="https://user-images.githubusercontent.com/1289702/178122032-23ff8513-a97b-442b-ba33-3a1becd6ad6a.png">

Select "Configure System" <br>

<img width="317" alt="image" src="https://user-images.githubusercontent.com/1289702/178122038-b3479c43-03ef-40de-925d-a534fa1ed5c3.png">

Scroll down to the section Labelled "Global properties" and check the box labelled "Environment Variables" <br>

<img width="210" alt="image" src="https://user-images.githubusercontent.com/1289702/178122046-bde964fd-b5df-452b-83db-5c1652372ef6.png">

Click the Add button <br>

<img width="115" alt="image" src="https://user-images.githubusercontent.com/1289702/178122051-750254f6-30d9-4426-b05b-aa22f826d26a.png">

Add a variable with the name "unity_nothing_game" set to the value: <br>

```batch
C:\Program Files\Unity\Hub\Editor\2021.1.16f1\Editor
```

or wherever you had your Unity installed on your machine <br>

<img width="295" alt="image" src="https://user-images.githubusercontent.com/1289702/178122068-6da3ac95-bfd0-458f-bd8a-cefc8bde0dcf.png">

Click the Save button at the bottom to accept. <br>

<img width="48" alt="image" src="https://user-images.githubusercontent.com/1289702/178122071-ca036ada-6b92-4512-9936-a5edf140aa89.png">

## Step 6: Create Your Jenkins Project
Click the New Item button in the main dashboard to create your project <br>

<img width="317" alt="image" src="https://user-images.githubusercontent.com/1289702/178122084-04ab6532-71c5-4cc1-912a-1ffd8a6fce7c.png">

Give it a project name <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178122096-1ccbe97c-4a31-4822-b4c0-327885039b5d.png">

Select Pipeline for project type. <br>

<img width="314" alt="image" src="https://user-images.githubusercontent.com/1289702/178122106-e80a1b5e-3097-400a-b6f3-3ce0597b6191.png">

In the General Section (the first section) check the GitHub Project checkbox and enter the url of the Github project: <br>

<img width="306" alt="image" src="https://user-images.githubusercontent.com/1289702/178122117-562e9d80-f9ad-4f4d-944f-56bfa1d9de68.png">

Basically use the same path you use when you do git clone to pull down the repo. <br>

<img width="315" alt="image" src="https://user-images.githubusercontent.com/1289702/178122120-7228a47c-9004-4662-8553-c7da7ea49bc5.png">

Scroll down the Build Triggers Section and Check the "Github hook trigger for GITScm Polling" checkbox. This basically just means that it will build every time you push to the branch that you have it set to watch. <br>

<img width="308" alt="image" src="https://user-images.githubusercontent.com/1289702/178122127-0178a4c1-532a-4db5-a7f1-f7b5c4cffeb4.png">


Scroll down to the Pipeline Section. <br>
By default it will look like this: <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178122139-9629a27b-53ea-414e-95db-31239c4bfb7d.png">

Use the ComboBox and switch from "Pipeline Script" to "Pipeline Script from SCM" SCM means source control management by the way. <br>

<img width="300" alt="image" src="https://user-images.githubusercontent.com/1289702/178122146-0a2deeea-a393-466f-9fa9-fd86f4f3a546.png">

Some new ComboBoxes will appear after that. <br>
In the SCM ComboBox that appears select "Git" instead of None for SCM. You are saying that on Git there will be a "Pipeline Script" <br>

<img width="308" alt="image" src="https://user-images.githubusercontent.com/1289702/178122170-a49c451a-60cd-4d44-9384-676762922517.png">

For Repository URL paste in that https:// url to the github repo again <br>

```
https://github.com/sitting-duck/unity-devops-jenkins.git
```

<img width="296" alt="image" src="https://user-images.githubusercontent.com/1289702/178122194-22b1beab-9b7b-48c0-9f56-f6258a61cca0.png">

You can skip the "Credentials" section since this is a public repo. <br>

By default the "Branch Specifier" is set to "master", change that to "main" <br>

<img width="315" alt="image" src="https://user-images.githubusercontent.com/1289702/178122210-93982a6e-be83-43a6-89a8-3b02f2610395.png">

after your change it will look like this: <br>

<img width="311" alt="image" src="https://user-images.githubusercontent.com/1289702/178122218-7229c9dd-f6f8-4a95-bcce-2ead9c616d37.png">

The next thing is "Script Path" and by default it just says Jenkinsfile. You can set the path to the Jenkinsfile here basically. Just leave it at the default, but if you needed to you could set a custom path like "deploy/Jenkinsfile" or have two Jenkinsfiles if you wanted and specify one build server to look for Jenkinsfile-win and the other to Jenkinsfile-mac and things like that if you wanted to. <br>

<img width="301" alt="image" src="https://user-images.githubusercontent.com/1289702/178122228-8574b8da-6689-4e68-b832-68b3b2948727.png">

Uncheck Lightweight Checkout <br>

<img width="295" alt="image" src="https://user-images.githubusercontent.com/1289702/178122236-f986679c-a84d-4fd2-9998-cf875f556efb.png">

I haven't set anything with lightweight checkout in my git commits so you won't need that to checkout and build the NothingGame. <br>

Click the Save button to accept these Settings. <br>

<img width="303" alt="image" src="https://user-images.githubusercontent.com/1289702/178122248-c8854efe-abaf-4d56-968c-9a3ea408025a.png">

Click the Build Now button <br>

<img width="313" alt="image" src="https://user-images.githubusercontent.com/1289702/178122260-dfd8f3f4-4de9-486c-af89-e2f93928fc28.png">

If it fails the first time with an error like this: <br>

```
groovy.lang.MissingPropertyException: No such property: build for class: groovy.lang.Binding
```

That's just because the first time it parses the Jenkinsfile it will choke on the parameters the first time, but it will work every time after that. <br>
Go ahead and build it again. You can see that the Build button says "Build with Parameters" now, so it will work with the parameters it finds in the Jenkinsfile now, <br>

<img width="311" alt="image" src="https://user-images.githubusercontent.com/1289702/178122278-dc8267e4-4111-4c85-aaff-e4c3590b445a.png">

You can leave the default parameters and click the build button: <br>
<img width="314" alt="image" src="https://user-images.githubusercontent.com/1289702/178122289-0789578e-1c3f-4ab2-8add-37c830114ce3.png">

View the Console Output by using the Triangle Menu next to the timestamp for the latest build and selecting Console Output <br>

<img width="316" alt="image" src="https://user-images.githubusercontent.com/1289702/178122305-59d17571-91a6-4ab6-9b17-52635935fc58.png">

### If You have Error Message During Build
Scroll past this section if you can build with no errors <br>
```
DisplayProgressNotification: Build Failed
'' is an incorrect path for a scene file. BuildPlayer expects paths relative to the project folder.
```
Solution: Add scenes to build in the BuildSettings. BuildSettings had no scenes to build. <br>

<img width="300" alt="image" src="https://user-images.githubusercontent.com/1289702/178122330-00556168-0a92-4a87-9cea-0e5b88beffff.png">

### After You're Done Building
You should go into your Jenkins workspace for that project and check that your game executable is actually there and that it actually runs.
Jenkins will show you "Success" at the end of a build if the script did not throw any errors, but Jenkins is not intelligent enough to know if your game actually built the way that you wanted, so you must do some testing and verify. <br>

<img width="148" alt="image" src="https://user-images.githubusercontent.com/1289702/178122345-e8b81abf-435c-4e26-a86c-98ec422166e7.png">

Sometimes this can be misleading. Check your workspace and make sure your build artifacts are actually there <br>

## Step 8: Check and Test Your Game
I can see my game is actually here in my workspace: <br>
<img width="318" alt="image" src="https://user-images.githubusercontent.com/1289702/178122367-9562dd99-6754-42c9-bfb0-ce3e18e9f61a.png">

And I double click the game to make sure it actually launches. <br>

<img width="308" alt="image" src="https://user-images.githubusercontent.com/1289702/178122372-b2275899-5d61-434e-815c-d16d660f6909.png">


And it does.


### My Tutorials Online: <br>
How to Jenkins Pipeline for Unity Game with SonarQube <br>
https://ashley-tharp.medium.com/how-to-create-a-jenkins-pipeline-to-build-a-unity-game-on-windows-and-analyze-code-quality-on-build-1cda04ac7cbe

Installing SonarQube as a Service on Boot on Windows 10/11 <br>
https://ashley-tharp.medium.com/how-to-install-sonarqube-on-windows-11-7361a26ca042 <br>
<br>
Installing Jenkins on Windows 10/11 <br>
https://ashley-tharp.medium.com/how-to-setup-jenkins-on-windows-10-ac969ee921f2 <br>

### References
[1] Generating and Using a Login Token in SonarQube. https://docs.sonarqube.org/latest/user-guide/user-token/ <br>
[2] DotNet 6.0 SDK Download Page Windows. https://dotnet.microsoft.com/en-us/download#windowsvs2015 <br>
[3] SonarScanner for MSBuild. https://docs.sonarqube.org/latest/analysis/scan/sonarscanner-for-msbuild/ <br>
[4] Unity Download Archive. https://unity3d.com/get-unity/download/archive <br>

