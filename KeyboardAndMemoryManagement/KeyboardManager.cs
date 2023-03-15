using WinApi;

namespace KeyboardManagement
{
    public class KeyboardManager : WinAPI
    {
        public static bool IsKeyPushedDown(System.Windows.Forms.Keys key)
        {
            return (GetAsyncKeyState((int)key) & 0x8000) != 0;
        }
    }
}
