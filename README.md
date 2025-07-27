#  Product Catalog App（ポートフォリオ用アプリ）

このアプリは、個人ポートフォリオとして作成した ASP.NET Core MVC 製の製品カタログ管理アプリです。  
業務経験以外でも C# のバックエンドスキルを示すためのサンプルとして作成しています。
本番環境は Render を使用しています。

---

## 技術構成

- 言語：C# / .NET 6
- フレームワーク：ASP.NET Core MVC
- ORM：Entity Framework Core（Code First）
- テンプレートエンジン：Razor View
- データベース：PostgreSQL（Render / ローカル 両対応）

---

## 現在実装されている主な機能

### 製品情報管理機能

- 製品の一覧表示（フィルタ・ソート・ページネーション対応）
  - 製品名検索
  - 価格範囲での絞り込み
  - カテゴリ・ステータス別のフィルタ
  - 価格の昇順／降順ソート
- 製品の詳細表示
- 製品の新規登録（バリデーションあり）
- 製品の編集（既存情報の読み取り／更新日時の保存）
- 製品の削除
- 製品登録・編集時にカテゴリ・ステータスの選択が可能（セレクトリスト）

### 日付管理

- 製品の登録日時（`CreatedAt`）および更新日時（`UpdatedAt`）を自動記録
- タイムゾーンはUTCで保存（PostgreSQL対応）

---

## 今後追加予定の機能

現時点では以下の機能を改善項目として検討しています：

- **カテゴリ・ステータスの管理画面の追加**
  - 現在はカテゴリおよびステータスの初期値がコード内で固定されており、変更には再ビルドが必要です。
  - 今後はデータベース上での追加・編集・削除を可能にすることで、保守性の向上を図ります。

- **JST（日本時間）での時刻表示対応**
  - 現在は `CreatedAt` / `UpdatedAt` がUTCで表示されていますが、ユーザー視点では日本時間での表示が望ましいため、表示時に JST へ変換する仕組みを導入予定です。

- **デモデータの自動初期化機構**
  - 誰でも製品情報を追加・編集できるため、定期的にデータベースを初期化するスクリプトの実装を検討しています。



---

##  ローカル環境での起動手順

1. このリポジトリをクローンします：
   ```bash
   git clone https://github.com/furisu/ProductCatalogApp.git
   cd ProductCatalogApp

2. `appsettings.Template.json`をコピーして`appsettings.json` を作成します：
   ```bash
   copy appsettings.Template.json appsettings.json
   
  - 以下を参考に PostgreSQL の接続情報を編集してください：
   ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=your_db;Username=your_user;Password=your_password"
    }
  }
```
  - Host: 通常は localhost
  - Database: 任意のデータベース名
  - Username: ローカルで作成したユーザー名
  - Password: ユーザーのパスワード



3. Visual Studio で ProductCatalogApp.sln または .csproj を開きます。


4. メニューバーから「▶ 実行」または F5 キーでデバッグ起動します。
- デフォルトでブラウザが開き、アプリケーションが表示されます。


---

##  本番環境（Render）

https://productcatalogapp.onrender.com/Products

Renderの無料プランを利用しているため、アクセス後表示されるまで数分かかる場合があります。
---

## 注意事項

- `appsettings.json` は `.gitignore` により Git 管理されていません。

### ローカル環境での起動にあたっての前提条件

このアプリケーションは **Visual Studio 上での起動**を前提としています。以下の環境が必要です：

- .NET 6 SDK がインストールされていること
- Visual Studio（.NET 開発用コンポーネント含む）がインストールされていること
- PostgreSQL がローカルにインストールされ、稼働していること
- ポート番号は標準（5432）で、アプリがアクセス可能であること
- `appsettings.json` に接続文字列が正しく記載されていること

---

