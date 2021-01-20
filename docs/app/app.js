let userId;

//#region *** Global Functions ***

const secToTimeNotation = function (seconds) {
  let time = new Date(seconds * 1000).toISOString().substr(11, 8);
  return time;
};

const secToRankingTimeNotation = function (sec_num) {
  var hours = Math.floor(sec_num / 3600);
  var minutes = Math.floor((sec_num - hours * 3600) / 60);
  var seconds = sec_num - hours * 3600 - minutes * 60;

  let output;
  if (sec_num == 0) {
    output = "";
  } else if (hours < 1) {
    output = `+${minutes}'${seconds}"`;
  } else {
    output = `+${hours}u${minutes}'${seconds}"`;
  }

  return output;
};

const datetimeToDateNotation = function (date) {
  date = new Date(date);
  let day = date.getDate(),
    month = date.getMonth() + 1,
    year = date.getFullYear();
  if (day < 10) {
    day = `0${day}`;
  }
  if (month < 10) {
    month = `0${month}`;
  }
  return `${day}/${month}/${year}`;
};

//#endregion

//#region *** Listen To ***

const listenToClickRound = function () {
  const rounds = document.querySelectorAll(".js-rounds-table-row");
  for (const item of rounds) {
    item.addEventListener("click", function () {
      window.location.href = `ronde_detail.html?roundId=${this.getAttribute(
        "data-roundId"
      )}`;
    });
  }
};

const listenToClickEtappe = function () {
  const rounds = document.querySelectorAll(".js-etappes-table-row");
  for (const item of rounds) {
    item.addEventListener("click", function () {
      localStorage.setItem(
        "etappeTitle",
        this.getAttribute("data-etappeTitle")
      );
      window.location.href = `etappe_detail.html?etappeId=${this.getAttribute(
        "data-etappeId"
      )}`;
    });
  }
};

const listenToToggle = function () {
  const etappeInput = document.querySelector(".js-option-etappe"),
    roundsRankingInput = document.querySelector(".js-option-rounds-ranking"),
    inputs = document.querySelectorAll(".js-option"),
    rankingContainer = document.querySelector(".c-ranking-container");
  for (const input of inputs) {
    input.addEventListener("change", function () {
      let urlParams = new URLSearchParams(window.location.search);
      const roundId = urlParams.get("roundId");
      if (etappeInput.checked) {
        rankingContainer.classList.remove("c-rounds-ranking--visible");
        document.querySelector('.js-etappes-table').innerHTML = '';
        showLoader();
        getEtappes(roundId);
      }
      if (roundsRankingInput.checked) {
        rankingContainer.classList.add("c-rounds-ranking--visible");
        document.querySelector('.js-rounds-ranking-table').innerHTML = '';
        showLoader();
        getRoundsRanking(roundId);
      }
    });
  }
};

const listenToClickGraphButton = function () {
  const btn = document.querySelector(".js-graph-button");
  btn.addEventListener("click", function () {
    window.location.href = `etappe_grafiek.html?etappeId=${this.getAttribute(
      "data-etappeId"
    )}`;
  });
};

const listenToClickLogo = function () {
  document
    .querySelector(".js-header-logo")
    .addEventListener("click", function () {
      window.location.pathname = "ronde_overzicht.html";
    });
};

//#endregion

//#region *** Show Data Functions ***

const showRounds = function (data) {
  data.sort((a, b) => (a.startDatum < b.startDatum ? 1 : -1));
  const table = document.querySelector(".js-rounds-table");
  let htmlString = `<div class="c-ranking-table__header">
  <p class="c-ranking-table__header-item">
    Startdatum
  </p>
  <p class="c-ranking-table__header-item u-text-align--left">
    Ronde
  </p>
  <p class="c-ranking-table__header-item">
    Totale tijd
  </p>
  <p class="c-ranking-table__header-item u-mr-clear">
    Positie
  </p>
</div>`;

//  Checken of data is not zero -> yes: user feedback;
  if(data.length == 0){
    htmlString += `
    <div class="c-ranking-table__row u-justify-content--center">U heeft nog geen data om weer te geven.</div>`
  }
  else{
    for (const item of data) {
      htmlString += `
          <div class="c-ranking-table__row js-rounds-table-row u-show-pointer" data-roundId='${
            item.rondeId
          }'>
        <p class="c-ranking-table__row-item">
        ${datetimeToDateNotation(item.startDatum)}
        </p>
        <div class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left">
          <p class="c-ranking-table__row-item--top">${item.rondeNaam}</p>
          <p class="c-ranking-table__row-item--sub">${
            item.aantalEtappes
          } etappes</p>
        </div>
        <p class="c-ranking-table__row-item">
        ${secToTimeNotation(item.totaalTijd)}
        </p>
        <p class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha u-mr-clear">
        #${item.plaats}
        </p>
      </div>`;
    }
  }
  hideLoader();
  table.innerHTML = htmlString;
  listenToClickRound();
  
};

const showRoundsRanking = function (data) {
  const table = document.querySelector(".js-rounds-ranking-table");
  let htmlString = ` <div class="c-ranking-table__header">
  <p class="c-ranking-table__header-item u-flex-basis-1-of-3">Positie</p>
  <p class="c-ranking-table__header-item u-flex-basis-1-of-3 u-text-align--left">
    Deelnemer
  </p>
  <p class="c-ranking-table__header-item u-flex-basis-1-of-3 u-mr-clear">Totale tijd</p>
</div>`;
  let fastestTime = data[0].totaalTijd;
  for (const item of data) {
    if (item.totaalTijd == fastestTime) {
      htmlString += `<div class="c-ranking-table__row">
      <p
        class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha c-result-item"
      >
      #${item.plaats}
      </p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left c-result-item">${item.gebruikersNaam.toUpperCase()}</p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--total-time c-result-item u-mr-clear">${secToTimeNotation(
        item.totaalTijd
      )}</p>
    </div>`;
    } else {
      htmlString += `
    <div class="c-ranking-table__row">
              <p
                class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha c-result-item"
              >
              #${item.plaats}
              </p>
              <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left c-result-item">${item.gebruikersNaam.toUpperCase()}</p>
              <div
                class="c-ranking-table__row-item c-ranking-table__row-item--total-time c-result-item u-mr-clear"
              >
                <p class="c-ranking-table__row-item--main">${secToTimeNotation(
                  item.totaalTijd
                )}</p>
                <p class="c-ranking-table__row-item--sub">${secToRankingTimeNotation(
                  item.totaalTijd - fastestTime
                )}</p>
              </div>
            </div>`;
    }
  }
  hideLoader();
  table.innerHTML = htmlString;
  
};

const showEtappes = function (data) {
  data.sort((a, b) => (a.startTijd < b.startTijd ? 1 : -1));
  const table = document.querySelector(".js-etappes-table");
  let htmlString = `      <div class="c-ranking-table__header">
  <p class="c-ranking-table__header-item">
    Startdatum
  </p>
  <p class="c-ranking-table__header-item u-text-align--left">
    Ronde
  </p>
  <p class="c-ranking-table__header-item">
    Totale tijd
  </p>
  <p class="c-ranking-table__header-item u-mr-clear">
    Positie
  </p>
</div>`;
//  Checken of data is not zero -> yes: user feedback;
if(data.length == 0){
  htmlString += `
  <div class="c-ranking-table__row u-justify-content--center">U heeft nog geen data om weer te geven.</div>`
}
else{
  let aantalEtappes = 0;
  for (const item of data) {
    if (item.etappeActief == false) {
      aantalEtappes++;
    }
  }
  for (const item of data) {
    if (item.etappeActief != true) {
      htmlString += `
      <div class="c-ranking-table__row js-etappes-table-row u-show-pointer" data-etappeId='${
        item.etappeId
      }' data-etappeTitle='Etappe ${aantalEtappes}'>
    <p class="c-ranking-table__row-item">
    ${datetimeToDateNotation(item.startTijd)}
    </p>
    <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left">Etappe 
    ${aantalEtappes}
    </p>
    <p class="c-ranking-table__row-item">
    ${secToTimeNotation(item.totaalTijd)}
    </p>
    <p class="c-ranking-table__row-item c-ranking-table__row-item--position u-mr-clear u-color-alpha ">
    #${item.plaats}
    </p>
  </div>`;
      aantalEtappes--;
    }
  }
  
  
}
  hideLoader();
  table.innerHTML = htmlString;
  listenToClickEtappe();
  
};

const showRoundInfo = function (data) {
  const roundName = document.querySelector(".js-round-name");
  roundName.innerText = data.rondeNaam;
};

const showEtappesRanking = function (data) {
  const table = document.querySelector(".js-etappes-ranking-table");
  let htmlString = `<div class="c-ranking-table__header">
  <p class="c-ranking-table__header-item u-flex-basis-1-of-3">
    Positie
  </p>
  <p
    class="c-ranking-table__header-item u-flex-basis-1-of-3 u-text-align--left"
  >
    Deelnemer
  </p>
  <p
    class="c-ranking-table__header-item u-flex-basis-1-of-3 u-mr-clear"
  >
    Totale tijd
  </p>
</div>`;
  let fastestTime = data[0].totaalTijd;
  for (const item of data) {
    if (item.totaalTijd == fastestTime) {
      htmlString += `<div class="c-ranking-table__row">
      <p
        class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha c-result-item"
      >
      #${item.plaats}
      </p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left c-result-item">${item.gebruikersNaam.toUpperCase()}</p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--total-time c-result-item u-mr-clear">${secToTimeNotation(
        item.totaalTijd
      )}</p>
    </div>`;
    } else {
      htmlString += `
    <div class="c-ranking-table__row">
              <p
                class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha c-result-item"
              >
              #${item.plaats}
              </p>
              <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left c-result-item">${item.gebruikersNaam.toUpperCase()}</p>
              <div
                class="c-ranking-table__row-item c-ranking-table__row-item--total-time c-result-item u-mr-clear"
              >
                <p class="c-ranking-table__row-item--main">${secToTimeNotation(
                  item.totaalTijd
                )}</p>
                <p class="c-ranking-table__row-item--sub">${secToRankingTimeNotation(
                  item.totaalTijd - fastestTime
                )}</p>
              </div>
            </div>`;
    }
  }
  hideLoader();
  table.innerHTML = htmlString;
  
};

const showEtappeInfo = function (data) {
  const etappeInfo = document.querySelector(".js-etappe-info"),
    etappeName = document.querySelector(".js-etappe-name");
  etappeInfo.innerText = `RNDS: ${data.laps} - DLN: ${
    data.aantalDeelnemers
  } - AFSTAND: ${
    Math.round((data.afstand + Number.EPSILON) * 100) / 100
  } KM - GEM SN: ${
    Math.round(
      ((3600 / data.totaalTijd) * data.afstand + Number.EPSILON) * 100
    ) / 100
  } KM/U`;
  etappeName.innerText = `${data.rondeNaam} - ${localStorage.getItem(
    "etappeTitle"
  )}`;
};

const showEtappeUserData = function (data) {
  const etappeInfo = document.querySelector(".js-etappe-info");
  etappeInfo.innerHTML = `<div class="c-etappe-info-container__item">
  <p class="c-etappe-info-subtitle">Gemiddelde rondetijd</p>
  <p class="c-etappe-info-data">${secToTimeNotation(data.gemiddeldeLaptijd)}</p>
</div>
<div class="c-etappe-info-container__item">
<p class="c-etappe-info-subtitle">Snelste rondetijd</p>
<p class="c-etappe-info-data">${secToTimeNotation(data.snelsteLapTijd)}</p>
</div>
<div class="c-etappe-info-container__item">
<p class="c-etappe-info-subtitle">Traagste rondetijd</p>
<p class="c-etappe-info-data">${secToTimeNotation(data.traagsteLapTijd)}</p>
</div>
  `;
  document.querySelector(".js-etappe-head").innerText = `${
    data.rondeNaam
  } - ${localStorage.getItem("etappeTitle")}`;
};

const showEtappeUserChartData = function (data) {
  let converted_labels = [];
  let converted_data = [];
  for (const item of data) {
    converted_labels.push(item.lapNummer);
    converted_data.push(item.tijdLap);
  }

  let ctx = document.querySelector(".js-etappe-chart").getContext("2d");

  let config = {
    type: "line",
    data: {
      labels: converted_labels,
      datasets: [
        {
          label: "seconden",
          backgroundColor: "#016fb7",
          borderColor: "#016fb7",
          data: converted_data,
          pointRadius: 3,
          pointHoverRadius: 8,
          fill: false,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: false,
      title: {
        display: true,
        text: "Afgewerkte rondes",
      },
      tooltips: {
        mode: "index",
        intersect: true,
      },
      hover: {
        mode: "nearest",
        intersect: true,
      },
      scales: {
        xAxes: [
          {
            display: true,
            scaleLabel: {
              display: true,
              labelString: "Rondes",
            },
          },
        ],
        yAxes: [
          {
            display: true,
            scaleLabel: {
              display: true,
              labelString: "Ronde Tijd",
            },
              ticks : {
                reverse : true
            },
            
          },
        ],
      },
    },
  };
  hideLoader();
  let speedChart = new Chart(ctx, config);
  
};

const hideLoader = function () {
  const loaders = document.querySelectorAll(".js-data-loader");
  for(const item of loaders){
    item.classList.add("o-display-none");
  }
  
};

const showLoader = function () {
  const loaders = document.querySelectorAll(".js-data-loader");
  for(const item of loaders){
    item.classList.remove("o-display-none");
  }
};

const showUserName = function(userName){
  userName = userName.split(" ");
  document.querySelector('.js-user-name').innerText = userName[0].toUpperCase();
}

//#endregion

//#region *** Get Data Functions ***

const getRounds = async function () {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/site/gebruiker/rondes/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showRounds(data);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

const getEtappes = async function (rondeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruiker/ronde/etappes/${rondeId}/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showEtappes(data);
    showRoundInfo(data[0]);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

const getRoundsRanking = async function (roundId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/klassement/rondes/${roundId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showRoundsRanking(data);
    showRoundInfo(data[0]);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

const getEtappesRanking = async function (etappeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/klassement/etappes/${etappeId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showEtappesRanking(data);
    getEtappeInfo(etappeId);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

const getEtappeInfo = async function (etappeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/info/etappes/${etappeId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showEtappeInfo(data);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

const getEtappeUserData = async function (etappeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/info/etappe/users/${etappeId}/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showEtappeUserData(data);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

const getEtappeUserChartData = async function (etappeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/info/etappe/laptijden/users/${etappeId}/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showEtappeUserChartData(data);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

//#endregion

//#region *** Google Authentication ***

function onSignIn(googleUser) {
  // var profile = googleUser.getBasicProfile();
  // console.log("ID: " + profile.getId()); // Do not send to your backend! Use an ID token instead.
  // console.log("Name: " + profile.getName());
  // console.log("Image URL: " + profile.getImageUrl());
  // console.log("Email: " + profile.getEmail()); // This is null if the 'email' scope is not present.
}

function signOut() {
  var auth2 = gapi.auth2.getAuthInstance();
  auth2.signOut().then(function () {
    // console.log("User signed out.");
    localStorage.removeItem("gebruikerId");
    localStorage.removeItem("name");
    window.location.pathname = "/index.html";
  });
}

//#endregion

document.addEventListener("DOMContentLoaded", function () {

  //Listen to click logo -> go to home
  listenToClickLogo();

  // get userid and name from user:
  userId = localStorage.getItem("gebruikerId");
  userName = localStorage.getItem("name");


  if (userId == null || userName == null) {
    window.location.pathname = "/index.html";
  } else {

    showUserName(userName);

    if (document.querySelector(".js-ronde-overzicht")) {
      showLoader();
      getRounds();
    }

    if (document.querySelector(".js-ronde-detail")) {
      showLoader();
      let urlParams = new URLSearchParams(window.location.search);
      const roundId = urlParams.get("roundId");
      if (roundId == null) {
        window.location.pathname = "/ronde_overzicht.html";
      } else {
        getEtappes(roundId);
        listenToToggle();
      }
    }

    if (document.querySelector(".js-etappe-detail")) {
      showLoader();
      let urlParams = new URLSearchParams(window.location.search);
      const etappeId = urlParams.get("etappeId");
      if (etappeId == null) {
        window.location.pathname = "/ronde_detail.html";
      } else {
        getEtappesRanking(etappeId);
        document
          .querySelector(".js-graph-button")
          .setAttribute("data-etappeId", etappeId);
        listenToClickGraphButton();
      }
    }

    if (document.querySelector(".js-etappe-grafiek")) {
      showLoader();
      let urlParams = new URLSearchParams(window.location.search);
      const etappeId = urlParams.get("etappeId");
      if (etappeId == null) {
        window.location.pathname = "/ronde_detail.html";
      } else {
        getEtappeUserData(etappeId);
        getEtappeUserChartData(etappeId);
      }
    }
  }
});
