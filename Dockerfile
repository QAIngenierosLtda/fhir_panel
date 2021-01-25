# Stage 1
# Instala las herramientas de desarrollo .net en un contenedor intermedio
# compila la aplicacion generando un AspStudio.dll
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app
# Stage 2
# Crea un contenedor solo con las librerias runtime de aspnet

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
LABEL MAINTAINER "Alejandro Mejia Ayala <alejandromejia@qaingenieros.com>"

RUN apt-get update
RUN apt-get install -y apt-utils
RUN apt-get install -y libgdiplus
RUN apt-get install -y nano

# establece el directorio de trabajo
WORKDIR /app
# copia la aplicacion del contenedor de desarrollo al contenedor de runtime
COPY --from=build /app .

# define las variables de entorno
ENV ALARM_REPORT=False
ENV CALIBRATION_INSTRUMENT="Indra-FK02GYW-"
ENV CALIBRATION_TYPE="None"
ENV CALIBRATION_METHOD="None"
ENV CALIBRATION_VALUE="37.3"

#Define el entrypoint (Punto de ejecucion)
ENTRYPOINT ["dotnet", "AspStudio.dll"]
