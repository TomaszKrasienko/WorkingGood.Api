FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY Src/Domain/Domain.csproj ./Domain/
COPY Src/Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Src/Application/Application.csproj ./Application/
COPY Src/WebApi/WebApi.csproj ./WebApi/
RUN dotnet restore ./WebApi/WebApi.csproj
COPY . ./
RUN dotnet publish ./WorkingGood.Api.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "WebApi.dll"]

ENV ASPNETCORE_ENVIRONMENT="Development"
ENV TZ="Europe/Warsaw"
EXPOSE 80
EXPOSE 443