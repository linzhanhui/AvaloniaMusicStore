using System.Collections.Generic;
using Avalonia;

namespace MusicStore.Services;

public class DialogManager
{
    private static readonly Dictionary<object, Visual> RegistrationMapper =
        new Dictionary<object, Visual>();
    
    public static readonly AttachedProperty<object?> RegisterProperty = AvaloniaProperty.RegisterAttached<DialogManager, Visual, object?>(
        "Register");
    
    /// <summary>
    /// Accessor for Attached property <see cref="RegisterProperty"/>.
    /// </summary>
    public static void SetRegister(AvaloniaObject element, object value)
    {
        element.SetValue(RegisterProperty, value);
    }

    /// <summary>
    /// Accessor for Attached property <see cref="RegisterProperty"/>.
    /// </summary>
    public static object? GetRegister(AvaloniaObject element)
    {
        return element.GetValue(RegisterProperty);
    }
}