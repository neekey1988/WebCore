FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /web

# copy csproj and restore as distinct layers
COPY *.sln .
COPY WebCore/*.csproj ./WebCore/
COPY WebCore.Common/*.csproj ./WebCore.Common/
COPY WebCore.Component/*.csproj ./WebCore.Component/
COPY WebCore.Domain/*.csproj ./WebCore.Domain/
COPY WebCore.Entity/*.csproj ./WebCore.Entity/
RUN dotnet restore

# copy everything else and build app
COPY WebCore/. ./WebCore/
COPY WebCore.Common/. ./WebCore.Common/
COPY WebCore.Component/. ./WebCore.Component/
COPY WebCore.Domain/. ./WebCore.Domain/
COPY WebCore.Entity/. ./WebCore.Entity/
WORKDIR /web/WebCore
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS runtime
WORKDIR /web
COPY --from=build /web/WebCore/out ./
ENTRYPOINT ["dotnet", "WebCore.dll"]