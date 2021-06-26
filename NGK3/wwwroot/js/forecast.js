
var connection = new signalR.HubConnectionBuilder().withUrl("/weatherHub").build();

//Disable send button until connection is established
document.getElementById("generateForecast").disabled = true;

connection.start().then(function () {
    document.getElementById("generateForecast").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("liveUpdate", function () {
    getLatestForcast();
});

document.getElementById("generateForecast").addEventListener("click", function (event) {
    let temp = {
        Date: new Date(),
        Location: {
            Name: "Chokoladen",
            Lat: 56.17,
            Lon: 10.18
        },
        TemperatureC: Math.floor(Math.random() * 56) - 20,
        Humidity: Math.floor(Math.random() * 101),
        AirPressure: Math.floor(Math.random() * 1031) + 980
    };
    fetch('api/WeatherForecast/GenerateForecast',
        {
            method: "POST",
            credentials: "include",
            body: JSON.stringify(temp),
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("token"),
                'Content-Type': 'application/json'
            }
        })
        .then()
        .catch((err) => {
            console.log(err.toString());
        });
    event.preventDefault();
});

async function getLatestForcast() {
    try {
        let response = await fetch('api/WeatherForecast/Latest',
            {
                method: "GET",
                credentials: "include",
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem("token"),
                    'Content-Type': 'application/json'
                }
            });
        if (response.ok) {
            let forecast = await response.json();
            document.getElementById("liveLocationName").textContent = forecast.location.name;
            document.getElementById("liveLocationLat").textContent = forecast.location.lat;
            document.getElementById("liveLocationLon").textContent = forecast.location.lon;
            document.getElementById("liveDateTime").textContent = forecast.date;
            document.getElementById("liveTemperature").textContent = forecast.temperatureC;
            document.getElementById("liveHumidity").textContent = forecast.humidity;
            document.getElementById("liveAirPressure").textContent = forecast.airPressure;
        } else {
            alert("Server returned: " + response.statusText);
        }
    } catch (err) {
        alert("Error: " + err);
    }
}

async function login() {
    let temp = {
        email: document.getElementById("textEmail").value,
        password: document.getElementById("textPassword").value
    };
    try {
        let response = await fetch("api/Account/Login",
            {
                method: "POST",
                body: JSON.stringify(temp),
                headers: new Headers({
                    "Content-Type": "application/json"
                })
            });
        if (response.ok) {

            let token = await response.json();
            localStorage.setItem("token", token.jwt);

        } else {
            alert("Server returned: " + response.statusText);
        }
    } catch (err) {
        alert("Error: " + err);
    }
    return;
}

document.getElementById("loginButton").addEventListener("click", function (event) {
    login();
});

function parseJwt(token) {
    var base64Url = token.split(".")[1];
    var base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    var jsonPayload = decodeURIComponent(
        atob(base64)
            .split("")
            .map(function (c) {
                return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
            })
            .join(""));
    return JSON.parse(jsonPayload);
}