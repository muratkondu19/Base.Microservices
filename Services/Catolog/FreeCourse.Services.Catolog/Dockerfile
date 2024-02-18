FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/FreeCourse.Shared/*.csproj Shared/FreeCourse.Shared/
COPY Services/Catolog/FreeCourse.Services.Catolog/*.csproj Services/Catolog/FreeCourse.Services.Catolog/
RUN dotnet restore Services/Catolog/FreeCourse.Services.Catolog/*.csproj
COPY . .
RUN dotnet publish Services/Catolog/FreeCourse.Services.Catolog/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet","FreeCourse.Services.Catolog.dll" ]