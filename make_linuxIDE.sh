#!/bin/sh
#

rm -rf bin/Release
msbuild -t:FoenixIDE -property:Configuration=Release
cp -r Main/roms bin/Release

mkbundle -o bin/Release/FoenixIDE --simple bin/Release/FoenixIDE.exe --machine-config /etc/mono/4.5/machine.config --config /etc/mono/config

# zip everything
cd bin/Release
zip FoenixIDE-Linux-amd64-beta-$(date +"%y%m%d%H%M").zip FoenixIDE -r roms Resources

