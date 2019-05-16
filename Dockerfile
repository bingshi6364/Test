FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["PLMSide.Data/PLMSide.Data.csproj", "PLMSide.Data/"]
RUN dotnet restore "PLMSide.Data/PLMSide.Data.csproj"
COPY ["PLMSide.Common/PLMSide.Common.csproj", "PLMSide.Common/"]
RUN dotnet restore "PLMSide.Common/PLMSide.Common.csproj"
COPY ["PLMSide/PLMSide.csproj", "PLMSide/"]
RUN dotnet restore "PLMSide/PLMSide.csproj"

COPY . .
WORKDIR "/src/PLMSide"
RUN dotnet build "PLMSide.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "PLMSide.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PLMSide.dll"]
