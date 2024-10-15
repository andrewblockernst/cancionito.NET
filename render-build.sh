#!/usr/bin/env bash
set -o errexit

# Instalar .NET 8 SDK
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0 --install-dir /usr/share/dotnet

# Agregar el nuevo SDK a la variable PATH
export PATH=/usr/share/dotnet:$PATH

# Ejecutar el build de la aplicación
dotnet publish -c Release -o out
