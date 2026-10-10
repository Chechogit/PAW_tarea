using Microsoft.EntityFrameworkCore;
using PAW.DataAccess.MSSQL;

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

    public virtual async Task<IEnumerable<T>> ReadAsync()
    {
        try
        {
            return await DbSet.ToListAsync();
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
            return await DbSet.FindAsync(id);
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