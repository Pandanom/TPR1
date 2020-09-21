using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TPR1
{
    class ArrayReader
    {
        public struct Arrays
        {
            public int size { get; private set; }
            public double[,] m;
            public double[,,] k;

            public Arrays(int size)
            {
                this.k = new double[size, size, size];
                this.m = new double[size, size];
                this.size = size;
            }
            public void writeArrays()
            {
                Console.Write("M = [");
                for (int i = 0; i < size; i++)
                {
                    Console.Write("{");
                    for (int j = 0; j < size; j++)
                        Console.Write(m[i, j].ToString(CultureInfo.InvariantCulture) + (j == 3 ? "" : ", "));
                    Console.WriteLine("}" + (i == 3 ? "];" : ","));
                }
                for (int z = 0; z < size; z++)
                {
                    Console.Write(String.Format("K{0} = [",z));
                    for (int i = 0; i < size; i++)
                    {
                        Console.Write("{");
                        for (int j = 0; j < size; j++)
                            Console.Write(k[z, i, j].ToString(CultureInfo.InvariantCulture) + (j == 3 ? "" : ", "));
                        Console.WriteLine("}" + (i == 3 ? "];" : ","));
                    }
                }
            }
            public double[][,] toArrayOfArrays()
            {
                double[][,] ret = new double[size + 1][,];
                ret[0] = m;
                for (int z = 0; z < size; z++)
                {
                    ret[z + 1] = new double[size, size];
                    for (int i = 0; i < size; i++)
                        for (int j = 0; j < size; j++)
                            ret[z + 1][i, j] = k[z, i, j];
                }
                return ret;
            }
        }



        string path;
        string content;      

        public void setPath(string path)
        {
            this.path = path;
        }

        public void readData()
        {
            content = File.ReadAllText(path, Encoding.UTF8);
        }

        public Arrays getArrays()
        {
            int size = 4;
            var ret = new Arrays(size);
            int arrNum = -1;
            string pattern = @"\[([^\[\]]+)\]";
            foreach (Match m in Regex.Matches(content, pattern))
            {
                for (int i = 0; i < size; i++)
                {
                    var buff = m.Groups[1].Value.Split("[]{}\n\r".ToCharArray(), 5,StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < size; j++)
                        if (arrNum < 0)
                        {
                            var num = buff[i].Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[j];
                            if (num.Contains("/"))
                            {
                                double first, second;
                                if (!(double.TryParse(num.Split('/')[0], out first) && double.TryParse(num.Split('/')[1], out second)))
                                    throw new System.ArgumentException(string.Format("File isn't parsable arr №{0}, i = {1}, j ={2}", arrNum, i, j), path);
                                ret.m[i, j] = Math.Round(first / second, 2);


                            }
                            else if (!double.TryParse(num, out ret.m[i, j]))
                                throw new System.ArgumentException(string.Format("File isn't parsable arr №{0}, i = {1}, j ={2}", arrNum, i, j), path);
                        }
                        else
                        {
                            var num = buff[i].Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[j];
                            if (num.Contains("/"))
                            {
                                double first, second;
                                if (!(double.TryParse(num.Split('/')[0], out first) && double.TryParse(num.Split('/')[1], out second)))
                                    throw new System.ArgumentException(string.Format("File isn't parsable arr №{0}, i = {1}, j ={2}", arrNum, i, j), path);
                                ret.k[arrNum, i, j] = Math.Round(first / second, 2);


                            }
                            else if (!double.TryParse(num, out ret.k[arrNum, i, j]))
                                throw new System.ArgumentException(string.Format("File isn't parsable arr №{0}, i = {1}, j ={2}", arrNum, i, j), path);
                        }
                    
                }
                arrNum++;
            }

            return ret;
        }

    }
}
