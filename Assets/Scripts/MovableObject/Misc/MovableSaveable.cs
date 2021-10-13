using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
#endif


namespace MovableObject.Misc
{
	public abstract class MovableSaveable
	{
		protected bool ShowSaveButton;
		protected string Path;

		private const string AssetEnd = ".asset";

#if UNITY_EDITOR

		/// <summary>
		/// Save as scriptable object.
		/// </summary>
		protected virtual string SaveAsScriptableObject()
		{
			if (string.IsNullOrEmpty(Path))
			{
				Debug.LogError("Path value is empty or wrong input was used, please check and try again.");
				return null;
			}

			return System.IO.Path.Combine(FileExistsInPath(Path), AssetEnd);
		}

		/// <summary>
		/// Returns the next available path if specified one already exists.
		/// Ex: if path "movableAction.asset" exists it will add an additional index to the end of the file name: "movableAction1.asset".
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private static string FileExistsInPath(string path)
		{
			var assetPath = System.IO.Path.Combine(path + AssetEnd);
			var fullPath = System.IO.Path.Combine(Application.dataPath, assetPath);

			if (File.Exists(fullPath))
			{
				//Check if indexed value.
				var index = fullPath.Last().ToString();

				if (int.TryParse(index, out var resultIndex))
					FileExistsInPath(path + (resultIndex + 1));
				else
					return path + 1;
			}

			return path;
		}

#endif

		/// <summary>
		/// Toggles save as scriptable object button visibility.
		/// </summary>
		/// <param name="toggle"></param>
		public void ToggleSaveButtonVisibility(bool toggle)
		{
			ShowSaveButton = toggle;
		}
	}
}