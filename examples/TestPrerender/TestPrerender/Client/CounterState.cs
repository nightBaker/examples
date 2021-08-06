using BlazorState;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using TestPrerender.Shared;
using static TestPrerender.Client.WeatherForecastState;

namespace TestPrerender.Client
{
    public class CounterState : State<CounterState>
    {
        public int Count { get; private set; }

        public override void Initialize()
        {
            Count = 3;
        }
    }

    public class WeatherForecastState : State<CounterState>
    {
        public List<WeatherForecast> Forecasts { get; set; }

        public override void Initialize()
        {
            Forecasts = new List<WeatherForecast>();
        }

        public class LoadForecasts : IAction
        {

        }
    }

    public class LoadForecastsHandler : ActionHandler<LoadForecasts>
    {
        private HttpClient _httpClient;

        public LoadForecastsHandler(IStore aStore, HttpClient httpClient) : base(aStore)
        {
            _httpClient = httpClient;
        }

        WeatherForecastState WeatherForecastState => Store.GetState<WeatherForecastState>();

        public override async Task<Unit> Handle(LoadForecasts aIncrementCountAction, CancellationToken aCancellationToken)
        {
            WeatherForecastState.Forecasts = await _httpClient.GetFromJsonAsync<List<WeatherForecast>>("WeatherForecast");
            return Unit.Value;
        }
    }
}
