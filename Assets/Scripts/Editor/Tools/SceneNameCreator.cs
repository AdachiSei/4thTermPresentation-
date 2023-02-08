using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ビルドセッティングスのシーン名を定数で管理するクラスを作成するスクリプト
/// </summary>
public static class SceneNameCreator
{
	// コマンド名
	private const string COMMAND_NAME = "Tools/CreateConstants/Scene Name";

	//作成したスクリプトを保存するパス
	private const string EXPORT_PATH = "Assets/Scripts/Constants/SceneName.cs";

	// ファイル名(拡張子あり、なし)
	private static readonly string FILENAME =
		Path.GetFileNameWithoutExtension(EXPORT_PATH);

	/// <summary>
	/// シーンのファイル名を定数で管理するクラスを作成します
	/// </summary>
	[MenuItem(COMMAND_NAME)]
	public static void Create()
	{
		if (!CanCreate()) return;

		CreateScript();

		Debug.Log("SceneNameを作成完了");
	}

	/// <summary>
	/// スクリプトを作成します
	/// </summary>
	public static void CreateScript()
	{
		var builder = new StringBuilder();

		builder.AppendLine("/// <summary>");
		builder.AppendLine("/// シーン名を定数で管理するクラス");
		builder.AppendLine("/// </summary>");
		builder.AppendFormat("public struct {0}", FILENAME).AppendLine();
		builder.AppendLine("{");
		builder.Append("\t").AppendLine("#region Constants");
		builder.AppendLine("\t");

		//BuildSetingsに入っているSceneの名前を全てとってくる
		foreach (var scene in EditorBuildSettings.scenes)
		{
			var name = Path.GetFileNameWithoutExtension(scene.path);
			builder
				.Append("\t")
				.AppendFormat
					(@"  public const string {0} = ""{1}"";",
						name.Replace(" ", "_").ToUpper(),
						name)
				.AppendLine();
		}

		builder.AppendLine("\t");
		builder.Append("\t").AppendLine("#endregion");
		builder.AppendLine("}");

		string directoryName = Path.GetDirectoryName(EXPORT_PATH);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}

		File.WriteAllText(EXPORT_PATH, builder.ToString(), Encoding.UTF8);
		AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
	}

	/// <summary>
	/// シーンのファイル名を定数で管理するクラスを作成できるかどうかを取得します
	/// </summary>
	[MenuItem(COMMAND_NAME, true)]
	private static bool CanCreate()
	{
		var isPlayingEditor = !EditorApplication.isPlaying;
		var isPlaying = !Application.isPlaying;
		var isCompiling = !EditorApplication.isCompiling;
		return isPlayingEditor && isPlaying && isCompiling;
	}
}
