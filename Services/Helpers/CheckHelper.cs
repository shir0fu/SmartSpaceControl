using Microsoft.AspNetCore.Identity;
using System.Text;

namespace SmartSpaceControl.Services.Helpers;

public static class CheckHelper
{
    public static void CheckNull<T>(T? obj, string? customExeption = null) where T : class
    {
        if (obj == null)
        {
            if (!string.IsNullOrEmpty(customExeption))
            {
                throw new ArgumentNullException(nameof(T), $"{customExeption}");
            }
            throw new ArgumentNullException(nameof(T), $"{typeof(T).Name} cannot be null");
        }
    }

    public static void ThrowResultExeptions(IdentityResult identityResult)
    {
        StringBuilder sb = new StringBuilder();
        List<string> createExeptions = new List<string>();

        foreach (var ex in identityResult.Errors)
        {
            createExeptions.Add(ex.Description);
        }

        sb.AppendJoin(", ", createExeptions);
        throw new Exception($"Error: {sb}");
    }
}
