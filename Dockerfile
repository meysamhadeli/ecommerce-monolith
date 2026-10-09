FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /

# Copy the solution-wide build props and the whole source tree
# (src/BuildingBlocks is the Griffin building-blocks git submodule)
COPY ./Directory.Build.props ./
COPY ./src ./src

# Restore nuget packages
RUN dotnet restore ./src/ECommerce.Api/ECommerce.Api.csproj

# Publish project to output folder
RUN dotnet publish -c Release --no-restore -o out ./src/ECommerce.Api/ECommerce.Api.csproj

FROM mcr.microsoft.com/dotnet/aspnet:10.0

# Setup working directory for the project
WORKDIR /
COPY --from=builder /out  .


ENV ASPNETCORE_URLS https://*:443, http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ECommerce.Api.dll"]

