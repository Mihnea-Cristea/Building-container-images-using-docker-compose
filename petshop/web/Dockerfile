# LogMonitor - https://github.com/microsoft/windows-container-tools/tree/master/LogMonitor
FROM mcr.microsoft.com/windows/nanoserver:1809 AS logmonitor
ARG LOGMONITOR_VERSION="v1.1"
ADD https://github.com/microsoft/windows-container-tools/releases/download/${LOGMONITOR_VERSION}/LogMonitor.exe .

FROM mcr.microsoft.com/dotnet/framework/aspnet:3.5-windowsservercore-ltsc2019
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ENV APP_ROOT='C:\petshop-web'

RUN Install-WindowsFeature NET-HTTP-Activation; \
    Remove-Website -Name 'Default Web Site'; \
    New-Website -Name 'petshop-web' -Port 80 -PhysicalPath $env:APP_ROOT -Force

COPY --from=logmonitor /LogMonitor.exe /LogMonitor.exe

COPY petshop-web.zip .
RUN Expand-Archive -Path petshop-web.zip -DestinationPath /

COPY config/LogMonitorConfig.json /LogMonitor/LogMonitorConfig.json
COPY web.config ${APP_ROOT}
COPY config/*.config ${APP_ROOT}/config/

ENTRYPOINT /LogMonitor.exe /ServiceMonitor.exe w3svc