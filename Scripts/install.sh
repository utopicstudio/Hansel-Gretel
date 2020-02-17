#! /bin/sh

BASE_URL=http://netstorage.unity3d.com/unity
HASH=20c1667945cf
VERSION=2019.2.0f1

download() {
  file=$1
  url="$BASE_URL/$HASH/$package"

  echo "Downloading from $url: "
  curl -o `basename "$package"` "$url"
}

install() {
  package=$1
  download "$package"

  echo "Installing "`basename "$package"`
  sudo installer -dumplog -package `basename "$package"` -target /
}

# See $BASE_URL/$HASH/unity-$VERSION-$PLATFORM.ini for complete list
# of available packages, where PLATFORM is `osx` or `win`

install "Windows64EditorInstaller/Unity-$VERSION.pkg"
install "Windows64EditorInstaller/UnitySetup-Windows-Support-for-Editor-$VERSION.pkg"
install "Windows64EditorInstaller/UnitySetup-Mac-Support-for-Editor-$VERSION.pkg"
install "Windows64EditorInstaller/UnitySetup-Linux-Support-for-Editor-$VERSION.pkg"