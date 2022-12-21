# OxrExtraFeatures

The OpenXR feature implementation is based on <https://github.com/joemarshall/openxrhands>.

## UPM package: com.ousttrue.oxr_extra_features

UPM git package.

`https://github.com/ousttrue/OpenXR_VRM1.git?path=Assets/OxrExtraFeatures#v0.2.1`

Implement 4 OpenXR extensions.

- [XR_EXT_hand_tracking](https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_EXT_hand_tracking)
- [XR_FB_body_tracking](https://developer.oculus.com/documentation/native/android/move-body-tracking/)
- [XR_FB_eye_tracking_social](https://developer.oculus.com/documentation/native/android/move-eye-tracking/)
- [XR_FB_face_tracking](https://developer.oculus.com/documentation/native/android/move-face-tracking/)

|extension|platform|note|
|-|-|-|
|XR_EXT_hand_tracking|quest, quest2, quest pro, hololens2 etc...|Works on all devices that support XR_EXT_hand_tracking. SteamVR + null device + LeapController can work.|
|XR_FB_body_tracking|quest, quest2, quest pro|Quest link also works. Require version 47 driver.|
|XR_FB_eye_tracking|quest pro|Quest link also works.|
|XR_FB_body_tracking|quest pro|Quest link also works.|

## VRM-1.0 tracking samples
The sample humanoid model is `seed-san` from https://github.com/vrm-c/vrm-specification/tree/master/samples.

Any vrm-0.x and vrm-1.0 models will work fine.

### XR_EXT_hand_tracking

scene: `Assets/OpenXR_VRM1/Scenes/VRM1Hand`

[hand.webm](https://user-images.githubusercontent.com/68057/204517601-0b59e031-fb99-460a-a85b-aedbe941e8fc.webm)

`=> VRM Humanoid`

### XR_FB_body_tracking

scene: `Assets/OpenXR_VRM1/Scenes/VRM1BodyEyeFace`

[XR_FB_body_tracking.webm](https://user-images.githubusercontent.com/68057/206843919-82a2cfb6-9d9a-4ab3-98a4-ba738ff4499b.webm)

`=> VRM Humanoid`

### XR_FB_eye_tracking_social / XR_FB_face_tracking

scene: `Assets/OpenXR_VRM1/Scenes/VRM1BodyEyeFace`

[eye_and_face_blink.webm](https://user-images.githubusercontent.com/68057/207227991-3e07893d-9934-4bf9-9250-536c4eee3161.webm)

`=> VRM LookAt`
`=> VRM Expression`

## scenes

### Assets/OpenXR_VRM1/Scenes/HandJoints
Simple cube scene.
### Assets/OpenXR_VRM1/Scenes/BodyJoints
Simple cube scene.
### Assets/OpenXR_VRM1/Scenes/EyeJoints
Simple cube scene.
### Assets/OpenXR_VRM1/Scenes/FaceWeights
Simple cube scene.
