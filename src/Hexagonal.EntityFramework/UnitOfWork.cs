using System.Text;
using Hexagonal.Domain.Entities.Comums;

namespace Hexagonal.EntityFramework;

public class UnitOfWork(IDbContext context) : IUnitOfWork
{
    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
    if (!this._disposed && disposing)
    {
        context.Dispose();
    }
    this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    public async Task<bool> Commit()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            throw new UserException(GetFullErrorMessage(e));
        }
    }
    private static string GetFullErrorMessage(Exception e)
    {
        var errorMessage = new StringBuilder();
        errorMessage.AppendLine(e.Message);

        var innerException = e.InnerException;
        while (innerException != null)
        {
            if (innerException.InnerException == null)
                errorMessage.AppendLine(innerException.Message);
            innerException = innerException.InnerException;
        }
        return errorMessage.ToString();
    }
}