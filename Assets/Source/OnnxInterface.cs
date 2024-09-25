using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Barracuda;

// namespace NNTracking
// {
    public class OnnxInterface
    {
        public int BatchSize { get; }
        public int NumChannels { get; }
        public int Width { get; }
        public int Height { get; }
        public int ImSize { get { return _im_size; } } 

        // public uint batch_size = 99999;
        // public uint n_channels = 99999;
        // public uint width = 99999;
        // public uint height = 99999;

        private Model _runtimeModel;
        private IWorker _worker;
        private int _im_size;
        private const string prediction_name = "onnx-demo";

        public OnnxInterface(NNModel model, int width, int height, int n_channels, int batch_size)
        {
            this.Width = width;
            this.Height = height;
            this.NumChannels = n_channels;
            this.BatchSize = batch_size;

            _im_size = checked((int)(Width *Height*NumChannels*BatchSize));

            _runtimeModel = ModelLoader.Load(model);
            _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputeRef , _runtimeModel);
        }

        public Tensor RunPrediction(Texture2D input_im)
        {
            // _im_size = checked((int)(Width *Height*NumChannels*BatchSize));


            // Texture2D tex = LoadImage();


            // float[] input_im = new float[_im_size];

            // for (int i=0; i<_im_size; i++)
            // {
            //     input_im[i] = 1f;
            // }

            var input = new Tensor(input_im, NumChannels, prediction_name);

            // var input = new Unity.Barracuda.Tensor(
            //     checked((int)BatchSize), 
            //     checked((int)Height), 
            //     checked((int)Width), 
            //     checked((int)NumChannels), 
            //     input_im, prediction_name); 

            Debug.Log($"ïnput_im dimension: {input_im.dimension}");

            _worker.Execute(input);

            Tensor output = _worker.PeekOutput("output0");
            //Tensor output = _worker.PeekOutput();


            Debug.Log("[OnnxInterface] Output:");
            Debug.Log($"[OnnxInterface] name: {output.name}");
            Debug.Log($"[OnnxInterface] shape: {output.shape}");

            input.Dispose();
            //output.Dispose();

            return output;
        }




    }


// }