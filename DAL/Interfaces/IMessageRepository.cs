using DAL.Entities;

namespace DAL.Interfaces;

public interface IMessageRepository
{
    void AddGroup(Group group);
    void RemoveConnection(Connection connection);
    Task<Connection> GetConnection(string connectionId);
    Task<Group> GetMessageGroup(string groupName);
    Task<Group> GetGroupForConnection(string connectionId);
    void AddMessage(Message message);
    void DeleteMessage(Message message);
    Task<Message> GetMessage(int id);
}