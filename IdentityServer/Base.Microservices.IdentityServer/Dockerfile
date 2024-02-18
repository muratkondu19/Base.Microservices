FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY IdentityServer/Base.Microservices.IdentityServer/*.csproj IdentityServer/Base.Microservices.IdentityServer/ 
RUN dotnet restore  IdentityServer/Base.Microservices.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/Base.Microservices.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","Base.Microservices.IdentityServer.dll" ]