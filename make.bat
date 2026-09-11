@setlocal enabledelayedexpansion
@set PREVPROMPT=%PROMPT%
@prompt $E[1A
@set MAKE=make.bat
@set PROJ=Mina
@echo on

@if "%~1" == "" (set TARGET=build
) else (set TARGET=%1 && shift)
:__ARGS_APPEND
@if "%~1" neq "" (set "ARGS=%ARGS% %1" && shift && goto :__ARGS_APPEND)

@call :%TARGET% %ARGS%
@prompt %PREVPROMPT%
@exit /b %ERRORLEVEL%

:build
	dotnet build --nologo -v q --clp:NoSummary -c Release %PROJ%.slnx %*
	@exit /b %ERRORLEVEL%

:clean
	dotnet clean --nologo -v q %PROJ%.slnx %*
	@exit /b %ERRORLEVEL%

:distclean
	@call :clean
	@for /F %%i in ('powershell -c Select-Xml -Path %PROJ%.slnx -XPath "//Solution/Project | ForEach-Object {$_.Node.Path}"') do @(
		echo rmdir /S /Q %%~dpibin
		rmdir /S /Q %%~dpibin 2>nul
		echo rmdir /S /Q %%~dpiobj
		rmdir /S /Q %%~dpiobj 2>nul
	)
	@exit /b %ERRORLEVEL%

:release
	git archive HEAD --output=%PROJ%-%DATE:/=%.zip
	
	dotnet publish src --nologo -v q --clp:NoSummary -c Release -o .tmp
	powershell -NoProfile $ProgressPreference = 'SilentlyContinue' ; Compress-Archive -Force -Path .tmp\*, README.md, LICENSE -DestinationPath %PROJ%-lib-%DATE:/=%.zip
	rmdir /S /Q .tmp 2>nul
	
	@exit /b %ERRORLEVEL%

:test
	dotnet test --nologo -v q -c Release %PROJ%.slnx %*
	@exit /b %ERRORLEVEL%

:bench
	dotnet run --project bench --no-launch-profile -c Release %*
	@exit /b %ERRORLEVEL%

:publish
	@call :setenv VERSION_NAME "powershell -Command $d = Get-Date; '{0}.{1}.{2}.{3}' -f $d.Year, $d.Month, $d.Day, ($d.Hour * 100 + $d.Minute)"
	@call :setenv BUILD_NAME   "powershell -Command Get-Date -Format HHmm"
	@set      VERSION=%VERSION_NAME%
	@set PACKAGE_NAME=Zenu.Mina.%VERSION%.nupkg
	git tag %VERSION%
	git push origin %VERSION%
	dotnet pack --nologo -v q src/Mina.csproj -c Release -o nupkg -p:PackageVersion=%VERSION%
	dotnet nuget push nupkg/%PACKAGE_NAME% --api-key %NUGET_APIKEY% --source https://api.nuget.org/v3/index.json
	@exit /b %ERRORLEVEL%

:setenv
	@setlocal
	@set "CMD=%~2"
	@for /f "usebackq delims=" %%x in (`cmd /c "!CMD!"`) do @(
		@endlocal
		@set %1=%%x
		@exit /b %ERRORLEVEL%
	)
	@endlocal
	@exit /b %ERRORLEVEL%
