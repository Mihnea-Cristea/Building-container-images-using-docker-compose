# LogMonitor - https://github.com/microsoft/windows-container-tools/tree/master/LogMonitor
FROM mcr.microsoft.com/windows/nanoserver:1809 AS logmonitor
ARG LOGMONITOR_VERSION="v1.1"
ADD https://github.com/microsoft/windows-container-tools/releases/download/${LOGMONITOR_VERSION}/LogMonitor.exe .

# builder
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS builder

WORKDIR /src/PetShop.Api
COPY src/PetShop.Api/PetShop.Api.sln .
COPY src/PetShop.Api/PetShop.Api.Entities/PetShop.Api.Entities.csproj ./PetShop.Api.Entities/
COPY src/PetShop.Api/PetShop.Api.Model/PetShop.Api.Model.csproj ./PetShop.Api.Model/
COPY src/PetShop.Api/PetShop.Api.Products/PetShop.Api.Products.csproj ./PetShop.Api.Products/
COPY src/PetShop.Api/PetShop.Api.Products/packages.config ./PetShop.Api.Products/
RUN nuget restore PetShop.Api.sln -PackagesDirectory packages

COPY src /src
RUN msbuild PetShop.Api.Products/PetShop.Api.Products.csproj /p:Configuration=Release /p:OutputPath=c:/out

# app
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019

ENV APP_ROOT=C:\\petshop-api

RUN Import-Module WebAdministration; \
    New-WebAppPool -Name PetShop; \
    Set-ItemProperty -Path IIS:\AppPools\PetShop -Name processModel.identityType -Value LocalSystem; \    
    Set-ItemProperty -Path IIS:\AppPools\PetShop -Name managedRuntimeVersion -Value 'v4.0'; \
    Set-ItemProperty -Path IIS:\AppPools\PetShop -Name processModel.loadUserProfile -Value $true; \
    Remove-Website -Name 'Default Web Site'; \
    New-Website -Name 'petshop-api' -Port 80 -PhysicalPath $env:APP_ROOT -Force -ApplicationPool PetShop

COPY --from=logmonitor /LogMonitor.exe /LogMonitor.exe
COPY --from=builder /out/_PublishedWebsites/PetShop.Api.Products ${APP_ROOT} 
COPY --from=builder /src/PetShop.Api/packages/Microsoft.Data.SqlClient.SNI.2.1.1/build/net46/Microsoft.Data.SqlClient.SNI.x64.dll ${APP_ROOT}/bin

COPY config/LogMonitorConfig.json /LogMonitor/LogMonitorConfig.json
COPY config/*.config ${APP_ROOT}/config/

COPY start.ps1 /
ENTRYPOINT ["powershell", "/start.ps1"]