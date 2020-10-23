namespace TF.NET.ClassificationExample.Entities
{
    /// <summary>
    /// Class used for constructing training sat
    /// </summary>
    public class ImageData
    {
        #region Fields
        private readonly string _imagePath;
        private readonly string _label;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imagePath">Image path.</param>
        /// <param name="label">Actual image label.</param>
        public ImageData(string imagePath, string label)
        {
            _imagePath = imagePath;
            _label = label;
        }

        /// <summary>
        /// File path of a image.
        /// </summary>
        public string ImagePath => _imagePath;

        /// <summary>
        /// Image label.
        /// </summary>
        public string Label => _label;
    }

    /// <summary>
    /// Class used when predicting the products in images.
    /// </summary>
    public class ImagePred
    {
        /// <summary>
        /// Probability Score of prediction result.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public float[] Score { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// Prediction result label value.
        /// </summary>
        public string PredictedLabel { get; set; }
    }

    /// <summary>
    /// Used to show prediction result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Probability confidence of prediction
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// Predicted label value.
        /// </summary>
        public string Label { get; set; }
    }

    /// <summary>
    /// Used when dealing with images during training process when the images are in application memory. 
    /// </summary>
    public class InMemoryImageData
    {
        #region Fields
        private readonly byte[] _image;
        private readonly string _label;
        private readonly string _imageFileName;
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="image"> Memory representation of a image.</param>
        /// <param name="label">Actual image label</param>
        /// <param name="imageFileName">Image path.</param>
        public InMemoryImageData(byte[] image, string label, string imageFileName)
        {
            _image = image;
            _label = label;
            _imageFileName = imageFileName;
        }

        /// <summary>
        /// Memory representation of a image.
        /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Image => _image;
#pragma warning restore CA1819 // Properties should not return arrays

        /// <summary>
        /// Actual image label.
        /// </summary>
        public string Label => _label;

        /// <summary>
        /// Image path.
        /// </summary>
        public string ImageFileName => _imageFileName;
    }
}
