#! /bin/sh

# Download Unity3D installer into the container
#  The below link will need to change depending on the version, this one is for 2018.3.3f1
#  Refer to https://unity3d.com/get-unity/download/archive and find the link pointed to by Mac "Unity Editor"

echo 'Downloading Unity 2019.2.0f1 pkg:'
curl --retry 5 -o Unity.pkg https://unity3d.com/get-unity/download?thank-you=update&download_nid=62773&os=Mac
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# Run installer(s)
echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /