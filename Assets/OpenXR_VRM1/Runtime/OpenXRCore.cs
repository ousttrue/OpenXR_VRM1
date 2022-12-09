using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace openxr
{
    public enum XrResult
    {
        XR_SUCCESS = 0,
        XR_TIMEOUT_EXPIRED = 1,
        XR_SESSION_LOSS_PENDING = 3,
        XR_EVENT_UNAVAILABLE = 4,
        XR_SPACE_BOUNDS_UNAVAILABLE = 7,
        XR_SESSION_NOT_FOCUSED = 8,
        XR_FRAME_DISCARDED = 9,
        XR_ERROR_VALIDATION_FAILURE = -1,
        XR_ERROR_RUNTIME_FAILURE = -2,
        XR_ERROR_OUT_OF_MEMORY = -3,
        XR_ERROR_API_VERSION_UNSUPPORTED = -4,
        XR_ERROR_INITIALIZATION_FAILED = -6,
        XR_ERROR_FUNCTION_UNSUPPORTED = -7,
        XR_ERROR_FEATURE_UNSUPPORTED = -8,
        XR_ERROR_EXTENSION_NOT_PRESENT = -9,
        XR_ERROR_LIMIT_REACHED = -10,
        XR_ERROR_SIZE_INSUFFICIENT = -11,
        XR_ERROR_HANDLE_INVALID = -12,
        XR_ERROR_INSTANCE_LOST = -13,
        XR_ERROR_SESSION_RUNNING = -14,
        XR_ERROR_SESSION_NOT_RUNNING = -16,
        XR_ERROR_SESSION_LOST = -17,
        XR_ERROR_SYSTEM_INVALID = -18,
        XR_ERROR_PATH_INVALID = -19,
        XR_ERROR_PATH_COUNT_EXCEEDED = -20,
        XR_ERROR_PATH_FORMAT_INVALID = -21,
        XR_ERROR_PATH_UNSUPPORTED = -22,
        XR_ERROR_LAYER_INVALID = -23,
        XR_ERROR_LAYER_LIMIT_EXCEEDED = -24,
        XR_ERROR_SWAPCHAIN_RECT_INVALID = -25,
        XR_ERROR_SWAPCHAIN_FORMAT_UNSUPPORTED = -26,
        XR_ERROR_ACTION_TYPE_MISMATCH = -27,
        XR_ERROR_SESSION_NOT_READY = -28,
        XR_ERROR_SESSION_NOT_STOPPING = -29,
        XR_ERROR_TIME_INVALID = -30,
        XR_ERROR_REFERENCE_SPACE_UNSUPPORTED = -31,
        XR_ERROR_FILE_ACCESS_ERROR = -32,
        XR_ERROR_FILE_CONTENTS_INVALID = -33,
        XR_ERROR_FORM_FACTOR_UNSUPPORTED = -34,
        XR_ERROR_FORM_FACTOR_UNAVAILABLE = -35,
        XR_ERROR_API_LAYER_NOT_PRESENT = -36,
        XR_ERROR_CALL_ORDER_INVALID = -37,
        XR_ERROR_GRAPHICS_DEVICE_INVALID = -38,
        XR_ERROR_POSE_INVALID = -39,
        XR_ERROR_INDEX_OUT_OF_RANGE = -40,
        XR_ERROR_VIEW_CONFIGURATION_TYPE_UNSUPPORTED = -41,
        XR_ERROR_ENVIRONMENT_BLEND_MODE_UNSUPPORTED = -42,
        XR_ERROR_NAME_DUPLICATED = -44,
        XR_ERROR_NAME_INVALID = -45,
        XR_ERROR_ACTIONSET_NOT_ATTACHED = -46,
        XR_ERROR_ACTIONSETS_ALREADY_ATTACHED = -47,
        XR_ERROR_LOCALIZED_NAME_DUPLICATED = -48,
        XR_ERROR_LOCALIZED_NAME_INVALID = -49,
        XR_ERROR_GRAPHICS_REQUIREMENTS_CALL_MISSING = -50,
        XR_ERROR_RUNTIME_UNAVAILABLE = -51,
        // Provided by XR_KHR_android_thread_settings
        XR_ERROR_ANDROID_THREAD_SETTINGS_ID_INVALID_KHR = -1000003000,
        // Provided by XR_KHR_android_thread_settings
        XR_ERROR_ANDROID_THREAD_SETTINGS_FAILURE_KHR = -1000003001,
        // Provided by XR_MSFT_spatial_anchor
        XR_ERROR_CREATE_SPATIAL_ANCHOR_FAILED_MSFT = -1000039001,
        // Provided by XR_MSFT_secondary_view_configuration
        XR_ERROR_SECONDARY_VIEW_CONFIGURATION_TYPE_NOT_ENABLED_MSFT = -1000053000,
        // Provided by XR_MSFT_controller_model
        XR_ERROR_CONTROLLER_MODEL_KEY_INVALID_MSFT = -1000055000,
        // Provided by XR_MSFT_composition_layer_reprojection
        XR_ERROR_REPROJECTION_MODE_UNSUPPORTED_MSFT = -1000066000,
        // Provided by XR_MSFT_scene_understanding
        XR_ERROR_COMPUTE_NEW_SCENE_NOT_COMPLETED_MSFT = -1000097000,
        // Provided by XR_MSFT_scene_understanding
        XR_ERROR_SCENE_COMPONENT_ID_INVALID_MSFT = -1000097001,
        // Provided by XR_MSFT_scene_understanding
        XR_ERROR_SCENE_COMPONENT_TYPE_MISMATCH_MSFT = -1000097002,
        // Provided by XR_MSFT_scene_understanding
        XR_ERROR_SCENE_MESH_BUFFER_ID_INVALID_MSFT = -1000097003,
        // Provided by XR_MSFT_scene_understanding
        XR_ERROR_SCENE_COMPUTE_FEATURE_INCOMPATIBLE_MSFT = -1000097004,
        // Provided by XR_MSFT_scene_understanding
        XR_ERROR_SCENE_COMPUTE_CONSISTENCY_MISMATCH_MSFT = -1000097005,
        // Provided by XR_FB_display_refresh_rate
        XR_ERROR_DISPLAY_REFRESH_RATE_UNSUPPORTED_FB = -1000101000,
        // Provided by XR_FB_color_space
        XR_ERROR_COLOR_SPACE_UNSUPPORTED_FB = -1000108000,
        // Provided by XR_FB_spatial_entity
        XR_ERROR_SPACE_COMPONENT_NOT_SUPPORTED_FB = -1000113000,
        // Provided by XR_FB_spatial_entity
        XR_ERROR_SPACE_COMPONENT_NOT_ENABLED_FB = -1000113001,
        // Provided by XR_FB_spatial_entity
        XR_ERROR_SPACE_COMPONENT_STATUS_PENDING_FB = -1000113002,
        // Provided by XR_FB_spatial_entity
        XR_ERROR_SPACE_COMPONENT_STATUS_ALREADY_SET_FB = -1000113003,
        // Provided by XR_FB_passthrough
        XR_ERROR_UNEXPECTED_STATE_PASSTHROUGH_FB = -1000118000,
        // Provided by XR_FB_passthrough
        XR_ERROR_FEATURE_ALREADY_CREATED_PASSTHROUGH_FB = -1000118001,
        // Provided by XR_FB_passthrough
        XR_ERROR_FEATURE_REQUIRED_PASSTHROUGH_FB = -1000118002,
        // Provided by XR_FB_passthrough
        XR_ERROR_NOT_PERMITTED_PASSTHROUGH_FB = -1000118003,
        // Provided by XR_FB_passthrough
        XR_ERROR_INSUFFICIENT_RESOURCES_PASSTHROUGH_FB = -1000118004,
        // Provided by XR_FB_passthrough
        XR_ERROR_UNKNOWN_PASSTHROUGH_FB = -1000118050,
        // Provided by XR_FB_render_model
        XR_ERROR_RENDER_MODEL_KEY_INVALID_FB = -1000119000,
        // Provided by XR_FB_render_model
        XR_RENDER_MODEL_UNAVAILABLE_FB = 1000119020,
        // Provided by XR_VARJO_marker_tracking
        XR_ERROR_MARKER_NOT_TRACKED_VARJO = -1000124000,
        // Provided by XR_VARJO_marker_tracking
        XR_ERROR_MARKER_ID_INVALID_VARJO = -1000124001,
        // Provided by XR_MSFT_spatial_anchor_persistence
        XR_ERROR_SPATIAL_ANCHOR_NAME_NOT_FOUND_MSFT = -1000142001,
        // Provided by XR_MSFT_spatial_anchor_persistence
        XR_ERROR_SPATIAL_ANCHOR_NAME_INVALID_MSFT = -1000142002,
        XR_RESULT_MAX_ENUM = 0x7FFFFFFF
    }

    [Flags]
    public enum XrSpaceLocationFlags : Int64
    {
        XR_SPACE_LOCATION_ORIENTATION_VALID_BIT = 0x00000001,
        XR_SPACE_LOCATION_POSITION_VALID_BIT = 0x00000002,
        XR_SPACE_LOCATION_ORIENTATION_TRACKED_BIT = 0x00000004,
        XR_SPACE_LOCATION_POSITION_TRACKED_BIT = 0x00000008
    };

    /*XrResult xrGetInstanceProcAddr(
        XrInstance                                  instance,
        const char*                                 name,
        PFN_xrVoidFunction*                         function);*/
    internal delegate int PFN_xrGetInstanceProcAddr(
        ulong instance,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        out IntPtr function);

    /*typedef struct XrVector2f {
        float    x;
        float    y;
    } XrVector2f;*/
    [StructLayout(LayoutKind.Sequential)]
    internal struct XrVector2f
    {
        public float x;
        public float y;
    }

    /*typedef struct XrVector3f {
        float    x;
        float    y;
        float    z;
    } XrVector3f;*/
    [StructLayout(LayoutKind.Sequential)]
    public struct XrVector3f
    {
        public float x;
        public float y;
        public float z;

        public override string ToString()
        {
            return $"[{x:0.00}, {y:0.00}, {z:0.00}]";
        }

        public Vector3 ToUnity()
        {
            return new Vector3(x, y, -z);
        }
    }

    /*typedef struct XrVector4f {
        float    x;
        float    y;
        float    z;
        float    w;
    } XrVector4f;*/
    [StructLayout(LayoutKind.Sequential)]
    public struct XrVector4f
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public override string ToString()
        {
            return $"[{x:0.00}, {y:0.00}, {z:0.00}, {w:0.00}]";
        }

        public Quaternion ToUnity()
        {
            return new Quaternion(x, y, -z, -w);
        }
    }

    /*typedef struct XrPosef {
        XrQuaternionf    orientation;
        XrVector3f       position;
    } XrPosef;*/
    [StructLayout(LayoutKind.Sequential)]
    public struct XrPosef
    {
        public XrVector4f orientation;
        public XrVector3f position;

        public override string ToString()
        {
            return $"<{orientation} {position}>";
        }
    }

    public enum XrReferenceSpaceType
    {
        XR_REFERENCE_SPACE_TYPE_VIEW = 1,
        XR_REFERENCE_SPACE_TYPE_LOCAL = 2,
        XR_REFERENCE_SPACE_TYPE_STAGE = 3,
        // Provided by XR_MSFT_unbounded_reference_space
        XR_REFERENCE_SPACE_TYPE_UNBOUNDED_MSFT = 1000038000,
        // Provided by XR_VARJO_foveated_rendering
        XR_REFERENCE_SPACE_TYPE_COMBINED_EYE_VARJO = 1000121000,
        XR_REFERENCE_SPACE_TYPE_MAX_ENUM = 0x7FFFFFFF
    }

    /*typedef struct XrReferenceSpaceCreateInfo {
        XrStructureType         type;
        const void*             next;
        XrReferenceSpaceType    referenceSpaceType;
        XrPosef                 poseInReferenceSpace;
    } XrReferenceSpaceCreateInfo;*/
    [StructLayout(LayoutKind.Sequential)]
    public struct XrReferenceSpaceCreateInfo
    {
        public XrStructureType type;
        public IntPtr next;
        public XrReferenceSpaceType referenceSpaceType;
        public XrPosef poseInReferenceSpace;
    };

    public delegate XrResult Type_xrCreateReferenceSpace(
        ulong session,
        in XrReferenceSpaceCreateInfo createInfo,
        ref ulong space);
}
