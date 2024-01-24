using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tensornet;
namespace meow
{
    internal class NeuralNetwork
    {
        private int[] numOfNeurons;
        public NeuralNetwork(int[] numOfNeurons)
        {
            this.numOfNeurons = numOfNeurons;
        }
    }

    internal abstract class Layer
    {
        private int layerSize;

        public Layer(int size)
        {
            layerSize = size;
        }
        public abstract Tensor<float> FeedForward(Tensor<float> floaty);
        public static void Relu(Tensor<float> floaty)
        {
            floaty.ForEachInplace(f => Math.Max(0, f));
        }
    }

    internal class Calculation : Layer
    {
        private int prevSize;
        public Func<float, float> ActivationFunction => _activationFunction;
        private Func<float, float> _activationFunction;

        private Tensor<float> weights;
        private Tensor<float> biases;

        public Calculation(int size, int prevSize, Func<float, float>? activationFunction = null) : base(size)
        {
            this.prevSize = prevSize;

            weights = Tensor.Zeros<float>(new TensorShape(prevSize, size));
            weights.ForEachInplace(f => Random.Shared.NextSingle() * 2 - 1);

            biases = Tensor.Zeros<float>(new TensorShape(size));
            biases.ForEachInplace(f => Random.Shared.NextSingle() * 2 - 1);

        }

        public override Tensor<float> FeedForward(Tensor<float> floaty)
        {
            // return weights * floaty + biases;
           // _activationFunction = activationFunction ?? Layer.Relu(weights * floaty + biases);

        }
    }

    internal class Output : Layer
    {
        private int prevSize;
        private Tensor<float> weights;
        private Tensor<float> biases;

        public Output(int size, int prevSize) : base(size)
        {
            this.prevSize = prevSize;
            weights = Tensor.Zeros<float>(new TensorShape(prevSize, size));
            weights.ForEachInplace(f => Random.Shared.NextSingle() * 2 - 1);

            biases = Tensor.Zeros<float>(new TensorShape(size));
            biases.ForEachInplace(f => Random.Shared.NextSingle() * 2 - 1);
        }

        public override Tensor<float> FeedForward(Tensor<float> floaty)
        {
            //normalize !!
            Tensor<float> temp = weights * floaty + biases;
            float sum = temp.Sum(f => f);
            temp.ForEachInplace(f => f / sum);
            return temp;
            //return weights * floaty + biases;
        }
    }

    internal class Input : Layer
    {
        public Input(int size) : base(size)
        {
            
        }

        public override Tensor<float> FeedForward(Tensor<float> floaty)
        {
            return floaty;
        }
    }


}
