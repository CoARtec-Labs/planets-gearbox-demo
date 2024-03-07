# Build Processor Tasks:

## Ensure that ndk location is present in 'local.properties' of the Android project
The following line should be present:
ndk.dir=/Applications/Unity/Hub/Editor/2022.3.10f1/PlaybackEngines/AndroidPlayer/NDK
(Replace location of ndk.dir with the locally installed NDK location)

## 1: Delete build directories:

Delete following directories, if present:

* ### unityLibrary build:
directory: ../unityLibrary/build

* ### xrmanifest.androidlib build:
directory ../unityLibrary/xrmanifest.androidlib/build

* ### unity3d java directory:
directory ../unityLibrary/src/main/java/com/unity3d


## 2: Modify Manifest files:


* ### launcherManifest: 
*File: ../launcher/src/main/AndroidManifest.xml*

  * Delete String 'package="*packageName"' from manifest files: (replace *packageName with package info. default=com.CoARTec.AR_Planetengetriebe_v1)
  * Change installlocation from "preferExternal" to "auto"

* ### unityLibraryManifest: default packageName=com.unity3d.player
*File ../unityLibrary/src/main/AndroidManifest.xml*

  1. Delete String 'package="com.unity3d.player"' from manifest file
  2. Replace String "com.unity3d.player" with "*packageName" (default *packageName="com.coartec.AR_Planetengetriebe_v1")
  3. Replace String "android:exported="true"" with string "android:exported="false""
  4. Replace String "android:hardwareAccelerated="false"" with string "android:hardwareAccelerated="true""
  5. Delete following info
  "'<intent-filter>
        <category android:name="android.intent.category.LAUNCHER" />
        <action android:name="android.intent.action.MAIN" />
      </intent-filter>'"


* ### Xrmanifest.androidlib Manifest: default packageName="com.UnityTechnologies.XR.Manifest"
*File ../unityLibrary/xrmanifest.androidlib/AndroidManifest.xml*

  * Delete String 'package="com.UnityTechnologies.XR.Manifest"' from manifest file
  * Replace String "com.unity3d.player" with string "*packageName" (default *packageName="com.coartec.AR_Planetengetriebe_v1")

## 3: Change build.gradle infos:

* ### Main build.gradle (Only needed, if app should be launched standalone, not only as a module):

file: ../build.gradle

#### Add version variables:

After "plugins { ... }" add following lines (Update variable names and values, if dependencies change):

ext {
    activityComposeVersion = '1.8.2'
    appcompatVersion = '1.6.1'
    composeBomVersion = '2023.10.01'
    coreKtxVersion = '1.12.0'
    timberVersion = '5.0.1'
    compileSdkVersionCode = 34
    targetSdkVersionCode = 34
    minSdkVersionCode = 26
}

* ### launcher build.gradle (Only needed, if app should be launched standalone, not only as a module)::

file: ../launcher/build.gradle

#### Add Namespace Info to build.gradle files:

First line after string "android {" add line with "namespace '*packageName'" (default *packageName=com.CoARTec.AR_Planetengetriebe_v1)

#### Change string compileSdkVersion:

Replace line with string "compileSdkVersion *versionInt" with string "compileSdk compileSdkVersionCode"

#### Delete build tools version info:

Delete Line with string "buildToolsVersion '33.0.1'"

#### Replace targetSdkVersion and minSdkVersion with variable:

* Replace line with string "targetSdkVersion *versionNumber" with string "targetSdkVersion targetSdkVersionCode"

* Replace line with string "minSdkVersion *versionNumber" with string "minSdkVersion minSdkVersionCode"


### unityLibrary build.gradle:

file: ../unityLibrary/build.gradle

#### Add plugins:

Replace line "apply plugin: 'com.android.library'" at the start of the file with:

"plugins {
    id 'com.android.library'
    id 'org.jetbrains.kotlin.android'
}"

#### Add group and version info to unity source modules (if source packages downloadable from external package managmement source):

Replace string "ext:'aar'" with string "ext:'aar', group: 'com.CoARTec.AR_Planetengetriebe_v1', version: '1.0'"

#### Add dependencies:

Add all unityLibrary module dependencies after line "implementation project('xrmanifest.androidlib')":
(Update Buildprocessor script everytime a dependency changes)



    implementation "androidx.appcompat:appcompat:$appcompatVersion"
    implementation "androidx.core:core-ktx:$coreKtxVersion"
    implementation "androidx.activity:activity-compose:$activityComposeVersion"
    implementation platform("androidx.compose:compose-bom:$composeBomVersion")
    implementation 'androidx.compose.ui:ui'
    implementation 'androidx.compose.ui:ui-graphics'
    implementation 'androidx.compose.ui:ui-tooling-preview'
    implementation 'androidx.compose.material3:material3'
    implementation "com.jakewharton.timber:timber:$timberVersion"

#### Add namespace info, change string compileSdkVersion, Delete build tools version:

namespace '*packageName' (default *packageName=com.CoARTec.AR_Planetengetriebe_v1)
See launcher build.gradle task


#### Add kotlin and compose features to build.gradle:

Add following lines after "compileOptions { ... }":

kotlinOptions {
        jvmTarget = '11'
    }
    buildFeatures {
        compose true
    }
    composeOptions {
        kotlinCompilerExtensionVersion '1.5.3'
    }


### xrmanifest.androidlib build.gradle:

file: ../unityLibrary/xrmanifest.androidlib/build.gradle

#### Change plugin info:

Replace line with string "apply plugin: 'android-library'" at the start of the file with: "apply plugin: 'com.android.library'"

#### Add namespace info, delete build tools version, change target sdk version:

namespace 'com.UnityTechnologies.XR.Manifest'
See launcher build.gradle task

#### Add Min sdk version:

After line starting with string "targetSdkVersion " add line "minSdkVersion minSdkVersionCode"

### Change unityLibrary values xml infos:

#### Change styles.xml Theme:

**files**: 
* ../unityLibrary/src/main/res/values/styles.xml: 
  * Replace "android:Theme.Holo.Light.NoActionBar.Fullscreen" with "Theme.AppCompat.Light.NoActionBar"

* ../unityLibrary/src/main/res/values-v21/styles.xml:
  * Replace "android:Theme.Material.Light.NoActionBar.Fullscreen" with "Theme.AppCompat.Light.NoActionBar"

* ../unityLibrary/src/main/res/values-v28/styles.xml:
  * Replace "android:Theme.Material.Light.NoActionBar.Fullscreen" with "Theme.AppCompat.Light.NoActionBar"

* ../unityLibrary/src/main/res/values-v31/styles.xml:
  * Replace "android:Theme.Holo.Light.NoActionBar.Fullscreen" with "Theme.AppCompat.Light.NoActionBar"

