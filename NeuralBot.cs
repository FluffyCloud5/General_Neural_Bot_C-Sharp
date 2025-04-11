using System.Collections;
using System.Diagnostics;
using System.Reflection.Metadata;

public class NeuralBot
    {
        public NeuralBot(int inputSize,int[] layerSizes, int outputSize) // all of theses are excluding the bias that will be added to the Matricies. 
        {
            // layerSizes.Length + 1 is how many matrixes there will be.
            // each matrix Mi for i in [2,layerSizes.Length] is equal to (layerSize[i-3])*(layerSize[i-2]+1) 

            //CHECKS
            #region
            if (inputSize <= 0 || outputSize <= 0) throw new ArgumentOutOfRangeException();
            for (int i = 0; i < layerSizes.Length; i++)
            {
                if (layerSizes[i] <= 0 ) throw new ArgumentOutOfRangeException();
            }
            #endregion


            //Initializing the matrixes
            #region
            int[] layers = new int[2 + layerSizes.Length];

            layers[0] = inputSize;
            layers[layers.Length - 1] = outputSize;
            for (int i = 1; i < layers.Length - 1; i++)
            {
                layers[i] = layerSizes[i - 1];
            }


            
            transformations = new Matrix[layers.Length-1];

            for (int i = 0; i < transformations.Length; i++)
            {
                transformations[i] = new Matrix(layers[i + 1]+1, layers[i]+1);
            }




#endregion
        }

        public Matrix[] transformations; // these all account for an extra bias

        private static double SquashingFunction(double num)
        {
            return 2 / (1 + Math.Exp(-num)) -1;
        }

        private static Matrix SquashingFunction(Matrix matrix)
        {
            if (matrix == null) throw new ArgumentNullException();
            if (matrix.cols != 1) throw new ArgumentOutOfRangeException();
            if (matrix.rows <1) throw new ArgumentOutOfRangeException();

            for (int i = 0; i < matrix.rows-1; i++)
            {
                matrix[i,0] = SquashingFunction(matrix[i,0]);
            }
            matrix[matrix.rows - 1, 0] = 1;

            return matrix;

        }

        public Matrix ComputeOutput(Matrix input)
        {
            if(input == null) throw new ArgumentNullException();
            if(input.cols != 1) throw new ArgumentOutOfRangeException();
            if (input.rows != transformations[0].cols) throw new Exception("INPUT MUST BE OF CORRECT SIZE");
            
            Matrix intermidiateLayer = transformations[0] * input;

            for (int i = 1; i < transformations.Length; i++)
            {
                SquashingFunction(intermidiateLayer);

                intermidiateLayer = transformations[i] * intermidiateLayer;
            }


            if (intermidiateLayer.cols != 1) throw new Exception("Transformations went wrong");

            Matrix output = new Matrix(intermidiateLayer.rows - 1, 1);

            for(int i = 0; i < output.rows; i++)
            {
                output[i, 0] = intermidiateLayer[i, 0];
            }


            return output;        
        }
    }
