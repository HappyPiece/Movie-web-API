# build
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /source
COPY . .
RUN dotnet restore "./MovieCatalog/MovieCatalog.csproj" --disable-parallel && dotnet publish "./MovieCatalog/MovieCatalog.csproj" -c debug -o /app --no-restore

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 
WORKDIR /app
COPY --from=build /app ./
LABEL maintainer="tryhardfailanyway@gmail.com"
USER 1000
ENV "ENVIRONMENT"="stage"

EXPOSE 5000

ENTRYPOINT ["dotnet", "MovieCatalog.dll"]
