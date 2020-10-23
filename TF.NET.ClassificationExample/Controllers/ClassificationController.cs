using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using TF.NET.ClassificationExample.Entities;

namespace TF.NET.ClassificationExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassificationController : ControllerBase
    {
        private readonly ILogger<ClassificationController> _logger;

        private readonly EnginesConfig enginesConfig;

        private readonly MLContext mlContext;

        private readonly string modelFolder = "TFModels";

        public ClassificationController(EnginesConfig engCfg, ILogger<ClassificationController> logger)
        {
            _logger = logger;
            enginesConfig = engCfg;
            mlContext = new MLContext(seed: 1);

        }

        [HttpGet]
        [Route("classifyimage")]
        public IActionResult ClassifyImage()
        {
            string modelFile = Path.Combine(AppContext.BaseDirectory, modelFolder, enginesConfig.ModelName);
            string imagePath = Path.Combine(AppContext.BaseDirectory, "TestImages", "TestImage.png");

            Image image = Image.FromFile(imagePath);
            using var ms = new MemoryStream();

            image.Save(ms, image.RawFormat);
            var imageByte = ms.ToArray();


            InMemoryImageData imageData = new InMemoryImageData(imageByte, "image.jpg", string.Empty);
            if (!System.IO.File.Exists(modelFile))
                return NotFound();

            Console.WriteLine($"Memory before loading engine 0: {Process.GetCurrentProcess().WorkingSet64 / (double)(1024 * 1024 * 1024):0.####} GB");

            // create instances of engine and load them into the memory
            List<PredictionEngine<InMemoryImageData, ImagePred>> engines = LoadPool(modelFile);

            // every first call of Predict() of the prediction engine instance increases the memory by approximately 500MB. This is too much!!!
            CallPredicts(engines, imageData);
            
            // subsequent call of Predict() of engine behaves normally
            CallPredicts(engines, imageData);

            return Ok($"The test is finished. :). Total memory consumption: {Process.GetCurrentProcess().WorkingSet64 / (double)(1024 * 1024 * 1024):0.####} GB");
        }

        private List<PredictionEngine<InMemoryImageData, ImagePred>> LoadPool(string modelFullPathName)
        {
            List<PredictionEngine<InMemoryImageData, ImagePred>> engines = new List<PredictionEngine<InMemoryImageData, ImagePred>>();

            for (int i = 0; i < enginesConfig.PoolSize; i++)
            {

                var mlnetModel = mlContext.Model.Load(modelFullPathName, out _);
                var predictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePred>(mlnetModel);
                Console.WriteLine($"Memory before after engine {i}: {Process.GetCurrentProcess().WorkingSet64 / (double)(1000 * 1000 * 1000):0.####} GB");
                engines.Add(predictionEngine);
            }

            return engines;
        }

        private void CallPredicts(List<PredictionEngine<InMemoryImageData, ImagePred>> predictionEngines, InMemoryImageData imageData)
        {
            int i = 0;
            foreach (var engine in predictionEngines)
            {
                var result = engine.Predict(imageData);

                Console.WriteLine($"Memory after calling Predict of engine {i++}: {Process.GetCurrentProcess().WorkingSet64 / (double)(1000 * 1000 * 1000):0.####} GB");
            }
        }
    }
}
