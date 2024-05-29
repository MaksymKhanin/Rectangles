
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /source
COPY SegmentRectangleIntersection.sln ./
COPY ./Api/Api.csproj ./Api/
COPY ./Business/Business.csproj ./Business/
COPY ./Persistence/Persistence.csproj ./Persistence/
COPY ./Api.Tests/Api.Tests.csproj ./Api.Tests/

RUN dotnet restore
COPY . .
WORKDIR /source/Api
RUN dotnet build -c $BUILD_CONFIGURATION -o /publish
WORKDIR /source/Business
RUN dotnet build -c $BUILD_CONFIGURATION -o /publish
WORKDIR /source/Persistence
RUN dotnet build -c $BUILD_CONFIGURATION -o /publish

WORKDIR /source
RUN dotnet publish SegmentRectangleIntersection.sln -c $BUILD_CONFIGURATION -o /publish
RUN dotnet test Api.Tests

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS final
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Api.dll"]
