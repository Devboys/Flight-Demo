using System;
using System.IO;
using System.Linq;
using UnityEngine;
namespace UnityEditor
{
    public class BuildGame
    {
        [MenuItem("Pipeline/Archived Build: Windows")]
        public static void BuildWindows()
        {
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = Path.Combine(buildMainFolderPath, "ArchivedBuilds", buildSubfolderName, filename),
                scenes = EditorBuildSettings.scenes.Where(n =>
                n.enabled).Select(n => n.path).ToArray(),
                target = BuildTarget.StandaloneWindows
            });
            Debug.Log(report);
        }

        [MenuItem("Pipeline/QuickBuild: Windows")]
        public static void QuickBuildWindows()
        {
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = Path.Combine(buildMainFolderPath, ".QuickBuild", filename),
                scenes = EditorBuildSettings.scenes.Where(n =>
                n.enabled).Select(n => n.path).ToArray(),
                target = BuildTarget.StandaloneWindows
            });
            Debug.Log(report);
        }

        /*
        * This is a static property which will return a string, representing a
        * build folder on the desktop. This does not create the folder when it
        * doesn't exists, it simply returns a suggested path. It is put on the
        * desktop, so it's easier to find but you can change the string to any
        * path really. Path combine is used, for better cross platform support
        */
        public static string buildMainFolderPath
        {
            get
            {
                return
                (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "builds"));
            }
        }

        public static string buildSubfolderName
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd_HHmm");
            }
        }
/*
* This returns the filename that the build should spit it. For a start
* this just returns a current date, in a simple lexicographical format
* with the exe extension appended. Later on, you can customize this to
* include more information, such as last person to commit, what branch
* were used, version of the game, or what git-hash the game were using
*/
public static string filename
        {
            get
            {
                return ($"{Application.productName}_v{Application.version}.exe");
            }
        }
    }
}