# �x�[�X�C���[�W�i���s�p�j
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# SDK�C���[�W�i�r���h�p�j
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# �v���W�F�N�g�t�@�C���ƃ\�[�X�R�[�h���R�s�[
COPY . .

# �p�b�P�[�W�����ƃr���h
RUN dotnet restore "./ProductCatalogApp.csproj"
RUN dotnet publish "./ProductCatalogApp.csproj" -c Release -o /app/publish

# ���s���ɃA�v�����R�s�[
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# �G���g���[�|�C���g
ENTRYPOINT ["dotnet", "ProductCatalogApp.dll"]
