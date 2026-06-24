FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["education-account-management.API/education-account-management.API.csproj", "education-account-management.API/"]
COPY ["education-account-management.BLL/education-account-management.BLL.csproj", "education-account-management.BLL/"]
COPY ["education-account-management.DAL/education-account-management.DAL.csproj", "education-account-management.DAL/"]

RUN dotnet restore "education-account-management.API/education-account-management.API.csproj"

COPY . .
RUN dotnet publish "education-account-management.API/education-account-management.API.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "education-account-management.API.dll"]
