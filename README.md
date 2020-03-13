# TrainTime
![見た目](https://gyazo.ingen084.net/3db6b39b8683a8a71f6f5557e3507c43)

電車の時間を調べるのがとても面倒だったので自動化しました.

起動すれば例外なくプライマリ画面の右下に表示されます.

現状は組み込みで船橋日大前駅(三鷹方面行き)のみ表示するようになっています.

汎用性を求めようとしていますが闇が深いので要望があれば

これで私の学校がばれました

## ビルド方法
dotnetCore3.0で開発されています. dotnetCore3.0のSDKとRuntimeをインストールしてください.
[ここ](https://dotnet.microsoft.com/download/dotnet-core/3.0)にあります.
### パターン1
`.sln`ファイルのある階層でコマンド`dotnet publish -c Release`と入力すれば生成されます
### パターン2
VisualStudioで`.sln`ファイルを開きビルドする
## ライセンス
- MIT
