@echo off
echo.
echo. - Configuring ODE VC6 Workspace -
echo.

copy _configs\config-double.h ..\include\ode\config.h
copy ..\config\msvcdefs.def msvcdefs.def

rem remove the Debug and Release dirs to force a complete rebuild
del /Q Debug
del /Q Release

echo. done.
echo.
pause