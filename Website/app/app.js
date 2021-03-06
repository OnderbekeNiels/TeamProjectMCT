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

//Listen to toggle button that links to admin page
const listenToClickAdminPage = function () {
  const btn = document.querySelector(".js-admin-button");
  btn.addEventListener("click", function () {
    window.location.href = `ronde_overzicht_admin.html`;
  });
};

//Listen to toggle button that links to deelnemer page
const listenToClickDeelnemerPage = function () {
  const btn = document.querySelector(".js-deelnemer-button");
  btn.addEventListener("click", function () {
    window.location.href = `ronde_overzicht.html`;
  });
};

//Event listener that listens to a click on a round from the round overview on the deelnemer page
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

//Event listener that listens to a click on a round from the round overview on the admin page
const listenToClickRoundAdmin = function () {
  const rounds = document.querySelectorAll(".js-rounds-table-row");
  for (const item of rounds) {
    item.addEventListener("click", function () {
      window.location.href = `ronde_detail_admin.html?roundId=${this.getAttribute(
        "data-roundId"
      )}`;
    });
  }
};

//Event listener that listens to a click on a etappe from the etappe overview on the deelnemer page
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

//Event listener that listens to a click on a etappe from the etappe overview on the admin page
const listenToClickEtappeAdmin = function () {
  const rounds = document.querySelectorAll(".js-etappes-table-row");
  for (const item of rounds) {
    item.addEventListener("click", function () {
      localStorage.setItem(
        "etappeTitle",
        this.getAttribute("data-etappeTitle")
      );
      window.location.href = `etappe_detail_admin.html?etappeId=${this.getAttribute(
        "data-etappeId"
      )}`;
    });
  }
};

//Event listener of the toggle button that switches between klassement and etappe view
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
        document.querySelector(".js-etappes-table").innerHTML = "";
        showLoader();
        getEtappes(roundId);
      }
      if (roundsRankingInput.checked) {
        rankingContainer.classList.add("c-rounds-ranking--visible");
        document.querySelector(".js-rounds-ranking-table").innerHTML = "";
        showLoader();
        getRoundsRanking(roundId);
      }
    });
  }
};

//Event listener of the toggle button that switches between klassement and etappe view
const listenToToggleAdmin = function () {
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
        document.querySelector(".js-etappes-table").innerHTML = "";
        showLoader();
        getEtappesAdmin(roundId);
      }
      if (roundsRankingInput.checked) {
        rankingContainer.classList.add("c-rounds-ranking--visible");
        document.querySelector(".js-rounds-ranking-table").innerHTML = "";
        showLoader();
        getRoundsRanking(roundId);
      }
    });
  }
};

//Event listener of the button that links the etappe_detail page to the graph page of a deelnemer
const listenToClickGraphButton = function () {
  const btn = document.querySelector(".js-graph-button");
  btn.addEventListener("click", function () {
    window.location.href = `etappe_grafiek.html?etappeId=${this.getAttribute(
      "data-etappeId"
    )}`;
  });
};

//Event listener of the button that links the etappe_detail page to the graph page of the admin
const listenToClickGraphButtonAdmin = function () {
  const btn = document.querySelector(".js-graph-button");
  btn.addEventListener("click", function () {
    window.location.href = `etappe_grafiek_admin.html?etappeId=${this.getAttribute(
      "data-etappeId"
    )}`;
  });
};

//Event listener home button
const listenToClickLogo = function () {
  const logo = document.querySelector(".js-header-logo");
  let destination = "ronde_overzicht.html";
  if (document.querySelector(".is-admin") != null) {
    destination = "ronde_overzicht_admin.html";
  }

  logo.addEventListener("click", function () {
    window.location.pathname = destination;
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
  if (data.length == 0) {
    htmlString += `
    <div class="c-ranking-table__row u-justify-content--center">U heeft nog geen data om weer te geven.</div>`;
  } else {
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

const calcAantalEtappesNotation = function (count) {
  if (count > 1) {
    return `${count} etappes`;
  } else {
    return `${count} etappe`;
  }
};

const showRoundsAdmin = function (data) {
  console.table(data);
  const table = document.querySelector(".js-rounds-table");
  let htmlString = `<div class="c-ranking-table__header">
  <p class="c-ranking-table__header-item">
    Startdatum
  </p>
  <p class="c-ranking-table__header-item u-text-align--left u-flex-basis-2-of-4">
    Ronde
  </p>
  <p class="c-ranking-table__header-item u-mr-clear">
    # Etappes
  </p>
</div>`;

  //  Checken of data is not zero -> yes: user feedback;
  if (data.length == 0) {
    htmlString += `
    <div class="c-ranking-table__row u-justify-content--center">U heeft nog geen data om weer te geven.</div>`;
  } else {
    for (const item of data) {
      htmlString += `<div class="c-ranking-table__row js-rounds-table-row u-show-pointer" data-roundid="${
        item.rondeId
      }">
      <p class="c-ranking-table__row-item u-flex-basis-1-of-4">
      ${datetimeToDateNotation(item.startDatum)}
      </p>
      <p class="c-ranking-table__row-item  u-text-align--left u-flex-basis-2-of-4">
      ${item.rondeNaam}
      </p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--position u-mr-clear u-flex-basis-1-of-4">
      ${calcAantalEtappesNotation(item.aantalEtappes)}
      </p>
    </div>`;
    }
  }
  hideLoader();
  table.innerHTML = htmlString;
  listenToClickRoundAdmin();
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

//checken of de data niet leeg is
  if (data.length == 0) {
    htmlString += `
  <div class="c-ranking-table__row u-justify-content--center">U heeft nog geen data om weer te geven.</div>`;
  } else {
    let fastestTime = data[0].totaalTijd;

    //Display the first in the rank with a yellow row.
    htmlString += `<div class="c-ranking-table__row u-yellow-jersey">
  <p
    class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha c-result-item"
  >
  #${data[0].plaats}
  </p>
  <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left c-result-item">${data[0].gebruikersNaam.toUpperCase()}</p>
  <p class="c-ranking-table__row-item c-ranking-table__row-item--total-time c-result-item u-mr-clear">${secToTimeNotation(
    data[0].totaalTijd
  )}</p>
</div>`;

    //Remove the first person and go on with showing the ranking
    data.shift();

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
  if (data.length == 0) {
    htmlString += `
  <div class="c-ranking-table__row u-justify-content--center">U heeft nog geen data om weer te geven.</div>`;
  } else {
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

const showEtappesAdmin = function (data) {
  const table = document.querySelector(".js-etappes-table");
  let htmlString = `      <div class="c-ranking-table__header">
  <p class="c-ranking-table__header-item u-flex-basis-1-of-3">
    Startdatum
  </p>
  <p class="c-ranking-table__header-item u-flex-basis-1-of-3 u-text-align--left">
    Ronde
  </p>
  <p class="c-ranking-table__header-item u-flex-basis-1-of-3 u-mr-clear">
    Etappe Tijd
  </p>
</div>`;
  //  Checken of data is not zero -> yes: user feedback;
  if (data.length == 0) {
    htmlString += `
  <div class="c-ranking-table__row u-justify-content--center">U heeft nog geen data om weer te geven.</div>`;
  } else {
    let aantalEtappes = 0;
    for (const item of data) {
      if (item.etappeActief == false) {
        aantalEtappes++;
      }
    }
    for (const item of data) {
      if (item.etappeActief != true) {
        if (item.snelsteTijd == 0) {
          htmlString += `
        <div class="c-ranking-table__row" data-etappeId='${
          item.etappeId
        }' data-etappeTitle='Etappe ${aantalEtappes}'>
      <p class="c-ranking-table__row-item u-flex-basis-1-of-3">
      ${datetimeToDateNotation(item.startTijd)}
      </p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left   u-flex-basis-1-of-3">Etappe 
      ${aantalEtappes}
      </p>
      <p class="c-ranking-table__row-item u-flex-basis-1-of-3 u-mr-clear">
      Afgelast
      </p>
    </div>`;
        } else {
          htmlString += `
      <div class="c-ranking-table__row js-etappes-table-row u-show-pointer" data-etappeId='${
        item.etappeId
      }' data-etappeTitle='Etappe ${aantalEtappes}'>
    <p class="c-ranking-table__row-item u-flex-basis-1-of-3">
    ${datetimeToDateNotation(item.startTijd)}
    </p>
    <p class="c-ranking-table__row-item c-ranking-table__row-item--item-name u-text-align--left   u-flex-basis-1-of-3">Etappe 
    ${aantalEtappes}
    </p>
    <p class="c-ranking-table__row-item u-flex-basis-1-of-3 u-mr-clear">
    ${secToTimeNotation(item.snelsteTijd)}
    </p>
  </div>`;
        }
        aantalEtappes--;
      }
    }
  }
  hideLoader();
  table.innerHTML = htmlString;
  listenToClickEtappeAdmin();
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
  localStorage.setItem("rondeNaam", data.rondeNaam);
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
        text: "Jouw afgewerkte rondes uitgedrukt in seconden",
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
              labelString: "Baanronde",
            },
          },
        ],
        yAxes: [
          {
            display: true,
            scaleLabel: {
              display: true,
              labelString: "Rondetijd (seconden)",
            },
            ticks: {
              reverse: true,
            },
          },
        ],
      },
    },
  };
  hideLoader();
  let speedChart = new Chart(ctx, config);
};

const getLapTijden = function (data) {
  let listLapTijden = [];
  for (let item of data) {
    listLapTijden.push(item.tijdLap);
  }
  return listLapTijden;
};

const showEtappeAdminChartData = function (data) {
  let lapNummers = [];
  for (let item of data[0].lapTijden) {
    lapNummers.push(item.lapNummer);
  }

  const lineColors = ["#FFC145", "#5B5F97", "#EA4200", "#00B8DB", "#2FB760"];

  let dataObjects = [];
  let counter = 0;
  for (let item of data) {
    var obj = {
      label: `${item.gebruikersNaam.toUpperCase()}`,
      borderColor: lineColors[counter],
      backgroundColor: lineColors[counter],
      fill: false,
      data: getLapTijden(item.lapTijden),
    };
    counter++;
    dataObjects.push(obj);
  }

  var lineChartData = {
    labels: lapNummers,
    datasets: dataObjects,
  };

  let ctx = document.querySelector(".js-etappe-chart").getContext("2d");

  let config = {
    type: "line",
    data: lineChartData,
    options: {
      responsive: true,
      maintainAspectRatio: false,
      title: {
        display: true,
        text: `Ronde tijden van de top ${data.length} uitgedrukt in seconden`,
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
              labelString: "Baanronde",
            },
          },
        ],
        yAxes: [
          {
            display: true,
            scaleLabel: {
              display: true,
              labelString: "Rondetijd (seconden)",
            },
            ticks: {
              reverse: true,
            },
          },
        ],
      },
    },
  };
  hideLoader();
  let speedChart = new Chart(ctx, config);
};

const showEtappeAdminData = function (data) {
  const etappeInfo = document.querySelector(".js-etappe-info");
  etappeInfo.innerHTML = `
<div class="c-etappe-info-container__item">
<p class="c-etappe-info-subtitle">Snelste rondetijd</p>
<p class="c-etappe-info-data">${secToTimeNotation(data.snelsteLapTijd)}</p>
</div>
<div class="c-etappe-info-container__item">
<p class="c-etappe-info-subtitle">Traagste rondetijd</p>
<p class="c-etappe-info-data">${secToTimeNotation(data.traagsteLapTijd)}</p>
</div>
  `;
  document.querySelector(".js-etappe-head").innerText = `${localStorage.getItem(
    "rondeNaam"
  )} - ${localStorage.getItem("etappeTitle")}`;
};

const hideLoader = function () {
  const loaders = document.querySelectorAll(".js-data-loader");
  for (const item of loaders) {
    item.classList.add("o-display-none");
  }
};

const showLoader = function () {
  const loaders = document.querySelectorAll(".js-data-loader");
  for (const item of loaders) {
    item.classList.remove("o-display-none");
  }
};

const showUserName = function (userName) {
  userName = userName.split(" ");
  document.querySelector(".js-user-name").innerText = userName[0].toUpperCase();
};

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

const getRoundsAdmin = async function () {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/site/admin/rondes/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showRoundsAdmin(data);
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

const getEtappesAdmin = async function (rondeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/site/admin/ronde/etappes/${rondeId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showEtappesAdmin(data);
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

const getEtappeAdminData = async function (etappeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/info/etappe/times/${etappeId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    console.table(data);
    showEtappeAdminData(data);
  } catch (error) {
    console.error("An error occured, try again.", error);
    alert("Er liep iets mis. Probeer opnieuw.");
  }
};

const getEtappeAdminChartData = async function (etappeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/info/admin/etappe/laptijden/users/${etappeId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showEtappeAdminChartData(data);
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

const pageChecker = function () {
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
      listenToClickAdminPage();
    }

    if (document.querySelector(".js-ronde-overzicht-admin")) {
      showLoader();
      getRoundsAdmin();
      listenToClickDeelnemerPage();
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

    if (document.querySelector(".js-ronde-detail-admin")) {
      showLoader();
      let urlParams = new URLSearchParams(window.location.search);
      const roundId = urlParams.get("roundId");
      if (roundId == null) {
        window.location.pathname = "/ronde_overzicht.html";
      } else {
        getEtappesAdmin(roundId);
        listenToToggleAdmin();
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

    if (document.querySelector(".js-etappe-detail-admin")) {
      showLoader();
      let urlParams = new URLSearchParams(window.location.search);
      const etappeId = urlParams.get("etappeId");
      if (etappeId == null) {
        window.location.pathname = "/ronde_overzicht_admin.html";
      } else {
        document
          .querySelector(".js-graph-button")
          .setAttribute("data-etappeId", etappeId);
        getEtappesRanking(etappeId);
        listenToClickGraphButtonAdmin();
      }
    }

    if (document.querySelector(".js-etappe-grafiek")) {
      showLoader();
      let urlParams = new URLSearchParams(window.location.search);
      const etappeId = urlParams.get("etappeId");
      if (etappeId == null) {
        window.location.pathname = "/ronde_overzicht.html";
      } else {
        getEtappeUserData(etappeId);
        getEtappeUserChartData(etappeId);
      }
    }

    if (document.querySelector(".js-etappe-grafiek-admin")) {
      showLoader();
      let urlParams = new URLSearchParams(window.location.search);
      const etappeId = urlParams.get("etappeId");
      if (etappeId == null) {
        window.location.pathname = "/ronde_overzicht_admin.html";
      } else {
        getEtappeAdminData(etappeId);
        getEtappeAdminChartData(etappeId);
      }
    }
  }
};

document.addEventListener("DOMContentLoaded", function () {
  //Listen to click logo -> go to home
  listenToClickLogo();
  pageChecker();
});
