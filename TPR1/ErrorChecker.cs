using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPR1
{
    class ErrorChecker
    {
        ArrayReader.Arrays arrays;

        public ErrorChecker(ArrayReader.Arrays arrays)
        {
            this.arrays = arrays;
        }

        public bool check()
        {
            bool ret = true;
            for (int i = 0; i < arrays.size; i++) // diag
            {
                if (arrays.m[i, i] != 1)
                {
                    Console.WriteLine(String.Format("m[{0},{1}] != 1", i, i));
                    ret = false;
                }
                for (int z = 0; z < arrays.size; z++)
                    if (arrays.k[z, i, i] != 1)
                    {
                        Console.WriteLine(String.Format("k{2}[{0},{1}] != 1", i, i, z));
                        ret = false;
                    }
            }
            for (int i = 0; i < arrays.size; i++)
                for (int j = i; j < arrays.size; j++)
                {
                    if (arrays.m[i, j] != Math.Round(1/arrays.m[j,i],2) && arrays.m[j, i] != Math.Round(1 / arrays.m[i, j], 2))
                    {
                        Console.WriteLine(String.Format("m[{0},{1}] != 1/m[{1},{0}]", i, j));
                        ret = false;
                    }
                    for (int z = 0; z < arrays.size; z++)
                        if (arrays.k[z, i, j] != Math.Round(1 / arrays.k[z,j, i], 2) && arrays.k[z,j, i] != Math.Round(1 / arrays.k[z,i, j], 2))
                        {
                            Console.WriteLine(String.Format("k{2}[{0},{1}] != 1/k{2}[{1},{0}]", i, j, z));
                            ret = false;
                        }


                }

            return ret;
        }

      
    }
}
