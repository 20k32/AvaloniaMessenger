#region

using System.Threading.Tasks;

#endregion

namespace DesktopClient.Models;

public static class TaskExtensions
{
    public static void DisableAwaitWarning(this Task _)
    {
        ;
    }
}