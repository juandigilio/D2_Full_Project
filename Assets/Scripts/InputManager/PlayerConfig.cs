public static class PlayerConfig
{
    private static float padSensitivity = 1.0f;
    private static float mouseSensitivity = 0.1f;


    public static float GetPadSensitivity()
    {
        return padSensitivity;
    }

    public static float GetMouseSensitivity()
    {
        return mouseSensitivity;
    }

    public static void SetPadSensitivity(float value)
    {
        padSensitivity = value;
    }

    public static void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}
