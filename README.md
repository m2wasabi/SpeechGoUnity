# SpeechGoUnity -言霊発射装置-

HoloLensで言霊を発射するアプリです。

[![動作シーン](https://img.youtube.com/vi/CfBzbpxRcrA/0.jpg)](https://www.youtube.com/watch?v=CfBzbpxRcrA)

## 動作環境

Unity 2017.1.0f3

## ビルド方法

1. ソースのダウンロード

    例えば以下のようなコマンドでソースを入手する

    ```
    git clone git@github.com:m2wasabi/SpeechGoUnity.git
    ```

2. アセットの組み込み

    以下のアセットを入手してプロジェクトに追加する。  
    Google Cloud Speech Recognition は有料アセット($20)なので気を付けよう。  

    + [Google Cloud Speech Recognition(V3.0)](https://www.assetstore.unity3d.com/jp/#!/content/72625)
    + [Cannon on a Platform](https://www.assetstore.unity3d.com/jp/#!/content/57534)
    + [HoloToolkit-Unity(master)](https://github.com/Microsoft/HoloToolkit-Unity)
    + StandardAssets/ParticleSystems

    ※Google Cloud Speech Recognition について、V3.0現在、そのままではUWPで動作しない。
    そこで以下のようにソースを書き換える。

    [\Assets\FrostweepGames\GCSpeechRecognition\Scripts\Core\Managers\ThreadManager.cs](https://gist.github.com/m2wasabi/aa1227bf26dc5dca3a2112228b05c8b0)

3. プロジェクト・シーンの再読み込みを行う

4. Google Croud Speech API が利用できるAPIキーを入手する

    参考URL: [クイックスタート Speech API – 音声認識 | Google Cloud Platform](https://cloud.google.com/speech/docs/getting-started?hl=ja)

5. Universal Windows Platform 用にアプリをビルドする

    参考URL: [ホログラム 100 | Holographic Academyの日本語訳](https://github.com/HoloMagicians/HolographicAcademyJP/blob/master/Academy/holograms_100.md)  
    4章を読むと良い

## 遊び方

### 基本的な操作法

|アクション|操作|
|---|---|
|視線の方向に言霊を飛ばす|エアタップしたまま喋る→指を離す|
|砲台から言霊を飛ばす|砲台の底に向けて話しかける→目をそらす|
|砲台を視線の正面に呼ぶ|「Cannon, come here」と喋る|

### 言霊エフェクト

|台詞|効果|
|---|---|
|爆発|一定時間後に言霊が爆発する|
|最高|歓声に包まれながら言霊が発射される|
