let userId;

userId = "547F309B-8596-4DBE-9439-333A7C9E79DE";

//#region *** Global Functions ***

const secToTimeNotation = function (seconds) {
  let time = new Date(seconds * 1000).toISOString().substr(11, 8);
  return time;
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

//#region *** Show Data Functions ***

const showRounds = function (data) {
  console.table(data);
  data.sort((a, b) => (a.startDatum < b.startDatum) ? 1 : -1)
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
  <p class="c-ranking-table__header-item">
    Positie
  </p>
</div>`;
  for (const item of data) {
    htmlString += `
        <div class="c-ranking-table__row js-rounds-table-row" data-roundid='${item.rondeId}'>
      <p class="c-ranking-table__row-item">
      ${datetimeToDateNotation(item.startDatum)}
      </p>
      <div class="c-ranking-table__row-item c-ranking-table__row-item--round-name u-text-align--left">
        <p class="c-ranking-table__row-item--top">${item.rondeNaam}</p>
        <p class="c-ranking-table__row-item--sub">${
          item.aantalEtappes
        } etappes</p>
      </div>
      <p class="c-ranking-table__row-item">
      ${secToTimeNotation(item.totaalTijd)}
      </p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha ">
      #${item.plaats}
      </p>
    </div>`;
  }
  table.innerHTML = htmlString;
};

const showEtappes = function (data) {
  console.table(data);
  data.sort((a, b) => (a.startTijd < b.startTijd) ? 1 : -1)
  const table = document.querySelector(".js-etappes-table");
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
  <p class="c-ranking-table__header-item">
    Positie
  </p>
</div>`;
  let Etappe = data.length;
  for (const item of data) {
    htmlString += `
        <div class="c-ranking-table__row js-rounds-table-row" data-roundid='${item.rondeId}'>
      <p class="c-ranking-table__row-item">
      ${datetimeToDateNotation(item.startTijd)}
      </p>
      <p class="c-ranking-table__row-item u-text-align--left">Etappe 
      ${Etappe}
      </p>
      <p class="c-ranking-table__row-item">
      ${secToTimeNotation(item.totaalTijd)}
      </p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha ">
      #${item.plaats}
      </p>
    </div>`;
    Etappe--;
  }
  table.innerHTML = htmlString;
};

//#endregion

const listenToToggle = function () {
  const etappeInput = document.querySelector(".js-option-etappe"),
    roundsRankingInput = document.querySelector(".js-option-rounds-ranking"),
    inputs = document.querySelectorAll(".js-option"),
    rankingContainer = document.querySelector(".c-ranking-container");
  for (const input of inputs) {
    input.addEventListener("change", function () {
      if (etappeInput.checked) {
        rankingContainer.classList.remove("c-rounds-ranking--visible");
        getEtappes('5F4266B2-D77E-46E9-95FE-0B76779B737E');
      }
      if (roundsRankingInput.checked) {
        rankingContainer.classList.add("c-rounds-ranking--visible");
        //getTable();
      }
    });
  }
};

//#region *** Get Data Functions ***

const getRounds = async function () {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruiker/rondes/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    console.log(data);
    showRounds(data);
  } catch (error) {
    console.error("An error occured, we handled it.", error);
  }
};

const getEtappes = async function (rondeId) {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruiker/ronde/etappes/${rondeId}/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    console.log(data);
    showEtappes(data);
  } catch (error) {
    console.error("An error occured, we handled it.", error);
  }
};

const getRoundsRanking = async function () {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruiker/rondes/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    console.log(data);
    showRounds(data);
  } catch (error) {
    console.error("An error occured, we handled it.", error);
  }
};

//#endregion

//#region *** Google Authentication ***

function onSignIn(googleUser) {
  var profile = googleUser.getBasicProfile();
  console.log("ID: " + profile.getId()); // Do not send to your backend! Use an ID token instead.
  console.log("Name: " + profile.getName());
  console.log("Image URL: " + profile.getImageUrl());
  console.log("Email: " + profile.getEmail()); // This is null if the 'email' scope is not present.
}

function signOut() {
  var auth2 = gapi.auth2.getAuthInstance();
  auth2.signOut().then(function () {
    console.log("User signed out.");
    window.location.pathname = "/index.html";
  });
}

//#endregion

document.addEventListener("DOMContentLoaded", function () {
  console.log("DOM loaded :)");
  // get userid from user:
  // userId = sessionStorage.getItem("gebruikerId");
  if(document.querySelector('.roundsoverview')){
    getRounds();
  }
  if(document.querySelector('.etappesoverview')){
    listenToToggle();
    //getEtappes('5F4266B2-D77E-46E9-95FE-0B76779B737E');
    //getRoundsRanking();
  }
  
});
