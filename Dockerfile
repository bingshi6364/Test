FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY PLMSide.sln ./
COPY PLMSide.Data/*.csproj ./PLMSide.Data/
COPY PLMSide.Common/*.csproj ./PLMSide.Common/
COPY PLMSide/*.csproj ./PLMSide/

RUN dotnet restore
COPY . .
WORKDIR /src/ PLMSide.Data
RUN dotnet build -c Release -o /app

WORKDIR /src/ PLMSide.Common
RUN dotnet build -c Release -o /app

WORKDIR /src/PLMSide
RUN dotnet build -c Release -o /app

FROM build AS publish

RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PLMSide.dll"]
