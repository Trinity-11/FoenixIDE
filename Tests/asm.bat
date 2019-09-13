@echo off

64tass.exe %1 --long-address -x --intel-hex -o %~n1.hex --list %~n1.lst
copy %~n1.lst ..\bin\Release\roms
copy %~n1.hex ..\bin\Release\roms
