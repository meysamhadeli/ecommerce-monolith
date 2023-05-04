FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
WORKDIR /

# Setup working directory for the project
COPY ./src/BuildingBlocks/BuildingBlocks.csproj ./BuildingBlocks/
COPY ./src/ECommerce/ECommerce.csproj ./ECommerce/
COPY ./src/ECommerce.Api/ECommerce.Api.csproj ./ECommerce.Api/


# Restore nuget packages
RUN dotnet restore ./ECommerce.Api/ECommerce.Api.csproj

# Copy project files
COPY ./src/BuildingBlocks ./BuildingBlocks/
COPY ./src/ECommerce/  ./ECommerce/
COPY ./src/ECommerce.Api/  ./ECommerce.Api/

# Build project with Release configuration
# and no restore, as we did it already

RUN ls
RUN dotnet build  -c Release --no-restore ./ECommerce.Api/ECommerce.Api.csproj

WORKDIR /ECommerce.Api

# Publish project to output folder
# and no build, as we did it already
RUN dotnet publish -c Release --no-build -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Setup working directory for the project
WORKDIR /
COPY --from=builder /ECommerce.Api/out  .


ENV ASPNETCORE_URLS https://*:443, http://*:80
ENV ASPNETCORE_ENVIRONMENT docker

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ECommerce.Api.dll"]

