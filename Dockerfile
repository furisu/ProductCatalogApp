# ベースイメージ（実行用）
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDKイメージ（ビルド用）
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# プロジェクトファイルとソースコードをコピー
COPY . .

# パッケージ復元とビルド
RUN dotnet restore "./ProductCatalogApp.csproj"
RUN dotnet publish "./ProductCatalogApp.csproj" -c Release -o /app/publish

# 実行環境にアプリをコピー
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# エントリーポイント
ENTRYPOINT ["dotnet", "ProductCatalogApp.dll"]
