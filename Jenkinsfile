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

