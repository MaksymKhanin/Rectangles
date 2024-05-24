
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /source
COPY Api.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish SegmentRectangleIntersection.sln -c Release -o /publish
RUN dotnet test Api.Tests

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS final
WORKDIR /publish
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Api.dll"]
