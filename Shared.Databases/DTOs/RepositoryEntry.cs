namespace Shared.Databases.DTOs;
// в программе будет две базы данных, для поиска пользователей и для подгрузки сообщений этих пользователей

// нужно детально продумать базовый класс для пользователей в двух базах данных
// в этом классе может быть абстрактый метод на копирование данных, а в классах наследниках этот метод переопределить
// в этом классе нужно оставить только уникальный идентификатор
public abstract class RepositoryEntry
{
    public string Id { get; protected init; } = null!;
    public string UserName { get; protected init; } = null!;
    public abstract void CopyValuesTo(RepositoryEntry entry);
}