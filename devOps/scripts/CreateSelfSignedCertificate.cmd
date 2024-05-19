@echo off
SETLOCAL

REM Get the path to the current script
SET "ScriptPath=%~dp0"
SET "ScriptName=%~n0"
SET "PowerShellScript=%ScriptPath%%ScriptName%.ps1"

REM Check if the PowerShell script exists
IF NOT EXIST "%PowerShellScript%" (
    echo PowerShell script "%PowerShellScript%" not found.
    EXIT /B 1
)

REM Elevate and run the PowerShell script
powershell -ExecutionPolicy Bypass -Command "Start-Process powershell -ArgumentList '-ExecutionPolicy Bypass -File ""%PowerShellScript%""' -Verb RunAs"

ENDLOCAL
