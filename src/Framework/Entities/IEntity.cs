namespace UserManagement.Framework.Entities;
public interface IEntity
{
    Guid Id { get; set; }

    string Name { get; set; }
}