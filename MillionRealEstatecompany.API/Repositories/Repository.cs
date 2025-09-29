using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Data;
using MillionRealEstatecompany.API.Interfaces;
using System.Linq.Expressions;

namespace MillionRealEstatecompany.API.Repositories;

/// <summary>
/// Implementación genérica del repositorio para operaciones de acceso a datos
/// </summary>
/// <typeparam name="T">Tipo de entidad que maneja el repositorio</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio genérico
    /// </summary>
    /// <param name="context">Contexto de la base de datos</param>
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Obtiene una entidad por su identificador
    /// </summary>
    /// <param name="id">Identificador de la entidad</param>
    /// <returns>Entidad encontrada o null si no existe</returns>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Obtiene todas las entidades del tipo especificado
    /// </summary>
    /// <returns>Lista de todas las entidades</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Busca entidades que cumplan con la expresión especificada
    /// </summary>
    /// <param name="expression">Expresión de filtrado</param>
    /// <returns>Lista de entidades que cumplen la condición</returns>
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    /// <summary>
    /// Obtiene la primera entidad que cumple con la expresión o null si no se encuentra
    /// </summary>
    /// <param name="expression">Expresión de filtrado</param>
    /// <returns>Primera entidad encontrada o null</returns>
    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    /// <summary>
    /// Agrega una nueva entidad al contexto
    /// </summary>
    /// <param name="entity">Entidad a agregar</param>
    /// <returns>La entidad agregada</returns>
    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Actualiza una entidad existente en el contexto
    /// </summary>
    /// <param name="entity">Entidad a actualizar</param>
    /// <returns>Tarea completada</returns>
    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Elimina una entidad del contexto
    /// </summary>
    /// <param name="entity">Entidad a eliminar</param>
    /// <returns>Tarea completada</returns>
    public virtual Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Verifica si existe una entidad con el identificador especificado
    /// </summary>
    /// <param name="id">Identificador a verificar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.FindAsync(id) != null;
    }

    /// <summary>
    /// Obtiene el número total de entidades
    /// </summary>
    /// <returns>Número total de entidades</returns>
    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    /// <summary>
    /// Obtiene el número de entidades que cumplen con la expresión especificada
    /// </summary>
    /// <param name="expression">Expresión de filtrado</param>
    /// <returns>Número de entidades que cumplen la condición</returns>
    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.CountAsync(expression);
    }
}