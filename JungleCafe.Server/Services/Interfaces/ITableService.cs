using JungleCafe.Server.Models;

namespace JungleCafe.Server.Services.Interfaces;

public interface ITablesService
{
    Task<IEnumerable<Table>> GetAllTables();
    Task<IEnumerable<int>> GetReservedTableIds(DateTime dateTime);
}