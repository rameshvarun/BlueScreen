set PROJECT_PATH="%CD%"

set WIN64=-buildWindows64Player ./Release/blue-screen-win/BlueScreen.exe
set OSX=-buildOSXUniversalPlayer ./Release/blue-screen-osx/BlueScreen.app
set LINUX64=-buildLinux64Player ./Release/blue-screen-linux/BlueScreen.x86_64

"%PROGRAMFILES%\Unity\Hub\Editor\2020.3.22f1\Editor\Unity.exe" -batchmode -quit -projectPath %PROJECT_PATH% %WIN64% %OSX% %LINUX64%