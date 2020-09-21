using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPR1
{
    class HierarchyCalculator
    {
        double[][] matrixVect;
        double[][] priorVect;
        double[][] Y;
        double[] lambdaMax, consistencyIndex, globalPrior;
        int size;
        public void compute(double[][,] arrays, int size)
        {
            matrixVect = new double[size+1][];
            priorVect = new double[size + 1][];
            lambdaMax = new double[size + 1];
            globalPrior = new double[size];
            consistencyIndex = new double[size + 1];
            this.size = size;
            Y = new double[size + 1][];
            for (int i = 0; i < size+1; i++)
            {
                matrixVect[i] = new double[size];
                priorVect[i] = new double[size];
                Y[i] = new double[size];
            }
            for (int z = 0; z < size + 1; z++)
            {
                double wSum = 0;
                for (int i = 0; i < size; i++)
                {
                    double w = 1;
                    for (int j = 0; j < size; j++)
                        w *= arrays[z][i, j];
                    matrixVect[z][i] = Math.Pow(w, 1.0 / size);
                    wSum += matrixVect[z][i];
                }
                for (int i = 0; i < size; i++)
                {
                    priorVect[z][i] = matrixVect[z][i] / wSum;
                    Y[z][i] = 0;                  
                }
                double lambdaSum = 0;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                        Y[z][i] += priorVect[z][j] * arrays[z][i, j];                    
                        lambdaSum += Y[z][i] / priorVect[z][i];
                }
                lambdaMax[z] = lambdaSum / size;
                consistencyIndex[z] = (lambdaMax[z] - size) / (size - 1);
            }

            for (int i = 1; i < size + 1; i++)
            {
                double sum = 0;
                for (int j = 0; j < size; j++)
                    sum += priorVect[0][j] * priorVect[i][j];
                globalPrior[i-1] = sum;
            }

        }
        public void writeResults()
        {
            Console.WriteLine("Матриця M: ");
            Console.Write("Власний вектор матрицi: ");
            writeArray(matrixVect[0], size);
            Console.Write("Вектор прiоритетiв X: ");
            writeArray(priorVect[0], size);
            Console.Write("Y:");
            writeArray(Y[0], size);
            Console.Write("Максимальне власне число матрицi: ");
            Console.WriteLine(lambdaMax[0].ToString(CultureInfo.InvariantCulture));
            Console.Write("Iндекс узгодженостi: ");
            Console.WriteLine(consistencyIndex[0].ToString(CultureInfo.InvariantCulture));
            Console.WriteLine("---------------------------------");
            for (int i = 1; i < size + 1; i++)
            {
                Console.WriteLine(String.Format("Матриця K{0}: ",i-1));
                Console.Write("Власний вектор матрицi: ");
                writeArray(matrixVect[i], size);
                Console.Write("Вектор прiоритетiв X: ");
                writeArray(priorVect[i], size);
                Console.Write("Максимальне власне число матрицi: ");
                Console.WriteLine(lambdaMax[i].ToString(CultureInfo.InvariantCulture));
                Console.Write("Iндекс узгодженостi: ");
                Console.WriteLine(consistencyIndex[i].ToString(CultureInfo.InvariantCulture));
                Console.WriteLine("---------------------------------");
            }
            Console.Write("Вектор глобальних прiоритетiв: ");
            writeArray(globalPrior, size);
        }

        void writeArray(double[] arr, int size)
        {
            Console.Write("{");
            for(int i = 0; i < size; i ++)
            Console.Write(Math.Round(arr[i], 2).ToString(CultureInfo.InvariantCulture) + (i == size -1 ? "}" : ", "));
            Console.WriteLine();
        }
        void writeArray(double[,] arr, int sizeX, int sizeY)
        {
            Console.Write("[");
            for (int i = 0; i < sizeX; i++)
            {
                Console.Write("{");
                for (int j = 0; j < sizeY; j++)
                    Console.Write(Math.Round(arr[i, j],2).ToString(CultureInfo.InvariantCulture) + (j == sizeY-1 ? "" : ", "));
                Console.WriteLine("}" + (i == sizeX-1 ? "];" : ","));
            }
        }
    }
}
