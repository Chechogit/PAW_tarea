using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;
using PAW.Models;

namespace PAW.Repositories;

/// <summary>
/// Interface for basic repository operations.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepositoryBase<T>
{
    Task<bool> UpsertAsync(T entity, bool isUpdating);
    Task<bool> CreateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<IEnumerable<T>> ReadAsync();
    Task<PagedResult<T>> ReadPagedAsync(int page, int pageSize);


    Task<T?> FindAsync(object id);

    Task<bool> UpdateAsync(T entity);
    Task<bool> UpdateManyAsync(IEnumerable<T> entities);
    Task<bool> ExistsAsync(T entity);
}

/// <summary>
/// Base class for repository operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{

    protected ProductDbContext DbContext { get; }
    protected DbSet<T> DbSet { get; }


    protected RepositoryBase(ProductDbContext context)
    {
        DbContext = context;
        DbSet = context.Set<T>();
    }

    public virtual async Task<bool> UpsertAsync(T entity, bool isUpdating)
    {
        return isUpdating
            ? await UpdateAsync(entity)
            : await CreateAsync(entity);
    }

    public virtual async Task<bool> CreateAsync(T entity)
    {
        try
        {
            await DbSet.AddAsync(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            DbSet.Update(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    public virtual async Task<bool> UpdateManyAsync(IEnumerable<T> entities)
    {
        try
        {
            DbSet.UpdateRange(entities);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    public virtual async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            DbSet.Remove(entity);
            return await SaveAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    protected virtual IQueryable<T> BaseQuery()
    {
        return DbSet.AsNoTracking();
    }

    public virtual async Task<PagedResult<T>> ReadPagedAsync(int page, int pageSize)
    {
        try
        {
            pageSize = Math.Clamp(pageSize, 1, Paging.MaxPageSize);

            var query = BaseQuery();
            var totalCount = await query.CountAsync();

            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            page = Math.Clamp(page, 1, totalPages);

            var keyName = DbContext.Model.FindEntityType(typeof(T))?
                .FindPrimaryKey()?.Properties[0].Name;
            if (keyName is not null)
                query = query.OrderBy(e => EF.Property<object>(e, keyName));

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }
    public virtual async Task<IEnumerable<T>> ReadAsync()
    {
        try
        {
            return await BaseQuery().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }


    public virtual async Task<T?> FindAsync(object id)
    {
        try
        {
            var entity = await DbSet.FindAsync(id);

            // Find deja la entidad "vigilada" (tracked) por EF. La soltamos para que,
            // si después se guarda otra copia con el mismo Id (Update), EF no se queje de
            // "another instance with the same key is already being tracked".
            if (entity is not null)
                DbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    public virtual async Task<bool> ExistsAsync(T entity)
    {
        try
        {
            var items = await ReadAsync();
            return items.Any(x => x.Equals(entity));
        }
        catch (Exception ex)
        {
            throw new PAWException(ex);
        }
    }

    protected async Task<bool> SaveAsync()
    {
        var result = await DbContext.SaveChangesAsync();
        return result > 0;
    }
}