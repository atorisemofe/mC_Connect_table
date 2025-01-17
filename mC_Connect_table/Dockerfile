# Get the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the csproj adn restore all of teh nuggets
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Copy the SQLite database file into the container
COPY RestaurantTablesContext-bc5cdb63-2d08-4188-8180-de32378d99a4.db /app/data/RestaurantTablesContext-bc5cdb63-2d08-4188-8180-de32378d99a4.db
COPY OrderViewContext-f2366df8-0dd3-469e-bdec-3914cb6bc7a3.db /app/data/OrderViewContext-f2366df8-0dd3-469e-bdec-3914cb6bc7a3.db

ENTRYPOINT ["dotnet", "mC_Connect_table.dll"]