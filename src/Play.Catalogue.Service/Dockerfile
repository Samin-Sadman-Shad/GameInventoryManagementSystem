#creating a empty container to compile and build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
#copy project file to the container
COPY ./packages/Play.Common.1.0.2.nupkg /app/nuget/
COPY ./InventoryManagementSystem/src/Play.Catalogue.Service/*.csproj /app/Play.Catalogue.Service/
COPY ./InventoryManagementSystem/src/Play.Catalog.Contracts/*.csproj /app/Play.Catalog.Contracts/
# COPY /Play.Common/*.csproj /app/
RUN ls -la /app/
RUN dotnet nuget add source /app/nuget/ --name local
#install the dependencies
RUN dotnet restore ./Play.Catalogue.Service/Play.Catalogue.Service.csproj
#copies rest of the file from the host build context(solution root folder, which contains all projects) to the container root folder
COPY ./InventoryManagementSystem/src /app/

WORKDIR /app/Play.Catalogue.Service
#compile and build the project inside container
RUN dotnet publish /app/Play.Catalogue.Service/Play.Catalogue.Service.csproj -c Release -o /app/out

#create a container for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
#copy the compiled dll from the previous container to the current one
COPY --from=build-env /app/out /app/
#when the container is run, execute this command
ENTRYPOINT ["dotnet", "Play.Catalogue.Service.dll"]

