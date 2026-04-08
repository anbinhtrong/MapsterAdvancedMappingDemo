using Mapster;
using MapsterCodeMaze.Models;
using System;

namespace MapsterCodeMaze.Mappings
{
    internal class DebugMapster
    {
        public static void DebugMapping<TSource, TDestination>()
        {
            // Get the Expression Tree that Mapster uses for the specific mapping pair
            var config = TypeAdapterConfig<TSource, TDestination>.ForType();
            var expression = config.Config.CreateMapExpression(new Mapster.Models.TypeTuple(typeof(TSource), typeof(TDestination)),
  Mapster.MapType.Projection);

            // Print the string representation of the mapping logic
            Console.WriteLine($"=== Mapster Generated Logic: {typeof(TSource).Name} -> {typeof(TDestination).Name} ===");
            Console.WriteLine(expression.ToString());
            Console.WriteLine("===============================");
        }
    }
}
