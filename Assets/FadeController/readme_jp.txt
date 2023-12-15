<<バージョン>>
1.0

<<更新情報>>
1.0
・リリース

1.1
・色指定が黒固定になっていたのを修正

<<使用方法>>
1. FadeController.Instanceで取得しましょう。アタッチの必要はありません。
2. 基本はFadeIn, Out(フェード時間)で動作します。例) FadeController.Instance.FadeIn(1.0f);
	2-2. FadeIn, Out(フェード時間, 色)
	2-1. FadeIn, Out(フェード時間, コールバック)
	2-2. FadeIn, Out(フェード時間, 色, コールバック)
3. IsCompleteプロパティからでも終了フラグを取得することが出来ます。

<<連絡>>
Twitter:@isemito_niko