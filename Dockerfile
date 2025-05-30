﻿FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ./ProductManagementAPI.csproj ./
RUN dotnet restore


COPY . ./
RUN dotnet publish -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ProductManagementAPI.dll"]
