using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class TablesService(CafeDbContext context) : ITablesService
{
    public async Task<IEnumerable<Table>> GetAllTables()
    {
        return await context.Tables
            .OrderBy(t => t.Zone)
            .ThenBy(t => t.Number)
            .ToListAsync();
    }

    public async Task<IEnumerable<int>> GetReservedTableIds(DateTime dateTime)
    {
        var startTime = dateTime.AddMinutes(-90);
        var endTime = dateTime.AddMinutes(90);

        return await context.Reservations
            .Where(r => r.ReservationDate >= startTime &&
                        r.ReservationDate <= endTime &&
                        r.Status != "cancelled")
            .Select(r => r.TableId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<Table>> GetAvailableTables(DateTime? date)
    {
        var query = context.Tables.AsQueryable();

        if (date.HasValue)
        {
            var reservedTableIds = await context.Reservations
                .Where(r => r.ReservationDate.Date == date.Value.Date)
                .Select(r => r.TableId)
                .Distinct()
                .ToListAsync();

            query = query.Where(t => !reservedTableIds.Contains(t.Id));
        }

        return await query
            .OrderBy(t => t.Zone)
            .ThenBy(t => t.Number)
            .ToListAsync();
    }
}