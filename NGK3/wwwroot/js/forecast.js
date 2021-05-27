

var connection = new signalR.HubConnectionBuilder().withUrl("/weatherHub").build();

//Disable send button until connection is established
document.getElementById("generateForecast").disabled = true;

connection.start().then(function () {
    document.getElementById("generateForecast").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("liveUpdate", function (forecast) {
    //document.getElementById("liveLocationName").value = forecast;
    document.getElementById("liveLocationName").value = forecast.Location.Name;
    document.getElementById("liveLocationLat").value = forecast.Location.Lat;
    document.getElementById("liveLocationLon").value = forecast.Location.Lon;
    document.getElementById("liveDateTime").value = forecast.Date;
    document.getElementById("liveTemperature").value = forecast.TemperatureC;
    document.getElementById("liveHumidity").value = forecast.Humidity;
    document.getElementById("liveAirPressure").value = forecast.AirPressure;
});

document.getElementById("generateForecast").addEventListener("click", function (event) {
    fetch('api/WeatherForecast/GenerateForecast',
        {
            method: "GET",
            //credentials: "include",
            //headers: {
            //    'Authorization': 'Bearer ' + localStorage.getItem("token"),
            //    'Content-Type': 'application/json'
            //}
        })
        .then()
        .catch((err) => {
            console.log(err.toString());
        });
    event.preventDefault();
});

document.getElementById("getLatests").addEventListener("click", function (event) {
    fetch('api/WeatherForecast/Latest')
        .then(responseJson => responseJson.json())
        .then(json_data => this.jobs = json_data)
        .catch((err) => {
            console.log(err.toString());
        });
    event.preventDefault();
});