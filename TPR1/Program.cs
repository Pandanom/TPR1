using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPR1
{
    class Program
    {
        static void Main(string[] args)
        {
            ArrayReader.Arrays arrs = new ArrayReader.Arrays(1);
            bool f = false;
            while (!f)
            {
                Console.Write("Enter file path: ");
                f = true;
                var reader = new ArrayReader();
                try
                {
                    reader.setPath(Console.ReadLine());
                    reader.readData();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    f = false;
                }
                arrs = reader.getArrays();
                arrs.writeArrays();
                ErrorChecker eCh = new ErrorChecker(arrs);
                if (!eCh.check())
                {
                    Console.WriteLine("Errors detected in file!");
                    f = false;
                }
                
            }
            HierarchyCalculator calc = new HierarchyCalculator();
            calc.compute(arrs.toArrayOfArrays(), arrs.size);
            calc.writeResults();
        }
    }
}
