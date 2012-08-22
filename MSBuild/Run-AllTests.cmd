@echo off
CALL :CHECK_ENV

SET MSBUILD_TOOL=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
PUSHD "%~dp0"
   ECHO Starting all tests...
  "%MSBUILD_TOOL%" /nologo /m /nr:false "Run-AllTests.proj"
POPD
ECHO.
PAUSE
goto :eof


:CHECK_ENV
IF NOT EXIST "%WINDIR%\Microsoft.NET\Framework\v4.0.30319" (
  ECHO Microsoft .Net Framework v4.0 is required.
  ECHO == Get Microsoft.Net Framework : http://www.microsoft.com/net
  EXIT /B 2
)
goto :eof
