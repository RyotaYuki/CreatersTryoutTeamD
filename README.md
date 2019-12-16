# DragonEscape
Group creation repository for games , This game genre is

## 仕様

### 使用する主なソフト

バージョン違いによるエラーなどをなくすため以下のようにする  

Unity 2018.4.10f1  

### Gitのフォルダ構成
* Git直下のフォルダ
	* Project　―　共有したいファイルをいれるフォルダ
		* Unity　―　Unityに直接関わるフォルダ
			* StarGenesis　―　UnityのProject
	* README.md　―　GitのWiki表示させるためのモノ
	* .gitignore　―　Gitで共有する上で除外したいものを指定するモノ

### UnityのAssetでのフォルダ構成

| フォルダ名 | 用途 |
|:-----------|------------:|
| Scenes | シーンデータを置く |
| Scripts | スクリプトデータを置く |
| Prefabs | Prefabを置く |
| Animations | アニメーションデータを置く |
| Database | DatabaseObjectデータとその中身のデータを置く |
| Models | インポートしてきた3Dモデルデータを置く |
| Materials | 色や光沢などのマテリアルデータを置く |
| Textures | 画像データを置く |
| Fonts | フォントデータを置く |
| Audio | BGMやSEなどのサウンドデータを置く |
| Resources | （Special）Resources.Loadで読み込むファイルを置く |
| Editor | （Special）Unityのエディタ拡張のためのスクリプトを置く |
| Plugins | （Special）ネイティブプラグインやその他プラグインを置く |

# コーディングルール
=================

規約をつくる目的
-----------------
* バグの発生源となりやすいコーディングの防止
  - プログラマが誤解する可能性を減らす
  - 意図が明確にする
  - 冗長さを排除する
* チーム内で均等なルールを決め保守性を向上させる

※ チーム内で定期的にルールを見直し、より良い改善内容については積極的に取り入れ修正しましょう

.csファイルのフォーマット
-----------------
* UTF-8
* 改行コードは CR/LF (DOS)
* タブインデント
* ※ エディタの設定でタブや行末スペースを可視化しておくと良い

コミットに関する注意点
-----------------
* ファイルと.metaは一緒にcommitする
* ディレクトリの場合は`.gitkeep`も一緒にcommitする
    - `.gitkeep`は空ディレクトリも管理するため
* commitはテキストとバイナリで分ける
    - テキスト: .cs, .txt, .xml, .facedなどのヒューマンリーダブルなファイル
    - バイナリ: .png, .prefab, .unity, .unity3dなどの人間の目でレビューが困難なファイル
    - 理由: レビュワーがレビューをし易いため
* 不要ファイルは(.bakなど)コミットしない

命名規則
-----------------

### クラス
* Pascal記法 (大文字で始まる) で記述する

```C#
// ◯
class SampleClass
{
・・・
}

// ☓
class sampleClass
{
・・・
}

// ☓
class sample_class
{
・・・
}
```

### メソッド
* Pascal記法 (大文字で始まる) で記述する
* 仮引数(parameter)はCamel記法とする
* メソッド名は動詞を使う
* コルーチンは最初に`Co`（CoHogeHuga）

```C#
// ◯
public string GetProfile(int userId)
{
・・・
}

// ◯  co-routine
public string CoGetProfile(int userId)
{
・・・
}

// ☓ name Camel
public string getProfile(int UserId)
{
・・・
}

// ☓ name snake
public string get_profile(int user_id)
{
・・・
}

// ☓ not verb
public string profile(int userId)
{
・・・
}
```

### enum
* enum名はPascal記法 (大文字で始まる) で記述する
* 値もPascal記法で記述する

```C#
// ◯
enum SampleEnum
{
    Foo,
    Bar,
    Hoge
}

// ☓ Camel
enum sampleEnum
{
    Foo,
    Bar,
    Hoge
}

// ☓ UpperCase
enum SampleEnum
{
    FOO,
    BAR,
    HOGE
}
```

### 変数
* Camel記法（小文字始まり）で記述する（`hogeFuga`)
* privateなフィールドは`_`から始める（`_hogeFuga`）
  - Inspectorに表示したいときは、`private`にして[SerializeField]をつける
* publicなフィールドは、`プロパティ`にする ※ 基本的に使わない

```C#
class Sample
{
    // ◯
    protected string userName;
    private int _userLevel;
    [SerializeField]
    private string _userMessage;

    // ☓ Pascal
    protected string UserName;
    // ☓ not `_`
    private int userLevel;
・・・
}
```

### プロパティ
* Camel記法（小文字始まり）で記述する（`hogeFuga`)

```C#
class Sample
{
    // ◯
    private double _speed;
    public double moveSpeed
    {
        get {
            return _speed;
        }

        set {
            _speed = value;
        }
    }

    // ☓ Pascal
    private double _speed;
    public double MoveSpeed
    {
        get {
            return _speed;
        }

        set {
            _speed = value;
        }
    }
}
```

インデント
-----------------
* タブを使用する(スペースはNG)

コールバック
-----------------
* `delegate`を使う。`Actionは`使わない。

1行の文字数
-----------------
* 努力目標：80文字
* 最大：100文字

改行ルール
-----------------
* 中括弧{}の前に改行を入れる。
* 1行で書く場合のみ改行しなくてもOK。

```
// ◯
if (true)
{
    ・・・
}

// ◯  one line
public bool isHoge
{
    get { return true; }
}

// ☓
if (true) {
    ・・・
}
```

`()`前後のスペース
-----------------
* 関数定義や関数呼び出しと()の間にはスペースは入れない。
* `if`, `while`, `switch` といった制御構文と`()`の間にはスペースを入れる

```
// ◯  メソッドの定義にはスペースを入れない
public void Hoge(Action callback)
{
    // ◯ if の後ろにはスペースを入れる
    if (callback != null)
    {
        // メソッドの呼び出しにはスペースを入れる
        callback();
    }
}

// ☓
public void Hoge (Action callback)
{
    // ☓
    if(callback != null)
    {
        callback ();
    }
}
```

if
-----------------
* 必ず{}を付ける
    - スコープが分かりにくくなりバグを生みやすくなるため

```
// ◯
if (callback != null)
{
    callback ();
}

// ☓
if (callback != null)
    callback ();
```

for
-----------------
* 必ず{}を付ける
    - スコープが分かりにくくなりバグを生みやすくなるため

```
// ◯
for (int i = 0; i < 10; i++)
{
    System.Console.WriteLine(i);
}

// ☓
for (int i = 0; i < 10; i++)
	System.Console.WriteLine(i);
```

usingの記述位置
-----------------
* ファイルの一番先頭に記載する。
* `namespace`よりも外側に記載する

```
◯
using System;
using UnityEngine;

namespace MySpace
{
    // code
}
```
