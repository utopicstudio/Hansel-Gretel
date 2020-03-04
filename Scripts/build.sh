#! /bin/sh

# NOTE the command args below make the assumption that your Unity project folder is
# a subdirectory of the repo root directory, e.g. for this repo "unity-ci-test" 
# the project folder is "UnityProject". If this is not true then adjust the 
# -projectPath argument to point to the right location.

## Run the editor unit tests
echo "Running editor unit tests for Hansel y Gretel Fusion"

/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-nographics \
	-silent-crashes \
	-logFile $(pwd)/unity.log \
	-projectPath "Hansel y Gretel Fusion" \
	-runEditorTests \
	-editorTestsResultFile $(pwd)/test.xml \
	-quit

rc0=$?
echo "Unit test logs"
cat $(pwd)/test.xml
# exit if tests failed
if [ $rc0 -ne 0 ]; then { echo "Failed unit tests"; exit $rc0; } fi

echo "Attempting build of Hansel y Gretel Fusion for OSX"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-nographics \
	-silent-crashes \
	-logFile $(pwd)/unity.log \
	-projectPath "Hansel y Gretel Fusion" \
	-buildOSXUniversalPlayer "Hansel y Gretel Fusion.app" \
	-quit

rc2=$?
echo "Build logs (OSX)"
cat $(pwd)/unity.log

exit $(($rc1|$rc2))