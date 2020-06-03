FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore

# Copy everything else and build
#COPY . ./
#RUN dotnet publish -c Release -o out

COPY . ./
RUN dotnet publish WebApi -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build-env /app/WebApi/out .

# Make sure the app binds to port 8080
ENV ASPNETCORE_URLS http://*:8080

ENTRYPOINT ["dotnet", "ReservationSystem.dll"]
