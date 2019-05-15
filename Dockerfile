FROM microsoft/dotnet:2.2-sdk AS build
COPY PLMSide.sln ./
COPY PLMSide.Data/*.csproj ./app/PLMSide.Data/
COPY PLMSide.Common/*.csproj ./app/PLMSide.Common/
COPY PLMSide/*.csproj ./app/PLMSide/
WORKDIR /app/PLMSide
RUN dotnet restore

COPY PLMSide/. ./
RUN dotnet publish -o out /p:PublishWithAspNetCoreTargetManifest="false"

FROM microsoft/dotnet:2.2-runtime AS runtime
ENV ASPNETCORE_URLS http://+:5000
WORKDIR /app
COPY --from=build /app/PLMSide/out ./
ENTRYPOINT ["dotnet", "PLMSide.dll"]
