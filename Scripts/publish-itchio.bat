set GAME_VERSION="0.1.0"
set BUTLER_PATH="%APPDATA%\itch\broth\butler\versions\15.18.0\butler.exe"

%BUTLER_PATH% push release/blue-screen-win varunramesh/blue-screen:win --userversion %GAME_VERSION%
%BUTLER_PATH% push release/blue-screen-osx varunramesh/blue-screen:osx --userversion %GAME_VERSION%
%BUTLER_PATH% push release/blue-screen-linux varunramesh/blue-screen:linux --userversion %GAME_VERSION%