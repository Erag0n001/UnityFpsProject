using UnityEditor;
using UnityEngine;

public class CustomTools : EditorWindow
{
    [MenuItem("File/Build and run Server and Client")]
    public static void BuildStuff()
    {
        BuildServer();
        BuildClient();
        EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Server;
    }
    public static void BuildClient() 
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/StarterScene.unity", "Assets/Scenes/Main menu.unity", "Assets/Scenes/SampleScene.unity" };
        buildPlayerOptions.locationPathName = "../../FpsProjectBuilds/Client/Client.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.AutoRunPlayer;
        buildPlayerOptions.options = BuildOptions.Development;
        EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Player;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
    public static void BuildServer() 
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/StarterScene.unity", "Assets/Scenes/Main menu.unity", "Assets/Scenes/SampleScene.unity" };
        buildPlayerOptions.locationPathName = "../../FpsProjectBuilds/Server/Server.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.AutoRunPlayer;
        buildPlayerOptions.options = BuildOptions.Development;
        EditorUserBuildSettings.standaloneBuildSubtarget = StandaloneBuildSubtarget.Server;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}