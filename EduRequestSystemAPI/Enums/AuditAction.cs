namespace EduRequestSystemAPI.Enums
{
    public enum AuditAction
    {
        Login,          // Вход в систему
        Logout,         // Выход
        CreateRequest,  // Создание заявки
        UpdateRequest,  // Обновление заявки
        ChangeStatus,   // Смена статуса менеджером
        AddComment,     // Добавление комментария
        DeleteComment,  // Удаление комментария
        Register      // Регистрация
    }
}
