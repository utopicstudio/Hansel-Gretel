#! /bin/sh


PROJECT_PATH=$(pwd)
UNITY_BUILD_DIR=$(pwd)/Build
LOG_FILE=$UNITY_BUILD_DIR/unity-android.log
UNITY_BUILD_APK_NAME=dev_travis.apk
UNITY_BUILD_APK_PATH=$PROJECT_PATH/Builds/Android/Development
UNITY_BUILD_APK=$UNITY_BUILD_APK_PATH/$UNITY_BUILD_APK_NAME


ERROR_CODE=1
echo "Items in project path ($PROJECT_PATH):"
ls "$PROJECT_PATH"


echo "Building project for Android..."

echo "Create Certificate Folder"
mkdir ~/Library/Unity
mkdir ~/Library/Unity/Certificates

cp CACerts.pem ~/Library/Unity/Certificates/

echo "activate license"
/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -serial "$UNITYKEY" -username "$UNITYEMAIL" -password "$UNITYPASSWORD" -logfile

cat ~/Library/Logs/Unity/Editor.log

echo "return license"

mkdir $UNITY_BUILD_DIR
/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -returnlicense -logfile

export EVENT_NOKQUEUE=1
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  --args buildName $UNITY_BUILD_APK_NAME \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile \
  -projectPath "$PROJECT_PATH" \
  -buildTarget "Android" \
  -username "$UNITYEMAIL" \
  -password "$UNITYPASSWORD" \
  -serial "$UNITYKEY" \
  -executeMethod "Infrastructure.EditorHelpers.Builder.BuildDevForAndroid" |
  tee "$LOG_FILE"


if [ $? = 0 ]; thenecho "Building Android apk completed successfully."
  ERROR_CODE=0
elseecho "Building Android apk failed. Exited with $?."
  ERROR_CODE=1


echo "Finishing with code $ERROR_CODE"
exit $ERROR_CODE