using UnityEngine;
using UnityEditor;
using System.Collections;

public class AnimationProcessor : AssetPostprocessor
{
    private void OnPreprocessAnimation()
    {
        var animatioImporter = assetImporter as ModelImporter;

        //����C���|�[�g�݂̂ɐ���
        if (!animatioImporter.importSettingsMissing) return;

        var clips = animatioImporter.clipAnimations;

        if (clips.Length == 0) clips = animatioImporter.defaultClipAnimations;

        foreach (var clip in clips)
        {
            clip.loopTime = true;
        }

        animatioImporter.clipAnimations = clips;
    }
}