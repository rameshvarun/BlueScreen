set GAME_VERSION="0.2.1"
set BUTLER_PATH="%APPDATA%\itch\broth\butler\versions\15.21.0\butler.exe"

%BUTLER_PATH% push release/blue-screen-win varunramesh/blue-screen:win --userversion %GAME_VERSION%
%BUTLER_PATH% push release/blue-screen-osx.app varunramesh/blue-screen:osx --userversion %GAME_VERSION%
%BUTLER_PATH% push release/blue-screen-linux varunramesh/blue-screen:linux --userversion %GAME_VERSION%
%BUTLER_PATH% push release/blue-screen-web varunramesh/blue-screen:web --userversion %GAME_VERSION%