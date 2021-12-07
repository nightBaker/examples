

using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using Spectre.Console;



var client = new BinanceClient(new BinanceClientOptions()
{
    ApiCredentials = new ApiCredentials("sOtUHvYU4uwLbEENnG7Sw8LNai2gMUQgOipRp8kHAPNSTs75J3wapfvWUxXpgdCi", "BSu5xyXMrbgVuQBJYpv2IQBFUh1mJd3j9GTRMoEqK02ljm7uyu1bcuDs6rWfQdGP")
});



var predictor = GetPredictionEngine(out var mlContext);
var lastPrediction = 0f;

       
while (true)
{
    var callResult = await client.Spot.Market.GetPriceAsync("BTCBUSD");
    // Make sure to check if the call was successful
    if (!callResult.Success)
    {

    }
    else
    {
        var prediction = predictor.Predict(new CoinPrice { Price = (float) callResult.Data.Price });

        var rightPredicted = Math.Abs((float)callResult.Data.Price - lastPrediction) < 10;

        var row = $"{(rightPredicted ? "[green]" : "[red]") + callResult.Data.Price + "[/]"} " + String.Join(' ', prediction.Score);

        predictor.CheckPoint(mlContext, "TrainedModel");
        
        lastPrediction = prediction.Score[0];

        AnsiConsole.MarkupLine(row);

        await Task.Delay(60000);
    }
}
    


TimeSeriesPredictionEngine<CoinPrice, CoinPricePrediction> GetPredictionEngine(out MLContext mlContext)
{
    mlContext = new MLContext(seed: 0);
    
    if (File.Exists("TrainedModel"))
    {
        // Load the forecast engine that has been previously saved.
        ITransformer forecaster;
        using (var file = File.OpenRead("TrainedModel"))
        {
            forecaster = mlContext.Model.Load(file, out DataViewSchema schema);
        }

        // We must create a new prediction engine from the persisted model.
        return forecaster.CreateTimeSeriesEngine<CoinPrice, CoinPricePrediction>(mlContext);
    }
    else
    {
        // STEP 1: Common data loading configuration
        IDataView baseTrainingDataView = mlContext.Data.LoadFromTextFile<CoinPrice>("trainingData.csv", hasHeader: false, separatorChar: ',');

            var trainer = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(CoinPricePrediction.Score),
                inputColumnName: nameof(CoinPrice.Price),
                windowSize: 60,
                seriesLength: 60 * 60 * 24,
                trainSize: 60 * 60 * 24,
                horizon: 5,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "Features",
                confidenceUpperBoundColumn: "Features");

        //var trainingPipeline = dataProcessPipeline.Append(trainer);

        ITransformer trainedModel = trainer.Fit(baseTrainingDataView);

        return trainedModel.CreateTimeSeriesEngine<CoinPrice, CoinPricePrediction>(mlContext);
    }
   
}

public class CoinPricePrediction
{
    [ColumnName("Score")]
    public float[] Score;
}

public class CoinPrice 
{
    [LoadColumn(0)]
    public float Price { get; set; }
}
