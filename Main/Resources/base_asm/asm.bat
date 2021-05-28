REM @echo off
64tass-1.55.2425-test.exe %1 -D TARGET_SYS=%2 --long-address --intel-hex --m65816 -o %~n1.hex --list %~n1.lst -Wno-portable
