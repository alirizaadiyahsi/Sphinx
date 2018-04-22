import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { WeatherForecast } from './models/weather-forecast';

@Component
export default class FetchDataComponent extends Vue {
    forecasts: WeatherForecast[] = [];

    mounted() {
        fetch('http://localhost:59813/api/Values/WeatherForecasts')
            .then(response => response.json() as Promise<WeatherForecast[]>)
            .then(data => {
                this.forecasts = data;
            });
    }
}
