
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /source
COPY Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish SegmentRectangleIntersection.sln -c $BUILD_CONFIGURATION -o /publish
RUN dotnet test Api.Tests

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Api.dll"]
