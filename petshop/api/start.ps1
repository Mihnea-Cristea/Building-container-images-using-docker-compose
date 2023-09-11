
# copy process-level environment variables (from `docker run`) machine-wide:
Write-Output 'STARTUP: Copying environment variables'
Stop-Service w3svc
foreach($key in [System.Environment]::GetEnvironmentVariables('Process').Keys) {
    if ([System.Environment]::GetEnvironmentVariable($key, 'Machine') -eq $null) {
        $value = [System.Environment]::GetEnvironmentVariable($key, 'Process')
        [System.Environment]::SetEnvironmentVariable($key, $value, 'Machine')
    }
}

Write-Output 'STARTUP: Running LogMonitor and ServiceMonitor'
C:\LogMonitor.exe C:\ServiceMonitor.exe w3svc