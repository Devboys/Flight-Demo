using UnityEngine;
using UnityEditor;
using System.IO;

namespace Protopia.EditorClasses.BuildUtilities
{
    public static class ScriptableObjectUtility
    {
        /// <summary>
        ///	Creates a scriptable object asset of type T and returns a reference to it. If path is left as null, will use currently selected folder in AssetsFolder.
        /// </summary>
        /// <param name="assetName"> The name of the final asset in its final destination </param>
        /// <param name="folderPath"> The path of the folder that the asset should go in once created </param>
        /// <param name="prependNew"> Whether the created asset name should start with "New" in its name. </param>
        /// <param name="focusAfterCreate"> Whether to have the editor focus on the created asset once created </param>
        public static T CreateAsset<T>(bool prependNew, string folderPath = null, string assetName = null, bool focusAfterCreate = true) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (folderPath == null)
            {
                folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (folderPath == "")
                {
                    folderPath = "Assets";
                }
                else if (Path.GetExtension(folderPath) != "")
                {
                    folderPath = folderPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
                }
            }

            string fileName = (assetName != null ? assetName: typeof(T).ToString());

            //Cut out duplicate ".asset" format name.
            if (fileName.EndsWith(".asset")) 
            {
                fileName = fileName.Substring(0, fileName.Length - 6);
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(folderPath + (prependNew ? "/New " : "") + fileName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (focusAfterCreate)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }

            return AssetDatabase.LoadAssetAtPath<T>(assetPathAndName);
        }
    }
}