@echo off
call :CHECK_NUGET

pushd "%~dp0"
  mkdir packages 2>NUL
  for /f %%i in ('dir /b /s packages.config') do (
    pushd packages
      nuget install "%%i"
    popd
  )
popd

goto :eof


:CHECK_NUGET
nuget 2>&1 >NUL
if %errorlevel% == 9009 (
  echo Can not found NuGet command!
  echo GET IT : http://nuget.codeplex.com
  pause
)
goto :eof
