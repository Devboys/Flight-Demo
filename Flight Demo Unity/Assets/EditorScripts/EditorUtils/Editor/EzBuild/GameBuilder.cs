using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Protopia.EditorClasses.BuildUtilities.EzBuild;

namespace Protopia.EditorClasses.BuildUtilities
{
    public class GameBuilder
    {
        public static string buildRootFolderPath
        {
            get
            {
                return
                (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Application.productName + "Builds"));
            }
        }

        public static string executableFilename
        {
            get
            {
                return ($"{Application.productName}_v{Application.version}.exe");
            }
        }

        public static void BuildWindows(string subfolderPath, bool deleteIfExists, string[] scenesToBuild = null)
        {
            string folderPath = Path.Combine(buildRootFolderPath, subfolderPath);

            Debug.Log("BUILDING TO: " + folderPath);
            if (Directory.Exists(folderPath) && deleteIfExists) Directory.Delete(folderPath, true);

            Directory.CreateDirectory(folderPath);

            string[] buildScenes;
            if (scenesToBuild == null) {
                buildScenes = EditorBuildSettings.scenes.Where(n => n.enabled).Select(n => n.path).ToArray();
            }
            else
            {
                buildScenes = scenesToBuild;
            }

            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = Path.Combine(folderPath, executableFilename),
                scenes = buildScenes,
                target = BuildTarget.StandaloneWindows64
                
            });

            if (Directory.Exists(buildRootFolderPath))
            {
                string reportPath = GenerateBuildReportDoc(report, folderPath);
                EditorUtility.RevealInFinder(reportPath);
            }
            else
            {
                throw new Exception("Build failed so build path does not exist");
            }
        }

        public static void BuildDevelopmentWindows(string subfolderPath, bool deleteIfExists, string[] scenesToBuild = null)
        {
            string folderPath = Path.Combine(buildRootFolderPath, subfolderPath);

            Debug.Log("DEV BUILDING TO: " + folderPath);

            if (Directory.Exists(folderPath) && deleteIfExists) Directory.Delete(folderPath, true);

            Directory.CreateDirectory(folderPath);

            string[] buildScenes;
            if (scenesToBuild == null)
            {
                buildScenes = EditorBuildSettings.scenes.Where(n => n.enabled).Select(n => n.path).ToArray();
            }
            else
            {
                buildScenes = scenesToBuild;
            }

            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                locationPathName = Path.Combine(folderPath, executableFilename),
                scenes = buildScenes,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development

            });

            if (Directory.Exists(buildRootFolderPath))
            {
                string reportPath = GenerateBuildReportDoc(report, folderPath);
                EditorUtility.RevealInFinder(reportPath);
            }
            else
            {
                throw new Exception("Build failed so build path does not exist");
            }
        }

        /// <summary>
        /// Generates a .txt document containing the build report at given path. Returns the path of the .txt itself.
        /// </summary>
        private static string GenerateBuildReportDoc(BuildReport report, string path)
        {
            //write the report to a file called build_report.txt in the build folder.
            string reportPath = Path.Combine(path, "build_report.txt");
            StreamWriter writer = new StreamWriter(reportPath, false);
            try
            {
                writer.WriteLine($"BUILD REPORT FOR {Application.productName} VERSION {Application.version}");
                writer.WriteLine($"BUILD VERSION: {Application.version}");
                writer.WriteLine($"BUILD TYPE: {EzBuildVarsUtility.GetBuildVarsAsset().BuildType}");
                writer.WriteLine($"TIME STARTED: {report.summary.buildStartedAt.ToString()}");
                writer.WriteLine($"TIME ENDED: {report.summary.buildEndedAt.ToString()}");
                writer.WriteLine($"TIME ELAPSED: {report.summary.totalTime.ToString(@"hh\:mm\:ss")}");
                writer.WriteLine($"TARGET PLATFORM: {report.summary.platform.ToString()}");
                writer.WriteLine($"BUILD GUID: {report.summary.guid.ToString()}");
                writer.WriteLine($"----");
                writer.WriteLine($"TOTAL ERRORS: {report.summary.totalErrors}");
                writer.WriteLine($"TOTAL WARNINGS: {report.summary.totalWarnings}");
                writer.WriteLine($"BUILD RESULT: {report.summary.result.ToString()}");
                writer.WriteLine($"----");

                bool noMessagesWritten = true;
                foreach(BuildStep step in report.steps)
                {
                    foreach(BuildStepMessage message in step.messages)
                    {
                        writer.WriteLine($"{message.type.ToString()}:  {message.content}");
                        noMessagesWritten = false;
                    }
                }
                if (noMessagesWritten)
                {
                    writer.WriteLine("<No Message Logs Found>");
                }
            }
            catch(Exception e)
            {
                writer.WriteLine("<< BUILD REPORT FAILED WRITING >>");
                writer.WriteLine(e.Message);
                writer.WriteLine(e.StackTrace);
                throw e;
            }
            finally
            {
                writer.Close();
            }

            //finaly, print report in editor.
            Debug.Log(report);
            return reportPath;
        }
    }
}