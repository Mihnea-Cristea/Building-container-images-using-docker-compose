# escape=`
FROM docker4dotnet/sql-server:2017-ltsc2019

ENV sa_password="dock3r-n3tfx"

COPY ./DatabaseScripts/ .
RUN ["powershell", "./InstallDatabases.ps1"]