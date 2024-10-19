using System;

namespace UserQueueManager.Contracts.Data;

/// <summary>
/// Представление клиента.
/// </summary>
/// <remarks>
/// 
/// </remarks>
public class User
{
    /// <summary>
    /// Идентификатор клиента.
    /// </summary>
    public int IdUser { get; set; }

    public override string ToString()
    {
        return $"{{idUser:{IdUser}}}";
    }
    
}
