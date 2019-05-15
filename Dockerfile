FROM microsoft/dotnet:2.2-sdk AS build
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
