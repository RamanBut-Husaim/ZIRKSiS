using System;
using Stenography.Core.Image;

namespace Stenography.Image.Console
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            if ("e".Equals(args[0], StringComparison.OrdinalIgnoreCase))
            {
                var rng = new PatchworkRNG(args[1]);
                var service = new PatchworkTransformService(rng);
                using (var tr = new ImagePatchworkTransformService(service, args[2]))
                {
                    tr.ApplyTransform();
                    tr.Save(args[3]);
                }
            }
            else
            {
                var rng = new PatchworkRNG(args[1]);
                var service = new PatchworkTransformService(rng);
                using (var tr = new ImagePatchworkTransformService(service, args[2]))
                {
                    var value = tr.Analyze();
                    System.Console.WriteLine("Difference: {0}", Math.Abs(value));
                }
            }

            System.Console.WriteLine("The operation has been completed successfully.");
            System.Console.ReadLine();
        }
    }
}
