using System.Collections.Generic;
using System.Threading.Tasks;

namespace Petrolhead
{
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates a specific data model
        /// </summary>
        /// <param name="item">The item to be validated</param>
        /// <returns>Boolean (async)</returns>
        bool Validate(T item);
        
    }

   
    
    public interface IConverter<TSource, TOutput>
    {
        Task<TOutput> ConvertAsync(TSource input);
    } 

    
}